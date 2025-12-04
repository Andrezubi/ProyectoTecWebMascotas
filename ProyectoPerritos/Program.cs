using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Custom_Entities;
using ProyectoMascotas.Core.Interfaces;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;
using ProyectoMascotas.Core.Services;
using ProyectoMascotas.Infrastructure.Data;
using ProyectoMascotas.Infrastructure.Filters;
using ProyectoMascotas.Infrastructure.Mappings;
using ProyectoMascotas.Infrastructure.Repositories;
using ProyectoMascotas.Infrastructure.Validators;



namespace ProyectoMascotas.Api
{
    public class Program {
        public static void Main(string[] args) {

            var builder = WebApplication.CreateBuilder(args);


            // Configuración base
            builder.Configuration.Sources.Clear();
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            // User Secrets solo en desarrollo
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
                Console.WriteLine("User Secrets habilitados para desarrollo");
            }

            



            // En producción, los secrets vendrán de Variables de Entorno o Azure Key Vault


            // Add services to the container.


            // ?? Imprimirlo en consola




            builder.Services.AddControllers();
            var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer");
            builder.Services.AddDbContext<MascotasContext>(options => options.UseSqlServer(connectionString));


            Console.WriteLine("?? Connection String cargado: ");
            Console.WriteLine(connectionString ?? "? NO SE ENCONTRÓ ConnectionSqlServer");


            builder.Services.AddAutoMapper(typeof(MappingProfile));



            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));


            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();


            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IFoundPetRepository, FoundPetRepository>();
            builder.Services.AddTransient<ILostPetRepository, LostPetRepository>();
            builder.Services.AddTransient<IPetPhotoRepository, PetPhotoRepository>();
            builder.Services.AddTransient<IMatchRepository, MatchRepository>();

            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IFoundPetService, FoundPetService>();
            builder.Services.AddTransient<ILostPetService, LostPetService>();
            builder.Services.AddTransient<IPetPhotoService, PetPhotoService>();
            builder.Services.AddTransient<IMatchService, MatchService>();
            builder.Services.AddTransient<ISecurityService, SecurityService>();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();

            // Variables de entorno (para producción)
            builder.Configuration.AddEnvironmentVariables();


            //configuracion de swagger 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "Backend Mascotas Perdidas API",
                    Version = "v1",
                    Description = "Documentacion de la API de Mascotas Perdidas - NET 9",
                    Contact = new()
                    {
                        Name = "Andres Zubieta Sempertegui",
                        Email = "andres.zubieta@ucb.edu.bo"
                    }
                });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.EnableAnnotations();

                //habilitar jwt en swagger
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Ingrese: Bearer {token}"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });

            builder.Services.AddControllers(options =>
            {

                options.Filters.Add<ValidationFilter>();
            });

            // FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<UserDTOValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<FoundPetDTOValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<LostPetDTOValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<PetPhotoDTOValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<MatchDTOValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetByIdRequestValidator>();

            // Services
            builder.Services.AddScoped<IValidationService, ValidationService>();

            // Registrar IDbConnectionFactory, UnitOfWork, DapperContext y repos
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();


            builder.Services.AddScoped<IDapperContext, DapperContext>();



            builder.Services.AddApiVersioning(options =>
            {
                // Reporta las versiones soportadas y obsoletas en encabezados de respuesta
                options.ReportApiVersions = true;

                // Versión por defecto si no se especifica
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);

                // Soporta versionado mediante URL, Header o QueryString
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),       // Ejemplo: /api/v1/...
                    new HeaderApiVersionReader("x-api-version"), // Ejemplo: Header ? x-api-version: 1.0
                    new QueryStringApiVersionReader("api-version") // Ejemplo: ?api-version=1.0
                );
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(
                                builder.Configuration["Authentication:SecretKey"]
                            )
                        )
                    };
            });




            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });



            builder.Services.Configure<PasswordOpt>
                (builder.Configuration.GetSection("PasswordOptions"));



            var app = builder.Build();

            //usar Swagger

            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Mascotas Perdidas API v1");
                options.RoutePrefix = string.Empty;
            });
            //}

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }

    }

}



