import Navbar from "./Navbar";
import Container from "./Container";
import {useEffect, useState} from "react";
import {useNavigate} from "react-router-dom";
import {validate} from "../../api-handlers/auth-handler";

const RootAuthLayout = ({ children }) => {
    const navigate = useNavigate();
    const [isAuthenticated, setIsAuthenticated] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const validateAuth = async () => {
            const result = await validate()
            setIsAuthenticated(result)
        }

        validateAuth();
        setIsLoading(false);
    }, [])

    if (!isAuthenticated) {
        navigate("/auth")
    }

    if (isLoading) {
        return (
            <div>Загрузка</div>
        )
    }

    return (
        <>
            <Navbar />
            <Container>
                {children}
            </Container>
        </>
    )
}

export default RootAuthLayout;