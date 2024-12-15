using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.DTOs
{
    public class UploadImageResponesDTO
    {
        public string signature { get; set; }
        public string api_key { get; set; }
        public ulong timestamp { get; set; }
        public string public_id { get; set; }
        public string cloud_name { get; set; }
    }
}
