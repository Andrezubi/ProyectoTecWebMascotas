using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Interfaces;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;
using ProyectoMascotas.Core.Services;
using ProyectoMascotas.Infrastructure.Filters;
using ProyectoMascotas.Infrastructure.Mappings;
using ProyectoMascotas.Infrastructure.Repositories;
using ProyectoMascotas.Infrastructure.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer");
builder.Services.AddDbContext<MascotasContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IFoundPetRepository, FoundPetRepository>();
builder.Services.AddTransient<ILostPetRepository, LostPetRepository>();
builder.Services.AddTransient<IPetPhotoRepository, PetPhotoRepository>();
builder.Services.AddTransient<IMatchRepository, MatchRepository>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IFoundPetService,FoundPetService>();
builder.Services.AddTransient<ILostPetService, LostPetService>();
builder.Services.AddTransient<IPetPhotoService, PetPhotoService>();
builder.Services.AddTransient<IMatchService, MatchService>();





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




builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
