# WhatIfHmmm
Does this make sense?


On the topic of "One command for several aggregates" on #eventsourcing 18/12/2018, someone who wasn't named Sherlock Holmes said:
_command handles logic and validation, aggregate handles application of state change to itself based on provided events
that depends on how you are storing those events_

_event store is just a concept_

_it can be mysql, mongo, key-value_

_whatever_

_you just have to pair events with their aggregates_

_that's all it is to it_

We (where i work) really do have valid cases where slower/fewer writes (per partition, per sql-db in this case) would be perfectly tollerable, as long as they where consistent(we are paying the bill for inconsistencies and split-brain).

The code is just a super rough sketch, a conversation-piece:
The idea is good ol' UnitOfWork with EF, where only events may alter state of the (UseCase)dbContext.
As events are emitted within the UOW, events are added to an eventstore that is part of the dbContext.
A unique-index (AggregateId + Revision) on the Event-table is used to enforce optimistic offline locking _for all aggregates in the transaction-scope_. 
So if just one of the aggregates in scope has changes in the meantime, the dbContext must be reloaded, and the entire thing can be retried.

It can even do sequences ( Command => event1 -> Policy(event1,dbContext) => event2) 
...based on intermediary state (dbContext), the hack is to "allow uncommitted reads" for the transactionscope
