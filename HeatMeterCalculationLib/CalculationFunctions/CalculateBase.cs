using System;
using System.Collections.Generic;

namespace HeatMeterCalculationLib.CalculationFunctions
{
    public enum Einbauort
    {
        EBAU_VORLAUF = 0,
        EBAU_RUECKLAUF = 1
    }

    public class Koeffizient
    {
        public int nIi { get; set; }
        public int nJi { get; set; }
        public double fni { get; set; }
        public Koeffizient()
        {

        }

        public Koeffizient(int nIi, int nJi, double fni)
        {
            this.nIi = nIi;
            this.nJi = nJi;
            this.fni = fni;
        }
    }

    public abstract class CalculateBase
    {

        public static readonly double T_NULL = 273.15;           // Normtemperatur
        public static readonly double P_STERN = 16.53E06;        // Pa
        public static readonly double T_STERN = 1386.0;          // K
        public static readonly double R_UNI_GAS = 461.525;       // Universelle Gaskonstante J / (kg * K )
        public static readonly double P_NORM = 1.02315;          // Normdruck
        public static readonly double RHO_LUFT_20 = 1.20;        // Dichte Luft bei 20°C 0% rel. Feuchte
        public static readonly double RHO_LUFT_BEHAELTER = 1.04; // Dichte Luft im Behälter bei 50°C 100% rel. Feuchte
        public static readonly double KOR_NORMAL_GEWICHT = 8000; // konventionelle Dichte der Normalgewichtstücke
        public static readonly double VOLUME_FAKTOR = 1000;

        private static readonly List<Koeffizient> KoeffizientList = new List<Koeffizient>
        {
            new Koeffizient ( 0, -2,  0.14632971213167),
            new Koeffizient ( 0, -1, -0.84548187169114),
            new Koeffizient( 0,  0, -0.37563603672040E1),
            new Koeffizient( 0,  1, 0.33855169168385E1),
            new Koeffizient(0,  2 ,-0.95791963387872),
            new Koeffizient(0,  3,  0.15772038513228),
            new Koeffizient(0,  4, -0.16616417199501E-1),
            new Koeffizient(0,  5,  0.81214629983568E-3),
            new Koeffizient(1, -9,  0.28319080123804E-3),
            new Koeffizient(1, -7, -0.60706301565874E-3),
            new Koeffizient(1, -1, -0.18990068218419E-1),
            new Koeffizient(1,  0, -0.32529748770505E-1),
            new Koeffizient(1,  1, -0.21841717175414E-1),
            new Koeffizient(1,  3, -0.52838357969930E-4),
            new Koeffizient(2, -3, -0.47184321073267E-3),
            new Koeffizient(2,  0, -0.30001780793026E-3),
            new Koeffizient(2,  1,  0.47661393906987E-4),
            new Koeffizient(2,  3, -0.44141845330846E-5),
            new Koeffizient(2, 17, -0.72694996297594E-15),
            new Koeffizient(3, -4, -0.31679644845054E-4),
            new Koeffizient(3,  0, -0.28270797985312E-5),
            new Koeffizient(3,  6, -0.85205128120103E-9),
            new Koeffizient(4, -5, -0.22425281908000E-5),
            new Koeffizient(4, -2, -0.65171222895601E-6),
            new Koeffizient(4, 10, -0.14341729937924E-12),
            new Koeffizient(5,   -8, -0.40516996860117E-6),
            new Koeffizient(8,  -11, -0.12734301741641E-8),
            new Koeffizient(8,   -6, -0.17424871230634E-9),
            new Koeffizient(21, -29, -0.68762131295531E-18),
            new Koeffizient(23, -31,  0.14478307828521E-19),
            new Koeffizient(29, -38,  0.26335781662795E-22),
            new Koeffizient(30, -39, -0.11947622640071E-22),
            new Koeffizient(31, -40,  0.18228094581404E-23),
            new Koeffizient(32, -41, -0.93537087292458E-25),
        };

        private static readonly double[] cn = new[] { 9.99839564E2, 6.7998613E-2, -9.1101468E-3, 1.0058299E-4, -1.1275659E-6, 6.5985371E-9 };
        private static readonly double[] an = new[] { 9.9983952E2, 1.6952577E1, -7.9905127E-3, -4.6241757E-5, 1.0584601E-7, -2.8103006E-10 };
        private static readonly double b = 1.6887236E-2;

        public int m_nPlatz { get; set; }

        protected double Dichte(double fT)
        {
            double fDichte = 0;
            int n;
            double fTemp = 1;

            if ((fT >= 0) && (fT <= 40))
            {
                for (n = 0; n < 6; n++)
                {
                    fDichte += cn[n] * fTemp;
                    fTemp *= fT;
                }
            }
            else if ((fT >= 0) && (fT <= 150))
            {
                for (n = 0; n < 6; n++)
                {
                    fDichte += an[n] * fTemp;
                    fTemp *= fT;
                }
                fDichte /= (1 + b * fT);
            }
            return fDichte;
        }

        protected double SpezifischesVolumen(double fp, double fT)
        {
            double pi;
            double tau;
            double gamma_pi;

            double f1;
            double f2;
            double fSpezVol;
            double fDichteInv;

            pi = fp * 10000 / P_STERN;  // bar ==> Pascal
            tau = T_STERN / fT;

            gamma_pi = 0;

            for (int i = 0; i < KoeffizientList.Count; i++)
            {
                f1 = Math.Pow(7.1 - pi, KoeffizientList[i].nIi - 1);
                f2 = Math.Pow(tau - 1.222, KoeffizientList[i].nJi);

                gamma_pi += -KoeffizientList[i].fni * KoeffizientList[i].nIi * f1 * f2;
            }

            // Zum Vergleich fDichteInv sollte gleich fSpezVol sein
            fDichteInv = 1 / Dichte(fT - T_NULL);

            fSpezVol = pi * gamma_pi * R_UNI_GAS * fT / fp / 10000;

            return fSpezVol;
        }

