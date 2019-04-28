    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Models
{
    public class AuthenticatedUser: BaseModel
    {
        public User User { get; set; }
        public List<Section> Menu { get; set; }
    }
}
