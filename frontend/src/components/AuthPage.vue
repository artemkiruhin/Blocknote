<script setup>
import { ref } from 'vue';
import { login, register } from '../../models/api-handler.js'; // Adjust the import path as needed

const isLogin = ref(true);

const username = ref('');
const password = ref('');
const confirmPassword = ref('');
const errorMessage = ref('');

const handleSubmit = async (event) => {
  event.preventDefault();

  if (!username.value || !password.value || (!isLogin.value && !confirmPassword.value)) {
    errorMessage.value = 'Пожалуйста, заполните все поля.';
    return;
  }

  if (!isLogin.value && password.value !== confirmPassword.value) {
    errorMessage.value = 'Пароли не совпадают.';
    return;
  }

  try {
    if (isLogin.value) {
      await login(username.value, password.value);
      console.log('User logged in successfully');
    } else {
      await register(username.value, password.value);
      console.log('User registered in successfully');
    }
  } catch (error) {
    errorMessage.value = error.message || 'Ошибка при авторизации.';
  }
};
</script>

<template>
  <div class="auth-container">
    <div class="auth-tabs">
      <a @click="isLogin = true" :class="['auth-tab', { active: isLogin }]">Вход</a>
      <a @click="isLogin = false" :class="['auth-tab', { active: !isLogin }]">Регистрация</a>
    </div>

    <form v-if="isLogin" class="auth-form" @submit="handleSubmit">
      <div class="form-group">
        <input type="text" v-model="username" class="form-input" id="user" placeholder=" ">
        <label class="form-label" for="email">Имя пользователя</label>
        <div class="error-message" v-if="errorMessage">{{ errorMessage }}</div>
      </div>

      <div class="form-group">
        <input type="password" v-model="password" class="form-input" id="password" placeholder=" ">
        <label class="form-label" for="password">Пароль</label>
        <div class="error-message" v-if="errorMessage">{{ errorMessage }}</div>
      </div>

      <button type="submit" class="submit-btn">Войти</button>
    </form>

    <form v-if="!isLogin" class="auth-form" @submit="handleSubmit">
      <div class="form-group">
        <input type="text" v-model="username" class="form-input" id="user" placeholder=" ">
        <label class="form-label" for="email">Имя пользователя</label>
        <div class="error-message" v-if="errorMessage">{{ errorMessage }}</div>
      </div>

      <div class="form-group">
        <input type="password" v-model="password" class="form-input" id="password" placeholder=" ">
        <label class="form-label" for="password">Пароль</label>
        <div class="error-message" v-if="errorMessage">{{ errorMessage }}</div>
      </div>

      <div class="form-group">
        <input type="password" v-model="confirmPassword" class="form-input" id="confirm-password" placeholder=" ">
        <label class="form-label" for="password">Подтвердите пароль</label>
        <div class="error-message" v-if="errorMessage">{{ errorMessage }}</div>
      </div>

      <button type="submit" class="submit-btn">Зарегистрироваться</button>
    </form>
  </div>
</template>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
}

a {
  text-decoration: none;
}

body, html {
  height: 100%;
  margin: 0;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #f4f4f4;
  overflow: hidden;
}

.auth-container {
  background: #121213;
  border-radius: 12px;
  padding: 2.5rem;
  width: 90%;
  max-width: 400px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  margin: 0 auto;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.auth-tabs {
  display: flex;
  margin-bottom: 2rem;
  border-bottom: 1px solid #333;
}

.auth-tab {
  flex: 1;
  padding: 1rem;
  text-align: center;
  cursor: pointer;
  color: #9ca3af;
  transition: all 0.3s ease;
}

.auth-tab.active {
  color: #fff;
  border-bottom: 2px solid #2563eb;
}

.auth-form {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.form-group {
  position: relative;
}

.form-label {
  position: absolute;
  left: 1rem;
  top: 0.75rem;
  color: #9ca3af;
  transition: all 0.2s ease;
  pointer-events: none;
  font-size: 0.9rem;
}

.form-input {
  width: 100%;
  background: #1a1a1a;
  border: 1px solid #333;
  color: white;
  padding: 0.75rem 1rem;
  border-radius: 8px;
  font-size: 1rem;
  transition: all 0.2s ease;
}

.form-input:focus {
  border-color: #2563eb;
  outline: none;
  box-shadow: 0 0 0 2px rgba(37, 99, 235, 0.2);
}

.form-input:focus + .form-label,
.form-input:not(:placeholder-shown) + .form-label {
  transform: translateY(-1.75rem) translateX(-0.25rem) scale(0.85);
  color: #2563eb;
}

.submit-btn {
  background: #2563eb;
  color: white;
  border: none;
  padding: 1rem;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  margin-top: 1rem;
}

.submit-btn:hover {
  background: #1d4ed8;
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(37, 99, 235, 0.2);
}

.error-message {
  color: #ef4444;
  font-size: 0.85rem;
  margin-top: 0.5rem;
}
</style>
