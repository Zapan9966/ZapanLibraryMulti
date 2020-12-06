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
using ZapanControls.Databases.MYSQL;
using ZapanControls.Databases.SQLite;

namespace DevZapanLibraryMulti_NET472
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //MySqlDatabase db = new MySqlDatabase("192.168.2.18", "", "root", "");
            //SQLiteDatabase db = new SQLiteDatabase(@"D:\Developement\Projets\XmlTvGrabber\XmlTvGrabberWebGui\grabber.db", true, 3);
        }
    }
}
