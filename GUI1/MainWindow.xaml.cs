using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PortCom classport = new PortCom();
        SerialPort serialPort = new SerialPort();
        public MainWindow()
        {
            InitializeComponent();
            classport.EventSerial += Classport_EventSerial;
        }

        private void Classport_EventSerial(object sender, ClassOpcode e)
        {
            this.Dispatcher.BeginInvoke((Action)delegate
            {
                switch (e.Opcode)
                {
                    case 1:
                        // برای نمایش دیتا و مشخص شدن opcode  موردنظر
                        break;

                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:
                        break;
                    case 6:
                        break;

                }
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] port = SerialPort.GetPortNames();
            foreach (string portName in port)
            {
                comprot.Items.Add(portName);
            }
            txtATT1.Foreground = Brushes.Black;
            txtATT2.Foreground = Brushes.Black;
            txtAttStep.Foreground = Brushes.Black;
            txtAttSum.Foreground = Brushes.Black;
            txtBaudrate.Foreground = Brushes.Black;
            txtStart.Foreground = Brushes.Black;
            txtSwitch.Foreground = Brushes.Black;
            txtTimeStep.Foreground = Brushes.Black;
            //-_-_-_-_-_-_-_-_-_-_
            //txtATT1.Text = GetConfig.GetParam("txtAtt1");
            //txtATT2.Text = GetConfig.GetParam("txtAtt2");
            //txtAttStep.Text = GetConfig.GetParam("txtAttStep");
            //txtAttSum.Text = GetConfig.GetParam("txtAttSum");
            //txtSwitch.Text = GetConfig.GetParam("txtTimeStep");
            //txtTimeStep.Text = GetConfig.GetParam("txtTimeStep");
            //txtBaudrate.Text = GetConfig.GetParam("txtBaudrate");
            //txtStart.Text = GetConfig.GetParam("txtStart");

        }


        private void btnOpenPort_Click(object sender, RoutedEventArgs e)
        {
            if (btnOpenPort.Content.Equals("OpenPort"))
            {
                classport.SETTINGSERIAL(int.Parse(txtBaudrate.Text), comprot.SelectedItem.ToString());
                //serialPort.Open();
                btnOpenPort.Content = "ClosePort";
            }
            else if (btnOpenPort.Content.Equals("ClosePort"))
            {
                serialPort.Close();
                btnOpenPort.Content = "OpenPort";
            }
        }

        private void txtBaudrate_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var txt = (TextBox)sender;
            txt.Foreground = Brushes.Red;
        }
        private void txtBaudrate_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var txt = (TextBox)sender;
            if (e.Key == Key.Enter)
            {
                GetConfig.SetParam("BaudRate", txt.Text);
                txt.Foreground = Brushes.Black;
            }
        }
    }
}
