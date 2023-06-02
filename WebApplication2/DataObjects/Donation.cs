using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("donations")]
    [Index(nameof(DonationTypeId), Name = "donation_type_id")]
    [Index(nameof(DonatorId), Name = "donator_id")]
    [Index(nameof(StationId), Name = "shelter_id")]
    public partial class Donation
    {
        [Key]
        [Column("donation_id", TypeName = "int(11)")]
        [Display(Name = "ID Пожертвования")]
        public int DonationId { get; set; }
        [Column("donator_id", TypeName = "int(11)")]
        [Display(Name = "ID Благотворителя")]
        public int DonatorId { get; set; }
        [Column("donation_type_id", TypeName = "int(11)")]
        [Display(Name = "ID Вида пожертвования")]
        public int DonationTypeId { get; set; }
        [Column("station_id", TypeName = "int(11)")]
        [Display(Name = "ID Станции")]
        public int StationId { get; set; }
        [Column("donation_amount", TypeName = "int(11)")]
        [Display(Name = "Величина пожертвования")]
        public int DonationAmount { get; set; }

        [ForeignKey(nameof(DonationTypeId))]
        [InverseProperty("Donations")]
        public virtual DonationType DonationType { get; set; }
        [ForeignKey(nameof(DonatorId))]
        [InverseProperty("Donations")]
        public virtual Donator Donator { get; set; }
        [ForeignKey(nameof(StationId))]
        [InverseProperty("Donations")]
        public virtual Station Station { get; set; }
    }
}
