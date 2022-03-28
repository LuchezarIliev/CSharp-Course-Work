/*
 * LogDataAnalyzer
 * Author: Lachezar Iliev
 * Faculty №: 121217151
 * Group: 37
 */
using System;
using System.Windows;

namespace LogDataAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string ExcelExtension = ".xlsx";
        private const string Criteria = "Wiki page updated";

        public bool Testing = false;

        public MainWindow()
        {
            InitializeComponent();
            DataAnalyzer.SetCriteria(Criteria);
        }

        public string ProcessData(string inputFilePath, Operation operationType, string operationName)
        {
            string result;
            if (inputFilePath.Equals(string.Empty) || !inputFilePath.EndsWith(ExcelExtension))
            {
                result = $"Трабва да изберете валиден Excel файл ({ExcelExtension}) за алгоритъма!";
                if (!Testing)
                    MessageBox.Show(result);
                return result;
            }

            try
            {
                DataAnalyzer.CreateReader(inputFilePath);

                result = DataAnalyzer.Analyze(operationType);
                if (!Testing)
                    MessageBox.Show($"\t{operationName} на данните\n{result}");
                return result;
            }
            catch (Exception ex)
            {
                result = $"Възникна грешка:\n{ex.Message}";
                if (!Testing)
                    MessageBox.Show(result);
                return result;
            }
        }

        private void StartAlgorithm_Click(object sender, RoutedEventArgs e)
        {
            var inputFilePath = inputFileTextBox.Text;
            string operationName;
            Operation operationType;
            if (absoluteCheckBox.IsChecked != null && (bool)absoluteCheckBox.IsChecked)
            {
                operationType = Operation.Absolute;
                operationName = absoluteCheckBox.Content.ToString();
            }
            else if (relativeCheckBox.IsChecked != null && (bool)relativeCheckBox.IsChecked)
            {
                operationType = Operation.Relative;
                operationName = relativeCheckBox.Content.ToString();
            }
            else if (medianCheckBox.IsChecked != null && (bool)medianCheckBox.IsChecked)
            {
                operationType = Operation.Median;
                operationName = medianCheckBox.Content.ToString();
            }
            else if (dispersionCheckBox.IsChecked != null && (bool)dispersionCheckBox.IsChecked)
            {
                operationType = Operation.Dispersion;
                operationName = dispersionCheckBox.Content.ToString();
            }
            else
            {
                operationType = Operation.None;
                operationName = "";
            }
            ProcessData(inputFilePath, operationType, operationName);
        }

        private void ReadInputFile_Click(object sender, RoutedEventArgs e)
        {
            inputFileTextBox.Text = GetFileNameDialog($"Excel files (*{ExcelExtension}) | *{ExcelExtension};");
        }

        private string GetFileNameDialog(string filter)
        {
            // Create OpenFileDialog 
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                // Set filter for file extension and default file extension 
                //dlg.DefaultExt = ".png";
                Filter = filter
            };

            // Display OpenFileDialog by calling ShowDialog method 
            var result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            return result == true ? dlg.FileName : "";
        }
    }
}
