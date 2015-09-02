using System.Collections.Generic;
using System.Runtime.Serialization;
using DynamicUtils;

namespace UberHypermedia_CSharp_Neo4J.Models.UberHypermedia
{
    [DataContract(Name ="uber")]
    public class UberDocument : ExtensibleObject, IUberDocument
    {
        [DataMember(Name = "version")]
        public string Version { get; set; }

        [DataMember(Name = "data")]
        public List<Data> Data { get; set; }

        [DataMember(Name = "error")]
        public Error Error { get; set; }
    }
}