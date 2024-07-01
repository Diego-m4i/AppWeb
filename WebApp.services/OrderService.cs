﻿using Microsoft.EntityFrameworkCore;
using WebApp.data;
using WebApp.models;

namespace WebApp.services;

public class OrderService
{
    private readonly AppDb _dbContext;

    public OrderService(AppDb dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _dbContext.Orders.Include(o => o.OrderItems).ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(Guid id)
    {
        return await _dbContext.Orders.Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == id);
    }

    public async Task UpdateOrderStatus(Guid id, string status)
    {
        var order = await GetOrderByIdAsync(id);
        if (order != null)
        {
            order.Status = status;
            await _dbContext.SaveChangesAsync();
        }
    }
}
