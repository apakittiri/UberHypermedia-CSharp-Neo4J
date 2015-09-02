using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DynamicUtils;

namespace UberHypermedia_CSharp_Neo4J.Models.UberHypermedia
{
    [DataContract]
    public class Data : ExtensibleObject
    {

        [DataMember(Name = "data")]
        public List<Data> Children { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "rel")]
        public List<string> Rel { get; set; }

        [DataMember(Name = "url")]
        public Uri Url { get; set; }

        [DataMember(Name = "action")]
        public string Action { get; set; }

        [DataMember(Name = "transclude")]
        public string Transclude { get; set; }

        //need to add validation
        [DataMember(Name = "model")]
        public string Model { get; set; }

        [DataMember(Name = "sending")]
        public List<string> Sending { get; set; }

        [DataMember(Name = "accepting")]
        public List<string> Receiving { get; set; }

        //need to add validation
        [DataMember(Name = "value")]
        public object Value { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "templated")]
        public string Templated { get; set; }
    }
}