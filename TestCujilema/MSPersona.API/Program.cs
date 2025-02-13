using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MSPersona.Data;
using MSPersona.Dominio.Interfaces;
using MSPersona.Services;
using Serilog;
using System.Text;
using NuGetCommon;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

//cadena de conexion (se utiliza variables de entorno)
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ??
                        $"server={Environment.GetEnvironmentVariable("DB_SERVER")},1433; " + 
                        $"Database={Environment.GetEnvironmentVariable("DB_NAME")}; " +
                        $"User ID={Environment.GetEnvironmentVariable("DB_USER")}; " +
                        $"password={Environment.GetEnvironmentVariable("DB_PASSWORD")}; " +
                        $"TrustServerCertificate=True";
Console.WriteLine($"Cadena de conexi0n: {connectionString}");

// Agregar la cadena de conexi0n en la configuraci0n
builder.Configuration["ConnectionStrings:DBConnection"] = connectionString;

builder.WebHost.UseUrls("http://0.0.0.0:80");

//Configuraci0n de CORS policy:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AccesoPersona", policy =>
    {
        policy.WithOrigins("http://registro_usuario_jwt:4200", "http://login:4201", "http://localhost:4200", "http://localhost:4201")
              .AllowAnyHeader()   // Permite cualquier encabezado
              .AllowAnyMethod();  // Permite cualquier m3todo HTTP
    });
});

//Agregando servicios al contenedor:
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});


// AppSettings 
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppiSettings>(appSettingsSection);

// JWT
var appSettings = appSettingsSection.Get<AppiSettings>(); //here
var llave = Encoding.ASCII.GetBytes(appSettings.Secreto);
builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(d=>
    {
        d.RequireHttpsMetadata = false;
        d.SaveToken = true;
        d.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(llave),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
 

//Inyecci0n de dependencia:
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();


//UseSerilog
builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration) 
            .ReadFrom.Services(services)
            .MinimumLevel.Information()
            .WriteTo.Console());


//Logging configuration:
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // Esto permite conexiones en todas las interfaces de red
});

// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ListenAnyIP(443, listenOptions =>
//     {
//         listenOptions.UseHttps();  // Configurar Kestrel para usar HTTPS en el puerto 443
//     });
// });


var app = builder.Build();

app.UseCors("AccesoPersona");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = string.Empty;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
