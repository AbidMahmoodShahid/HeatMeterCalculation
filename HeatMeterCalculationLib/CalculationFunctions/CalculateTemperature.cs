using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterCalculationLib.CalculationFunctions
{
    public class CalculateTemperature : CalculateBase
    {
        public CalculateTemperature()
        {

        }

        //TODO AM: erst wird Temperatur berechnet? die klasse wird von PS8 aufgerufen?
        public override void Calculate(int nPPIx, int nKombi, int nPlaetze)//TODO AM : was ist nPPIx, nKombi? nPlaetze = Anzahl der Plätze?
        {
            double fTVor_Ist;
            double fTVor_Ref;
            double fTRueck_Ist;
            double fTRueck_Ref;
            double fTDiff_Ist;
            double fTDiff_Ref;
            double fErr;
            double fMaxErr;
            bool bBefund;

            // Liest Vorlauf und Rücklauftemperatur aus Datenbank
            SetTempRef(nPPIx, nKombi);

            // Spezielle Auswertungen abh. Prüfungstyp
            GetErgWert(nPPIx, "Temp_Vor_Ist", out fTVor_Ist); //TODO AM: warum ein referenz temperatur? war das nicht gemessen? wie wird hier die error gemessen?
            GetErgWert(nPPIx, "Temp_Vor_Ref", out fTVor_Ref);
            fErr = 100 * (fTVor_Ist - fTVor_Ref) / (fTVor_Ref + T_NULL);
            SetErgWert(nPPIx, "Temp_Vor_Err", fErr);
            GetErgWert(nPPIx, "Befund", out bBefund);//TODO AM: was ist Befund?

            GetErgWert(nPPIx, "Temp_Rueck_Ist", out fTRueck_Ist);
            GetErgWert(nPPIx, "Temp_Rueck_Ref", out fTRueck_Ref);
            fErr = 100 * (fTRueck_Ist - fTRueck_Ref) / (fTRueck_Ref + T_NULL);
            SetErgWert(nPPIx, "Temp_Rueck_Err", fErr);

            fTDiff_Ist = fTVor_Ist - fTRueck_Ist;
            fTDiff_Ref = fTVor_Ref - fTRueck_Ref;
            fErr = 100 * (fTDiff_Ist - fTDiff_Ref) / (fTDiff_Ref + T_NULL);

            SetErgWert(nPPIx, "Delta_Teta_Ist", fTDiff_Ist);
            SetErgWert(nPPIx, "Delta_Teta_Ref", fTDiff_Ref);
            SetErgWert(nPPIx, "Delta_Teta_Err", fErr);

            // Max Fehler bei Tempoeraturberechnung
            double fDiff = Math.Abs(fTVor_Ref - fTRueck_Ref);
            double fDiffMin = GetDoubleFromTable("DeltaTetaMin");
            if (fDiffMin == 0)
                fDiffMin = 3;
            if (fDiff < fDiffMin)
                fDiff = fDiffMin;
            fMaxErr = 0.5 + 3 * fDiffMin / fDiff;
            if (bBefund == true)
                fMaxErr *= 2;
            SetErgWert(nPPIx, "Delta_Teta_Err_Max", fMaxErr);

            if (Math.Abs(fErr) > fMaxErr)
                SetErgWert(nPPIx, "Delta_Teta_Err_Flag", true);
            else
                SetErgWert(nPPIx, "Delta_Teta_Err_Flag", false);
        }
    }
}
