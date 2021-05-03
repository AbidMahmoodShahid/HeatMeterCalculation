using System;

namespace HeatMeterCalculationLib.CalculationFunctions
{
    public class CalculateRechenwerk : CalculateBase
    {
        public CalculateRechenwerk()
        {

        }

        //TODO AM: was kalkuliert die Rechenwerk?
        //TODO AM: erst wird Temperatur, Volume und Energie berechnet und dann Rechenwerk von PS8 aufgerufen?
        public override void Calculate(int nPPIx, int nKombi, int nPlaetze)
        {
            double fVolumen_Ist;
            double fVolumen_Ref;
            double fTVor;
            double fTRueck;
            double fEn_Ref;
            double fEn_Ist;
            double fErr;
            double fVolumenGes = 0;
            double fTimeMut;
            double fTimeRef;
            double fTempBankEin;
            double fTempBankAus;
            double fDruckBankEin;
            double fDruckBankAus;
            double fMasseWaage;
            double fTemp_Ref;
            int nWaageNr;
            int nNormalBits;
            string str;
            int nKlasse;
            double fQp;
            double fQref;
            double fK_Ref;
            double fMaxErr;
            bool bBefund;

            // Liest Vorlauf und Rücklauftemperatur aus Datenbank
            SetTempRef(nPPIx, nKombi);

            // Spezielle Auswertungen abh. Prüfungstyp
            GetErgWert(nPPIx, "Temp_Vor_Ref", out fTVor);
            GetErgWert(nPPIx, "Temp_Rueck_Ref", out fTRueck);
            GetErgWert(nPPIx, "Temp_Bank_Ein_1_MID", out fTempBankEin);
            GetErgWert(nPPIx, "Temp_Bank_Aus_1_MID", out fTempBankAus);
            GetErgWert(nPPIx, "Druck_Bank_Ein_1_MID", out fDruckBankEin);
            GetErgWert(nPPIx, "Druck_Bank_Aus_1_MID", out fDruckBankAus);
            GetErgWert(nPPIx, "Befund", out bBefund);

            double tPruef = fTempBankEin - (fTempBankEin - fTempBankAus) / nPlaetze * nPPIx;                //RR 10 durch Variable ersetzen
            double pPruef = P_NORM + fDruckBankEin - (fDruckBankEin - fDruckBankAus) / nPlaetze * nPPIx;                //RR 10 durch Variable ersetzen

            GetErgWert(nPPIx, "Vol_Ist", out fVolumen_Ist);
            GetErgWert(nPPIx, "Messzeit_Ist", out fTimeMut);

            GetErgWert(nPPIx, "Waage_Nr", out nWaageNr);
            GetErgWert(nPPIx, "NormalBits", out nNormalBits);

            if ((nWaageNr == 0) && (nNormalBits == 0))
                GetErgWert(nPPIx, "Vol_Ref", out fVolumenGes);
            else if (nWaageNr != 0)
            {
                // Masse auf Volumen am Prüfling umrechnen
                GetErgWert(nPPIx, "Masse_Waage", out fMasseWaage);
                fVolumenGes = fMasseWaage * SpezifischesVolumen(pPruef, tPruef + T_NULL) * VOLUME_FAKTOR;

                GetErgWert(nPPIx, "Messzeit_Ref_Waage", out fTimeRef);
                fVolumenGes = fVolumenGes / fTimeRef * fTimeMut;
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    if ((nNormalBits & (0x01 << i)) != 0)
                    {
                        str = string.Format("Vol_Ref_{0}", i + 1);
                        GetErgWert(nPPIx, str, out fVolumen_Ref);

                        str = string.Format("Temp_Normal_{0}_MID", i + 1);
                        GetErgWert(nPPIx, str, out fTemp_Ref);

                        double fMasse = fVolumen_Ref / SpezifischesVolumen(fDruckBankAus, fTemp_Ref + T_NULL) / VOLUME_FAKTOR;
                        double fVolumen = fMasse * SpezifischesVolumen(pPruef, tPruef + T_NULL) * VOLUME_FAKTOR;

                        str = string.Format("Messzeit_Ref_{0}", i + 1);
                        GetErgWert(nPPIx, str, out fTimeRef);
                        fVolumenGes += fVolumen / fTimeRef * fTimeMut;
                    }
                }
            }

            SetErgWert(nPPIx, "Vol_Ref", fVolumenGes);

            fErr = 100 * (fVolumen_Ist - fVolumenGes) / fVolumenGes;
            SetErgWert(nPPIx, "Vol_Err", fErr);

            // Max Fehler bei Volumenberechnung

            nKlasse = GetIntFromTable("Klasse");
            fQp = GetDoubleFromTable("Qp");
            GetErgWert(nPPIx, "Q_Ref", out fQref);

            fMaxErr = 0.02 * fQp / fQref + nKlasse;
            if (fMaxErr > 5)
                fMaxErr = 5;
            if (bBefund == true)
                fMaxErr *= 2;

            SetErgWert(nPPIx, "Vol_Err_Max", fMaxErr);

            if (Math.Abs(fErr) > fMaxErr)
                SetErgWert(nPPIx, "Vol_Err_Flag", true);
            else
                SetErgWert(nPPIx, "Vol_Err_Flag", false);

            GetErgWert(nPPIx, "E_Ist", out fEn_Ist);

            if (GetIntFromTable("Einbauort") == (int)Einbauort.EBAU_VORLAUF)
            {
                fEn_Ref = Waermemenge(fVolumenGes / VOLUME_FAKTOR, fDruckBankAus, fTVor, fTRueck, true);
                fK_Ref = Waermekoeffizient(fDruckBankAus, fTVor, fTRueck, true);
            }
            else
            {
                fEn_Ref = Waermemenge(fVolumenGes / VOLUME_FAKTOR, fDruckBankAus, fTVor, fTRueck, false);
                fK_Ref = Waermekoeffizient(fDruckBankAus, fTVor, fTRueck, false);
            }
            SetErgWert(nPPIx, "E_Ref", fEn_Ref);
            GetErgWert(nPPIx, "E_Ist", out fEn_Ist);

            fK_Ref /= 1000000;
            SetErgWert(nPPIx, "K_Ref", fK_Ref);

            fErr = 100 * (fEn_Ist - fEn_Ref) / fEn_Ref;
            SetErgWert(nPPIx, "E_Err", fErr);

            // Max Fehler bei Energieberechnung
            fMaxErr = 0.5 + GetDoubleFromTable("DeltaTetaMin") / (fTVor - fTRueck);
            if (bBefund == true)
                fMaxErr *= 2;

            SetErgWert(nPPIx, "E_Err_Max", fMaxErr);

            if (Math.Abs(fErr) > fMaxErr)
                SetErgWert(nPPIx, "E_Err_Flag", true);
            else
                SetErgWert(nPPIx, "E_Err_Flag", false);
        }
    }
}
