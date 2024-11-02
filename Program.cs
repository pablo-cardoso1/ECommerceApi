using Microsoft.EntityFrameworkCore;
using ECommerceApi.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração da string de conexão
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona serviços ao contêiner
builder.Services.AddControllers();

// Configura o Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce API", Version = "v1" });
});

// Cria a aplicação
var app = builder.Build();

// Habilita o middleware para servir a página de erros durante o desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Habilita o Swagger e o Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API V1");
    c.RoutePrefix = "swagger"; // Altere para string.Empty se quiser acessar na raiz
});

// Configura o middleware para roteamento
app.UseRouting();
app.UseAuthorization();

// Mapeia os controladores
app.MapControllers();

// Inicia a aplicação
app.Run();
