using System.Collections.Generic;
using System.Web.Http;
using UberHypermedia_CSharp_Neo4J.Models.UberHypermedia;

namespace UberHypermedia_CSharp_Neo4J.Controllers
{
    [RoutePrefix("api")]
    public class WorkerController : ApiController
    {
        [Route(""), HttpGet]
        public IHttpActionResult Definition()
        {
            UberDocument uber = new UberDocument { Version = "1.0" };

            string authToken = Request.Headers.Authorization.Parameter;
            List<string> allowedActions = GetAllowedActions(authToken);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(uber);

            return Ok(new { uber });


        }

        [Route("worker"), HttpGet]
        public IHttpActionResult Read()
        {
            UberDocument uber = new UberDocument { Version = "1.0" };
            return Ok(new { uber });
        }

        [Route("worker/{id}"), HttpGet]
        public IHttpActionResult Read(int id)
        {
            UberDocument uber = new UberDocument { Version = "1.0" };
            return Ok(new { uber });
        }

        [Route("worker/search"), HttpGet]
        public IHttpActionResult Search([FromUri] string firstName, [FromUri] string lastName, [FromUri] string last4SSN, [FromUri] string[] skill, [FromUri] string branchNumber)
        {
            UberDocument uber = new UberDocument { Version = "1.0" };
            return Ok(new { uber });
        }

        [Route("worker"), HttpPost]
        public IHttpActionResult Append([FromBody] dynamic worker)
        {
            UberDocument uber = new UberDocument { Version = "1.0" };
            return Ok(new { uber });
        }

        [Route("worker/{id}"), HttpPut]
        public IHttpActionResult Replace(int id, [FromBody] dynamic worker)
        {
            UberDocument uber = new UberDocument { Version = "1.0" };
            return Ok(new { uber });
        }

        [Route("worker/{id}"), HttpPatch]
        public IHttpActionResult Partial(int id, [FromBody] dynamic worker)
        {
            UberDocument uber = new UberDocument { Version = "1.0" };
            return Ok(new { uber });
        }

        [Route("worker/{id}"), HttpDelete]
        public IHttpActionResult Remove(int id)
        {
            UberDocument uber = new UberDocument { Version = "1.0" };
            return Ok(new { uber });
        }

        private List<string> GetAllowedActions(string token)
        {
            return new List<string>() { Actions.Append, Actions.Partial, Actions.Read, Actions.Remove, Actions.Replace };
        }

    }
}
