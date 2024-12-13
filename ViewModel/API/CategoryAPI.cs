using IT008_QuanLyBanHang.DTOs;
using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel.API
{
    public class CategoryAPI : BaseAPI<Category, CategoryDTO>
    {
        public static async Task<List<Category>> GetAllCategories()
        {
            var dtos = await GetAllItemsDTO();
            List<Category> categories = new();
            foreach (var dto in dtos)
            {
                Category? category = ConvertFromDTO(dto);
                if (category != null)
                    categories.Add(category);
            }
            return categories;
        }

        public static CategoryDTO? ConvertToDTO(Category category)
        {
            CategoryDTO categoryDTO = new()
            {
                id = category.Id,
                category_name = category.CategoryName
            };
            return categoryDTO;
        }

        public static Category? ConvertFromDTO(CategoryDTO dto)
        {
            Category category = new()
            {
                Id = dto.id,
                CategoryName = dto.category_name
            };
            return category;
        }
    }
}