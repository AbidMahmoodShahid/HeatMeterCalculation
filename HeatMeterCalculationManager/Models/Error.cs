using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterCalculationManager.Models
{
    internal class Error
    {

        #region Properties

        //Output
        public double FErr { get; set; }//multiple values: dictionary<string,double>/List<string,double> ? (Vol_Err, E_Err)

        public double FMaxErr { get; set; }//multiple values: dictionary<string,double>/List<string,double> ? (Vol_Err_Max, E_Err_Max)

        public bool Vol_Err_Flag { get; set; }//NEW Property --> (Math.Abs(fErr) > fMaxErr)

        public bool E_Err_Flag { get; set; }//NEW Property --> (Math.Abs(fErr) > fMaxErr)

        #endregion

        internal Error()
        {

        }
    }
}
