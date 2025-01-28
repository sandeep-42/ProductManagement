﻿namespace ProductManagementAPI.Data.Entities
{
    internal class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockAvailability { get; set; }
    }
}
