using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.DTOs
{
    public class OrderDTO
    {
        public int id { get; set; }
        public string? total_amount { get; set; }
        public DateTime order_date { get; set; }
        public string? status { get; set; }
        public List<OrderItemDTO>? order_items { get; set; }
        public CustomerDTO? customer { get; set; }

        public OrderDTO(Order order)
        {
            id = order.Id;
            total_amount = order.TotalAmount.ToString();
            order_date = order.OrderDate;
            status = order.Status;
            if (order.Customer != null)
                customer = new CustomerDTO(order.Customer);
            if (order.OrderItems != null)
            {
                order_items = new List<OrderItemDTO>();
                foreach (var item in order.OrderItems)
                {
                    order_items.Add(new OrderItemDTO(item));
                }
            }
        }

        public OrderDTO()
        {
            
        }

        public class OrderItemDTO
        {
            public int id { get; set; }
            public int batch_id { get; set; }
            public string? price { get; set; }
            public BatchDTO? batch { get; set; }
            public ProductDTO? product { get; set; }
            public int quantity { get; set; }

            public OrderItemDTO(OrderItem orderItem)
            {
                id = orderItem.Id;
                batch_id = orderItem.BatchId;
                price = orderItem.Price.ToString();
                quantity = orderItem.Quantity;
                if (orderItem.Batch != null)
                    batch = new BatchDTO(orderItem.Batch);
                if (orderItem.Product != null)
                    product = new ProductDTO(orderItem.Product);
            }

            public OrderItemDTO()
            {
                
            }
        }
    }
}
