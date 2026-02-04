using JuniorApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CORS (tiene que ir ANTES de builder.Build())
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ Activar CORS (esto va DESPUÉS del Build)
app.UseCors();

// "Base de datos" en memoria
var tasks = new List<TaskItem>
{
    new TaskItem { Id = 1, Title = "Aprender Vue", IsDone = false },
    new TaskItem { Id = 2, Title = "Aprender .NET", IsDone = false }
};

// GET /api/tasks
app.MapGet("/api/tasks", () => tasks);

// POST /api/tasks
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

// PUT /api/tasks/{id}/toggle  -> invierte IsDone (false->true, true->false)
app.MapPut("/api/tasks/{id}/toggle", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null) return Results.NotFound();

    task.IsDone = !task.IsDone;
    return Results.Ok(task);
});

// DELETE /api/tasks/{id} -> borra una tarea por id
app.MapDelete("/api/tasks/{id}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null) return Results.NotFound();

    tasks.Remove(task);
    return Results.NoContent(); // 204 (borrado OK sin devolver nada)
});

app.Run();
