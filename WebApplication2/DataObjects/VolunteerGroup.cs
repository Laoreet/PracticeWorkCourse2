using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("volunteer_groups")]
    [Display(Name = "Волонтерская группа")]
    [Index(nameof(StationId), Name = "volunteer_groups_ibfk_1")]
    public partial class VolunteerGroup
    {
        public VolunteerGroup()
        {
            VolunteerApplications = new HashSet<VolunteerApplication>();
            Volunteers = new HashSet<Volunteer>();
        }

        [Key]
        [Column("volunteer_group_id", TypeName = "int(11)")]
        [Display(Name = "ID Волонтерской группы")]
        public int VolunteerGroupId { get; set; }
        [Column("station_id", TypeName = "int(11)")]
        [Display(Name = "ID Станции")]
        public int StationId { get; set; }
        [Required(ErrorMessage = "Введите корректный вид работы!")]
        [Column("work_type")]
        [Display(Name = "Вид работы")]
        [StringLength(50)]
        public string WorkType { get; set; }

        [ForeignKey(nameof(StationId))]
        [InverseProperty("VolunteerGroups")]
        public virtual Station Station { get; set; }
        [InverseProperty(nameof(VolunteerApplication.VolunteerGroup))]
        public virtual ICollection<VolunteerApplication> VolunteerApplications { get; set; }
        [InverseProperty(nameof(Volunteer.VolunteerGroup))]
        public virtual ICollection<Volunteer> Volunteers { get; set; }
    }
}
