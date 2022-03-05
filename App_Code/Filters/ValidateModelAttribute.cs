using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;



    class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Common cm = new Common();
            if (actionContext.ModelState.IsValid == false)
            {
                var modelstate = actionContext.ModelState;
                var errors = modelstate.Keys.SelectMany(k => modelstate[k].Errors).Select(m => m.ErrorMessage).ToArray();
                //var errors = modelstate.Where(s => s.Value.Errors.Count > 0).Select(s => new KeyValuePair<string, string>(s.Key, s.Value.Errors.First().ErrorMessage)).ToArray();
                //var matches = from val in errors select val.Value;
                //string match1 = string.Empty;
                //foreach (var match in matches)
                //{
                //    match1 += match;
                //    match1 += ";";
                //}
                //actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, cm.StatusTime(false, match1.ToString()));
                string match1 = string.Empty;
                foreach (var match in errors)
                {
                    match1 += match;
                    match1 += ";";
                }
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);//, cm.StatusTime(false, match1.ToString()));

                actionContext.Response.Content = new StringContent(cm.StatusTime(false, match1.ToString()), Encoding.Unicode);
            }
            else if (actionContext.ActionArguments.ContainsValue(null))
            {
                // actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, cm.StatusTime(false,"Request body cannot be empty"));
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);//, cm.StatusTime(false, match1.ToString()));

                actionContext.Response.Content = new StringContent(cm.StatusTime(false, "Request body cannot be empty"), Encoding.Unicode);
            }
        }
    }

