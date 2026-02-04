<script setup>
import { ref } from "vue"; //ref = herramienta para crear datos que actualicen la pantalla (reactividad)

const count = ref(0); //creo contador que comienza en 0
const items = ref([]); //creo una lista vacia de items
const text = ref(""); //creo un texto vacio donde ira el input

function increment() { //aumentar el contador
  count.value++;
}

function addItem() { //agregar elemento
  const value = text.value.trim(); //trim saca los espacios al principio y al final
  if (!value) return;

  items.value.push(value);
  text.value = ""; //limpiamos el input para agregar el siguiente elemento
}
</script>

<template>
  <main style="max-width: 720px; margin: 40px auto; font-family: system-ui;">
    <h1>Vue 3 - Prueba</h1>

    <section style="display:flex; gap: 12px; align-items:center; margin: 16px 0;">
      <button @click="increment" style="padding: 8px 12px;"> <!--funcion incrementar al hacer click-->
        Incrementar
      </button>
      <p>Contador: <b>{{ count }}</b></p> <!--mostramos el contador "count=valor actual"-->
    </section>

    <hr />

    <section style="margin-top: 16px;">
      <h2>Lista</h2>

      <div style="display:flex; gap: 8px;"> <!--lo que escriba el usuario se guarda en text-->
        <input
          v-model="text" 
          placeholder="Escribí algo..."
          style="flex:1; padding: 10px;"
          @keyup.enter="addItem"
        />
        <button @click="addItem" style="padding: 10px 14px;">
          Agregar
        </button>
      </div>

      <ul style="margin-top: 12px;"> <!--por cada cosa en items, crea un <li>, key="index" da un numero a cada elemento-->
        <li v-for="(item, index) in items" :key="index">
          {{ index + 1 }}. {{ item }}
          <button @click="items.splice(index, 1)">Borrar</button>
        </li>
      </ul>
    </section>
  </main>
</template>
