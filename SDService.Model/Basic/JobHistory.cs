using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDService.Model.Basic
{
    public class JobHistory
    {
        public string JobName { set; get; }
        public IEnumerable<HistoryItem> JobHistories { set; get; }
        public HistoryItem LastBuild { set; get; }
      //  public string colorStatus { set; get; }
    }

    public class HistoryItem
    {
        public string Duration { set; get; }
        public string FullDisplayName { set; get; }
        public string Id { set; get; }
        public string Number { set; get; }
        public string Result { set; get; }
    }
}
