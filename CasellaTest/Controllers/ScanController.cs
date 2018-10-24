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
    /// <summary>
    /// Web Api Endpoint provided to Scan incoming raw text.
    /// Accepts an HTTP POST used to scan raw incoming text data encoded UTF-8
    /// </summary>
    public class ScanController : ApiController
    {
        private readonly int MAX_DEFAULT_RETURNED_WORDS = 50;       // defined constant for Max results from default POST
                                                                    // could implement a parametered POST to get a custom
                                                                    // number of results.


        public ScanController()
        {

        }

        // POST: api/Scan
        /// <summary>
        ///
        /// Responds with a JSON document content providing:
        ///     1.  The 50 most frequently used words in the document along with their frequency.  Comparison
        ///         is case insensitive.  Sorted by frequency then by word.
        ///     2.  Total number of words in the document.
        ///     3.  The percentage (decimal) of characters in the document that are whitespace.
        ///     4.  The percentage (decimal) of characters in the document that are punctuation marks.
        /// </summary>
        /// <returns> Returns JSON document UTF-8 encoded in the content body of the HttpResponseMessage. </returns>
        [HttpPost]
        public HttpResponseMessage Post()
        {
            try
            {                         
                //retrieve raw string from BODY of POST encoded as UTF-8
                byte[] byteArray = Request.Content.ReadAsByteArrayAsync().Result;
                string responseString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                                
                Document doc = new Document(responseString);
                doc.Process();

                string jsonResponse = doc.GetJsonForTop(MAX_DEFAULT_RETURNED_WORDS);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");

                return response;
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.RequestMessage.Content = new StringContent(ex.Message);

                return response;
            }
        }
                      
        
    }
}
