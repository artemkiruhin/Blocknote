import { save as saveJwt } from './cookie-manager';

const loadConfig = async () => {
    const response = await fetch('/appsettings.json');
    if (!response.ok) {
        throw new Error('Failed to load configuration file.');
    }
    return response.json();
};

export const login = async (username, password) => {
    try {
        const config = await loadConfig();
        const apiUrl = `http://${config.domain}:${config.port}/api/`;

        const response = await fetch(`${apiUrl}auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include', // Включаем передачу cookies
            body: JSON.stringify({ username, password }),
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Ошибка авторизации.');
        }

        const { token } = await response.json();
        console.log('JWT Token получен:', token);

        saveJwt(token);
        console.log('JWT Token сохранен в cookies.');

        return token;
    } catch (error) {
        console.error('Ошибка при входе:', error);
        throw error;
    }
};

export const register = async (username, password) => {
    try {
        const config = await loadConfig();
        const apiUrl = `http://${config.domain}:${config.port}/api/`;

        const response = await fetch(`${apiUrl}auth/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password }),
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Ошибка регистрации.');
        }

        console.log('Регистрация успешна!');
        return true;
    } catch (error) {
        console.error('Ошибка при регистрации:', error);
        throw error;
    }
};