using AplicationLayer.Interfaces;
using InfrastructureLayer.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _databaseContext;

        public OrderRepository(AppDbContext database)
        {
            _databaseContext = database;
        }

        public Task<bool> AddCreditCardDetails()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddOrEditAdress()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateDeliveryFromCart()
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetAllOrdersForUser(int userID)
        {
            throw new NotImplementedException();
        }
    }
}
