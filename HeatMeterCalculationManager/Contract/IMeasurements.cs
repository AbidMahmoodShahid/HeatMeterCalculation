namespace HeatMeterCalculationManager.Contract
{
    public interface IMeasurements
    {
        #region Properties

        double FTVor { get; set; }
        double FTRueck { get; set; }
        double FTempBankEin { get; set; }
        double FTempBankAus { get; set; }
        double FDruckBankEin { get; set; }
        double FDruckBankAus { get; set; }
        bool BBefund { get; set; }
        double FVolumen_Ist { get; set; }
        double FTimeMut { get; set; }
        int NWaageNr { get; set; }
        int NNormalBits { get; set; }
        double FVolumen_Ref { get; set; }
        double FTemp_Ref { get; set; }
        double FTimeRef { get; set; }//multiple values: dictionary<string,double>/List<string,double> ?
        double FMasseWaage { get; set; }
        double FTempWaage { get; set; }
        double FEn_Ist { get; set; }
        double FQp { get; set; } //GetDoubleFromTable("Qp");
        int NKlasse { get; set; }//GetIntFromTable("Klasse");
        int Einbauort { get; set; }//NEW Property -->  GetIntFromTable("Einbauort")
        double DeltaTetaMin { get; set; }//NEW Property -->  GetDoubleFromTable("DeltaTetaMin")
        double FVolumenGes { get; set; }
        double FQref { get; set; }
        double FTVor_Ist { get; set; }
        double FTVor_Ref { get; set; }
        double FTRueck_Ist { get; set; }
        double FTRueck_Ref { get; set; }
        double NWaage_Nr { get; set; }

        #endregion
    }
}
