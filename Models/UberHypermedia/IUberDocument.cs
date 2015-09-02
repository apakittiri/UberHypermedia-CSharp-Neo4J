using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UberHypermedia_CSharp_Neo4J.Models.UberHypermedia
{
    interface IUberDocument
    {
        [DataMember(Name = "version")]
        string Version { get; set; }

        [DataMember(Name = "data")]
        List<Data> Data { get; set; }

        [DataMember(Name = "error")]
        Error Error { get; set; }
    }
}
