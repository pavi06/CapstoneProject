using HospitalManagement.Contexts;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Context
            builder.Services.AddDbContext<HospitalManagementContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"))
                );
            #endregion

            #region Repositories
            builder.Services.AddScoped<IRepository<int, User>, UserRepository>();
            builder.Services.AddScoped<IRepository<int, UserDetails>, UserDetailsRepository>();
            builder.Services.AddScoped<IRepository<int, Doctor>, DoctorRepository>();
            builder.Services.AddScoped<IRepository<int, InPatient>, InPatientRepository>();
            builder.Services.AddScoped<IRepository<int, OutPatient>, OutPatientRepository>();
            builder.Services.AddScoped<IRepository<int, InPatientDetails>, InPatientDetailsRepository>();
            builder.Services.AddScoped<IRepository<int, WardBedAvailability>, WardBedAvailabilityRepository>();
            builder.Services.AddScoped<IRepository<int, Prescription>, PrescriptionRepository>();
            builder.Services.AddScoped<IRepository<int, MedicalRecord>, MedicalRecordRepository>();
            builder.Services.AddScoped<IRepository<int, Bill>, BillRepository>();
            builder.Services.AddScoped<IRepository<int, Appointment>, AppointmentRepository>();
            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
