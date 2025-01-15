<script setup>
import Header from "@/components/Header.vue";
import Notecard from "@/components/Notecard.vue";

defineProps({
  notes: {
    type: Array,
    required: true
  }
});

</script>

<template>
  <Header/>
  <div class="container">

      <Notecard
          v-for="note in notes"
          :key="note.id"
          :id="note.id"
          :title="note.title"
          :subtitle="note.subtitle"
          class="note-item"
      />


    <div v-if="!notes.length" class="empty-state">
      <p>Заметок пока нет</p>
    </div>
  </div>
</template>