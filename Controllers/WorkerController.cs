using Neo4jClient.Cypher;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using UberHypermedia_CSharp_Neo4J.Models;
using UberHypermedia_CSharp_Neo4J.Models.UberHypermedia;

namespace UberHypermedia_CSharp_Neo4J.Controllers
{
    [RoutePrefix("api")]
    public class WorkerController : ApiController
    {
        [Route(""), HttpGet]
        public IHttpActionResult Definition()
        {
            UberDocument uberDocument = new UberDocument { Version = "1.0", Data = new List<Data>() };
            Data data = new Data();
            data.Rel = new List<string>();
            data.Rel.Add("self");
            data.Rel.Add(Request.Headers.Host + "/api");

            //string authToken = Request.Headers.Authorization.Parameter;
            List <string> allowedActions = GetAllowedActions(string.Empty);
            data.Children = GetDataElements(allowedActions, "{id}", null, null);
            uberDocument.Data.Add(data);
            string uber = JsonConvert.SerializeObject(uberDocument, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return Ok(new { uber });

        }

        [Route("worker/{id}"), HttpGet]
        public IHttpActionResult Read(string id)
        {
            var query = WebApiConfig.GraphClient.Cypher
                .Match("(w:Worker)")
                .Where((Worker w) => w.id == id)
                .Return(w => w.As<Worker>());
                
            var workers = query.Results.ToList();

            UberDocument uberDocument = new UberDocument { Version = "1.0", Data = new List<Data>() };
            Worker worker = null;
            if (workers.Count > 0)
            {
                worker = workers[0];

                Data data = new Data();
                data.Rel = new List<string>();
                data.Rel.Add("self");
                data.Rel.Add(Request.Headers.Host + "/api/worker/" + id);

                List<Data> result = new List<Data>();
                result.Add(GetReadData(id, worker));
                result.Add(GetSkillData(id, null));
                result.Add(GetBranchData(id, null));
                result.Add(GetActionData(id, Actions.Replace));
                result.Add(GetActionData(id, Actions.Remove));
                data.Children = result;
                uberDocument.Data.Add(data);
            }

            string uber = JsonConvert.SerializeObject(uberDocument, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return Ok(new { uber });
        }

        [Route("worker/{id}/skill"), HttpGet]
        public IHttpActionResult GetWorkerSkill(string id)
        {
            var query = WebApiConfig.GraphClient.Cypher
                .Match("(w:Worker)-[e:EXPERTIN]->(s:Skill)")
                .Where((Worker w) => w.id == id)
                .Return((w, s) => new
                {
                    worker = w.As<Worker>(),
                    skill = Return.As<string>("collect(s.name)")
                });

            var queryData = query.Results.ToList();

            var workerSkills = new List<WorkerSkill>();
            foreach (var item in queryData)
            {
                WorkerSkill workerSkill = new WorkerSkill
                {
                    workerNumber = item.worker.number,
                    firstName = item.worker.firstName,
                    lastName = item.worker.lastName,
                    skill = item.skill,
                    experience = "EXPERT"
                };
                workerSkills.Add(workerSkill);
            }

            query = WebApiConfig.GraphClient.Cypher
                .Match("(w:Worker)-[e:PROFICIENTIN]->(s:Skill)")
                .Where((Worker w) => w.id == id)
                .Return((w, s) => new
                {
                    worker = w.As<Worker>(),
                    skill = Return.As<string>("collect(s.name)")
                });

            queryData = query.Results.ToList();

            foreach (var item in queryData)
            {
                WorkerSkill workerSkill = new WorkerSkill
                {
                    workerNumber = item.worker.number,
                    firstName = item.worker.firstName,
                    lastName = item.worker.lastName,
                    skill = item.skill,
                    experience = "PROFICIENT"
                };
                workerSkills.Add(workerSkill);
            }


            UberDocument uberDocument = new UberDocument { Version = "1.0", Data = new List<Data>() };
            if (workerSkills.Count > 0)
            {
                Data data = new Data();
                data.Rel = new List<string>();
                data.Rel.Add("self");
                data.Rel.Add(Request.Headers.Host + "/api/worker/" + id + "/skill");
                List<Data> result = new List<Data>();

                foreach(WorkerSkill ws in workerSkills)
                {
                    result.Add(GetSkillData(id, ws));
                }

                data.Children = result;
                uberDocument.Data.Add(data);
            }

            string uber = JsonConvert.SerializeObject(uberDocument, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return Ok(new { uber });
        }

        [Route("worker/{id}/branch"), HttpGet]
        public IHttpActionResult GetWorkerBranch(string id)
        {
            var query = WebApiConfig.GraphClient.Cypher
                .Match("(w:Worker)-[e:WORKSIN]->(b:Branch)")
                .Where((Worker w) => w.id == id)
                .Return((w, b) => new
                {
                    worker = w.As<Worker>(),
                    branch = Return.As<string>("collect(b.number)")
                });

            var queryData = query.Results.ToList();

            var workerBranches = new List<WorkerBranch>();
            foreach (var item in queryData)
            {
                WorkerBranch workerBranch = new WorkerBranch
                {
                    workerNumber = item.worker.number,
                    firstName = item.worker.firstName,
                    lastName = item.worker.lastName,
                    BranchNumber = item.branch
                };
                workerBranches.Add(workerBranch);
            }


            UberDocument uberDocument = new UberDocument { Version = "1.0", Data = new List<Data>() };
            if (workerBranches.Count > 0)
            {
                Data data = new Data();
                data.Rel = new List<string>();
                data.Rel.Add("self");
                data.Rel.Add(Request.Headers.Host + "/api/worker/" + id + "/branch");
                List<Data> result = new List<Data>();

                foreach (WorkerBranch wb in workerBranches)
                {
                    result.Add(GetBranchData(id, wb));
                }

                data.Children = result;
                uberDocument.Data.Add(data);
            }

            string uber = JsonConvert.SerializeObject(uberDocument, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return Ok(new { uber });
        }

        [Route("worker/search"), HttpGet]
        public IHttpActionResult Search([FromUri] string firstName, [FromUri] string lastName, [FromUri] string last4SSN, [FromUri] string[] skill)
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.NotImplemented);
        }

        [Route("worker"), HttpPost]
        public IHttpActionResult Append([FromBody] dynamic worker)
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.NotImplemented);
        }

