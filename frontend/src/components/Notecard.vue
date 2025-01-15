<script setup>
import {defineEmits, ref} from 'vue';
defineProps({
  title: String,
  subtitle: ref(""),
});
defineEmits(['open', 'edit', 'delete']);
</script>

<template>
  <div class="note-card">
    <div class="note-card-content">
      <span class="note-card-title">{{ title }}</span>
      <span class="note-card-subtitle" v-if="subtitle!==undefined" >{{ subtitle }}</span>
    </div>
    <div class="note-card-menu">
      <div class="menu-item" @click="$emit('open')">Открыть</div>
      <div class="menu-item" @click="$emit('edit')">Редактировать</div>
      <div class="menu-item delete" @click="$emit('delete')">Удалить</div>
    </div>
  </div>
</template>

