using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApplication2.DataObjects;

#nullable disable

namespace WebApplication2.Models
{
    public partial class animal_shelter_testContext : DbContext
    {
        public animal_shelter_testContext()
        {
        }

        public animal_shelter_testContext(DbContextOptions<animal_shelter_testContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<AnimalApplication> AnimalApplications { get; set; }
        public virtual DbSet<AnimalType> AnimalTypes { get; set; }
        public virtual DbSet<CarrierVolunteer> CarrierVolunteers { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<DonationType> DonationTypes { get; set; }
        public virtual DbSet<Donator> Donators { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }
        public virtual DbSet<VolunteerApplication> VolunteerApplications { get; set; }
        public virtual DbSet<VolunteerGroup> VolunteerGroups { get; set; }
        public virtual DbSet<VolunteerOrganization> VolunteerOrganizations { get; set; }
        public virtual DbSet<Worker> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=;database=animal_shelter_test");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>(entity =>
            {
                entity.Property(e => e.Breed).HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Coloring).HasDefaultValueSql("'NULL'");

                entity.Property(e => e.History).HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Photo).HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Specificity).HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.AnimalType)
                    .WithMany(p => p.Animals)
                    .HasForeignKey(d => d.AnimalTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("animals_ibfk_1");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.Animals)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("animals_ibfk_2");
            });

            modelBuilder.Entity<AnimalApplication>(entity =>
            {
                entity.HasOne(d => d.Animal)
                    .WithMany(p => p.AnimalApplications)
                    .HasForeignKey(d => d.AnimalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("animal_applications_ibfk_1");
            });

            modelBuilder.Entity<CarrierVolunteer>(entity =>
            {
                entity.Property(e => e.Vehicle).HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.CarrierVolunteers)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("carrier_volunteers_ibfk_1");
            });

            modelBuilder.Entity<Donation>(entity =>
            {
                entity.Property(e => e.DonatorId).HasDefaultValueSql("'1'");

                entity.HasOne(d => d.DonationType)
                    .WithMany(p => p.Donations)
                    .HasForeignKey(d => d.DonationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("donations_ibfk_2");

                entity.HasOne(d => d.Donator)
                    .WithMany(p => p.Donations)
                    .HasForeignKey(d => d.DonatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("donations_ibfk_1");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.Donations)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("donations_ibfk_3");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Level).HasDefaultValueSql("'''client'''");
            });

            modelBuilder.Entity<Volunteer>(entity =>
            {
                entity.Property(e => e.VolunteerOrgId).HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.VolunteerGroup)
                    .WithMany(p => p.Volunteers)
                    .HasForeignKey(d => d.VolunteerGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("volunteers_ibfk_2");

                entity.HasOne(d => d.VolunteerOrg)
                    .WithMany(p => p.Volunteers)
                    .HasForeignKey(d => d.VolunteerOrgId)
                    .HasConstraintName("volunteers_ibfk_3");
            });

            modelBuilder.Entity<VolunteerApplication>(entity =>
            {
                entity.Property(e => e.VolunteerOrgId).HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.VolunteerGroup)
                    .WithMany(p => p.VolunteerApplications)
                    .HasForeignKey(d => d.VolunteerGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("volunteer_applications_ibfk_2");

                entity.HasOne(d => d.VolunteerOrg)
                    .WithMany(p => p.VolunteerApplications)
                    .HasForeignKey(d => d.VolunteerOrgId)
                    .HasConstraintName("volunteer_applications_ibfk_3");
            });

            modelBuilder.Entity<VolunteerGroup>(entity =>
            {
                entity.Property(e => e.WorkType).HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.VolunteerGroups)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("volunteer_groups_ibfk_1");
            });

            modelBuilder.Entity<VolunteerOrganization>(entity =>
            {
                entity.HasKey(e => e.VolunteerOrgId)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.Property(e => e.Age).HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("workers_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
