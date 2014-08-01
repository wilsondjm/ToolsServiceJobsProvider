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
        public ICollection<Job> Jobs { get; set; }
    }
}
