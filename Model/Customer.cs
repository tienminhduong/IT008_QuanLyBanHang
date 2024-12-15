using IT008_QuanLyBanHang.DTOs;
using IT008_QuanLyBanHang.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public float Point { get; set; }
        public string? FullName { get; set; }
        public List<Order>? Orders { get; set; } = null;

        public Customer(CustomerDTO dto)
        {
            Id = dto.id;
            FirstName = dto.first_name;
            LastName = dto.last_name;
            Phone = dto.phone;
            Point = dto.point;
            FullName = $"{LastName} {FirstName}";
        }
        public Customer()
        {
            
        }

        public void UpdateOrderList(List<Order> orders)
        {
            Orders = new();
            foreach (Order order in orders)
            {
                if (order.Customer != null && order.Customer.Id == Id)
                    Orders.Add(order);
            }
        }
    }
}
