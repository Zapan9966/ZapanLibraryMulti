using System;
using System.Collections.Generic;
using System.Text;

namespace DevZapanLibraryMulti_NETCOREAPP3_1.Models
{
    public class ListViewDataModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsActrive { get; set; }
    }
}
