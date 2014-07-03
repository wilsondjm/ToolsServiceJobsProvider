using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDService.Model
{
    public class JobReport
    {
        public string JobName { set; get; }
        public bool Completed { set; get; }
        public string Report { set; get; }
        public int Offset { set; get; }
    }
}
