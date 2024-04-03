using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.OrderDetailsService;
using DataAccessLayer.Migrations;
using DataAccessLayer.Models;
using DataAccessLayer.services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BusinessLogicLayer.OrderService
{
    public class OrderLogic
    {
        private readonly MainRepository<Order> orderRepository;
        private readonly OrderDetailsLogic orderDetailsLogic;
        public OrderLogic(MainRepository<Order> orderRepository, OrderDetailsLogic orderDetailsLogic)
        {
            this.orderRepository = orderRepository;
            this.orderDetailsLogic = orderDetailsLogic;
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
                var orderDetails = orderDetailsLogic.OrderDetailsByOrderId(Id);
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
            var orderDetails = orderDetailsLogic.OrderDetailsByOrderId(Id);
            order.orderDetails = orderDetails;
            return order;
        }
        public List<Order> OrderList()
        {
            var orders = orderRepository.Get().Result.ToList();
            foreach (var order in orders)
            {
                var orderDetails = orderDetailsLogic.OrderDetailsByOrderId(order.Id);
                order.orderDetails = orderDetails;
            }
            return orders;
        }
        public Order FindOpenOrderByUser(string user_Id)
        {
            var order = new Order();
            if (!string.IsNullOrEmpty(user_Id))
            {
                order = OrderList().Where(o => o.User_Id == user_Id && o.IsFinally == false).FirstOrDefault();
            }
            return order;
        }

    }
}
