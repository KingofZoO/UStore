using System;

namespace UStore.Models {
    public class OrdersModel {
        public string UserName { get; set; }
        public string Product { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}
