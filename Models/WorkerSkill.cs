using Neo4jClient;

namespace UberHypermedia_CSharp_Neo4J.Models
{
    public class WorkerSkill
    {
        public string workerNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string skill { get; set; }
        public string experience { get; set; }
    }
}