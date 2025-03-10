import {API_URL} from "./base-handler";

const login = async (username, password) => {
    try {
        const response = await fetch(`${API_URL}/auth/login`, {
            method: 'POST',
            body: JSON.stringify({
                username: username,
                password: password
            }),
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }

        const data = await response.json()

        return data.token

    } catch (e) {
        console.error("Ошибка авторизации: ", e)
    }
}

const register = async (username, password) => {
    try {
        const response = await fetch(`${API_URL}/auth/register`, {
            method: 'POST',
            body: JSON.stringify({
                username: username,
                password: password
            }),
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include'
        })

        if (!response.ok || response.statusText !== '201') {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }

        return true

    } catch (e) {
        console.error("Ошибка регистрации: ", e)
        return false
    }
}
const logout = async () => {
    try {
        const response = await fetch(`${API_URL}/auth/logout`, {
            method: 'POST',
            credentials: 'include'
        })

        if (!response.ok) {
            console.error(`Ошибка: ${response.statusText} | ${response.status}`)
        }
        return true

    } catch (e) {
        console.error("Ошибка разлогирования: ", e)
        return false
    }
}
const validate = async () => {
    try {
        const response = await fetch(`${API_URL}/auth/validate`, {
            method: 'GET',
            credentials: 'include'
        })

        return response.status === 200

    } catch (e) {
        console.error("Ошибка валидации: ", e)
        return false
    }
}

export {
    login,
    register,
    logout,
    validate,
}