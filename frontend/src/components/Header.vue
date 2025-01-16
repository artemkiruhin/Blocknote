<script setup>
import { ref } from 'vue';
import NoteModal from './Notemodal.vue';

const showModal = ref(false);
const showNoteView = ref(false);
const currentNote = ref(null);
const notes = ref([]);

const addNote = (note) => {
  notes.value.push({
    id: Date.now().toString(),
    ...note
  });
  showModal.value = false;
};

const deleteNote = (id) => {
  if (confirm('Вы уверены, что хотите удалить эту заметку?')) {
    notes.value = notes.value.filter(note => note.id !== id);
    if (currentNote.value?.id === id) {
      showNoteView.value = false;
    }
  }
};

const openNote = (note) => {
  currentNote.value = note;
  showNoteView.value = true;
};

const editNote = (id) => {
  const note = notes.value.find(note => note.id === id);
  if (note) {
    currentNote.value = note;
    showModal.value = true;
  }
};
</script>

<template>
  <div class="header">
    <h1>Мои заметки</h1>
    <button class="add-note-btn" @click="showModal = true">
      <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <line x1="12" y1="5" x2="12" y2="19"></line>
        <line x1="5" y1="12" x2="19" y2="12"></line>
      </svg>
      Добавить заметку
    </button>

    <NoteModal
        v-if="showModal"
        :note="currentNote"
        @save="addNote"
        @close="showModal = false"
    />
  </div>
</template>