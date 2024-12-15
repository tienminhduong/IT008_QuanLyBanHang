using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.DTOs
{
    public class CustomerDTO
    {
        public int id { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? phone { get; set; }
        public float point { get; set; }

        public CustomerDTO(Customer customer)
        {
            id = customer.Id;
            first_name = customer.FirstName;
            last_name = customer.LastName;
            phone = customer.Phone;
            point = customer.Point;
        }
        public CustomerDTO()
        {
            
        }
    }
}
