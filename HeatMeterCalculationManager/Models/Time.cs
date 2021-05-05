using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterCalculationManager.Models
{
    internal class Time
    {
        #region Properties

        //Input
        public double FTimeMut { get; set; }

        public double FTimeRef { get; set; }//multiple values: dictionary<string,double>/List<string,double> ?

        #endregion

        internal Time()
        {

        }
    }
}
