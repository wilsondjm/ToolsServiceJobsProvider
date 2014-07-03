using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDService.Model.Basic
{
    public class JobHistory
    {
        public string jobName { set; get; }
        public IEnumerable<HistoryItem> jobHistories { set; get; }
        public HistoryItem lastBuild { set; get; }
        public string colorStatus { set; get; }
    }

    public class HistoryItem
    {
        public string duration { set; get; }
        public string fullDisplayName { set; get; }
        public string id { set; get; }
        public string number { set; get; }
        public string result { set; get; }
    }
}
