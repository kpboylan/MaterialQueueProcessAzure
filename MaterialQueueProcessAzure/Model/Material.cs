using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialQueueProcessAzure.Model
{
    public class Material
    {
        public int MaterialID { get; set; }
        public string? MaterialName { get; set; }
        public string? Description { get; set; }
        public decimal CurrentStock { get; set; }

        public bool Active { get; set; }

        // foreign keys
        public int UOMId { get; set; }
        public int MaterialType { get; set; }

        public string? CountryCode { get; set; } = "CA";
    }
}
