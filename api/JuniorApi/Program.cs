using JuniorApi.Models;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

var filePath = Path.Combine(app.Environment.ContentRootPath, "tasks.json");

List<TaskItem> LoadTasks()
{
    if (!File.Exists(filePath)) return new List<TaskItem>();

    var json = File.ReadAllText(filePath);
    return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
}

void SaveTasks(List<TaskItem> list)
{
    var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(filePath, json);
}

var tasks = LoadTasks();


//GET /api/tasks
app.MapGet("/api/tasks", () => tasks);

//POST /api/tasks
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
    SaveTasks(tasks);
    return task;
});

//PUT /api/tasks/{id}/toggle  -> invierte IsDone (false->true, true->false)
app.MapPut("/api/tasks/{id}/toggle", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null) return Results.NotFound();

    task.IsDone = !task.IsDone;
    SaveTasks(tasks);
    return Results.Ok(task);
});

//DELETE /api/tasks/{id} -> borra una tarea por id
app.MapDelete("/api/tasks/{id}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null) return Results.NotFound();

    tasks.Remove(task);
    SaveTasks(tasks);
    return Results.NoContent(); // 204 (borrado OK sin devolver nada)
});

//PUT /api/tasks/{id}  -> actualiza el título
app.MapPut("/api/tasks/{id}", (int id, TaskItem input) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null) return Results.NotFound();

    var newTitle = input.Title?.Trim();
    if (string.IsNullOrWhiteSpace(newTitle))
        return Results.BadRequest("Title is required");

    task.Title = newTitle;
    SaveTasks(tasks);
    return Results.Ok(task);
});

app.Run();
