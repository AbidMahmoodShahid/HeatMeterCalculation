using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterCalculationManager.Models
{
    internal class HeatEnergy
    {
        #region Properties

        //Input
        public double FEn_Ist { get; set; }

        //Output
        public double FEn_Ref { get; set; }

        #endregion

        internal HeatEnergy()
        {

        }
    }
}
