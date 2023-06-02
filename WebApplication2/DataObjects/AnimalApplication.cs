using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("animal_applications")]
    [Index(nameof(AnimalId), Name = "animal_id")]
    public partial class AnimalApplication
    {
        [Key]
        [Column("animal_application_id", TypeName = "int(11)")]
        [Display(Name = "ID Заявки на животное")]
        public int AnimalApplicationId { get; set; }
        [Column("animal_id", TypeName = "int(11)")]
        [Display(Name = "ID Животного")]
        public int AnimalId { get; set; }
        [Required(ErrorMessage = "Необходимо указать имя!")]
        [Column("firstname")]
        [Display(Name = "Имя")]
        [StringLength(40)]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Необходимо указать пол!")]
        [Column("sex")]
        [Display(Name = "Пол")]
        [StringLength(3)]
        public string Sex { get; set; }

        [Required(ErrorMessage = "Необходимо указать возраст!")]
        [Column("age", TypeName = "int(3)")]
        [Display(Name = "Возраст")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Необходимо указать номер телефона!")]
        [Column("phone_number", TypeName = "bigint(12)")]
        [Display(Name = "Номер телефона")]
        public long PhoneNumber { get; set; }

        [ForeignKey(nameof(AnimalId))]
        [InverseProperty("AnimalApplications")]
        public virtual Animal Animal { get; set; }
    }
}