        [Route("worker/{id}"), HttpPut]
        public IHttpActionResult Replace(int id, [FromBody] dynamic worker)
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.NotImplemented);
        }

        [Route("worker/{id}"), HttpDelete]
        public IHttpActionResult Remove(int id)
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.NotImplemented);
        }

        private List<string> GetAllowedActions(string token)
        {
            return new List<string>() { Actions.Append, Actions.Partial, Actions.Read, Actions.Remove, Actions.Replace };
        }

        private List<Data> GetDataElements(List<string> actions, string id, Worker worker, List<WorkerSkill> workerSkills)
        {
            List<Data> result = new List<Data>();

            if(actions.Contains(Actions.Read))
            {
                result.Add(GetReadData(id, worker));
                if (!string.IsNullOrEmpty(id))
                {
                    if (null == workerSkills)
                    {
                        result.Add(GetSkillData(id, null));
                    }
                    else
                    {
                        foreach (WorkerSkill s in workerSkills)
                        {
                            result.Add(GetSkillData(id, s));
                        }
                    }
                }
                if (worker == null)
                {
                    result.Add(GetSearchData());
                }
            }
            if (actions.Contains(Actions.Append) && worker == null)
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

        private Data GetReadData(string id, Worker worker)
        {
            Data data = new Data()
            {
                Name = "worker",
                Url = new System.Uri(Request.Headers.Host + "/api/worker/" + id)
            };
            data.Rel = new List<string>();
            data.Rel.Add("item");
            data.Rel.Add(Request.Headers.Host + "/api/rels/worker");

            List<Data> children = new List<Data>();
            children.Add(new Data { Name = "number", Label = "Number" , Value = worker == null ? null : worker.number});
            children.Add(new Data { Name = "firstName", Label = "First Name", Value = worker == null ? null : worker.firstName });
            children.Add(new Data { Name = "lastName", Label = "Last Name", Value = worker == null ? null : worker.lastName });
            children.Add(new Data { Name = "last4SSN", Label = "Last 4 digits of SSN", Value = worker == null ? null : worker.last4SSN });
            children.Add(new Data { Name = "dob", Label = "Date Of Birth", Value = worker == null ? null : worker.dob.ToString() });
            children.Add(new Data { Name = "status", Label = "Status", Value = worker == null ? null : worker.status });
            children.Add(new Data { Name = "avatarUrl", Transclude = "true", Url = new System.Uri("http://example.org/avatars/{number}"), Value = "Worker Photo" });

            data.Children = children;

            return data;
        }

        private Data GetSkillData(string id, WorkerSkill workerSkill)
        {
            Data data = new Data()
            {
                Name = "worker",
                Url = new System.Uri(Request.Headers.Host + "/api/worker/" + id + "/skill")
            };
            data.Rel = new List<string>();
            data.Rel.Add("item");
            data.Rel.Add(Request.Headers.Host + "/api/rels/worker");

            List<Data> children = new List<Data>();
            children.Add(new Data { Name = "workerNumber", Label = "Worker Number", Value = workerSkill == null ? null : workerSkill.workerNumber });
            children.Add(new Data { Name = "firstName", Label = "First Name", Value = workerSkill == null ? null : workerSkill.firstName });
            children.Add(new Data { Name = "lastName", Label = "Last Name", Value = workerSkill == null ? null : workerSkill.lastName });
            children.Add(new Data { Name = "skill", Label = "Skill", Value = workerSkill == null ? null : workerSkill.skill });
            children.Add(new Data { Name = "experience", Label = "Experience", Value = workerSkill == null ? null : workerSkill.experience });

            data.Children = children;

            return data;
        }

        private Data GetBranchData(string id, WorkerBranch workerBranch)
        {
            Data data = new Data()
            {
                Name = "worker",
                Url = new System.Uri(Request.Headers.Host + "/api/worker/" + id + "/branch")
            };
            data.Rel = new List<string>();
            data.Rel.Add("item");
            data.Rel.Add(Request.Headers.Host + "/api/rels/worker");

            List<Data> children = new List<Data>();
            children.Add(new Data { Name = "workerNumber", Label = "Worker Number", Value = workerBranch == null ? null : workerBranch.workerNumber });
            children.Add(new Data { Name = "firstName", Label = "First Name", Value = workerBranch == null ? null : workerBranch.firstName });
            children.Add(new Data { Name = "lastName", Label = "Last Name", Value = workerBranch == null ? null : workerBranch.lastName });
            children.Add(new Data { Name = "branchNumber", Label = "Branch Number", Value = workerBranch == null ? null : workerBranch.BranchNumber });

            data.Children = children;

            return data;
        }

        private Data GetSearchData()
        {
            Data data = new Data()
            {
                Name = "search",
                Url = new System.Uri(Request.Headers.Host + "/api//worker/search?firstName={firstName}&lastName={lastName}&last4SSN={last4SSN}&skill[0]={skill[0]}&skill[1]={skill[1]}&branchNumber={branchNumber}"),
                Templated = "true"
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
