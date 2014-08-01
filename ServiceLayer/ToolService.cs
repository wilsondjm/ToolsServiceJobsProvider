using RequestClient;
using SDService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ToolService
    {
        // tool service use some clients 
        ToolClient toolClient;

        public ToolService(){
            // init these clients
            toolClient = new ToolClient();
        }


        public IEnumerable<Tool> GetAllTools(Collection<string> fields)
        {
            IEnumerable<Tool> tools = toolClient.QueryAllTools();


            return null;
        }

        public Tool GetTool(string Tool, Collection<string> fields)
        {
            return null;
        }
    }
}
