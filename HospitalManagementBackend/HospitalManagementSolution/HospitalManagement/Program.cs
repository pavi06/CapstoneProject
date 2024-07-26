using Hangfire;
using HospitalManagement.Contexts;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using HospitalManagement.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HospitalManagement
{
    public class Program
    {

        public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
        {
            public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateOnly.FromDateTime(reader.GetDateTime());
            }

            public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            {
                var isoDate = value.ToString("O");
                writer.WriteStringValue(isoDate);
            }
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddLogging(l => l.AddLog4Net());

            #region HangfireService
            builder.Services.AddHangfire((sp, config) =>
            {
                var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("hangfireConnection");
                config.UseSqlServerStorage(connectionString);
            });

            builder.Services.AddHangfireServer();
            #endregion

            #region SwaggerGen
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
            });

            #endregion

            #region Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = "Role",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                });
            #endregion

            #region CORS
            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("MyCors", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            #endregion

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
            builder.Services.AddScoped<IRepositoryForCompositeKey<int,DateTime, DoctorAvailability>, DoctorAvailabilityRepository>();
            #endregion

            #region Services
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IHospitalBasicService, HospitalBasicService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IReceptionistService, ReceptionistService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            #endregion

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("MyCors");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
