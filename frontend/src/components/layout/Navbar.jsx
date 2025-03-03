import React, { useState, useRef, useEffect } from 'react';
import { Link } from 'react-router-dom';
import '../../styles/Navbar.css';

const Navbar = () => {
    const [menuOpen, setMenuOpen] = useState(false);
    const menuRef = useRef(null);

    const toggleMenu = () => {
        setMenuOpen(!menuOpen);
    };

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
                <a className="logo">Blocknote</a>
                <ul className="nav-list">
                    <li className="nav-item">
                        <a className="nav-link">+ Добавить запись</a>
                    </li>
                    <li className="nav-item" ref={menuRef}>
                        <div className="menu-dots" onClick={toggleMenu}>
                            <span className="menu-dots-icon">⋮</span>
                        </div>
                        <div className={`dropdown-menu ${menuOpen ? 'active' : ''}`}>
                            <div className="dropdown-item">Экспорт</div>
                            <div className="dropdown-item">Профиль</div>
                            <div className="dropdown-item">Шаринги</div>
                            <div className="dropdown-item">Выйти</div>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>
    );
};

export default Navbar;