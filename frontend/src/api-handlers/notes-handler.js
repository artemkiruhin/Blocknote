import {API_URL} from "./base-handler";

const getAllNotes = async () => {
    try {
        const response = await fetch(`${API_URL}/notes/`, {
            method: 'GET',
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        return data.notes
    } catch (e) {
        console.error("Ошибка получения всех заметок: ", e)
    }
}
const getNoteById = async (noteId) => {
    try {
        const response = await fetch(`${API_URL}/notes/${noteId}`, {
            method: 'GET',
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        return data.note
    } catch (e) {
        console.error("Ошибка получения заметки по ID: ", e)
    }
}
const createNote = async (title, subtitle, content) => {
    try {
        const response = await fetch(`${API_URL}/notes/create/`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                title: title,
                subtitle: subtitle,
                content: content
            }),
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        return data.note
    } catch (e) {
        console.error("Ошибка создания заметки: ", e)
    }
}
const updateNote = async (noteId, title, subtitle, content) => {
    try {
        const response = await fetch(`${API_URL}/notes/update/`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: noteId,
                title: title,
                subtitle: subtitle,
                content: content
            }),
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        return data.note
    } catch (e) {
        console.error("Ошибка создания заметки: ", e)
    }
}
const deleteNote = async (noteId) => {
    try {
        const response = await fetch(`${API_URL}/notes/delete/${noteId}`, {
            method: 'DELETE',
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        return data.note
    } catch (e) {
        console.error("Ошибка удаления заметки: ", e)
    }
}

export {
    getAllNotes,
    getNoteById,
    createNote,
    updateNote,
    deleteNote,
}
