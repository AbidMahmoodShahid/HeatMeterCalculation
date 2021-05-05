using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterCalculationManager.Models
{
    internal class Temperature
    {
        #region Properties

        //Input
        public double FTVor { get; set; }

        public double FTRueck { get; set; }

        public double FTempBankEin { get; set; }

        public double FTempBankAus { get; set; }

        public double FTemp_Ref { get; set; }

        public double FTempWaage { get; set; }

        public double FTVor_Ist { get; set; }

        public double FTVor_Ref { get; set; }

        public double FTRueck_Ist { get; set; }

        public double FTRueck_Ref { get; set; }

        //Output
        public int[] TempRef { get; set; }//NEW Property --> SetTempRef(int nIx, int nKombi)

        public double FTDiff_Ist { get; set; }

        public double FTDiff_Ref { get; set; }

        #endregion

        internal Temperature()
        {

        }


    }
}
