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

        public DbSet<User> Users { get; set; }
        public DbSet<UserLoginDetails> UserLoginDetails { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<AdmissionDetails> AdmissionDetails { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<MedicineMaster> MedicineMaster { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<WardRoomsAvailability> WardBedAvailabilities { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailability { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasIndex(x => x.ContactNo)
            .IsUnique();

            modelBuilder.Entity<DoctorAvailability>().HasKey(da => new { da.DoctorId, da.AppointmentDate });

            modelBuilder.Entity<Doctor>()
            .Property(d => d.AvailableDays)
            .HasConversion(v => string.Join(",", v),
              v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
             );

            modelBuilder.Entity<Doctor>()
            .Property(d => d.LanguagesKnown)
            .HasConversion(v => string.Join(",", v),
              v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
              );

            modelBuilder.Entity<Doctor>()
            .Property(d => d.Slots)
            .HasConversion(v => string.Join(",", v),
              v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s=> TimeOnly.FromTimeSpan(TimeSpan.Parse(s))).ToList()
              );

            modelBuilder.Entity<DoctorAvailability>()
            .Property(d => d.AvailableSlots)
            .HasConversion(v => string.Join(",", v),
              v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => TimeOnly.FromTimeSpan(TimeSpan.Parse(s))).ToList()
              );

            modelBuilder.Entity<MedicineMaster>()
            .Property(d => d.DosagesAvailable)
            .HasConversion(v => string.Join(",", v),
              v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
             );

            modelBuilder.Entity<MedicineMaster>()
            .Property(d => d.FormsAvailable)
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

            modelBuilder.Entity<Admission>()
              .HasOne(ad => ad.Patient)
              .WithMany(p => p.Admissions)
              .HasForeignKey(ad => ad.PatientId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Admission>()
              .HasOne(ad => ad.DoctorInCharge)
              .WithMany(p => p.Admissions)
              .HasForeignKey(ad => ad.DoctorId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AdmissionDetails>()
              .HasOne(ad => ad.Admission)
              .WithMany(a => a.AdmissionDetails)
              .HasForeignKey(ad => ad.AdmissionId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalRecord>()
             .HasOne(mr => mr.Doctor)
             .WithMany(d => d.MedicalRecords)
             .HasForeignKey(mr => mr.DoctorId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalRecord>()
             .HasOne(mr => mr.Patient)
             .WithMany(d => d.MedicalRecords)
             .HasForeignKey(mr => mr.PatientId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
             .HasOne(r => r.WardBed)
             .WithMany(b => b.Rooms)
             .HasForeignKey(r => r.WardTypeId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prescription>()
               .HasOne(p => p.Patient)
               .WithMany(pa => pa.Prescriptions)
               .HasForeignKey(p => p.PatientId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Medication>()
               .HasOne(a => a.Prescription)
               .WithMany(p => p.Medications)
               .HasForeignKey(a => a.PrescriptionId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
             .HasOne(b => b.Patient)
             .WithMany(p => p.Bills)
             .HasForeignKey(b => b.PatientId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
             .HasOne(p => p.Bill)
             .WithMany(b => b.Payments)
             .HasForeignKey(p => p.BillId)
             .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
