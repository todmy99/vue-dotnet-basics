<script setup>
import { ref, onMounted } from "vue";

// --- Contador (local) ---
const count = ref(0);
function increment() {
  count.value++;
}

// --- Tareas (desde la API .NET) ---
const apiBase = "http://localhost:5055";

const tasks = ref([]);
const title = ref("");
const error = ref("");

// --- Edición ---
const editingId = ref(null);
const editTitle = ref("");

async function loadTasks() {
  error.value = "";
  try {
    const res = await fetch(`${apiBase}/api/tasks`);
    tasks.value = await res.json();
  } catch {
    error.value = "No pude cargar tareas. ¿Está corriendo la API?";
  }
}

async function addTask() {
  const value = title.value.trim();
  if (!value) return;

  error.value = "";
  try {
    await fetch(`${apiBase}/api/tasks`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ title: value })
    });

    title.value = "";
    await loadTasks();
  } catch {
    error.value = "No pude crear la tarea.";
  }
}

async function toggleTask(id) {
  error.value = "";
  try {
    await fetch(`${apiBase}/api/tasks/${id}/toggle`, { method: "PUT" });
    await loadTasks();
  } catch {
    error.value = "No pude actualizar la tarea.";
  }
}

async function deleteTask(id) {
  error.value = "";
  try {
    await fetch(`${apiBase}/api/tasks/${id}`, { method: "DELETE" });
    await loadTasks();
  } catch {
    error.value = "No pude borrar la tarea.";
  }
}

// --- Editar título ---
function startEdit(task) {
  editingId.value = task.id;
  editTitle.value = task.title;
}

function cancelEdit() {
  editingId.value = null;
  editTitle.value = "";
}

async function saveEdit(id) {
  const value = editTitle.value.trim();
  if (!value) return;

  error.value = "";
  try {
    await fetch(`${apiBase}/api/tasks/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ title: value })
    });

    cancelEdit();
    await loadTasks();
  } catch {
    error.value = "No pude editar la tarea.";
  }
}

onMounted(loadTasks);
</script>

<template>
  <main style="max-width: 720px; margin: 40px auto; font-family: system-ui;">
    <h1>Vue + .NET Basics</h1>

    <!-- Contador local -->
    <section style="display:flex; gap: 12px; align-items:center; margin: 16px 0;">
      <button @click="increment" style="padding: 8px 12px;">Incrementar</button>
      <p>Contador: <b>{{ count }}</b></p>
    </section>

    <hr />

    <!-- Tareas desde .NET -->
    <section style="margin-top: 16px;">
      <h2>Tareas (desde .NET)</h2>

      <div style="display:flex; gap: 8px; margin: 10px 0;">
        <input
          v-model="title"
          placeholder="Nueva tarea..."
          style="flex:1; padding: 10px;"
          @keyup.enter="addTask"
        />
        <button @click="addTask" style="padding: 10px 14px;">
          Agregar
        </button>
      </div>

      <p v-if="error" style="color: crimson;">{{ error }}</p>

      <ul style="padding-left: 0; list-style: none; display: grid; gap: 8px;">
        <li
          v-for="t in tasks"
          :key="t.id"
          style="display:flex; gap:10px; align-items:center; padding: 10px; border: 1px solid #ddd; border-radius: 8px;"
        >
          <input type="checkbox" :checked="t.isDone" @change="toggleTask(t.id)" />

          <!-- Vista normal -->
          <span
            v-if="editingId !== t.id"
            :style="{ textDecoration: t.isDone ? 'line-through' : 'none' }"
          >
            #{{ t.id }} - {{ t.title }}
          </span>

          <!-- Modo edición -->
          <input
            v-else
            v-model="editTitle"
            style="flex:1; padding: 6px 8px;"
            @keyup.enter="saveEdit(t.id)"
          />

          <!-- Botones -->
          <button
            v-if="editingId !== t.id"
            @click="startEdit(t)"
            style="margin-left:auto; padding: 6px 10px;"
          >
            Editar
          </button>

          <button
            v-else
            @click="saveEdit(t.id)"
            style="margin-left:auto; padding: 6px 10px;"
          >
            Guardar
          </button>

          <button
            v-if="editingId === t.id"
            @click="cancelEdit"
            style="padding: 6px 10px;"
          >
            Cancelar
          </button>

          <button
            v-if="editingId !== t.id"
            @click="deleteTask(t.id)"
            style="padding: 6px 10px;"
          >
            Borrar
          </button>
        </li>
      </ul>
    </section>
  </main>
</template>