using IT008_QuanLyBanHang.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IT008_QuanLyBanHang.DTOs.OrderDTO;

namespace IT008_QuanLyBanHang.Model
{
    public class Order
    {
        public int Id { get; set; }
        public float TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
        public Customer? Customer { get; set; }
        public List<OrderItem>? OrderItems { get; set; }

        public Order(OrderDTO dto, List<Batch> batches, List<Category> categories)
        {
            Id = dto.id;
            TotalAmount = float.Parse(dto.total_amount ?? "0");
            OrderDate = dto.order_date;
            Status = dto.status;
            if (dto.customer != null)
                Customer = new Customer(dto.customer);
            if (dto.order_items != null)
            {
                OrderItems = new List<OrderItem>();
                foreach (var item in dto.order_items)
                {
                    OrderItems.Add(new OrderItem(item, batches, categories));
                }
            }
        }
        public Order()
        {
            
        }
    }
    public class OrderItem
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public float Price { get; set; }
        public Batch? Batch { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }

        public OrderItem(OrderItemDTO dto, List<Batch> batches, List<Category> categories)
        {
            Id = dto.id;
            BatchId = dto.batch_id;
            Price = float.Parse(dto.price ?? "0");
            Quantity = dto.quantity;
            if (dto.batch != null)
                Batch = new Batch(dto.batch);
            if (dto.product != null)
                Product = new Product(dto.product, batches, categories);
        }

        public OrderItem()
        {
            
        }
    }
}
