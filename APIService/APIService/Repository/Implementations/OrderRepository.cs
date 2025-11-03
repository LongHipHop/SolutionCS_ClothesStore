using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class OrderRepository : RepositoryBase<Orders>, IOrderRepository
    {
        public OrderRepository(ShopDbContext context) : base(context)
        {
        }

        public async Task<Orders> GetOrderById(int id)
        {
            var item = await FindByCondition(o => o.Id == id, trackChanges: true, includeRole: false);
            if(item == null)
            {
                return null;
            }
            return item;
        }

        public Task UpdateOrder(Orders order)
        {
            return Update(order);
        }

        public Task CreateOrder(Orders order)
        {
            return Create(order);
        }

        public Task<List<Orders>> GetAll()
        {
            return FindAll(false).ToListAsync();
        }

        public async Task<Orders> GetOrderDetailById(int id)
        {
            return await _context.Orders
                .Include(o => o.Payments)
                .Include(o => o.Shipments)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
