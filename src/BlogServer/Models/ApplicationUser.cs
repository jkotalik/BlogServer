﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlogServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string EmailAddress { get; set; }
        public string NameIdentifier { get; set; }
        public string LoginName { get; set; }
    }
}
