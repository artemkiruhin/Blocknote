import {API_URL} from "./base-handler";

const getAllSharings = async () => {
    try {
        const response = await fetch(`${API_URL}/sharings/`, {
            method: 'GET',
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        return data.sharings
    } catch (e) {
        console.error("Ошибка получения всех шарингов: ", e)
    }
}
const getSharingById = async (sharingId) => {
    try {
        const response = await fetch(`${API_URL}/sharings/${sharingId}`, {
            method: 'GET',
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        return data.sharing
    } catch (e) {
        console.error("Ошибка получения шаринга по ID: ", e)
    }
}
const getSharingByCode = async (code) => {
    try {
        const response = await fetch(`${API_URL}/sharings/code/${code}`, {
            method: 'GET',
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        return data.sharing
    } catch (e) {
        console.error("Ошибка получения шаринга по коду: ", e)
    }
}
const createSharing = async (noteId, finishDate, allowedAll, hasFinishDate) => {
    try {
        const response = await fetch(`${API_URL}/sharings/create/`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: noteId,
                finishDate: finishDate,
                allowedAll: allowedAll,
                hasFinishDate: hasFinishDate
            }),
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        return data.note
    } catch (e) {
        console.error("Ошибка создания шаринга: ", e)
    }
}
const updateSharing = async (noteId, finishDate, allowedAll, hasFinishDate) => {
    try {
        const response = await fetch(`${API_URL}/sharings/update/`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: noteId,
                finishDate: finishDate,
                allowedAll: allowedAll,
                hasFinishDate: hasFinishDate
            }),
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        return data.note
    } catch (e) {
        console.error("Ошибка создания шаринга: ", e)
    }
}
const deleteSharing = async (sharingId) => {
    try {
        const response = await fetch(`${API_URL}/sharings/delete/${sharingId}`, {
            method: 'DELETE',
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        return data.note
    } catch (e) {
        console.error("Ошибка удаления шаринга: ", e)
    }
}

export {
    getAllSharings,
    getSharingById,
    getSharingByCode,
    createSharing,
    updateSharing,
    deleteSharing,
}
