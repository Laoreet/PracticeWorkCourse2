using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("donation_types")]
    [Display(Name = "Вид пожертвования")]
    public partial class DonationType
    {
        public DonationType()
        {
            Donations = new HashSet<Donation>();
        }

        [Key]
        [Column("donation_type_id", TypeName = "int(11)")]
        [Display(Name = "ID Вида пожертвования")]
        public int DonationTypeId { get; set; }
        [Required(ErrorMessage = "Введите корректное описание вида пожертвования!")]
        [Column("donation_type_description")]
        [Display(Name = "Описание вида пожертвования")]
        [StringLength(125)]
        public string DonationTypeDescription { get; set; }

        [InverseProperty(nameof(Donation.DonationType))]
        public virtual ICollection<Donation> Donations { get; set; }
    }
}
