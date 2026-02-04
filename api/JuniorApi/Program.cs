var builder = WebApplication.CreateBuilder(args);

// 1)permite usar "Controllers" (clases que reciben requests)
builder.Services.AddControllers();

// 2)habilita que Swagger pueda "descubrir" tus endpoints
builder.Services.AddEndpointsApiExplorer();

// 3)habilita Swagger (la página /swagger)
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4)swagger solo se muestra en Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();      //genera el JSON de swagger
    app.UseSwaggerUI();    //muestra la página web de swagger
}

app.UseAuthorization();

// 5)conecta tus controllers con las rutas (ej: /api/...)
app.MapControllers();

app.Run();
