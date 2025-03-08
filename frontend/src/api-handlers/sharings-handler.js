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
            credentials: 'include',
        });

        if (!response.ok) {
            throw new Error(`Ошибка: ${response.statusText} | ${response.status}`);
        }

        const data = await response.json();

        if (!data) {
            throw new Error('Данные не получены');
        }

        console.log(data.sharing)
        return data.sharing; // Возвращаем данные с сервера
    } catch (e) {
        console.error('Ошибка при получении шаринга:', e);
        throw e; // Пробрасываем ошибку для обработки в компоненте
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

        const body = {
            noteId: noteId,
            allowedAll: allowedAll,
            hasFinishDate: hasFinishDate
        }
        if (hasFinishDate) {
            body.finishDate = finishDate;
        }

        const response = await fetch(`${API_URL}/sharings/create/`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(body),
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        const data = await response.json()
        console.log(data)
        return data.id
    } catch (e) {
        console.error("Ошибка создания шаринга: ", e)
    }
}
const updateSharing = async (id, isAllowedAll, hasExpires, expiresAt) => {
    try {
        const body = {
            id: id,
            isAllowedAll: isAllowedAll,
            hasExpires: hasExpires,
        };

        // Добавляем expiresAt только если hasExpires === true
        if (hasExpires) {
            body.expiresAt = expiresAt;
        }

        const response = await fetch(`${API_URL}/sharings/update/`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body),
            credentials: 'include',
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error(`Ошибка: ${errorData.message || response.statusText} | ${response.status}`);
            throw new Error(errorData.message || 'Не удалось обновить шаринг');
        }

        const data = await response.json();
        return data; // Возвращаем данные с сервера
    } catch (e) {
        console.error("Ошибка обновления шаринга: ", e);
        throw e; // Пробрасываем ошибку для обработки в handleSaveChanges
    }
};
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
