using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterCalculationManager.Models
{
    internal class VolumeFlow
    {

        #region Properties

        //Input
        public double FQp { get; set; } //GetDoubleFromTable("Qp");

        //Output
        public double FQ_Mid { get; set; }

        public double FQ_Ges { get; set; }// --> = fMasseWaage / fDichteRef / fTimeRef * 3600;

        public double FQ { get; set; }

        public double FQ_Mut { get; set; }

        #endregion

        internal VolumeFlow()
        {

        }
    }
}
