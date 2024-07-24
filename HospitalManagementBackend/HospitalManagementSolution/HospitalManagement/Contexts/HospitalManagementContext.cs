using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Xml;

namespace HospitalManagement.Contexts
{
    public class HospitalManagementContext : DbContext
    {
        public HospitalManagementContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<OutPatient> OutPatients { get; set; }
        public DbSet<InPatient> InPatients { get; set; }
        public DbSet<InPatientDetails> InPatientDetails { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<WardBedAvailability> WardBedAvailabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoctorAvailability>().HasKey(da => new { da.DoctorId, da.Date });

            modelBuilder.Entity<Doctor>()
            .Property(d => d.AvailableDays)
            .HasConversion(v => string.Join(",", v),
              v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
             );

            modelBuilder.Entity<Doctor>()
            .Property(d => d.Slots)
            .HasConversion(v => string.Join(",", v),
              v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
              );

            modelBuilder.Entity<DoctorAvailability>()
            .Property(d => d.AvailableSlots)
            .HasConversion(v => string.Join(",", v),
              v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
              );

            modelBuilder.Entity<Appointment>()
            .Property(a => a.Slot)
            .HasConversion(
                t => TimeSpan.Parse(t.ToString()),
                t => new TimeOnly(t.Hours, t.Minutes, t.Seconds)
            );

            modelBuilder.Entity<Doctor>()
            .Property(a => a.ShiftStartTime)
            .HasConversion(
                t => TimeSpan.Parse(t.ToString()),
                t => new TimeOnly(t.Hours, t.Minutes, t.Seconds)
            );

            modelBuilder.Entity<Doctor>()
           .Property(a => a.ShiftEndTime)
           .HasConversion(
               t => TimeSpan.Parse(t.ToString()),
               t => new TimeOnly(t.Hours, t.Minutes, t.Seconds)
           );

            modelBuilder.Entity<Appointment>()
               .HasOne(a => a.Patient)
               .WithMany(p => p.Appointments)
               .HasForeignKey(a => a.PatientId)
               .OnDelete(DeleteBehavior.Restrict);
               
            modelBuilder.Entity<Appointment>()
               .HasOne(a => a.Doctor)
               .WithMany(d => d.Appointments)
               .HasForeignKey(a => a.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InPatientDetails>()
              .HasOne(pd => pd.InPatient)
              .WithMany(p => p.InPatientDetails)
              .HasForeignKey(pd => pd.InPatientId)
              .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<MedicalRecord>()
            // .HasOne(mr => mr.Patient)
            // .WithMany(p => p.MedicalRecords)
            // .HasForeignKey(mr => mr.PatientId)
            // .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalRecord>()
             .HasOne(mr => mr.Doctor)
             .WithMany(d => d.MedicalRecords)
             .HasForeignKey(mr => mr.DoctorId)
             .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Bill>()
            //   .HasOne(b => b.Patient)
            //   .WithMany(p => p.Bills)
            //   .HasForeignKey(b => b.PatientId)
            //   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
             .HasOne(r => r.WardBed)
             .WithMany(b => b.Rooms)
             .HasForeignKey(r => r.WardTypeId)
             .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
