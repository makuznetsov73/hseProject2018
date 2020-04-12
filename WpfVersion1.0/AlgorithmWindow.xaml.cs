using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfVersion1._0
{
    /// <summary>
    /// Логика взаимодействия для AlgorithmWindow.xaml
    /// </summary>
    public partial class AlgorithmWindow : Window
    {
        MainWindow Window { get; }

        public AlgorithmWindow(MainWindow window)
        {
            InitializeComponent();
            Window = window;
            this.Closed += AlgorithmWindow_Closed;

            window.latestSolution = window.algorithm(window.input);
            window.latestInput = new KnapsackAlgorithmInput(window.input);

            window.textField.Text += window.latestSolution;
            window.startBtn.IsEnabled = true;
            window.stopBtn.IsEnabled = false;
            window.saveInputBtn.IsEnabled = true;
            window.saveSolBtn.IsEnabled = true;
        }

        private void AlgorithmWindow_Closed(object sender, EventArgs e)
        {
            Window.startBtn.IsEnabled = true;
            Window.stopBtn.IsEnabled = false;
            Window.saveInputBtn.IsEnabled = true;
            Window.saveSolBtn.IsEnabled = true;
        }
    }
}
