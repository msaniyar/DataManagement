using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace DataManagement.Models
{
    public partial class DataTable
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public JObject Tree { get; set; }
    }
}
