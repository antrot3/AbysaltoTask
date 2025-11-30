using DomainLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicationLayer.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool> GetAllOrdersForUser(int userID);
        Task<bool> CreateDeliveryFromCart();
        Task<bool> AddOrEditAdress();
        Task<bool> AddCreditCardDetails();
    }
}
