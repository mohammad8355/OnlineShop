using DataAccessLayer.Models;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.OrderDetailsService
{
    public class OrderDetailsLogic
    {
        private readonly MainRepository<OrderDetails> OrderDetailsRepository;
        public OrderDetailsLogic(MainRepository<OrderDetails> OrderDetailsRepository)
        {
            this.OrderDetailsRepository = OrderDetailsRepository;
        }
        public async Task<bool> CreateOrderDetails(OrderDetails model)
        {
            if (model == null || model.count == 0 || model.TotalPrice == 0 | model.order_Id == 0)
            {
                return false;
            }
            else
            {
                await OrderDetailsRepository.AddItem(model);
                return true;
            }
        }
        public bool EditOrderDetails(OrderDetails model)
        {
            if (model == null || model.count == 0 || model.TotalPrice == 0 | model.order_Id == 0)
            {
                return false;
            }
            else
            {
                OrderDetailsRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteOrderDetails(int Id)
        {
            if (Id != 0)
            {
                if (await OrderDetailsRepository.DeleteItem(Id))
                {
                    return true;
                }

            }
            return false;
        }
        public OrderDetails OrderDetailsDetail(int Id)
        {
            var OrderDetails = OrderDetailsRepository.Get(o => o.Id == Id).Result.FirstOrDefault();
            return OrderDetails;
        }
        public List<OrderDetails> OrderDetailsList()
        {
            var OrderDetailss = OrderDetailsRepository.Get(null,o => o.Product).Result.ToList();
            return OrderDetailss;
        }
    }
}
