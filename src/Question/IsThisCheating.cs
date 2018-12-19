using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WhatIfHmmm.Model;

namespace WhatIfHmmm.Question
{
    public class IsThisCheating : UsecaseContext
    {        
        public DbSet<Order> Orders => Set<Order>();

        public void Handle(OpenOrder command)
        {
            //No Invariants ..Imagine verifying the customer can open orders or whatever...
            
            var order = NewOrder(command.OrderId);
            Apply(new Effect
            {
               Event = new OrderOpened
               {
                   OrderId = command.OrderId, CustomerId = command.CustomerId
               },
               Entity = order
            });

            SaveChanges();
            //All "Handle" methods could be wrapped in a 
            //(save changes / retry n-times on revision conflict)
        }

        public void Handle(ManyNewOrdersInOneTransaction command)
        {
            //No Invariants ..Imagine verifying the customer can open orders or whatever...

            var newOrders = command.NewOrderIds.Select(NewOrder);
            foreach (var newOrder in newOrders)
            {
                Apply(new Effect
                {
                    Event = new OrderOpened
                    {
                        OrderId = newOrder.OrderId, CustomerId = command.CustomerId
                    },
                    Entity = newOrder
                });
            }
            
            SaveChanges();
            //Saving all events and all state of those events in one transaction
        }



        Order NewOrder(Guid orderId)
        {
            var o = Order.New(orderId);
            Orders.Add(o);
            return o;
        }
        
    }

    
}