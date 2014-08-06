using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDService.Model
{
    public class Project: View
    {
        public string ProjectName { get; set; }
        public static const string Prefix = "Project-";

        //public Project()
        //    : base()
        //{
        //    Prefix = "Project-";
        //}
    }
}
