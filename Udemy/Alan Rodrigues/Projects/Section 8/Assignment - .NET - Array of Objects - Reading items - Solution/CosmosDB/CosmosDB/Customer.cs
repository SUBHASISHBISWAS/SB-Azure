using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDB
{
    public class Customer
    {
        [JsonProperty("id")]
        public string customerId { get; set; }
        
        public string customerName { get; set; }
        public string customerCity { get; set; }
        public List<Order> orders {get; set; }
    }
}
