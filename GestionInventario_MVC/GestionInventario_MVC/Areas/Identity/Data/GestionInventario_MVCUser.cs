using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GestionInventario_MVC.Areas.Identity.Data
{
    public class GestionInventario_MVCUser : IdentityUser
    {

        public string FullName { get; set; }
    }
}
