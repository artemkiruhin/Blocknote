import {useEffect, useState} from "react";

const AuthRoute = (children) => {

    const [isAuthenticated, setIsAuthenticated] = useState(null);

    useEffect(() => {
        const checkAuth = async () => {
            //stub
            setIsAuthenticated(true);
        }
    }, []);

    return (
        <>
            {isAuthenticated ? {children} : <div>Вы не авторизованы!</div>}
        </>
    )
}

export default AuthRoute;