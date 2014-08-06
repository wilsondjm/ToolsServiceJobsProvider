using SDService.Model.Utils;
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
        public string Name { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }

    public class ViewFieldProperties
    {
        public bool containsViewName;
        public bool containsName;
        public bool containsJobs;

        public ViewFieldProperties(ICollection<string> fields)
        {
            setViewFieldProperties(fields);
        }
        private void setViewFieldProperties(ICollection<string> fields)
        {

            if (fields == null || fields.Contains("*"))
            {
                containsViewName = true;
                containsName = true;
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
                    case Constants.NameField:
                        containsName = true;
                        break;
                    case Constants.JobsField:
                        containsJobs = true;
                        break;
                }
            }
        }
    }
}
