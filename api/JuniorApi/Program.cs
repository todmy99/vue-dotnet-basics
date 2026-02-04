using JuniorApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// "Base de datos" en memoria (se reinicia si parás la API)
var tasks = new List<TaskItem>
{
    new TaskItem { Id = 1, Title = "Aprender Vue", IsDone = false },
    new TaskItem { Id = 2, Title = "Aprender .NET", IsDone = false }
};

// GET /api/tasks -> devuelve la lista
app.MapGet("/api/tasks", () =>
{
    return tasks;
});

// POST /api/tasks -> agrega una tarea nueva
app.MapPost("/api/tasks", (TaskItem input) =>
{
    var newId = tasks.Count == 0 ? 1 : tasks.Max(t => t.Id) + 1;

    var task = new TaskItem
    {
        Id = newId,
        Title = input.Title,
        IsDone = false
    };

    tasks.Add(task);
    return task;
});

app.Run();