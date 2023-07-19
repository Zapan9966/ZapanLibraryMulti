using DevZapanLibraryMulti_NETCOREAPP3_1.Helpers;
using DevZapanLibraryMulti_NETCOREAPP3_1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DevZapanLibraryMulti_NETCOREAPP3_1.DesignModels
{
    internal class MainWindowDesignModel
    {
        public ObservableCollection<ListViewDataModel> ListViewDatas { get; set; }

        public MainWindowDesignModel()
        {
            ListViewDatas = new ObservableCollection<ListViewDataModel>(DevHelpers.GenerateListViewData());
        }
    }
}
