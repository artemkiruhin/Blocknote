<script setup>
import Notepage from "@/components/Notepage.vue";
import {getToken} from '/models/cookie-manager.js'
import {ref} from "vue";

import Note from "../models/Note.js";
import AuthPage from "@/components/AuthPage.vue";
const notes = [
  new Note("1", "Title 1", "Subtitle 1", "Content 1"),
  new Note("2", "Title 2", "Subtitle 2", "Content 2"),
  new Note("3", "Title 3", "Subtitle 3", "Content 3"),
  new Note("4", "Title 4", "Subtitle 4", "Content 4"),
  new Note("5", "Title 5", "Subtitle 5", "Content 5")
];
</script>

<template>
  <AuthPage v-if="!getToken()" />
  <Notepage v-if="getToken()" :notes="notes || []"/>
</template>