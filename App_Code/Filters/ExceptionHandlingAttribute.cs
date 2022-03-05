
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Filters;


    class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Common cm = new Common();
            //if (context.Exception is MissingMethodException)
            //{
            if (context.Exception is Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(context.Exception.Message, Encoding.Unicode),
                    ReasonPhrase = "Custom Exception "
                });
            }
            //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
            //{
            //    Content = new StringContent(context.Exception.Message),
            //    ReasonPhrase = "Custom Exception "
            //});

            //}

            //Log Critical errors
            // Debug.WriteLine(context.Exception);

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred, please try again or contact the administrator at it.software@goldmedalindia.com.", Encoding.Unicode),
                ReasonPhrase = "Critical Exception"
            });
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed)
            {
                Content = new StringContent("contact the administrator at it.software@goldmedalindia.com.", Encoding.Unicode),
                ReasonPhrase = "Critical"
            });
        }
    }

