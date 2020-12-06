using System;
using System.Collections.Generic;
using System.Linq;

namespace ZapanControls.Controls.Calendar.Common
{
    public static class Filters
    {
        public static IEnumerable<Appointment> ByDate(this IEnumerable<Appointment> appointments, DateTime date)
        {
            if (appointments == null)
                return null;

            var app = from a in appointments
                      where a.StartTime.Date == date.Date
                      select a;
            return app;
        }
    }
}
