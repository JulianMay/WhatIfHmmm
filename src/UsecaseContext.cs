using Microsoft.EntityFrameworkCore;
using WhatIfHmmm.Model;

namespace WhatIfHmmm
{
    public class Effect
    {
        public AggregateState Entity;
        public object Event;
    }
    
    public abstract class UsecaseContext : DbContext
    {
        protected DbSet<Event> Events { get; set; }

        public void Apply(params Effect[] effects)
        {
            foreach (var effect in effects)
            {
                //Add to intermediary state and eventstreams
                effect.Entity.Apply(effect.Event);
                Events.Add(new Event
                {
                    Data = Serialized(effect.Event),
                    //...
                });

                //Nothing commit'ed yet.
            }
        }

        private string Serialized(object evnt) => "yadayada";
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {     
            //Enables optimistic offline locking on all aggregates at once
            modelBuilder.Entity<Order>().Ignore(o => o.UncomittedEvents);
            modelBuilder.Entity<Order>()
            .HasIndex(p => new { p.OrderId, p.Revision })
                .IsUnique();
            
            modelBuilder.Entity<OrderLine>()
                .HasKey(l => new { l.LineNumber, l.OrderId});
           
            modelBuilder.Entity<Order>().HasMany<OrderLine>();
            
            base.OnModelCreating(modelBuilder);
        }
    }
    
    
}