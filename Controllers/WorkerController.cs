using Newtonsoft.Json;
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
            UberDocument uberDocument = new UberDocument { Version = "1.0" };

            //string authToken = Request.Headers.Authorization.Parameter;
            List<string> allowedActions = GetAllowedActions(string.Empty);
            uberDocument.Data = GetDataElements(allowedActions, "{id}");

            string uber = JsonConvert.SerializeObject(uberDocument, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return Ok(new { uber });

        }

        [Route("worker/{id}"), HttpGet]
        public IHttpActionResult Read(int id)
        {
            UberDocument uberDocument = new UberDocument { Version = "1.0" };
            string uber = JsonConvert.SerializeObject(uberDocument, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return Ok(new { uber });
        }

        [Route("worker/search"), HttpGet]
        public IHttpActionResult Search([FromUri] string firstName, [FromUri] string lastName, [FromUri] string last4SSN, [FromUri] string[] skill, [FromUri] string branchNumber)
        {
            UberDocument uberDocument = new UberDocument { Version = "1.0" };
            string uber = JsonConvert.SerializeObject(uberDocument, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return Ok(new { uber });
        }

        [Route("worker"), HttpPost]
        public IHttpActionResult Append([FromBody] dynamic worker)
        {
            UberDocument uberDocument = new UberDocument { Version = "1.0" };
            string uber = JsonConvert.SerializeObject(uberDocument, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return Ok(new { uber });
        }

        [Route("worker/{id}"), HttpPut]
        public IHttpActionResult Replace(int id, [FromBody] dynamic worker)
        {
            UberDocument uberDocument = new UberDocument { Version = "1.0" };
            string uber = JsonConvert.SerializeObject(uberDocument, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return Ok(new { uber });
        }

        [Route("worker/{id}"), HttpDelete]
        public IHttpActionResult Remove(int id)
        {
            UberDocument uberDocument = new UberDocument { Version = "1.0" };
            string uber = JsonConvert.SerializeObject(uberDocument, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return Ok(new { uber });
        }

        private List<string> GetAllowedActions(string token)
        {
            return new List<string>() { Actions.Append, Actions.Partial, Actions.Read, Actions.Remove, Actions.Replace };
        }

        private List<Data> GetDataElements(List<string> actions, string id)
        {
            List<Data> result = new List<Data>();

            if(actions.Contains(Actions.Read))
            {
                result.Add(GetReadData(id));
                result.Add(GetSearchData());
            }
            if (actions.Contains(Actions.Append))
            {
                result.Add(GetActionData(string.Empty,Actions.Append));
            }
            if (actions.Contains(Actions.Replace))
            {
                result.Add(GetActionData(id, Actions.Replace));
            }
            if (actions.Contains(Actions.Remove))
            {
                result.Add(GetActionData(id, Actions.Remove));
            }

            return result;
        }

        private Data GetReadData(string id)
        {
            Data data = new Data()
            {
                Name = "worker",
                Url = new System.Uri(Request.Headers.Host + "/api//worker/id")
            };
            data.Rel = new List<string>();
            data.Rel.Add("item");
            data.Rel.Add(Request.Headers.Host + "/api/rels/worker");

            List<Data> children = new List<Data>();
            children.Add(new Data { Name = "number", Label = "Number" });
            children.Add(new Data { Name = "firstName", Label = "First Name" });
            children.Add(new Data { Name = "lastName", Label = "Last Name" });
            children.Add(new Data { Name = "last4SSN", Label = "Last 4 digits of SSN" });
            children.Add(new Data { Name = "dob", Label = "Date Of Birth" });
            children.Add(new Data { Name = "status", Label = "Status" });
            children.Add(new Data { Name = "avatarUrl", Transclude = true, Url = new System.Uri("http://example.org/avatars/{number}"), Value = "Worker Photo" });

            data.Children = children;

            return data;
        }

        private Data GetSearchData()
        {
            Data data = new Data()
            {
                Name = "search",
                Url = new System.Uri(Request.Headers.Host + "/api//worker/search?firstName={firstName}&lastName={lastName}&last4SSN={last4SSN}&skill[0]={skill[0]}&skill[1]={skill[1]}&branchNumber={branchNumber}"),
                Templated= true
            };
            data.Rel = new List<string>();
            data.Rel.Add("search");
            data.Rel.Add("collection");

            return data;
        }

        private Data GetActionData(string id, string action)
        {
            string url = string.IsNullOrEmpty(id) ? "/api//worker/" : "/api//worker/" + id;
            string rel = "/api/rels/" + action;
            Data data = new Data()
            {
                Name = "create",
                Url = new System.Uri(Request.Headers.Host + url),
                Model = "n={number}&f={firstName}&l={lastName}&s={ssn},d={dob},status={status},a={avatarUrl}",
                Action = action
            };
            data.Rel = new List<string>();
            data.Rel.Add("item");
            data.Rel.Add(Request.Headers.Host + rel);

            return data;
        }

    }
}
