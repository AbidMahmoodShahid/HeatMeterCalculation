using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterCalculationManager.Models
{
    internal class Volume
    {
        #region Properties

        //Input
        public double FVolumen_Ist { get; set; }

        public double FVolumen_Ref { get; set; }

        public double FQref { get; set; }

        //Input + Output
        public double FVolumenGes { get; set; }

        //Output
        public double FVolumenMid { get; set; }


        #endregion

        internal Volume()
        {

        }
    }
}
