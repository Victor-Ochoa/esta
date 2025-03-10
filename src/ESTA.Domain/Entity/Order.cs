﻿using ESTA.Domain.Enum;
using ESTA.Domain.Event;
using ESTA.Domain.ValueObject;

namespace ESTA.Domain.Entity;

public class Order : Base.Entity
{
    public IList<Product> Products { get; set; } = [];
    public required Seller Seller { get; set; }
    public Address DeliveryAddress { get; set; }
    public DateTime? DispatchedAtUtc { get; set; }
    public DateTime? OrderOutForDeliveryAtUtc { get; set; }
    public DateTime? DeliveredAtUtc { get; set; }
    public EOrderStatus OrderStatus{ get; set; }

    public void Apply(OrderCreated created)
    {
        OrderStatus = EOrderStatus.Created;
        Products = [.. created.Products.Select(x => new Product { Id = x})];
        Seller = new Seller { Id = created.Seller };
        DeliveryAddress = created.DeliveryAddress;
    }

    public void Apply(OrderDelivered delivered)
    {
        OrderStatus = EOrderStatus.Delivered;
        DeliveredAtUtc = delivered.DeliveredAtUtc;
    }
    public void Apply(OrderDispatched dispatched)
    {
        OrderStatus = EOrderStatus.Dispatched;
        DispatchedAtUtc = dispatched.DispatchedAtUtc;
    }
    public void Apply(OrderOutForDelivery outForDelivery)
    {
        OrderStatus = EOrderStatus.OutForDelivery;
        OrderOutForDeliveryAtUtc = outForDelivery.OrderOutForDeliveryAtUtc;
    }
    public void Apply(OrderAddressUpdate addressUpdate)
    {
        DeliveryAddress = addressUpdate.DeliveryAddress;
    }
}
