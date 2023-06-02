using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("donators")]
    [Display(Name = "Благотворитель")]
    public partial class Donator
    {
        public Donator()
        {
            Donations = new HashSet<Donation>();
        }

        [Key]
        [Column("donator_id", TypeName = "int(11)")]
        [Display(Name = "ID Благотворителя")]
        public int DonatorId { get; set; }
        [Required(ErrorMessage = "Введите корректный никнейм благотворителя!")]
        [Column("donator_nickname")]
        [Display(Name = "Никнейм благотворителя")]
        [StringLength(64)]
        public string DonatorNickname { get; set; }

        [InverseProperty(nameof(Donation.Donator))]
        public virtual ICollection<Donation> Donations { get; set; }
    }
}
