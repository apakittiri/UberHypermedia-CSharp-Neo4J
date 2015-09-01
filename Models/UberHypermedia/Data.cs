﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DynamicUtils;

namespace UberHypermedia_CSharp_Neo4J.Models.UberHypermedia
{
    [DataContract]
    public class Data : ExtensibleObject
    {
        public Data()
        {
            Children = new List<Data>();
            Rel = new List<string>();
            Sending = new List<string>();
            Receiving = new List<string>();

        }

        [DataMember(Name = "data")]
        public IList<Data> Children { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "rel")]
        public IList<string> Rel { get; set; }

        [DataMember(Name = "url")]
        public Uri Url { get; set; }

        [DataMember(Name = "action")]
        public string Action { get; set; }

        [DataMember(Name = "transclude")]
        public bool Transclude { get; set; }

        //need to add validation
        [DataMember(Name = "model")]
        public string Model { get; set; }

        [DataMember(Name = "sending")]
        public IList<string> Sending { get; set; }

        [DataMember(Name = "accepting")]
        public IList<string> Receiving { get; set; }

        //need to add validation
        [DataMember(Name = "value")]
        public object Value { get; set; }

        [DataMember(Name = "label")]
        public object Label { get; set; }

        [DataMember(Name = "templated")]
        public object Templated { get; set; }
    }
}