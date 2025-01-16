<script setup>
import { ref, watch } from 'vue';

const props = defineProps({
  note: Object,
});

const emit = defineEmits(['save', 'close']);

const id = ref('');
const title = ref('');
const subtitle = ref('');
const content = ref('');

watch(
    () => props.note,
    (newNote) => {
      if (newNote) {
        title.value = newNote.title;
        content.value = newNote.content;
      } else {
        title.value = '';
        content.value = '';
      }
    },
    {immediate: true}
);

const handleSave = () => {
  if (title.value) {
    emit('save', {
      title: title.value,
      subtitle: subtitle.value,
      content: content.value,
    });
    title.value = '';
    content.value = '';
  }
};
</script>

<template>
  <div class="modal">
    <div class="modal-content">
      <h2 v-if="id" >Редактировать заметку</h2>
      <h2 v-else>Добавить заметку</h2>
      <input
          v-model="title"
          class="modal-input"
          placeholder="Заголовок заметки"
          type="text"
      />
      <input
          v-model="subtitle"
          class="modal-input"
          placeholder="Подзаголовок заметки (необязательно)"
          type="text"
      />
      <textarea
          v-model="content"
          class="modal-textarea"
          placeholder="Содержание заметки (необязательно)"
      ></textarea>
      <div class="modal-buttons">
        <button class="modal-cancel" @click="$emit('close')">Отмена</button>
        <button class="modal-save" @click="handleSave">Сохранить</button>
      </div>
    </div>
  </div>
</template>
