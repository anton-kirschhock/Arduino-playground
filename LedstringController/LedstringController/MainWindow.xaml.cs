using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.IO.Ports;
using System.Reactive.Linq;

namespace LedstringController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private SerialPort port;
        private string _colour = "#00FF0000";
        
        private bool _hasComSelected = false;
        private List<string> _availableComs = new List<string>();
        private string _selectedCom = null;

        public bool HasComSelected
        {
            get => _hasComSelected;
            set
            {
                _hasComSelected = value;
                InvokePropertyChanged();
            }
        }
        public string Colour
        {
            get => _colour;
            set
            {
                _colour = value;
                InvokePropertyChanged();
                SendColour();
            }
        }

        public List<string> AvailableComs
        {
            get => _availableComs;
        }

        public string SelectedCom
        {
            get => _selectedCom;
            set
            {
                _selectedCom = value;
                InvokePropertyChanged();
                if (_selectedCom != null)
                {
                    ConnectCom();
                }
            }
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Closing += MainWindow_Closing;
            LoadPorts();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            CloseCom();
        }

        private void InvokePropertyChanged([CallerMemberName] string sender=null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(sender));
        }
        

        private void LoadPorts()
        {
            _availableComs = new List<string>(SerialPort.GetPortNames());
            InvokePropertyChanged(nameof(AvailableComs));
        }

        private void CloseCom()
        {
            port?.Close();
            port?.Dispose();
        }
        private void ConnectCom()
        {
            CloseCom();
            port = new SerialPort(SelectedCom, 9600);
            port.Open();

            Dispatcher.Invoke(async () => {
                await Task.Delay(1500);
                port.WriteLine("00FF00");
                HasComSelected = true;
            });
        }

        public void SendColour()
        {
            if (!port?.IsOpen ?? false) {
                ConnectCom();
            }

            port?.WriteLine(Colour.Substring(3));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendColour();
        }
    }
}
