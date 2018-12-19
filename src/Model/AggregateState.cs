using System.Collections.Generic;

namespace WhatIfHmmm.Model
{
    public abstract class AggregateState
    {
        public List<object> UncomittedEvents = new List<object>();
        public abstract int Revision { get; protected set; }
        public abstract void Apply(object evnt);
    }
}