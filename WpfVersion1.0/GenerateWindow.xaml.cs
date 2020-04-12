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
    /// Логика взаимодействия для GenerateWindow.xaml
    /// </summary>
    public partial class GenerateWindow : Window
    {
        MainWindow Window { get; set; }
        public Button[] modeBtns;
        public int capacity = 0;
        public int types = 0;
        

        public GenerateWindow(MainWindow window)
        {
            InitializeComponent();
            modeBtns = new Button[] { capBtn, typesBtn, randBtn };
            Window = window;
            Window.IsEnabled = false;
            this.Closed += GenerateWindowBatch_Closed;
            okBtn.IsEnabled = false;
            
            capBtn.Click += CapBtn_Click;
            typesBtn.Click += TypesBtn_Click;
            randBtn.Click += RandBtn_Click;

            okBtn.Click += OkBtn_Click;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (randBtn.Background == Brushes.Purple)
            {
                Window.input = KnapsackAlgorithmInput.GenerateFullRandomInput();
                Window.inputs = null;
                Close();
            }
            else if (capBtn.Background == Brushes.Purple)
            {
                int cap;
                if (int.TryParse(capTB.Text, out cap) && cap > 1)
                {
                    Window.input = KnapsackAlgorithmInput.GenerateInputCapacity(cap);
                    Window.inputs = null;
                    Close();
                }
                else
                {
                    MessageBox.Show("Something with your requirements, true again", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (typesBtn.Background == Brushes.Purple)
            {
                Random rand = new Random();
                int types;
                if (int.TryParse(typeTB.Text, out types) && types > 1)
                {
                    Window.input = KnapsackAlgorithmInput.GenerateInput(rand.Next(1, 100000), rand.Next(1000, 50000), types);
                    Window.inputs = null;
                    Close();
                }
                else
                {
                    MessageBox.Show("Something with your requirements, true again", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RandBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in modeBtns)
            {
                btn.Background = Brushes.MediumPurple;
            }
            randBtn.Background = Brushes.Purple;
            okBtn.IsEnabled = true;
        }

        private void TypesBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in modeBtns)
            {
                btn.Background = Brushes.MediumPurple;
            }
            typesBtn.Background = Brushes.Purple;
            okBtn.IsEnabled = true;
        }

        private void CapBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in modeBtns)
            {
                btn.Background = Brushes.MediumPurple;
            }
            capBtn.Background = Brushes.Purple;
            okBtn.IsEnabled = true;
        }

        private void TypeTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            int n;
            if (int.TryParse(typeTB.Text, out n) && n > 0)
            {
                okBtn.IsEnabled = true;
                types = n;
                capacity = 0;
            }
            else
            {
                typeTB.Text = "";
            }
        }

        private void CapTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            int n;
            if (int.TryParse(capTB.Text, out n) && n > 0)
            {
                okBtn.IsEnabled = true;
                types = 0;
                capacity = n;
            }
            else
            {
                capTB.Text = "";
            }
        }

        private void GenerateWindowBatch_Closed(object sender, EventArgs e)
        {
            Window.IsEnabled = true;
        }
    }
}
