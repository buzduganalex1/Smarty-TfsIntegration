using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    using ConsoleApp1;

    public class WorkItemsController : ApiController
    {
        // GET: api/WorkItems
        public IHttpActionResult Get()
        {
            var test = new TfsService();

            var results = test.GetWorkItemsForUser("alexandru.buzdugan@centric.eu");

            return this.Ok(results);
        }

        // GET: api/WorkItems/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/WorkItems
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/WorkItems/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/WorkItems/5
        public void Delete(int id)
        {
        }
    }
}
