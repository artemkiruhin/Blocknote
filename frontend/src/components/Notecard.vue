<script setup>
import {defineEmits, ref} from 'vue';
import Notemodal from "@/components/Notemodal.vue";
defineProps({
  id: String,
  title: String,
  subtitle: String,
  content: String
});

const showModal = ref(false);

defineEmits(['open', 'edit', 'delete']);
</script>

<template>
  <div class="note-card">
    <div class="note-card-content" @click="showModal = true" >
      <span class="note-card-title">{{ title }}</span>
      <span class="note-card-subtitle" v-if="subtitle" >{{ subtitle }}</span>
    </div>
    <div class="note-card-menu">
      <div class="menu-item" @click="$emit('open')">Открыть</div>
      <div class="menu-item" @click="$emit('edit')">Редактировать</div>
      <div class="menu-item delete" @click="$emit('delete')">Удалить</div>
    </div>
  </div>
  <Notemodal
      v-if="showModal"
      :title="title"
      :subtitle="subtitle"
      :content="content"
      :id="id"
      @close="showModal = false"
  />
</template>

