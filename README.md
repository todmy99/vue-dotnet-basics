# **Vue + .NET Basics (Mini proyecto)** :rocket:

Descripción
- Frontend en Vue 3
- Backend en .NET (Minimal API)
- CRUD de tareas: listar, crear, editar, marcar como hecha y borrar
- Persistencia simple: se guarda en un archivo tasks.json

Requisitos
- Node.js + npm
- .NET SDK

Cómo levantar el backend (API)
1) Abrir una terminal en la raíz del repo
2) Ejecutar:
   cd api\JuniorApi
   dotnet run

3) Swagger:
   http://localhost:5055/swagger
   
Endpoints principales
```
- GET    /api/tasks               -> lista tareas
- POST   /api/tasks               -> crea tarea { "title": "..." }
- PUT    /api/tasks/{id}          -> edita título { "title": "..." }
- PUT    /api/tasks/{id}/toggle   -> cambia IsDone (true/false)
- DELETE /api/tasks/{id}          -> borra tarea
```
Persistencia
- Las tareas se guardan en:
  api\JuniorApi\tasks.json
- Si reiniciás la API, las tareas siguen porque se leen desde ese archivo.

Cómo levantar el frontend (Vue)
1) Abrir otra terminal en la raíz del repo
2) Ejecutar:
   cd web\junior-web
   npm install
   npm run dev

3) Abrir la web (normalmente):
   http://localhost:5173

Notas
- Para que funcione, el backend debe estar corriendo antes de usar la web.
- Si cambia el puerto del backend, actualizá apiBase en App.vue.
