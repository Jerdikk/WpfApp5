using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public double width12;
        public double width22;
        public GridLength width14;
        public double width24;

        public class BaseViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public class TestData : BaseViewModel
        {
            private int _ID;
            public int ID
            {
                get
                {
                    return _ID;
                }
                set
                {
                    _ID = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
            private string _Name;
            public string Name
            {
                get
                {
                    return _Name;
                }
                set
                {
                    if (_Name != value)
                    {
                        _Name = value;
                    }
                    OnPropertyChanged(nameof(Name));
                }
            }

            public TestData(int iD, string name)
            {
                ID = iD;
                Name = name;
            }
        }

        //  public class TestDriveDataGrid : BaseViewModel
        //  {
        public ObservableCollection<TestData> testData { get; set; }
        //  }

        //public TestDriveDataGrid tgf { get;set; }
        public Window2()
        {
            //TestDriveDataGrid tgf = new TestDriveDataGrid();
            testData = new ObservableCollection<TestData>();
            InitializeComponent();
            dgTable1.ItemsSource = testData;
        }

        private void dgTable1_Loaded(object sender, RoutedEventArgs e)
        {
            testData.Add(new TestData(1, "1111"));
            width12 = grid2.ActualWidth;
            width22 = grid2.Width;
            width14 = mainGrod.ColumnDefinitions[0].Width;
            width24 = mainGrod.ColumnDefinitions[0].ActualWidth;
            // dgTable1.ItemsSource = dataGrid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            testData.Add(new TestData(2, "2111"));
        }

        private void GridSplitter_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            double w1 = grid2.ActualWidth;
            double w2 = grid2.Width;
        }

        private void GridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            double w1 = grid2.ActualWidth;
            double w2 = grid2.Width;
            double w3 = mainGrod.ColumnDefinitions[0].ActualWidth;
            but.Width = w3;
        }
    }
}

