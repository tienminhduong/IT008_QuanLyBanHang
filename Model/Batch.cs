﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.Model
{
    public class Batch
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("batch_number")]
        public string? BatchNumber { get; set; }
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("price")]
        public string? Price { get; set; }
        [JsonPropertyName("expiration_date")]
        public DateTime ExpirationDate { get; set; }
        [JsonPropertyName("manufacture_date")]
        public DateTime ManufactureDate { get; set; }
    }

    public class BatchData
    {
        [JsonPropertyName("items")]
        public List<Batch>? Items { get; set; }
    }

    public class BatchResponse
    {
        [JsonPropertyName("data")]
        public BatchData? Data { get; set; }
    }
}
