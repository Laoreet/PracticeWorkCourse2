using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("users")]
    public partial class User
    {
        [Key]
        [Column("user_id", TypeName = "int(11)")]
        [Display(Name = "ID Пользователя")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Введите корректный логин!")]
        [Column("login")]
        [Display(Name = "Логин")]
        [StringLength(30)]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите корректный пароль!")]
        [Column("password")]
        [Display(Name = "Пароль")]
        [StringLength(30)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Введитие корректную электронную почту!")]
        [Column("email")]
        [Display(Name = "Электронная почта")]
        [StringLength(50)]
        public string Email { get; set; }


        [Column("level")]
        [Display(Name = "Права")]
        [DefaultValue("client")]
        [StringLength(30)]
        public string Level { get; set; }

    }
}
