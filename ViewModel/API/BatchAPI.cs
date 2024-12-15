using IT008_QuanLyBanHang.DTOs;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel.API
{
    public class BatchAPI : BaseAPI<Batch, BatchDTO>
    {
        public static async Task<List<Batch>> GetAllBatches()
        {
            List<BatchDTO> dtos = await GetAllItemsDTO();
            List<Batch> resultList = new();

            foreach (var dto in dtos)
            {
                Batch? batch = ConvertFromDTO(dto);
                if (batch != null)
                    resultList.Add(batch);
            }
            return resultList;
        }

        public static BatchDTO? ConvertToDTO(Batch batch)
        {
            BatchDTO batchDTO = new(batch);
            return batchDTO;
        }

        public static Batch? ConvertFromDTO(BatchDTO batchDTO)
        {
            Batch batch = new(batchDTO);
            return batch;
        }
    }
}
