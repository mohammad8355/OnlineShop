using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Migrations;
using DataAccessLayer.Models;
using DataAccessLayer.services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BusinessLogicLayer.OrderService
{
    public class OrderLogic
    {
        private readonly MainRepository<Order> orderRepository;
        private readonly MainRepository<OrderDetails> orderDetailsRepository;
        public OrderLogic(MainRepository<Order> orderRepository, MainRepository<OrderDetails> orderDetailsRepository)
        {
            this.orderRepository = orderRepository;
            this.orderDetailsRepository = orderDetailsRepository;
        }
        public async Task<bool> CreateOrder(Order model)
        {
            if (model == null || string.IsNullOrEmpty(model.User_Id) || model.TotalCount == 0 || model.TotalPrice == 0)
            {
                return false;
            }
            else
            {
                await orderRepository.AddItem(model);
                return true;
            }
        }
        public bool EditOrder(Order model)
        {
            if (model == null || string.IsNullOrEmpty(model.User_Id) || model.TotalCount == 0 || model.TotalPrice == 0 || model.Id == 0)
            {
                return false;
            }
            else
            {
                orderRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteOrder(int Id)
        {
            if (Id != 0)
            {
                var orderDetails = orderDetailsRepository.Get(od => od.order_Id == Id).Result.ToList();
                if (orderDetails != null)
                {
                    if (orderDetails.Count() == 0)
                    {

                        if (await orderRepository.DeleteItem(Id))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
    public Order OrderDetail(int Id)
        {
            var order = orderRepository.Get(o => o.Id == Id).Result.FirstOrDefault();
            var orderDetails = orderDetailsRepository.Get(od => od.order_Id == Id, d => d.Product).Result.ToList();
            order.orderDetails = orderDetails;
            return order;
        }
        public List<Order> OrderList()
        {
            var orders = orderRepository.Get().Result.ToList();
            foreach (var order in orders)
            {
                var orderDetails = orderDetailsRepository.Get(od => od.order_Id == order.Id, d => d.Product).Result.ToList();
                order.orderDetails = orderDetails;
            }
            return orders;
        }

    }
}
