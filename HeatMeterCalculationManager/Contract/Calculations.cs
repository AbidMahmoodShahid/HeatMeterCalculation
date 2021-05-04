using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterCalculationManager.Contract
{
    public class Calculations
    {
        #region Properties

        #region CalculateEnergie

        //Output (werten die berechnet, und im DB gespeichert werden muss)
        public double FQ_Mid { get; set; }
        public double FErr { get; set; }//multiple values: dictionary<string,double>/List<string,double> ? (Vol_Err, E_Err)
        public double FMaxErr { get; set; }//multiple values: dictionary<string,double>/List<string,double> ? (Vol_Err_Max, E_Err_Max)
        public double FEn_Ref { get; set; }
        public double FK_Ref { get; set; }
        public bool Vol_Err_Flag { get; set; }//NEW Property --> (Math.Abs(fErr) > fMaxErr)
        public bool E_Err_Flag { get; set; }//NEW Property --> (Math.Abs(fErr) > fMaxErr)
        public int[] TempRef { get; set; }//NEW Property --> SetTempRef(int nIx, int nKombi)


        //Input + Output (werten die von DB gelesen, im program durch berechnung angepasst, und dann im DB gespeichert werden muss)
        public double FVolumenGes { get; set; }


        //Middle State (werten die von input werten berechnet wird aber nicht im DB gespeichert, sondern damit wird weitere Kalkulationen gemacht)
        public string Str { get; set; }
        public double FDichteRef { get; set; }// --> = 1 / SpezifischesVolumen(fDruckBankAus, fTempWaage + T_NULL);
        public double FDichteMut { get; set; }// --> = 1 / SpezifischesVolumen(pPruef, tPruef + T_NULL);
        public double FQ_Ges { get; set; }// --> = fMasseWaage / fDichteRef / fTimeRef * 3600;




        //Input (werten die nur von DB gelesen werden muss)
        //public double FVolumen_Ist { get; set; }
        //public double FVolumen_Ref { get; set; }
        //public double FTVor { get; set; }
        //public double FTRueck { get; set; }
        //public double FEn_Ist { get; set; }
        //public double FTimeMut { get; set; }
        //public double FTimeRef { get; set; }
        //public double FTempBankEin { get; set; }
        //public double FTempBankAus { get; set; }
        //public double FDruckBankEin { get; set; }
        //public double FDruckBankAus { get; set; }
        //public double FMasseWaage { get; set; }
        //public double FTemp_Ref { get; set; }
        //public int NWaageNr { get; set; }
        //public int NNormalBits { get; set; }
        //public int NKlasse { get; set; }
        //public double FQp { get; set; }
        //public bool BBefund { get; set; }
        //public double FTempWaage { get; set; }

        #endregion

        #region CalculateRechenwerk

        //Input (werten die nur von DB gelesen werden muss)
        //public double FQref { get; private set; }

        #endregion

        #region CalculateTemperature

        //Output (werten die berechnet, und im DB gespeichert werden muss)
        public double FTDiff_Ist { get; private set; }
        public double FTDiff_Ref { get; private set; }


        //Input (werten die nur von DB gelesen werden muss)
        //public double FTVor_Ist { get; private set; }
        //public double FTVor_Ref { get; private set; }
        //public double FTRueck_Ist { get; private set; }
        //public double FTRueck_Ref { get; private set; }

        #endregion

        #region CalculateVolumen


        //Middle State (werten die von input werten berechnet wird aber nicht im DB gespeichert, sondern damit wird weitere Kalkulationen gemacht)
        public double FQ { get; private set; }
        public double FQ_Mut { get; private set; }


        //Middle State (werten die von input werten berechnet wird aber nicht im DB gespeichert, sondern damit wird weitere Kalkulationen gemacht)
        public double FVolumenMid { get; private set; }



        //Input (werten die nur von DB gelesen werden muss)
        //public double NWaage_Nr { get; private set; }

        #endregion

        #endregion

        public Calculations()
        {

        }
    }
}
