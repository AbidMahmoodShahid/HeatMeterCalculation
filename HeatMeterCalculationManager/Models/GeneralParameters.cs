using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterCalculationManager.Models
{
    internal class GeneralParameters
    {

        #region Properties

        //Input
        public bool BBefund { get; set; }

        public int NWaageNr { get; set; }

        public int NNormalBits { get; set; }

        public int NKlasse { get; set; }//GetIntFromTable("Klasse");

        public int Einbauort { get; set; }//NEW Property -->  GetIntFromTable("Einbauort")

        public double DeltaTetaMin { get; set; }//NEW Property -->  GetDoubleFromTable("DeltaTetaMin")

        public double NWaage_Nr { get; set; }

        //Output
        public double FK_Ref { get; set; }

        #endregion

        internal GeneralParameters()
        {

        }

    }
}
