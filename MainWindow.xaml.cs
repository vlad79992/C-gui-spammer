using System;
using System.Linq;
using System.Net.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Demo;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private bool isWorking = false;



        public MainWindow()
        {
            //System.Diagnostics.Process.Start("notepad");
            InitializeComponent();
            startSpamBtn.Content = "START";
            startSpamBtn.Background = Brushes.LawnGreen;
        }

        private void waitForStart()
        {
            int timeStart = Int32.Parse(WaitBeforeStartTextBox.Text);
            if (WaitBeforeStartType.SelectedIndex == 1 )
            {
                timeStart *= 1000;
            }
            if (WaitBeforeStartType.SelectedIndex == 2)
            {
                timeStart *= (1000  * 60);
            }
            Task.Delay(timeStart).Wait();
        }

        private void generateInput()
        {
            int time = Int32.Parse(IntervalTextBox.Text);
            if (intervalType.SelectedIndex == 1)
            {
                time *= 1000;
            }
            if (intervalType.SelectedIndex == 2)
            {
                time *= (1000 * 60);
            }
            System.Timers.Timer timer = new System.Timers.Timer(time);
            timer.Enabled = false;
            //timer.AutoReset = true;
            waitForStart();
            for (int i = 0; i < Int32.Parse(IterationsTextBox.Text); i++)
            {
                Task.Delay(1000).Wait();
                    Demo.Keyboard.Type(SpamTextBox.Text);
                    Demo.Keyboard.Type(Key.Enter);
            }
        } 

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void showMessageBox()
        {

            string stats = SpamTextBox.Text + '\n';
            stats += "Iteratins = " + IterationsTextBox.Text + "\n";
            stats += "Interval = " + IntervalTextBox.Text + " " + intervalType.SelectedValue + "\n";
            stats += "Wait before start = " + WaitBeforeStartTextBox.Text + " " + WaitBeforeStartType.SelectedValue + "\n";
            string textErrors = "";
            if (!IterationsTextBox.Text.All(char.IsDigit) || !(IterationsTextBox.Text.Length > 0)
                || Int32.Parse(IterationsTextBox.Text) < 0) 
            {
                textErrors += "Iterations must be an unsigned integer\n";
                IterationsTextBox.Text = "10";
            }
            if (!IntervalTextBox.Text.All(char.IsDigit) || !(IntervalTextBox.Text.Length > 0)
                || Int32.Parse(IntervalTextBox.Text) < 0) 
            {
                textErrors += "Interval must be an unsigned integer\n";
                IntervalTextBox.Text = "1000";
            }
            if (!WaitBeforeStartTextBox.Text.All(char.IsDigit) || !(WaitBeforeStartTextBox.Text.Length > 0)
                || Int32.Parse(WaitBeforeStartTextBox.Text) < 0)
            {
                textErrors += "Time before start must be an unsigned integer\n";
                WaitBeforeStartTextBox.Text = "5000";
            }
            if (SpamTextBox.Text.Length < 2)
            {
                textErrors += "Spam text must be not empty\n";
            }
            if (textErrors.Length == 0)
            {
                MessageBoxButton btn = MessageBoxButton.OK;
                MessageBox.Show(stats, "Stats", btn,
                    MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                isWorking = false;
                MessageBoxButton btn = MessageBoxButton.OK;
                MessageBox.Show(textErrors, "ERROR", btn,
                    MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void startSpam_Click(object sender, RoutedEventArgs e)
        {
            if (isWorking)
            {
                goto START;
            }
                startSpamBtn.Background = Brushes.Red;
                startSpamBtn.Content = "STOP";
                isWorking = true;
                //showMessageBox();
                generateInput();
            //
            START:
            //
                isWorking = false;
                startSpamBtn.Content = "START";
                startSpamBtn.Background = Brushes.LawnGreen;
                isWorking=false;
                //System.Timers.Timer timer = new System.Timers.Timer(3000);
        }

        private void iterationsUp_Click(object sender, RoutedEventArgs e)
        {
            string text = IterationsTextBox.Text;
            try
            {
                int result = Int32.Parse(text);
                result += 10;
                text = result.ToString();
                IterationsTextBox.Text = text;
            }
            catch(FormatException) { }
        }

        private void iterationsDown_Click(object sender, RoutedEventArgs e)
        {
            string text = IterationsTextBox.Text;
            try
            {
                int result = Int32.Parse(text);
                result -= 10;
                if (result < 0)
                {
                    result = 0;
                }
                text = result.ToString();
                IterationsTextBox.Text = text;
            }
            catch (FormatException) { }
        }

        private void intervalDown_Click(object sender, RoutedEventArgs e)
        {
            string text = IntervalTextBox.Text;
            try
            {
                int result = Int32.Parse(text);
                result -= 10;
                if (result < 0)
                {
                    result = 0;
                }
                text = result.ToString();
                IntervalTextBox.Text = text;
            }
            catch (FormatException) { }
        }

        private void intervalUp_Click(object sender, RoutedEventArgs e)
        {
            string text = IntervalTextBox.Text;
            try
            {
                int result = Int32.Parse(text);
                result += 10;
                text = result.ToString();
                IntervalTextBox.Text = text;
            }
            catch (FormatException) { }
        }

        private void WaitUp_Click(object sender, RoutedEventArgs e)
        {
            string text = WaitBeforeStartTextBox.Text;
            try
            {
                int result = Int32.Parse(text);
                result += 100;
                text = result.ToString();
                WaitBeforeStartTextBox.Text = text;
            }
            catch (FormatException) { }
        }

        private void waitDown_Click(object sender, RoutedEventArgs e)
        {
            string text = WaitBeforeStartTextBox.Text;
            try
            {
                int result = Int32.Parse(text);
                result -= 100;
                if (result < 0)
                {
                    result = 0;
                }
                text = result.ToString();
                WaitBeforeStartTextBox.Text = text;
            }
            catch (FormatException) { }
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
