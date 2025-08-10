using BusinessLogicLayer.ProductService;
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
        private readonly ProductLogic _productLogic;
        public OrderDetailsLogic(MainRepository<OrderDetails> OrderDetailsRepository, ProductLogic productLogic)
        {
            this.OrderDetailsRepository = OrderDetailsRepository;
            _productLogic = productLogic;
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
        public async Task<OrderDetails> OrderDetailsDetail(int Id)
        {
            var OrderDetails = OrderDetailsRepository.Get(o => o.Id == Id,o => o.order).FirstOrDefault();
            if(OrderDetails != null)
            {
                OrderDetails.Product = await _productLogic.ProductDetail(OrderDetails.Product_Id);
            }
            return OrderDetails;
        }
        public async Task<List<OrderDetails>> OrderDetailsList()
        {
            var OrderDetailss = OrderDetailsRepository.Get(null,o => o.Product).ToList();
            foreach(var orderdetail in OrderDetailss)
            {
                orderdetail.Product = await _productLogic.ProductDetail(orderdetail.Product_Id);
            }
            return OrderDetailss;
        }
        public async Task<List<OrderDetails>> OrderDetailsByOrderId(int order_Id)
        {
            var orderDetailsList = await OrderDetailsList();
            var orderdetails = orderDetailsList.Where(od => od.order_Id == order_Id).ToList();
            return orderdetails;
        }
    }
}
