import { useEffect, useState } from "react";
import { validate } from "../../api-handlers/auth-handler";
import { useNavigate } from "react-router-dom";

const AuthRoute = ({ children }) => {
    const navigate = useNavigate();
    const [isAuthenticated, setIsAuthenticated] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const checkAuth = async () => {
            const result = await validate();
            setIsAuthenticated(result);
            setIsLoading(false);
        };

        checkAuth();
    }, []);

    if (isLoading) {
        return <div>Проверка аутентификации...</div>;
    }

    if (isAuthenticated) {
        return children;
    } else {
        navigate("/login");
        return null;
    }
};

export default AuthRoute;
