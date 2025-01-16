<script setup>
import { ref, watch } from 'vue';

const props = defineProps({
  id: String,
  title: String,
  subtitle: String,
  content: String,
});

const emit = defineEmits(['save', 'close']);

const localTitle = ref(props.title);
const localSubtitle = ref(props.subtitle);
const localContent = ref(props.content);

watch(() => props.title, (newValue) => {
  localTitle.value = newValue;
});
watch(() => props.subtitle, (newValue) => {
  localSubtitle.value = newValue;
});
watch(() => props.content, (newValue) => {
  localContent.value = newValue;
});

const handleSave = () => {
  emit('save', {
    id: props.id,
    title: localTitle.value,
    subtitle: localSubtitle.value,
    content: localContent.value,
  });
};
</script>

<template>
  <div class="modal">
    <div class="modal-content">
      <h2>{{ id ? "Редактировать заметку" : "Добавить заметку" }}</h2>
      <input
          v-model="localTitle"
          class="modal-input"
          placeholder="Заголовок заметки"
          type="text"
      />
      <input
          v-model="localSubtitle"
          class="modal-input"
          placeholder="Подзаголовок заметки"
          type="text"
      />
      <textarea
          v-model="localContent"
          class="modal-textarea"
          placeholder="Содержание заметки"
      ></textarea>
      <div class="modal-buttons">
        <button class="modal-cancel" @click="$emit('close')">Отмена</button>
        <button class="modal-save" @click="handleSave">Сохранить</button>
      </div>
    </div>
  </div>
</template>
