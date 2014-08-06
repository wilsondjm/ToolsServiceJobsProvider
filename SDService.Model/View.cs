using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDService.Model
{
    public class View
    {
        public string ViewName { get; set; }
        //protected bool hasSet;
        //protected string prefix;
        //public string Prefix
        //{
        //    get
        //    {
        //        if (!hasSet) throw new InvalidOperationException("value not set");
        //        return prefix;
        //    }
        //    set
        //    {
        //        if (hasSet) throw new InvalidOperationException("value already set");
        //        this.prefix = value;
        //        this.hasSet = true;
        //    }
        //}
        public ICollection<Job> Jobs { get; set; }
    }
}
