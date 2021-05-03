using System;

namespace HeatMeterCalculationLib.CalculationFunctions
{
    public class CalculateVolumen : CalculateBase
    {
        public CalculateVolumen()
        {
        }

        public override void Calculate(int nPPIx, int nKombi, int nPlaetze)
        {
            double fVolumen_Ist;
            double fVolumen_Ref;
            double fErr;
            double fQ;
            double fQp;
            int nKlasse;
            double fMaxErr;
            double fVolumenGes;
            double fTimeMut;
            double fTimeRef;
            string str;
            int nWaage_Nr;
            int nNormalBits;
            double fTempBankEin;
            double fTempBankAus;
            double fDruckBankEin;
            double fDruckBankAus;
            double fMasseWaage;
            double fTemp_Ref;
            double fQ_Mut;
            double fVolumenMid;
            double fQ_Mid = 0;
            double fQ_Ges;
            double fDichteMid = 0;
            double fDichteMut = 0;
            double fTempWaage;
            double fDichteRef;
            bool bBefund;

            GetErgWert(nPPIx, "Temp_Bank_Ein_1_MID", out fTempBankEin);
            GetErgWert(nPPIx, "Temp_Bank_Aus_1_MID", out fTempBankAus);
            GetErgWert(nPPIx, "Druck_Bank_Ein_1_MID", out fDruckBankEin);
            GetErgWert(nPPIx, "Druck_Bank_Aus_1_MID", out fDruckBankAus);
            GetErgWert(nPPIx, "Befund", out bBefund);

            double tPruef = fTempBankEin - (fTempBankEin - fTempBankAus) / nPlaetze * m_nPlatz;
            double pPruef = P_NORM + fDruckBankEin - (fDruckBankEin - fDruckBankAus) / nPlaetze * m_nPlatz;

            GetErgWert(nPPIx, "Vol_Ist", out fVolumen_Ist);
            GetErgWert(nPPIx, "Messzeit_Ist", out fTimeMut);

            fQ_Mut = fVolumen_Ist / fTimeMut * 3600 / VOLUME_FAKTOR;

            GetErgWert(nPPIx, "Waage_Nr", out nWaage_Nr);
            GetErgWert(nPPIx, "NormalBits", out nNormalBits);

            // Neue Version Zeitlorrektur VOR Umrechnng

            fVolumenMid = 0;
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

                    double fVolumenMidKor = fVolumen_Ref / fTimeRef * fTimeMut;
                    fQ_Mid = fVolumen_Ref / fTimeRef * 3600 / VOLUME_FAKTOR;

                    fDichteMid = 1 / SpezifischesVolumen(P_NORM + fDruckBankAus, fTemp_Ref + T_NULL);
                    double fMasse = fVolumenMidKor * fDichteMid / VOLUME_FAKTOR;

                    fDichteMut = 1 / SpezifischesVolumen(pPruef, tPruef + T_NULL);
                    fVolumenMid += fMasse / fDichteMut * VOLUME_FAKTOR;
                }
            }

            if (nWaage_Nr != 0)
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
            else
            {
                fVolumenGes = fVolumenMid;
                fQ_Ges = fQ_Mid;
                fDichteRef = fDichteMid;
            }

            SetErgWert(nPPIx, "Q_Ref", fQ_Ges);
            SetErgWert(nPPIx, "Q_Ist", fQ_Mut);

            SetErgWert(nPPIx, "Dichte_Ref", fDichteRef);
            SetErgWert(nPPIx, "Dichte_Mut", fDichteMut);

            SetErgWert(nPPIx, "Vol_Ref", fVolumenGes);

            fErr = 100 * (fVolumen_Ist - fVolumenGes) / fVolumenGes;
            SetErgWert(nPPIx, "Vol_Err", fErr);

            // Max Fehler bei Volumenmessung

            nKlasse = GetIntFromTable("Klasse");
            fQp = GetDoubleFromTable("Qp");

            fQ = fVolumenGes / fTimeMut * 3600 / VOLUME_FAKTOR;

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
        }
    }
}
