namespace HeatMeterCalculationManager.Contract
{
    public interface ICalculation
    {
        #region Properties

        double FQ_Mid { get; set; }
        double FErr { get; set; }//multiple values: dictionary<string,double>/List<string,double> ? (Vol_Err, E_Err)
        double FMaxErr { get; set; }//multiple values: dictionary<string,double>/List<string,double> ? (Vol_Err_Max, E_Err_Max)
        double FEn_Ref { get; set; }
        double FK_Ref { get; set; }
        bool Vol_Err_Flag { get; set; }//NEW Property --> (Math.Abs(fErr) > fMaxErr)
        bool E_Err_Flag { get; set; }//NEW Property --> (Math.Abs(fErr) > fMaxErr)
        int[] TempRef { get; set; }//NEW Property --> SetTempRef(int nIx, int nKombi)
        double FVolumenGes { get; set; }
        string Str { get; set; }
        double FDichteRef { get; set; }// --> = 1 / SpezifischesVolumen(fDruckBankAus, fTempWaage + T_NULL);
        double FDichteMut { get; set; }// --> = 1 / SpezifischesVolumen(pPruef, tPruef + T_NULL);
        double FQ_Ges { get; set; }// --> = fMasseWaage / fDichteRef / fTimeRef * 3600;
        double FTDiff_Ist { get; set; }
        double FTDiff_Ref { get; set; }
        double FQ { get; set; }
        double FQ_Mut { get; set; }
        double FVolumenMid { get; set; }

        #endregion
    }
}
