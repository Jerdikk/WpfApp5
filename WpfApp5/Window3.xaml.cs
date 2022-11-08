using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public string currDirectory;
        public Window3()
        {
            InitializeComponent();
        }

        private void getDir_Click(object sender, RoutedEventArgs e)
        {
            trView.Items.Clear();
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Tag = drive;
                item.Header = drive.ToString();
                item.Items.Add("*");
                trView.Items.Add(item);
            }

            /*
            currDirectory = Directory.GetCurrentDirectory();
            string[] dirs =  Directory.GetDirectories(currDirectory);
            if (dirs.Length > 0)
            {
                foreach(string dir in dirs)
                {
                    string[] files = Directory.GetFiles(dir);
                    foreach(string file in files)
                    {

                    }
                }
            }
            else
            {
                string[] files = Directory.GetFiles(currDirectory);
            }
            */

        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            item.Items.Clear();
            DirectoryInfo dir;
            if (item.Tag is DriveInfo)
            {
                DriveInfo drive = (DriveInfo)item.Tag;
                dir = drive.RootDirectory;
            }
            else 
                dir = (DirectoryInfo)item.Tag;

            lbFiles.Items.Clear();

            try
            {
                FileInfo[] fileInfos = dir.GetFiles();

                
                foreach (FileInfo file in fileInfos)
                {
                    lbFiles.Items.Add(file.Name);
                }
            }

            catch { }

            try
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    TreeViewItem newItem = new TreeViewItem();
                    newItem.Tag = subDir;
                    newItem.Header = subDir.ToString();
                    newItem.Items.Add("*");
                    item.Items.Add(newItem);
                }
            }
            catch
            { }
        }

        private void trView_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;            
            DirectoryInfo dir;
            if (item.Tag is DriveInfo)
            {
                DriveInfo drive = (DriveInfo)item.Tag;
                dir = drive.RootDirectory;
            }
            else
                dir = (DirectoryInfo)item.Tag;

            lbFiles.Items.Clear();

            try
            {
                FileInfo[] fileInfos = dir.GetFiles();


                foreach (FileInfo file in fileInfos)
                {
                    lbFiles.Items.Add(file.Name);
                }
            }

            catch { }


        }
    }
}
