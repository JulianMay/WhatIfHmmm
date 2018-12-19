using System;
using System.Collections.Generic;

namespace WhatIfHmmm.Model
{
    public class Order : AggregateState
    {

        
        public static Order New(Guid orderId) => new Order {OrderId = orderId};
        public Guid CustomerId { get; protected set; }
        public override int Revision { get; protected set; }       
        public string Status { get; protected set; }
        public Guid? Payment { get; protected set; }
        public Guid OrderId { get; protected set; }
        
        
        public List<OrderLine> OrderLines { get; protected set; }
        
        
        public override void Apply(object evnt)
        {
            switch (evnt)
            {
                case OrderOpened opened:
                    CustomerId = opened.CustomerId;
                    OrderId = opened.OrderId;
                    Status = Consts.OPENED;
                    break;
                
                case OrderLineAdded added:
                    OrderLines.Add(new OrderLine(added.LineNumber,
                        added.ProductCode,added.Description,added.Qty, added.Price));
                    break;                                
            }

            Revision++;
        }
    }

    public class OrderLine
    {
        public Guid OrderId { get; protected set; }
        public int LineNumber  { get; protected set; }
        public string ProductCode  { get; protected set; }
        public string Description  { get; protected set; }
        public int Qty  { get; protected set; }
        public decimal Amount  { get; protected set; }

        public OrderLine(int lineNumber, string productCode, string description, int qty, decimal amount)
        {
            LineNumber = lineNumber;
            ProductCode = productCode;
            Description = description;
            Qty = qty;
            Amount = amount;
        }
        
        //For EF
        protected OrderLine(){} 
    }

    public static class Consts
    {
        public const string OPENED = "Opened";
        public const string PAYED = "payed";
        public const string SHIPPED = "Shipped";
    }
    
    
    //Messages

    public class OpenOrder
    {
        public Guid OrderId;
        public Guid CustomerId;
    }
    public class OrderOpened
    {
        public Guid OrderId;
        public Guid CustomerId;
    }

    public class AddOrderLine
    {
        public string ProductCode { get; protected set; }
        public string Description { get; protected set; }
        public int Qty  { get; protected set; }
        public decimal Price  { get; protected set; }
    }
    public class OrderLineAdded
    {
        public int LineNumber  { get; protected set; }
        public string ProductCode  { get; protected set; }
        public string Description  { get; protected set; }
        public int Qty  { get; protected set; }
        public decimal Price  { get; protected set; }
    }

    public class ManyNewOrdersInOneTransaction
    {
        public Guid[] NewOrderIds;
        public Guid CustomerId;
    }
    
    
    
    
}