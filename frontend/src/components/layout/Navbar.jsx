import React from 'react';
import { Link } from 'react-router-dom';

const Navbar = () => {

    return (
        <nav className="navbar">
            <div className="container">
                <a className="logo">Blocknote</a>
                <ul className="nav-list">
                    <li className="nav-item">
                        <a className="nav-link">+ Добавить запись</a>
                    </li>
                </ul>
            </div>
            <div className="messages-header">
                <h2>Сообщения</h2>
                <button
                    className="new-chat-button"

                >
                    Новый чат
                </button>
            </div>
        </nav>

    );
};

export default Navbar;