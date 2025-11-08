using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Interfaces;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;
using ProyectoMascotas.Core.Services;
using ProyectoMascotas.Infrastructure.Data;
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



builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));


builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();


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

var app = builder.Build();

//usar Swagger

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Mascotas Perdidas API v1");
        options.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
