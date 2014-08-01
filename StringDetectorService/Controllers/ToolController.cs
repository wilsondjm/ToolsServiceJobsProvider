using SDService.Model;
using SDService.Model.Utils;
using ServiceLayer;
using StringDetectorService.ReqResModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StringDetectorService.Controllers
{
     [RoutePrefix("api/tools")]
    public class ToolController : ApiController
    {
         ToolService toolService;
         public ToolController()
        {
            toolService = new ToolService();
        }

          [Route("")]
         [HttpGet]
         public IEnumerable<Tool> GetAllTools()
         {
             Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
             IEnumerable<Tool> tools = toolService.GetAllTools(fields);
             return tools;
         }


         [Route("{toolName}")]
         [HttpGet]
         public Tool getTool(string toolName)
         {
             Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
             Tool tool = toolService.GetTool(toolName, fields);

             return tool;
         }
        
    }
}
