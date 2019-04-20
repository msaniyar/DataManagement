using System;
using System.Collections.Generic;

namespace DataManagement.Models
{
    /// <summary>
    /// This is the model of incoming requests.
    /// </summary>
    public partial class TreeListTable
    {
        public Guid Id { get; set; }
        public string Tree { get; set; }
    }
}
