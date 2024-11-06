using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User  : IdentityUser<Guid>
    {
        
        public DateTime CreatedAt { get; set; } 

        public DateTime UpdatedAt { get; set; } 
    }
}
