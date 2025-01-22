import { save as saveJwt, remove as removeJwt, getToken } from './cookie-manager';

const loadConfig = async () => {
    const response = await fetch('/appsettings.json');
    if (!response.ok) {
        throw new Error('Failed to load configuration file.');
    }
    return response.json();
};

const getAuthHeaders = () => {
    const token = getToken();
    if (!token) {
        throw new Error('Нет токена авторизации');
    }

    return {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
    };
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

        saveJwt(token); // Сохраняем токен в cookies
        console.log('JWT Token сохранен в cookies.');

        return token;
    } catch (error) {
        console.error('Ошибка при входе:', error);
        throw error;
    }
};

export const fetchNotes = async () => {
    try {
        const config = await loadConfig();
        const apiUrl = `http://${config.domain}:${config.port}/api/`;

        const response = await fetch(`${apiUrl}notes`, {
            method: 'GET',
            headers: getAuthHeaders(),
        });

        if (!response.ok) {
            throw new Error('Ошибка при получении заметок.');
        }

        const notes = await response.json();
        console.log('Заметки:', notes);
        return notes;
    } catch (error) {
        console.error('Ошибка при запросе заметок:', error);
        throw error;
    }
};

export const fetchNoteById = async (id) => {
    try {
        const config = await loadConfig();
        const apiUrl = `http://${config.domain}:${config.port}/api/`;

        const response = await fetch(`${apiUrl}notes/${id}`, {
            method: 'GET',
            headers: getAuthHeaders(),
        });

        if (!response.ok) {
            throw new Error('Ошибка при получении заметки.');
        }

        const note = await response.json();
        console.log('Заметка:', note);
        return note;
    } catch (error) {
        console.error('Ошибка при запросе заметки:', error);
        throw error;
    }
};

export const createNote = async (title, subtitle, content) => {
    try {
        const config = await loadConfig();
        const apiUrl = `http://${config.domain}:${config.port}/api/`;

        const requestBody = {
            Title: title,
            Subtitle: subtitle,
            Content: content,
        };

        const response = await fetch(`${apiUrl}notes/create`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(requestBody),
        });

        if (!response.ok) {
            throw new Error('Ошибка при создании заметки.');
        }

        const createdNote = await response.json();
        console.log('Заметка создана:', createdNote);
        return createdNote;
    } catch (error) {
        console.error('Ошибка при создании заметки:', error);
        throw error;
    }
};

export const updateNote = async (id, title, subtitle, content) => {
    try {
        const config = await loadConfig();
        const apiUrl = `http://${config.domain}:${config.port}/api/`;

        const requestBody = {
            Id: id,
            Title: title,
            Subtitle: subtitle,
            Content: content,
        };

        const response = await fetch(`${apiUrl}notes/update`, {
            method: 'PUT',
            headers: getAuthHeaders(),
            body: JSON.stringify(requestBody),
        });

        if (!response.ok) {
            throw new Error('Ошибка при обновлении заметки.');
        }

        console.log('Заметка обновлена.');
        return true;
    } catch (error) {
        console.error('Ошибка при обновлении заметки:', error);
        throw error;
    }
};

export const deleteNote = async (id) => {
    try {
        const config = await loadConfig();
        const apiUrl = `http://${config.domain}:${config.port}/api/`;

        const response = await fetch(`${apiUrl}notes/delete/${id}`, {
            method: 'DELETE',
            headers: getAuthHeaders(),
        });

        if (!response.ok) {
            throw new Error('Ошибка при удалении заметки.');
        }

        console.log('Заметка удалена.');
        return true;
    } catch (error) {
        console.error('Ошибка при удалении заметки:', error);
        throw error;
    }
};