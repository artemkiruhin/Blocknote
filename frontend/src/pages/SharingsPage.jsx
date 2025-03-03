import React, { useState } from 'react';
import '../styles/SharingsPage.css';
import {useNavigate} from "react-router-dom";

const SharingsPage = () => {

    const navigate = useNavigate();

    const [searchId, setSearchId] = useState('');
    const [sharings, setSharings] = useState([
        {
            id: '1',
            title: 'Заметка о проекте X',
            createdAt: '2025-02-28T14:30:00',
            closedAt: null,
            accessLevel: 'public' // 'public' для всех пользователей, 'registered' только для зарегистрированных
        },
        {
            id: '2',
            title: 'Исследование нового API',
            createdAt: '2025-02-25T10:15:00',
            closedAt: '2025-03-01T18:45:00',
            accessLevel: 'registered'
        },
        {
            id: '3',
            title: 'Заметки по встрече',
            createdAt: '2025-03-01T09:30:00',
            closedAt: null,
            accessLevel: 'public'
        }
    ]);

    const handleSearch = () => {
        // Функция поиска по ID (в будущей реализации)
        console.log('Поиск шаринга с ID:', searchId);
    };

    const handleAddSharing = () => {
        // Функция добавления нового шаринга (в будущей реализации)
        console.log('Добавление нового шаринга');
    };

    // Форматирование даты и времени
    const formatDateTime = (dateTimeString) => {
        if (!dateTimeString) return null;
        const date = new Date(dateTimeString);
        return date.toLocaleString('ru-RU', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    return (
        <div className="sharings-page">
            <header className="sharings-page-header">
                <div className="search-container">
                    <input
                        type="text"
                        className="search-input"
                        placeholder="Поиск шаринга по ID"
                        value={searchId}
                        onChange={(e) => setSearchId(e.target.value)}
                    />
                    <button className="search-button" onClick={handleSearch}>
                        Поиск
                    </button>
                </div>
                <button className="add-sharing-button" onClick={handleAddSharing}>
                    Добавить шаринг
                </button>
            </header>
            <div className="sharings-page-content">
                <div className="sharings-list">
                    {sharings.map((sharing) => (
                        <div className="sharing-item" key={sharing.id} onClick={() => navigate(`/sharings/${sharing.id}`)} >
                            <h3 className="sharing-title">{sharing.title}</h3>
                            <div className="sharing-meta">
                                <span className="sharing-created">
                                    Создано: {formatDateTime(sharing.createdAt)}
                                </span>
                                {sharing.closedAt && (
                                    <span className="sharing-closed">
                                        Закрыто: {formatDateTime(sharing.closedAt)}
                                    </span>
                                )}
                                <div className="sharing-access">
                                    Доступ: {
                                    sharing.accessLevel === 'public'
                                        ? 'Всем пользователям'
                                        : 'Только зарегистрированным'
                                }
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default SharingsPage;