using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ZapanControls.Controls;
using ZapanControls.Controls.CalendarPicker;
using ZapanControls.Databases.MYSQL;
using ZapanControls.Databases.SQLite;
using ZapanControls.Helpers;

namespace DevZapanLibraryMulti_NETCOREAPP3_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ZapWindow
    {
        private DateTime? _debut;
        private DateTime? _fin;
        private Brush _brush;

        //public DateTime? Debut 
        //{   
        //    get => _debut; 
        //    set => Set(ref _debut, value); 
        //}

        //public DateTime? Fin
        //{
        //    get => _fin;
        //    set => Set(ref _fin, value);
        //}

        //public Brush Color
        //{
        //    get => _brush;
        //    set => Set(ref _brush, value);
        //}

        public MainWindow()
        {
            //Debut = DateTime.Today.AddDays(-6);
            //Fin = DateTime.Today;

            //Color = Brushes.Red;

            InitializeComponent();

            //MySqlDatabase db = new MySqlDatabase("192.168.2.18", "", "root", "");
            //SQLiteDatabase db = new SQLiteDatabase(@"D:\Developement\Projets\XmlTvGrabber\XmlTvGrabberWebGui\grabber.db", true, 3);

            // calendar styles
            //cbTheme.Items.Add("Oceatech");
            //cbTheme.Items.Add("Generic");
            //cbTheme.Items.Add("Aero Normal");
            //cbTheme.Items.Add("Office Black");
            //cbTheme.Items.Add("Office Blue");
            //cbTheme.Items.Add("Office Silver");
            //cbTheme.Items.Add("XP Homestead");
            //cbTheme.Items.Add("XP Metallic");
            //cbTheme.Items.Add("XP Normal");
            ////cbTheme.Items.Add("Zune");
            //cbTheme.SelectedIndex = 0;


            //zapCal.SelectedDate = new DateTime(2020, 12, 25);

            Task.Run(async () =>
            {
                while (true)
                    await LongTask();
            });

            Task.Run(async () =>
            {
                int pass = 1;
                while (true)
                {
                    await LongTaskText(pass);
                    pass++;
                }
            });

            Task.Run(async () =>
            {
                int pass = 1;
                while (true)
                {
                    await LongTaskMixed(pass);
                    pass++;
                }
            });

        }

        private async Task LongTask()
        {
            int max = 2000;
            for (int i = 0; i < max; i++)
            {
                double value = (i + 1d) / max * 100d;
                progressFlat.SetProgress(value, "Basic long task");
                progressFlat1.SetProgress(value, "Basic long task");
                await Task.Delay(1);
            }
        }

        private async Task LongTaskText(int pass)
        {
            int max = 1000;
            for (int i = 0; i < max; i++)
            {
                double value = (i + 1d) / max * 100d;
                progressGlass.SetProgress(value, $"Text long task, pass: {pass}");
                progressGlass1.SetProgress(value, $"Text long task, pass: {pass}");
                await Task.Delay(1);
            }
        }

        private async Task LongTaskMixed(int pass)
        {
            int max = 500;
            for (int i = 0; i < max; i++)
            {
                if (pass % 2 == 0)
                {
                    progressIndeterminate.SetProgress($"Mixed long task, pass: {pass}");
                    progressIndeterminate1.SetProgress($"Mixed long task, pass: {pass}");
                }
                else
                {
                    double value = (i + 1d) / max * 100d;
                    progressIndeterminate.SetProgress(value, $"Mixed long task, pass: {pass}");
                    progressIndeterminate1.SetProgress(value, $"Mixed long task, pass: {pass}");
                }
                await Task.Delay(1);
            }
        }

        private void cbTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //e.Handled = true;
            //switch (cbTheme.SelectedItem.ToString())
            //{
            //    case "Generic":
            //        zapCal.Theme = Themes.Classic.ToString();
            //        break;
            //    case "Aero Normal":
            //        zapCal.Theme = Themes.AeroNormal.ToString();
            //        break;
            //    case "XP Homestead":
            //        zapCal.Theme = Themes.LunaHomestead.ToString();
            //        break;
            //    case "XP Metallic":
            //        zapCal.Theme = Themes.LunaMetallic.ToString();
            //        break;
            //    case "XP Normal":
            //        zapCal.Theme = Themes.LunaNormal.ToString();
            //        break;
            //    case "Office Black":
            //        zapCal.Theme = Themes.OfficeBlack.ToString();
            //        break;
            //    case "Office Blue":
            //        zapCal.Theme = Themes.OfficeBlue.ToString();
            //        break;
            //    case "Office Silver":
            //        zapCal.Theme = Themes.OfficeSilver.ToString();
            //        break;
            //    case "Oceatech":
            //        zapCal.Theme = Themes.Oceatech.ToString();
            //        break;
            //    //case "Zune":
            //    //    Cld.Theme = Calendar.Themes.Zune.ToString();
            //    //    break;
            //    default:
            //        zapCal.Theme = Themes.AeroNormal.ToString();
            //        break;
            //}
        }

        private void ZapButtonFlat_Click(object sender, RoutedEventArgs e)
        {
            //Debut = Debut?.AddDays(1);

            //if (Color == Brushes.Red)
            //{
            //    Color = Brushes.Lime;
            //}
            //else if (Color == Brushes.Lime)
            //{
            //    Color = null;
            //}
            //else
            //{
            //    Color = Brushes.Red;
            //}
        }

        private void ZapTabControl_TabAdd(object sender, ZapanControls.Controls.ControlEventArgs.TabAddEventArgs e)
        {
            //if (e.NewTabItem is ZapTabItem tab)
            //{
            //    tab.Header = $"Test {tabControl.Items.Count}";
            //    tab.Content = new TextBlock { Text = $"Content tab {tabControl.Items.Count}" };
            //}
        }

        private void ListView_ItemDoubleClick(object sender, ZapanControls.Controls.ControlEventArgs.ListViewItemDoubleClickEventArgs e)
        {
            var item = e.Item;
        }

        private void ZapButton_Click(object sender, RoutedEventArgs e)
        {
            //VisualTreeHelpers.FindVisualChildren<ZapanControls.Controls.CalendarPicker.Calendar>(this)
            //    .ToList()
            //    .ForEach(c => c.ReloadThemes());
        }
    }
}
