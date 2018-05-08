using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MacLookup.Models
{
    [Table("Mac")]
    public class Mac {
        [Column("Registry")]
        public string Registry { get; set; }
        [PrimaryKey, Column("Assignment"), Indexed]
        public string Assignment { get; set; }
        [Column("OrganizationName")]
        public string OrganizationName { get; set; }
        [Column("OrganizationAddress")]
        public string OrganizationAddress { get; set; }
    }
}
