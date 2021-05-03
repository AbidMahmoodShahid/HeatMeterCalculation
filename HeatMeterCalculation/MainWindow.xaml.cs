using System;
using System.IO;
using System.Reflection;
using System.Windows;


namespace HeatMeterCalculation
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static string path = @"C:\VC_Projekte\HeatMeterCalculation\HeatMeterCalculationLib\bin\Debug";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string calculatorLibrary = Path.Combine(path, "HeatMeterCalculationLib.Dll");
            Assembly ass = Assembly.LoadFrom(calculatorLibrary);
            string calculatorClass = "HeatMeterCalculationLib.HeatMeterCalculationObj";
            Type t = ass.GetType(calculatorClass);
            if (t == null)
            {
                throw new ArgumentException(String.Format("Type '{0}' not found in assembly '{1}'", calculatorClass, calculatorLibrary));
            }
            MessageBox.Show("Dll geladen");
        }
    }
}
