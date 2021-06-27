using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDate_Application.Model
{
    class TimeDate
    {
        public string Time { get; set; }
        public string Date { get; set; }

        public TimeDate()
        {
                this.Time = DateTime.Now.ToShortTimeString();
                this.Date = DateTime.Now.ToShortDateString();
        }
    }
}
