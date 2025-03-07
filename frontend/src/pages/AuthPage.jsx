import React, { useState } from 'react';
import styles from '../styles/AuthPage.module.css';
import {login, register} from "../api-handlers/auth-handler";
import {useNavigate} from "react-router-dom";

const AuthPage = () => {
    const navigate = useNavigate();

    const [isLoginForm, setIsLoginForm] = useState(true);
    const [loginFormData, setLoginFormData] = useState({
        email: '',
        password: '',
    });
    const [registerFormData, setRegisterFormData] = useState({
        username: '',
        email: '',
        password: '',
    });
    const [loginErrors, setLoginErrors] = useState({
        email: '',
        password: '',
    });
    const [registerErrors, setRegisterErrors] = useState({
        username: '',
        email: '',
        password: '',
    });

    const handleLoginChange = (e) => {
        const { id, value } = e.target;
        setLoginFormData({
            ...loginFormData,
            [id.replace('login-', '')]: value,
        });
    };

    const handleRegisterChange = (e) => {
        const { id, value } = e.target;
        setRegisterFormData({
            ...registerFormData,
            [id.replace('register-', '')]: value,
        });
    };

    const validateLoginForm = () => {
        let valid = true;
        const newErrors = { email: '', password: '' };

        if (!loginFormData.password) {
            newErrors.password = 'Пожалуйста, введите пароль';
            valid = false;
        }

        setLoginErrors(newErrors);
        return valid;
    };

    const validateRegisterForm = () => {
        let valid = true;
        const newErrors = { username: '', email: '', password: '' };

        if (!registerFormData.username) {
            newErrors.username = 'Пожалуйста, введите логин';
            valid = false;
        }

        if (!registerFormData.email || !registerFormData.email.includes('@')) {
            newErrors.email = 'Пожалуйста, введите корректный email';
            valid = false;
        }

        if (!registerFormData.password || registerFormData.password.length < 6) {
            newErrors.password = 'Пароль должен содержать минимум 6 символов';
            valid = false;
        }

        setRegisterErrors(newErrors);
        return valid;
    };

    const handleLoginSubmit = async (e) => {
        e.preventDefault();
        if (validateLoginForm()) {
            console.log('Login form submitted', loginFormData);
            const jwt = await login(loginFormData.email, loginFormData.password);
            if (jwt !== null) {
                localStorage.setItem('jwt_token', jwt.token);
                navigate('/');
            }
        }
    };

    const handleRegisterSubmit = async (e) => {
        e.preventDefault();
        if (validateRegisterForm()) {
            console.log('Register form submitted', registerFormData);
            const result = await register(registerFormData.username, registerFormData.password);
            if (result && result === true) {
                setIsLoginForm(false);
            }
        }
    };

    return (
        <div className={styles.authPageWrapper}>
            <div className={styles.authContainer}>
                <div className={styles.authHeader}>
                    <div className={styles.logo}>
                        Notes<span>.</span>
                    </div>
                </div>

                <div className={styles.authTabs}>
                    <div
                        className={`${styles.authTab} ${styles.loginTab} ${isLoginForm ? styles.active : ''}`}
                        onClick={() => setIsLoginForm(true)}
                    >
                        Вход
                    </div>
                    <div
                        className={`${styles.authTab} ${styles.registerTab} ${!isLoginForm ? styles.active : ''}`}
                        onClick={() => setIsLoginForm(false)}
                    >
                        Регистрация
                    </div>
                </div>

                <div
                    className={styles.formsContainer}
                    style={{ height: isLoginForm ? '420px' : '480px' }}
                >
                    <div
                        className={`${styles.formWrapper} ${styles.loginForm}`}
                        style={{ transform: isLoginForm ? 'translateX(0)' : 'translateX(-100%)' }}
                    >
                        <form onSubmit={handleLoginSubmit}>
                            <div className={styles.formGroup}>
                                <label htmlFor="login-email">Логин или Email</label>
                                <input
                                    type="text"
                                    id="login-email"
                                    placeholder="Введите логин или email"
                                    value={loginFormData.email}
                                    onChange={handleLoginChange}
                                />
                                {loginErrors.email && <div className={styles.errorMessage}>{loginErrors.email}</div>}
                            </div>
                            <div className={styles.formGroup}>
                                <label htmlFor="login-password">Пароль</label>
                                <input
                                    type="password"
                                    id="login-password"
                                    placeholder="Введите пароль"
                                    value={loginFormData.password}
                                    onChange={handleLoginChange}
                                />
                                {loginErrors.password && <div className={styles.errorMessage}>{loginErrors.password}</div>}
                            </div>
                            <button type="submit" className={styles.authButton}>Войти</button>
                        </form>
                        <div className={styles.formFooter}>
                            <p>Нет аккаунта? <a onClick={() => setIsLoginForm(false)}>Зарегистрироваться</a></p>
                        </div>
                    </div>

                    <div
                        className={`${styles.formWrapper} ${styles.registerForm}`}
                        style={{ transform: isLoginForm ? 'translateX(100%)' : 'translateX(0)' }}
                    >
                        <form onSubmit={handleRegisterSubmit}>
                            <div className={styles.formGroup}>
                                <label htmlFor="register-username">Логин</label>
                                <input
                                    type="text"
                                    id="register-username"
                                    placeholder="Выберите логин"
                                    value={registerFormData.username}
                                    onChange={handleRegisterChange}
                                />
                                {registerErrors.username && <div className={styles.errorMessage}>{registerErrors.username}</div>}
                            </div>
                            <div className={styles.formGroup}>
                                <label htmlFor="register-email">Email</label>
                                <input
                                    type="email"
                                    id="register-email"
                                    placeholder="Введите email"
                                    value={registerFormData.email}
                                    onChange={handleRegisterChange}
                                />
                                {registerErrors.email && <div className={styles.errorMessage}>{registerErrors.email}</div>}
                            </div>
                            <div className={styles.formGroup}>
                                <label htmlFor="register-password">Пароль</label>
                                <input
                                    type="password"
                                    id="register-password"
                                    placeholder="Введите пароль"
                                    value={registerFormData.password}
                                    onChange={handleRegisterChange}
                                />
                                {registerErrors.password && <div className={styles.errorMessage}>{registerErrors.password}</div>}
                            </div>
                            <button type="submit" className={styles.authButton}>Зарегистрироваться</button>
                        </form>
                        <div className={styles.formFooter}>
                            <p>Уже есть аккаунт? <a onClick={() => setIsLoginForm(true)}>Войти</a></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AuthPage;
