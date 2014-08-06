using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDService.Model
{
    public class Tool:View
    {
        //public Tool():base()
        //{
        //    Prefix = "Tool-";
        //}

        public string ToolName { get; set; }
        public static const string Prefix ="Tool-";
    }
    public class ToolFieldProperties
    {
        public bool containsViewName;
        public bool containsToolName;
        public bool containsJobs;

        public ToolFieldProperties(ICollection<string> fields)
        {
            setToolFieldProperties(fields);
        }
        private void setToolFieldProperties(ICollection<string> fields)
        {

            if (fields == null || fields.Contains("*"))
            {
                containsViewName = true;
                containsToolName = true;
                containsJobs = true;
                return;
            }

            foreach (string field in fields)
            {
                string effectiveField = field.ToLower();
                switch (effectiveField)
                {
                    case Constants.ViewNameField:
                        containsViewName = true;
                        break;
                    case Constants.ToolNameField:
                        containsToolName = true;
                        break;
                    case Constants.JobsField:
                        containsJobs = true;
                        break;
                }
            }
        }
    }
}
