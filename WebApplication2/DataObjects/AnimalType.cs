using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("animal_types")]
    [Display(Name = "Вид животного")]
    public partial class AnimalType
    {
        public AnimalType()
        {
            Animals = new HashSet<Animal>();
        }

        [Key]
        [Column("animal_type_id", TypeName = "int(11)")]
        [Display(Name = "ID Вида")]
        public int AnimalTypeId { get; set; }
        [Required(ErrorMessage = "Введите корректное название вида!")]
        [Column("animal_type_name")]
        [Display(Name = "Название вида")]
        [StringLength(40)]
        public string AnimalTypeName { get; set; }

        [InverseProperty(nameof(Animal.AnimalType))]
        public virtual ICollection<Animal> Animals { get; set; }
    }
}
