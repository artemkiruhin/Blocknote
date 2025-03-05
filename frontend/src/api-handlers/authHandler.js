import {API_URL} from "./base-handler";

const login = async (username, password) => {
    try {
        const response = await fetch(`${API_URL}/login`, {
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
        const response = await fetch(`${API_URL}/register`, {
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
        console.error("Ошибка регистрации: ", e)
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

export {
    login,
    register,
    logout,
}