using System.Collections.Generic;
using System.Runtime.Serialization;
using DynamicUtils;

namespace UberHypermedia_CSharp_Neo4J.Models.UberHypermedia
{
    public class Error : ExtensibleObject
    {
        [DataMember(Name = "data")]
        public IList<Data> Children { get; set; }
    }
}