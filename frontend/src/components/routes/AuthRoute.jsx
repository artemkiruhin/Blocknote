import {useEffect, useState} from "react";

const AuthRoute = (children) => {

    const [isAuthenticated, setIsAuthenticated] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const checkAuth = async () => {
            //stub
            setIsAuthenticated(true);
        }

        checkAuth();
        setIsLoading(false);
    }, []);

    if (isLoading) {
        return <div>Проверка аутентификации...</div>;
    }

    return (
        <>
            {isAuthenticated ? children : <div>Вы не авторизованы!</div>}
        </>
    )
}

export default AuthRoute;