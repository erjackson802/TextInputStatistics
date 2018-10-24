using CasellaTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CasellaTest.Controllers
{
    public class ScanController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        // POST: api/Scan
        [HttpPost]
        public HttpResponseMessage Post()
        {
            //Input from POST Body (Text Raw)
            //var sRaw = Request.Content.ReadAsStringAsync().Result;

            var byteArray = Request.Content.ReadAsByteArrayAsync().Result;
            var responseString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

            Document doc = new Document(responseString);
            doc.Process();                        
            //Json Output
            string jsonResponse = doc.GetJsonForTop(50);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");

            return response;
        }
                      
        
    }
}
