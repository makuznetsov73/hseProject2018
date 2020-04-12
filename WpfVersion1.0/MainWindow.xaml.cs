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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace WpfVersion1._0
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public Button[] algorithmsBtns;

        public Algorithm.AlgorithmDelegate algorithm;

        public int batchSize = 5;

        public KnapsackAlgorithmInput input = null;
        public KnapsackAlgorithmInputs inputs = null;
        public KnapsackSolution latestSolution;
        public KnapsackSolutions latestSolutions;
        public KnapsackAlgorithmInput latestInput;
        public KnapsackAlgorithmInputs latestInputs;

        public GenerateWindow subWindow;
        public GenerateWindowBatch subWindowBatch;
        public AlgorithmWindow algorithmWindow;

        public string strCapacity;
        public string strWeights;
        public string strPrices;

        public int manualInputCapacity;
        public int[] manualInputWeights;
        public int[] manualInputPrices;
        public int manualInputsTypes; 

        public bool capInputSwitch = true;
        public bool pricesInputSwitch = true;
        public bool weightsInputSwitch = true;

        public AboutWindow aboutWindow;

        public MainWindow()
        {
            InitializeComponent();
            KnapsackAlgorithmInput currentInput;
            algorithmsBtns = new Button[] { greedyBtn, bbBtn, ftpasBtn, dpBtn };
            greedyBtn.Click += GreedyBtn_Click;
            bbBtn.Click += BbBtn_Click;
            ftpasBtn.Click += FtpasBtn_Click;
            dpBtn.Click += DpBtn_Click;

            modeBtn.Click += ModeBtn_Click;
            generateBtn.Click += GenerateBtn_Click;

            startBtn.Click += StartBtn_Click;
            stopBtn.Click += StopBtn_Click;
            
            stopBtn.IsEnabled = false;
            modeBtn.Content = "Off";
            modeBtn.Background = Brushes.Red;

            inputCap.PreviewMouseDown += InputCap_MouseDown;
            inputPrices.PreviewMouseDown += InputPrices_MouseDown;
            inputWeights.PreviewMouseDown += InputWeights_MouseDown;

            inputCap.TextChanged += InputCap_TextChanged;
            inputPrices.TextChanged += InputPrices_TextChanged;
            inputWeights.TextChanged += InputWeights_TextChanged;

            saveSolBtn.Click += SaveSolBtn_Click;
            saveInputBtn.Click += SaveInputBtn_Click;
            loadInputBtn.Click += LoadInputBtn_Click;
            loadSolBtn.Click += LoadSolBtn_Click;

            manualBtn.Click += ManualBtn_Click;
            clearBtn.Click += ClearBtn_Click;

            aboutBtn.Click += AboutBtn_Click;
        }

        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            aboutWindow = new AboutWindow(this);
            aboutWindow.Show();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            textField.Text = "";
        }

        private void LoadSolBtn_Click(object sender, RoutedEventArgs e)
        {
            if (modeBtn.Content.ToString() == "Off")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text file (*.ksl)|*.ksl";
                if (openFileDialog.ShowDialog() == true)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        using (FileStream fs = new FileStream(openFileDialog.FileName,
                            FileMode.OpenOrCreate))
                        {
                            latestSolution = (KnapsackSolution)formatter.Deserialize(fs);
                            textField.Text += latestSolution;
                        }
                    }
                    catch(Exception)
                    {
                        MessageBox.Show("Looks like this is not a real solution file", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text file (*.ksls)|*.ksls";
                if (openFileDialog.ShowDialog() == true)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        using (FileStream fs = new FileStream(openFileDialog.FileName,
                            FileMode.OpenOrCreate))
                        {
                            latestSolutions = (KnapsackSolutions)formatter.Deserialize(fs);
                            textField.Text += latestSolutions;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Looks like this is not a real solutions file", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void LoadInputBtn_Click(object sender, RoutedEventArgs e)
        {
            if (modeBtn.Content.ToString() == "Off")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text file (*.kip)|*.kip";
                if (openFileDialog.ShowDialog() == true)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        using (FileStream fs = new FileStream(openFileDialog.FileName,
                            FileMode.OpenOrCreate))
                        {
                            input = (KnapsackAlgorithmInput)formatter.Deserialize(fs);
                            textField.Text += input;
                        }
                    }
                    catch(Exception)
                    {
                        MessageBox.Show("Looks like this is not a real input file", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text file (*.kips)|*.kips";
                if (openFileDialog.ShowDialog() == true)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        using (FileStream fs = new FileStream(openFileDialog.FileName,
                            FileMode.OpenOrCreate))
                        {
                            inputs = (KnapsackAlgorithmInputs)formatter.Deserialize(fs);
                            textField.Text += inputs;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Looks like this is not a real inputs file", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SaveInputBtn_Click(object sender, RoutedEventArgs e)
        {
            if (modeBtn.Content.ToString() == "Off")
            {
                if (input != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text file (*.kip)|*.kip";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();

                        using (FileStream fs = new FileStream(saveFileDialog.FileName, 
                            FileMode.OpenOrCreate))
                        {
                            formatter.Serialize(fs, input);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You have no input, look at your batch mode", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (inputs != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text file (*.kips)|*.kips";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();

                        using (FileStream fs = new FileStream(saveFileDialog.FileName,
                            FileMode.OpenOrCreate))
                        {
                            formatter.Serialize(fs, inputs);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You have no inputs, look at your batch mode", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveSolBtn_Click(object sender, RoutedEventArgs e)
        {
            if (modeBtn.Content.ToString() == "Off")
            {
                if (latestSolution != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text file (*.ksl)|*.ksl";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();

                        using (FileStream fs = new FileStream(saveFileDialog.FileName,
                            FileMode.OpenOrCreate))
                        {
                            formatter.Serialize(fs, latestSolution);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You have no solution, look at your batch mode", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (latestSolutions != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text file (*.ksls)|*.ksls";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();

                        using (FileStream fs = new FileStream(saveFileDialog.FileName,
                            FileMode.OpenOrCreate))
                        {
                            formatter.Serialize(fs, latestSolutions);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You have no solutions, look at your batch mode", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ManualBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(strCapacity.Trim(), out manualInputCapacity) && manualInputCapacity > 1)
                {

                    try
                    {
                        manualInputWeights = strWeights.Trim().Split(' ').
                            Where(x => !string.IsNullOrWhiteSpace(x)).
                            Select(x => int.Parse(x)).ToArray();

                        manualInputPrices = strPrices.Trim().Split(' ').
                            Where(x => !string.IsNullOrWhiteSpace(x)).
                            Select(x => int.Parse(x)).ToArray();

                        if (manualInputPrices.Length != manualInputWeights.Length)
                        {
                            throw new Exception();
                        }
                        else
                        {

                            if (algorithm != null)
                            {
                                textField.Text += input;
                                startBtn.IsEnabled = false;
                                stopBtn.IsEnabled = true;
                                saveInputBtn.IsEnabled = false;
                                saveSolBtn.IsEnabled = false;

                                input = new KnapsackAlgorithmInput(manualInputPrices.Length,
                                    manualInputPrices, manualInputWeights, manualInputCapacity, 0);

                                latestSolution = algorithm(input);
                                //textField.VerticalOffset = textField.; 
                                latestInput = new KnapsackAlgorithmInput(input);

                                textField.Text += latestSolution;
                                startBtn.IsEnabled = true;
                                stopBtn.IsEnabled = false;
                                saveInputBtn.IsEnabled = true;
                                saveSolBtn.IsEnabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Your haven't chosen an algorithm!", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something wrong with your weights and prices",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                    }
                }
                else
                {
                    MessageBox.Show("Something wrong with your capacity, remember it should be more than 1",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong with your manual input",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InputWeights_TextChanged(object sender, TextChangedEventArgs e)
        {
            strWeights = inputWeights.Text;
        }

        private void InputPrices_TextChanged(object sender, TextChangedEventArgs e)
        {
            strPrices = inputPrices.Text;
        }

        private void InputCap_TextChanged(object sender, TextChangedEventArgs e)
        {
            strCapacity = inputCap.Text;
        }

        private void InputWeights_MouseDown(object sender, MouseButtonEventArgs e)
        {
            inputWeights.Text = "";
            capInputSwitch = false;
        }

        private void InputPrices_MouseDown(object sender, MouseButtonEventArgs e)
        {
            inputPrices.Text = "";
            capInputSwitch = false;
        }

        private void InputCap_MouseDown(object sender, MouseButtonEventArgs e)
        {
            inputCap.Text = "";
            capInputSwitch = false;
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            algorithmWindow.Close();
            latestSolution = null;
        }

        public void SolveInput ()
        {
            latestSolution = algorithm(input);
            latestInput = new KnapsackAlgorithmInput(input);
        }

        private async void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (input == null && inputs == null)
            {
                MessageBox.Show("Your have no input!", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (input != null)
                {
                    if (algorithm != null)
                    {
                        textField.Text += input;
                        startBtn.IsEnabled = false;
                        stopBtn.IsEnabled = true;
                        saveInputBtn.IsEnabled = false;
                        saveSolBtn.IsEnabled = false;

                        latestSolution = algorithm(input);
                        latestInput = new KnapsackAlgorithmInput(input);

                        textField.Text += latestSolution;
                        startBtn.IsEnabled = true;
                        stopBtn.IsEnabled = false;
                        saveInputBtn.IsEnabled = true;
                        saveSolBtn.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Your haven't chosen an algorithm!", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (inputs != null)
                {
                    if (algorithm != null)
                    {
                        textField.Text += inputs;
                        startBtn.IsEnabled = false;
                        stopBtn.IsEnabled = true;
                        saveInputBtn.IsEnabled = false;
                        saveSolBtn.IsEnabled = false;

                        latestSolutions = Algorithm.BigTest(inputs.Inputs, algorithm);
                        latestInputs = new KnapsackAlgorithmInputs(inputs);

                        textField.Text += latestSolutions;
                        startBtn.IsEnabled = true;
                        stopBtn.IsEnabled = false;
                        saveInputBtn.IsEnabled = true;
                        saveSolBtn.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Your haven't chosen an algorithm!", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (modeBtn.Content == "Off")
            {
                subWindow = new GenerateWindow(this);
                subWindow.Show();
            }
            else
            {
                subWindowBatch = new GenerateWindowBatch(this);
                subWindowBatch.Show();
            }
        }

        private void ModeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (modeBtn.Content == "Off")
            {
                modeBtn.Content = "On";
                modeBtn.Background = Brushes.Green;
                loadInputBtn.Content = "Load inputs";
                generateBtn.Content = "Generate inputs";
                loadSolBtn.Content = "Load solutions";
                saveSolBtn.Content = "Save solutions";
                saveInputBtn.Content = "Save inputs";
            }
            else
            {
                modeBtn.Content = "Off";
                modeBtn.Background = Brushes.Red;
                loadInputBtn.Content = "Load input";
                generateBtn.Content = "Generate input";
                loadSolBtn.Content = "Load solution";
                saveSolBtn.Content = "Save solution";
                saveInputBtn.Content = "Save input";
            }
        }

        private void DpBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in algorithmsBtns)
            {
                btn.Background = Brushes.MediumPurple;
            }
            dpBtn.Background = Brushes.Purple;
            algorithm = Algorithm.DynamicProgrammingAlgorithm;
        }

        private void FtpasBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in algorithmsBtns)
            {
                btn.Background = Brushes.MediumPurple;
            }
            ftpasBtn.Background = Brushes.Purple;
            algorithm = Algorithm.FptasAlgorithm;
        }

        private void BbBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in algorithmsBtns)
            {
                btn.Background = Brushes.MediumPurple;
            }
            bbBtn.Background = Brushes.Purple;
            algorithm = Algorithm.BranchBoundAlgorithm;
        }

        private void GreedyBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach(Button btn in algorithmsBtns)
            {
                btn.Background = Brushes.MediumPurple;
            }
            greedyBtn.Background = Brushes.Purple;
            algorithm = Algorithm.GreedyAlgorithm;
        }

    }
}
