<script setup>
import { ref, onMounted } from "vue";

//contador (local, no depende de la API)
const count = ref(0);
function increment() {
  count.value++;
}

//tareas (vienen de la API .NET)
const apiBase = "http://localhost:5055";
const tasks = ref([]);
const title = ref("");
const error = ref("");

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

onMounted(loadTasks);
</script>

<template>
  <main style="max-width: 720px; margin: 40px auto; font-family: system-ui;">
    <h1>Vue + .NET Basics</h1>

    <section style="display:flex; gap: 12px; align-items:center; margin: 16px 0;">
      <button @click="increment" style="padding: 8px 12px;">Incrementar</button>
      <p>Contador: <b>{{ count }}</b></p>
    </section>

    <hr />

    <section style="margin-top: 16px;">
      <h2>Tareas (desde .NET)</h2>

      <div style="display:flex; gap: 8px; margin: 10px 0;">
        <input
          v-model="title"
          placeholder="Nueva tarea..."
          style="flex:1; padding: 10px;"
          @keyup.enter="addTask"
        />
        <button @click="addTask" style="padding: 10px 14px;">Agregar</button>
      </div>

      <p v-if="error" style="color: crimson;">{{ error }}</p>

      <ul>
        <li v-for="t in tasks" :key="t.id">
          #{{ t.id }} - {{ t.title }} (hecha: {{ t.isDone }})
        </li>
      </ul>
    </section>
  </main>
</template>