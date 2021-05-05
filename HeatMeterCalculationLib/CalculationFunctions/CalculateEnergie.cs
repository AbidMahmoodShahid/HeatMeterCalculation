using System;

namespace HeatMeterCalculationLib.CalculationFunctions
{
    public class CalculateEnergie : CalculateBase
    {
        public CalculateEnergie()
        {
        }

        //TODO AM: dritte wird Energie berechnet? die klasse wird von PS8 aufgerufen?
        public override void Calculate(int nPPIx, int nKombi, int nPlaetze)
        {
            double fVolumen_Ist;
            double fVolumen_Ref;
            double fTVor;
            double fTRueck;
            double fEn_Ref;
            double fEn_Ist;
            double fErr;
            double fVolumenGes;
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
            int nKlasse;
            string str;
            double fQp;
            double fMaxErr;
            double fK_Ref;
            double fQ_Mid;
            bool bBefund;
            double fTempWaage;
            double fDichteRef;
            double fDichteMut;
            double fQ_Ges;

            // Liest Vorlauf und Rücklauftemperatur aus Datenbank
            SetTempRef(nPPIx, nKombi);//TODO AM: was sind nPPIx und nKombi und was macht SetTempRef()?

            // Spezielle Auswertungen abh. Prüfungstyp
            GetErgWert(nPPIx, "Temp_Vor_Ref", out fTVor);
            GetErgWert(nPPIx, "Temp_Rueck_Ref", out fTRueck);
            GetErgWert(nPPIx, "Temp_Bank_Ein_1_MID", out fTempBankEin);
            GetErgWert(nPPIx, "Temp_Bank_Aus_1_MID", out fTempBankAus);
            GetErgWert(nPPIx, "Druck_Bank_Ein_1_MID", out fDruckBankEin);
            GetErgWert(nPPIx, "Druck_Bank_Aus_1_MID", out fDruckBankAus);
            GetErgWert(nPPIx, "Befund", out bBefund);

            double tPruef = fTempBankEin - (fTempBankEin - fTempBankAus) / nPlaetze * m_nPlatz;
            double pPruef = P_NORM + fDruckBankEin - (fDruckBankEin - fDruckBankAus) / nPlaetze * m_nPlatz;

            GetErgWert(nPPIx, "Vol_Ist", out fVolumen_Ist);
            GetErgWert(nPPIx, "Messzeit_Ist", out fTimeMut);
            GetErgWert(nPPIx, "Waage_Nr", out nWaageNr);
            GetErgWert(nPPIx, "NormalBits", out nNormalBits);

            fVolumenGes = 0;
            if ((nWaageNr == 0) && (nNormalBits == 0))
                GetErgWert(nPPIx, "Vol_Ref", out fVolumenGes);
            fVolumen_Ref = 0;
            for (int i = 0; i < 8; i++)
            {
                if ((nNormalBits & (0x01 << i)) != 0)
                {
                    str = string.Format("Vol_Ref_{0}", i + 1);
                    GetErgWert(nPPIx, str, out fVolumen_Ref);
                    str = string.Format("Temp_Normal_{0}_MID", i + 1);
                    GetErgWert(nPPIx, str, out fTemp_Ref);
                    str = string.Format("Messzeit_Ref_{0}", i + 1);
                    GetErgWert(nPPIx, str, out fTimeRef);

                    fVolumen_Ref = fVolumen_Ref / fTimeRef * fTimeMut;

                    double fMasse = fVolumen_Ref / SpezifischesVolumen(fDruckBankAus, fTemp_Ref) / VOLUME_FAKTOR;
                    fVolumenGes += fMasse * SpezifischesVolumen(pPruef, tPruef) * VOLUME_FAKTOR;

                    fQ_Mid = fVolumen_Ref / fTimeRef * 3600 / VOLUME_FAKTOR;
                    SetErgWert(nPPIx, "Q_Ref", fQ_Mid);
                }
            }
            if (nWaageNr != 0)
            {
                // Masse auf Volumen am Prüfling umrechnen
                GetErgWert(nPPIx, "Masse_Waage", out fMasseWaage);
                GetErgWert(nPPIx, "Messzeit_Ref_Waage", out fTimeRef);
                GetErgWert(nPPIx, "Temp_Waage_MID", out fTempWaage);

                // Masse auf die Messzeit bezogen
                double fMasseWaageKor = fMasseWaage / fTimeRef * fTimeMut;

                // Dichte an der Waage
                fDichteRef = 1 / SpezifischesVolumen(fDruckBankAus, fTempWaage + T_NULL);
                double fKala = Luftauftrieb(fDichteRef);

                // mittlerer Fluss in die Waage
                fQ_Ges = fMasseWaage / fDichteRef / fTimeRef * 3600;

                // Dichte am Prüfling
                fDichteMut = 1 / SpezifischesVolumen(pPruef, tPruef + T_NULL);

                // Volumen am Prüfling
                fVolumenGes = fMasseWaageKor / fDichteMut * fKala * VOLUME_FAKTOR;
            }

            SetErgWert(nPPIx, "Vol_Ref", fVolumenGes);

            fErr = 100 * (fVolumen_Ist - fVolumenGes) / fVolumenGes;
            SetErgWert(nPPIx, "Vol_Err", fErr);

            fQp = GetDoubleFromTable("Qp");

            double fQ = fVolumenGes / fTimeMut * 3600 / VOLUME_FAKTOR;

            nKlasse = GetIntFromTable("Klasse");
            // Max Fehler bei Volumenberechnung
            if (nKlasse == 2)
                fMaxErr = 0.02 * fQp / fQ + nKlasse;
            else
                fMaxErr = 0.05 * fQp / fQ + nKlasse;
            if (fMaxErr > 5)
                fMaxErr = 5;

            if (bBefund == true)
                fMaxErr *= 2;

            SetErgWert(nPPIx, "Vol_Err_Max", fMaxErr);

            if (Math.Abs(fErr) > fMaxErr)
                SetErgWert(nPPIx, "Vol_Err_Flag", true);
            else
                SetErgWert(nPPIx, "Vol_Err_Flag", false);

            if (GetIntFromTable("Einbauort") == (int)Einbauort.EBAU_VORLAUF)
            {
                fK_Ref = Waermekoeffizient(fDruckBankAus, fTVor, fTRueck, true);
                fEn_Ref = Waermemenge(fVolumen_Ref / VOLUME_FAKTOR, fDruckBankAus, fTVor, fTRueck, true);
            }
            else
            {
                fK_Ref = Waermekoeffizient(fDruckBankAus, fTVor, fTRueck, false);
                fEn_Ref = Waermemenge(fVolumen_Ref / VOLUME_FAKTOR, fDruckBankAus, fTVor, fTRueck, false);
            }
            SetErgWert(nPPIx, "E_Ref", fEn_Ref);
            SetErgWert(nPPIx, "K_Ref", fK_Ref);

            GetErgWert(nPPIx, "E_Ist", out fEn_Ist);

            fErr = 100 * (fEn_Ist - fEn_Ref) / fEn_Ref;
            SetErgWert(nPPIx, "E_Err", fErr);
            SetErgWert(nPPIx, "E_Err_Max", fMaxErr);

            // Max Fehler bei Energieberechnung
            fMaxErr = 0.5 + (GetDoubleFromTable("DeltaTetaMin") / (fTVor - fTRueck));

            if (Math.Abs(fErr) > fMaxErr)
                SetErgWert(nPPIx, "E_Err_Flag", true);
            else
                SetErgWert(nPPIx, "E_Err_Flag", false);
        }

    }
}
