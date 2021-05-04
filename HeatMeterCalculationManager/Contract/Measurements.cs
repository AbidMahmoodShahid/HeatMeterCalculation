using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterCalculationManager.Contract
{
    /// <summary>
    /// Stores all measurements in variables.
    /// </summary>
    public class Measurements
    {
        #region Properties

        #region CalculateEnergie

        //Input (werten die nur von DB gelesen werden muss)
        public double FTVor { get; set; }
        public double FTRueck { get; set; }
        public double FTempBankEin { get; set; }
        public double FTempBankAus { get; set; }
        public double FDruckBankEin { get; set; }
        public double FDruckBankAus { get; set; }
        public bool BBefund { get; set; }
        public double FVolumen_Ist { get; set; }
        public double FTimeMut { get; set; }
        public int NWaageNr { get; set; }
        public int NNormalBits { get; set; }
        public double FVolumen_Ref { get; set; }
        public double FTemp_Ref { get; set; }
        public double FTimeRef { get; set; }//multiple values: dictionary<string,double>/List<string,double> ?
        public double FMasseWaage { get; set; }
        public double FTempWaage { get; set; }
        public double FEn_Ist { get; set; }
        public double FQp { get; set; } //GetDoubleFromTable("Qp");
        public int NKlasse { get; set; }//GetIntFromTable("Klasse");
        public int Einbauort { get; set; }//NEW Property -->  GetIntFromTable("Einbauort")
        public double DeltaTetaMin { get; set; }//NEW Property -->  GetDoubleFromTable("DeltaTetaMin")


        //Input + Output (werten die von DB gelesen, im program durch berechnung angepasst, und dann im DB gespeichert werden muss)
        public double FVolumenGes { get; set; }



        //Output
        //public double FEn_Ref { get; set; }
        //public double FErr { get; set; }
        //public double FMaxErr { get; set; }
        //public double FK_Ref { get; set; }
        //public double FQ_Mid { get; set; }
        //public string Str { get; set; }
        //public double FDichteRef { get; set; }
        //public double FDichteMut { get; set; }
        //public double FQ_Ges { get; set; }

        #endregion

        #region CalculateRechenwerk

        //Input (werten die nur von DB gelesen werden muss)
        public double FQref { get; private set; }

        #endregion

        #region CalculateTemperature

        //Input (werten die nur von DB gelesen werden muss)
        public double FTVor_Ist { get; private set; }
        public double FTVor_Ref { get; private set; }
        public double FTRueck_Ist { get; private set; }
        public double FTRueck_Ref { get; private set; }



        //Output
        //public double FTDiff_Ist { get; private set; }
        //public double FTDiff_Ref { get; private set; }

        #endregion

        #region CalculateVolumen

        //Input (werten die nur von DB gelesen werden muss)
        public double NWaage_Nr { get; private set; }



        //Output
        //public double FQ { get; private set; }
        //public double FQ_Mut { get; private set; }
        //public double FVolumenMid { get; private set; }

        #endregion

        #endregion

        public Measurements()
        {

        }

    }
}
