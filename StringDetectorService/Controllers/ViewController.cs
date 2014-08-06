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
using System.Web.Http.Cors;

namespace StringDetectorService.Controllers
{
     [RoutePrefix("api")]
     [EnableCors(origins:"*",headers: "*",methods:"*")]
    public class ViewController : ApiController
    {
         ViewService viewService;
         public ViewController()
        {
            viewService = new ViewService();
        }


         [Route("views")]
         [HttpGet]
         public IEnumerable<ViewDto> GetAllViews()
         {
             Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
             IEnumerable<View> views = viewService.GetAllViews(fields, Constants.DefaultTag);
             return views.Select(tool => tool.ToViewDto());
         }


         [Route("views/{viewName}")]
         [HttpGet]
         public ViewDto getView(string viewName)
         {
             Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
             View tool = viewService.GetView(viewName, fields, Constants.DefaultTag);
             ViewDto responseData = tool.ToViewDto();
             return responseData;
         }

          [Route("tools")]
         [HttpGet]
         public IEnumerable<ViewDto> GetAllTools()
         {
             Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
             IEnumerable<View> views = viewService.GetAllViews(fields,Constants.ToolTag);
             return views.Select(tool=>tool.ToViewDto());
         }


         [Route("tools/{toolName}")]
         [HttpGet]
         public ViewDto getTool(string toolName)
         {
             Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
             View tool = viewService.GetView(toolName, fields, Constants.ToolTag);
             ViewDto responseData = tool.ToViewDto();
             return responseData;
         }


         [Route("projects")]
         [HttpGet]
         public IEnumerable<ViewDto> GetAllProjects()
         {
             Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
             IEnumerable<View> views = viewService.GetAllViews(fields, Constants.ProjectTag);
             return views.Select(tool => tool.ToViewDto());
         }


         [Route("projects/{projectName}")]
         [HttpGet]
         public ViewDto getProject(string projectName)
         {
             Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
             View tool = viewService.GetView(projectName, fields, Constants.ProjectTag);
             ViewDto responseData = tool.ToViewDto();
             return responseData;
         }

         
    }
}
