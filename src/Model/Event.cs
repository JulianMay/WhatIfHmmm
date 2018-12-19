using System;

namespace WhatIfHmmm.Model
{
    public class Event
    {
        public string AggregateType { get; set; }
        public Guid AggregateId { get; set; }
        public int Revision { get; set; }
        public string Data { get; set; }
        public string MetaData { get; set; }
    }
}