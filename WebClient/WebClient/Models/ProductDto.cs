﻿namespace WebClient.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Price { get; set; }
        public int Stock { get; set; }
    }
}
