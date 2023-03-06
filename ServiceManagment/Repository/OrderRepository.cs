﻿using Microsoft.EntityFrameworkCore;
using ServiceManagment.Data;
using ServiceManagment.Data.Enum;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;

namespace ServiceManagment.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Order order)
        {
            _context.Orders.Add(order);
            return Save();
        }

        public bool Delete(Order order)
        {
            _context.Remove(order);
            return Save();
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders
                .Include(i => i.Customer)
                .Include(j => j.Product)
                .Include(k => k.Payment)
                .OrderBy(x => x.OrderStatus)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByStatus(string status)
        {
            return await _context.Orders
                .Include(i => i.Customer)
                .Include(j => j.Product)
                .Where(x => x.OrderStatus == (OrderStatus)Enum.Parse(typeof(OrderStatus), status))
                .ToListAsync();
        }

        public async Task<Order> GetPaymentByOrderId(int id)
        {
            return await _context.Orders
                .Include(i => i.Payment)
                .FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _context.Orders
                .Include(i => i.Customer)
                .Include(j => j.Product)
                .Include(k => k.Payment)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Order order)
        {
            _context.Update(order);
            return Save();
        }
    }
}
