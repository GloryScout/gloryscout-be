using GloryScout.Data.Models.Payment;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GloryScout.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResponse> GetOrderByIdAsync(string orderId)
        {
            return await _context.OrderResponses
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task UpdateOrderAsync(OrderResponse order)
        {
            if (order == null)
                return;

            _context.OrderResponses.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
