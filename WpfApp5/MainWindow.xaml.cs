using Prism.Commands;
using Prism.Mvvm;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public class MyMathModel : Prism.Mvvm.BindableBase
    {



        private readonly ObservableCollection<int> _myValues = new ObservableCollection<int>();
        public readonly ReadOnlyObservableCollection<int> MyPublicValues;
        public MyMathModel()
        {
            MyPublicValues = new ReadOnlyObservableCollection<int>(_myValues);
        }
        //добавление в коллекцию числа и уведомление об изменении суммы
        public void AddValue(int value)
        {
            _myValues.Add(value);
            RaisePropertyChanged("Sum");
        }
        //проверка на валидность, удаление из коллекции и уведомление об изменении суммы
        public void RemoveValue(int index)
        {
            //проверка на валидность удаления из коллекции - обязанность модели
            if (index >= 0 && index < _myValues.Count) _myValues.RemoveAt(index);
            RaisePropertyChanged("Sum");
        }
        public int Sum => MyPublicValues.Sum(); //сумма
    }


    public class MainVM : BindableBase
    {
        private void GetColorId()
        {
            ColorCollection.Add(new ColorItem( Colors.Black, nameof(Colors.Black)));
            ColorCollection.Add(new ColorItem( Colors.Yellow, nameof(Colors.Yellow)));
            ColorCollection.Add(new ColorItem( Colors.Green, nameof(Colors.Green)));
            ColorCollection.Add(new ColorItem( Colors.Gray, nameof(Colors.Gray)));
            ColorCollection.Add(new ColorItem( Colors.PaleGreen, nameof(Colors.PaleGreen)));
            ColorCollection.Add(new ColorItem( Colors.Violet, nameof(Colors.Violet)));
            ColorCollection.Add(new ColorItem( Colors.CadetBlue, nameof(Colors.CadetBlue)));
        }
        public ObservableCollection<ColorItem> ColorCollection
        { get; set; }

        public ColorItem selectedItem { get; set; }

        readonly MyMathModel _model = new MyMathModel();
        public MainVM()
        {
            ColorCollection = new ObservableCollection<ColorItem>();            
            GetColorId();

            //таким нехитрым способом мы пробрасываем изменившиеся свойства модели во View
            _model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            AddCommand = new DelegateCommand<string>(str => {
                //проверка на валидность ввода - обязанность VM
                int ival;
                if (int.TryParse(str, out ival)) _model.AddValue(ival);
            });
            RemoveCommand = new DelegateCommand<int?>(i => {
                if (i.HasValue) _model.RemoveValue(i.Value);
            });
        }
        public DelegateCommand<string> AddCommand { get; }
        public DelegateCommand<int?> RemoveCommand { get; }
        public int Sum => _model.Sum;
        public ReadOnlyObservableCollection<int> MyValues => _model.MyPublicValues;
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class ColorItem : BaseViewModel
    {
        public ColorItem( Color name, string textName)
        {
            
            Name = name;
            TextName = textName;
        }

        private Color name;
        public Color Name

        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string textName;
        public string TextName
        {
            get
            {
                return textName;
            }
            set
            {
                textName = value;
                OnPropertyChanged(nameof(TextName));
            }
        }
    }

    public partial class MainWindow : Window
    {
        public MainVM ssmain;

        public MainWindow()
        {
            InitializeComponent();
            ssmain = new MainVM();
            DataContext = ssmain;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 w = new Window1();
            w.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Define parameters used to create the BitmapSource.
            PixelFormat pf = PixelFormats.Bgr32;
            int width = 200;
            int height = 200;
            int rawStride = (width * pf.BitsPerPixel + 7) / 8;
            byte[] navBarRawImage = new byte[rawStride * height];

            // Initialize the image with data.
            /*Random value = new Random();
            value.NextBytes(rawImage);*/

            for (int y = 0; y < height; y++)
                for(int x = 0; x < width; x++)
                {
                    navBarRawImage[y * width * 4 + 4 * x] = 0;//(byte)(( y) % 255);  blue
                    navBarRawImage[y * width * 4 + 4 * x + 1] = 0;//(byte)((x * y) % 255);  green
                    navBarRawImage[y * width * 4 + 4 * x + 2] = 0 ;//(byte)((x ) % 255);  red
                    navBarRawImage[y * width*4 + 4 * x+3] = (byte)((x*y)%255);
                }

            // Create a BitmapSource.
            BitmapSource bitmap = BitmapSource.Create(width, height,
                96, 96, pf, null,
                navBarRawImage, rawStride);

            imager1.Source = bitmap;

            // Create an image element;
            /*Image myImage = new Image();
            myImage.Width = 200;
            // Set image source.
            myImage.Source = bitmap;*/
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ColorItem tt = ((WpfApp5.ColorItem)((object[])e.AddedItems)[0]);
            ColorItem tt1 = (sender as ListBox).SelectedItem as ColorItem;
            ColorItem tt2 = ssmain.selectedItem;
        }

        private void comboBox1_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {            
            string[] pos = (sender as ListBox).SelectedItem.ToString().Split(' ');            
            
            var color1 = (Color)ColorConverter.ConvertFromString(pos[1]);

            //Color ssss = Color.
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window2 w2 = new Window2();
            w2.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Window3 window3 = new Window3();
            window3.Owner = this;
            window3.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window3.ShowDialog();
        }
    }
}
