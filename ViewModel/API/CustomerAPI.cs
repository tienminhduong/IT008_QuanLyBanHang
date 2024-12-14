using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using IT008_QuanLyBanHang.DTOs;

namespace IT008_QuanLyBanHang.ViewModel.API
{
    public class CustomerAPI : BaseAPI<Customer, CustomerDTO>
    {
        static public async Task<List<Customer>> GetAllCustomer()
        {
            var dtos = await GetAllItemsDTO();
            List<Customer> customers = new();
            foreach (var dto in dtos)
            {
                Customer? customer = ConvertFromDTO(dto);
                if (customer != null)
                    customers.Add(customer);
            }
            return customers;
        }

        static public CustomerDTO? ConvertToDTO(Customer customer)
        {
            return new(customer);
        }

        static public Customer? ConvertFromDTO(CustomerDTO dto)
        {
            return new(dto);
        }
    }
}
