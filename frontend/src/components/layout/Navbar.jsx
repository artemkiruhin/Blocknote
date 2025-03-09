import React, { useState, useRef, useEffect } from 'react';
import {Link, useNavigate} from 'react-router-dom';
import '../../styles/Navbar.css';
import {logout} from "../../api-handlers/auth-handler";

const Navbar = () => {
    const [menuOpen, setMenuOpen] = useState(false);
    const menuRef = useRef(null);

    const navigate = useNavigate();

    const toggleMenu = () => {
        setMenuOpen(!menuOpen);
    };

    const logoutHandler = () => {
        //stub
        navigate('/login');
    }

    // Close menu when clicking outside
    useEffect(() => {
        const handleClickOutside = (event) => {
            if (menuRef.current && !menuRef.current.contains(event.target)) {
                setMenuOpen(false);
            }
        };

        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, []);

    return (
        <nav className="navbar">
            <div className="container">
                <a className="logo" onClick={() => navigate("/")}>Blocknote</a>
                <ul className="nav-list">
                    <li className="nav-item">
                        <a className="nav-link" onClick={() => navigate("/notes/new")}>+ Добавить запись</a>
                    </li>
                    <li className="nav-item" ref={menuRef}>
                        <div className="menu-dots" onClick={toggleMenu}>
                            <span className="menu-dots-icon">⋮</span>
                        </div>
                        <div className={`dropdown-menu ${menuOpen ? 'active' : ''}`}>
                            <div className="dropdown-item">Экспорт</div>
                            <div className="dropdown-item">Профиль</div>
                            <div className="dropdown-item" onClick={() => navigate("/sharings")}>Шаринги</div>
                            <div className="dropdown-item" onClick={ async () => {
                                await logout();
                                navigate("/auth");
                            }}>Выйти</div>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>
    );
};

export default Navbar;