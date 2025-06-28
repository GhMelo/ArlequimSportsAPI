using Application.Interfaces.IService;
using Application.Services;
using ArlequimSportsAPI.Middlewares;
using Domain.Interfaces.IRepository;
using Infrastructure.Configurations.MongoConfigurations;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Carregar configuração externa
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();


#region Configuração de Autenticação e Autorização

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("Vendedor", policy => policy.RequireRole("Vendedor", "Administrador"));
});

#endregion

#region Configuração dos Controllers e Swagger

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Sistema de Cadastro Arlequim", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header. Exemplo : Bearer TokenGeradoAleatoriamente",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

#endregion

#region Configuração do MongoDB

builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));

#endregion

#region Configuração do Entity Framework e Banco de Dados

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
    options.UseLazyLoadingProxies();
}, ServiceLifetime.Scoped);

#endregion

#region Registro de Repositórios e Serviços

builder.Services.AddScoped<IEsporteModalidadeRepository, EsporteModalidadeRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IProdutoEstoqueRepository, ProdutoEstoqueRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IStatusPedidoRepository, StatusPedidoRepository>();
builder.Services.AddScoped<ITipoOperacaoRepository, TipoOperacaoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPedidoProdutoRepository, PedidoProdutoRepository>();
builder.Services.AddScoped<IProdutoEstoqueMovimentacaoRepository, ProdutoEstoqueMovimentacaoRepository>();
builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEsporteModalidadeService, EsporteModalidadeService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IProdutoEstoqueService, ProdutoEstoqueService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IStatusPedidoService, StatusPedidoService>();
builder.Services.AddScoped<ITipoOperacaoService, TipoOperacaoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

#endregion

#region Configuração do Pipeline HTTP

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<DatabaseLoggingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

#endregion
