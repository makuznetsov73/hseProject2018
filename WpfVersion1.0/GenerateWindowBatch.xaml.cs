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
    /// Логика взаимодействия для GenerateWindowBatch.xaml
    /// </summary>
    public partial class GenerateWindowBatch : Window
    {
        MainWindow Window { get; set; }
        public Button[] modeBtns;
        public int batchSize = 0;
        public int capacity = 0;
        public int types = 0;

        public GenerateWindowBatch(MainWindow window)
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
                int n;
                if (int.TryParse(sizeTB.Text, out n) && n > 0)
                {
                    List<KnapsackAlgorithmInput> inputs = new List<KnapsackAlgorithmInput>();
                    for (int i = 0; i < n; i++)
                    {
                        inputs.Add(KnapsackAlgorithmInput.GenerateFullRandomInput());
                    }
                    Window.inputs = new KnapsackAlgorithmInputs(inputs);
                    Window.input = null;
                    Close();
                }
                else
                {
                    MessageBox.Show("Something with your requirements, true again", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (capBtn.Background == Brushes.Purple)
            {
                int cap, size;
                if (int.TryParse(capTB.Text, out cap) && cap > 1 
                    && int.TryParse(sizeTB.Text, out size) && size > 0)
                {
                    List<KnapsackAlgorithmInput> inputs = new List<KnapsackAlgorithmInput>();
                    for (int i = 0; i < cap; i++)
                    {
                        inputs.Add(KnapsackAlgorithmInput.GenerateInputCapacity(cap));
                    }
                    Window.inputs = new KnapsackAlgorithmInputs(inputs);
                    Window.input = null;
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
                int types, size;
                if (int.TryParse(typeTB.Text, out types) && types > 1
                    && int.TryParse(sizeTB.Text, out size) && size > 0)
                {
                    List<KnapsackAlgorithmInput> inputs = new List<KnapsackAlgorithmInput>();
                    for (int i = 0; i < types; i++)
                    {
                        inputs.Add(KnapsackAlgorithmInput.GenerateInput(rand.Next(1, 100000), rand.Next(1000, 50000), types));
                    }
                    Window.inputs = new KnapsackAlgorithmInputs(inputs);
                    Window.input = null;
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

        private void SizeTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            int n;
            if (int.TryParse(sizeTB.Text, out n) && n > 0)
            {
                okBtn.IsEnabled = true;
            }
            else
            {
                sizeTB.Text = "";
            }
        }

        private void GenerateWindowBatch_Closed(object sender, EventArgs e)
        {
            Window.IsEnabled = true;
        }
    }
}
