﻿using GIP.PRJ.TraiteurApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIP.PRJ.TraiteurApp.ViewModels.Admin
{
    public class UserViewModel
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public List<IdentityRole> UserRoles { get; set; }
    }
}