        protected double SpezifischeEnthalpie(double fp, double fT)
        {
            double pi;
            double tau;
            double gamma_tau;

            double f1;
            double f2;

            pi = fp * 10000 / P_STERN;
            tau = T_STERN / fT;

            gamma_tau = 0;

            for (int i = 0; i < KoeffizientList.Count; i++)
            {
                f1 = Math.Pow(7.1 - pi, KoeffizientList[i].nIi);
                f2 = Math.Pow(tau - 1.222, KoeffizientList[i].nJi - 1);

                gamma_tau += KoeffizientList[i].fni * f1 * KoeffizientList[i].nJi * f2;
            }

            return tau * gamma_tau * R_UNI_GAS * fT;
        }

        protected double Waermekoeffizient(double fp, double _fTVor, double _fTRueck, bool bEinbauVorlauf)
        {
            double fSpezVol;
            double fWKoeff;
            double fEnthVor;
            double fEnthRueck;

            double fTVor = _fTVor + T_NULL;
            double fTRueck = _fTRueck + T_NULL;

            if (bEinbauVorlauf == true)     // Einbauort Vorlauf ?
                fSpezVol = SpezifischesVolumen(fp, fTVor);
            else
                fSpezVol = SpezifischesVolumen(fp, fTRueck);

            fEnthVor = SpezifischeEnthalpie(fp, fTVor);
            fEnthRueck = SpezifischeEnthalpie(fp, fTRueck);

            fWKoeff = 1 / fSpezVol * (fEnthVor - fEnthRueck) / (fTVor - fTRueck);

            return fWKoeff;
        }

        protected double Waermemenge(int nTime, double fQMasse, double fp, double fTVor, double fTRueck, bool bEinbauVorlauf)
        {
            double fWMenge;
            double fEnthVor;
            double fEnthRueck;

            // Umrechnung bar --> Pascal und Absoluttemperaturen

            fEnthVor = SpezifischeEnthalpie(fp, fTVor + T_NULL);
            fEnthRueck = SpezifischeEnthalpie(fp, fTRueck + T_NULL);

            fWMenge = fQMasse * (fEnthVor - fEnthRueck) * nTime / 3600;

            return fWMenge;
        }

        protected double Waermemenge(double fVolumen, double fp, double fTVor, double fTRueck, bool bEinbauVorlauf)
        {
            double fWMenge;
            double fWKoeff;

            // Umrechnung bar --> Pascal und Absoluttemperaturen
            fWKoeff = Waermekoeffizient(fp, fTVor, fTRueck, bEinbauVorlauf);

            fWMenge = fWKoeff * (fTVor - fTRueck) * fVolumen;

            return fWMenge / 1000 / 3600;       // kWh
        }

        protected double Luftauftrieb(double fSpezVol)
        {
            double fKala;

            fKala = (1 - RHO_LUFT_20 / KOR_NORMAL_GEWICHT) / (1 - RHO_LUFT_BEHAELTER / fSpezVol);

            return fKala;
        }

        protected bool GetErgWert(int ix, string strName, out double fValue)
        {
            bool bRet = false;
            /*
                CInoValue* pInoValue;
                if (ix < m_ArResult.GetSize())
                {
                    CInoDBObj* inoObj = (CInoDBObj*)m_ArResult.GetAt(ix);
                    pInoValue = inoObj->GetValueTObj(strName);
                    if (pInoValue != NULL)
                    {
                        if (pInoValue->m_bEmpty == false)
                        {
                            fValue = ((CInoDouble*)pInoValue)->GetValue();
                            bRet = true;
                        }
                    }
                }
            */
            fValue = 0;
            return bRet;
        }

        protected bool GetErgWert(int nPPIx, string name, out int value)
        {
            bool bRet = false;
            value = 0;
            return bRet;
        }

        protected bool GetErgWert(int nPPIx, string name, out bool value)
        {
            bool bRet = false;
            value = false;
            return bRet;
        }

        protected void SetErgWert(int nPPIx, string name, double value)
        {

        }

        protected void SetErgWert(int nPPIx, string name, bool value)
        {

        }

        protected int GetIntFromTable(string name)
        {
            return 0;
        }

        protected double GetDoubleFromTable(string name)
        {
            return 0;
        }

        public void SetTempRef(int nIx, int nKombi)
        {
            /*
                CInoDatabase inoDB;
                CResistor resistor;
                double fValue;
                CString str;

                // Reale Werte der Kombunation aus DB auslesen
                if ((nKombi > -1) && (m_pSensorTemp[0] != NULL) && (m_pSensorTemp[1] != NULL))
                {
                    inoDB.Open(g_InoDB_Str);
                    CInoResultArray* results = GetResults();

                    if (m_pSensorTemp[0]->GetTypeStr(str) == true)
                    {
                        fValue = resistor.GetResistor(&inoDB, m_nPlatz + 1, str, nKombi, true);
                        results->SetErgWert(nIx, _T("Temp_Vor_Ref"), fValue);
                    }
                    if (m_pSensorTemp[1]->GetTypeStr(str) == true)
                    {
                        fValue = resistor.GetResistor(&inoDB, m_nPlatz + 1, str, nKombi, false);
                        results->SetErgWert(nIx, _T("Temp_Rueck_Ref"), fValue);
                    }
                    inoDB.Close();
                }
            */
        }

        public abstract void Calculate(int nPPIx, int nKombi, int nPlaetze);
    }
}
