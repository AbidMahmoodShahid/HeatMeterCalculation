using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterCalculationManager.Models
{
    internal class Density
    {

        #region Properties

        //Output
        public double FDichteRef { get; set; }// --> = 1 / SpezifischesVolumen(fDruckBankAus, fTempWaage + T_NULL);

        public double FDichteMut { get; set; }// --> = 1 / SpezifischesVolumen(pPruef, tPruef + T_NULL);

        #endregion


        internal Density()
        {

        }
    }
}
