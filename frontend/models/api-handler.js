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
        const httpRequest = `http://${config.domain}:${config.port}/api/`;

        const response = await fetch(`${httpRequest}auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include',
            body: JSON.stringify({ username, password }),
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Login failed.');
        }

        const { token } = await response.json();
        console.log('JWT Token:', token);

        return token;
    } catch (error) {
        console.error('Login error:', error);
        throw error;
    }
};
