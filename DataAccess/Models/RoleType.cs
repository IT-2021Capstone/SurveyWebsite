using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class RoleType
    {
        public RoleType()
        {
            Logins = new HashSet<Login>();
        }

        public int RoleId { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Login> Logins { get; set; }
    }
}
