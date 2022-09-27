using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC.BusinessLogic
{
    public class HttpClientWrapper : HttpClient
    {
        public HttpClientWrapper()
        {
        }

        public HttpClientWrapper(HttpMessageHandler handler) : base(handler)
        {
        }

        public HttpClientWrapper(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler)
        {
        }
    }
}
