using DevZapanLibraryMulti_NETCOREAPP3_1.Helpers;
using DevZapanLibraryMulti_NETCOREAPP3_1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<ListViewDataModel> ListViewDatas { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            //MySqlDatabase db = new MySqlDatabase("192.168.2.18", "", "root", "");
            //SQLiteDatabase db = new SQLiteDatabase(@"D:\Developement\Projets\XmlTvGrabber\XmlTvGrabberWebGui\grabber.db", true, 3);

            Task.Run(() => 
            {
                ListViewDatas = new ObservableCollection<ListViewDataModel>(DevHelpers.GenerateListViewData());
            });

            Task.Run(async () =>
            {
                while (true)
                {
                    await LongTask();
                }
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
    }
}
