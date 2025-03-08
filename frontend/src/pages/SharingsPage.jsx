import React, {useEffect, useState} from 'react';
import '../styles/SharingsPage.css';
import {useNavigate} from "react-router-dom";
import {getAllSharings} from "../api-handlers/sharings-handler";

const SharingsPage = () => {

    const navigate = useNavigate();

    const [searchId, setSearchId] = useState('');
    const [sharings, setSharings] = useState([]);

    const fetchSharings = async () => {
        const notes = await getAllSharings();
        setSharings(notes);
    }

    useEffect(() => {
        fetchSharings();
    }, [])

    const handleSearch = async () => {
        if (searchId) {
            const filteredSharings = sharings.filter(x => x.id === searchId);
            setSharings(filteredSharings);
        } else {
            await fetchSharings();
        }
    };

    // const handleAddSharing = () => {
    //     // Функция добавления нового шаринга (в будущей реализации)
    //     console.log('Добавление нового шаринга');
    // };

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
                {/*<button className="add-sharing-button" onClick={handleAddSharing}>*/}
                {/*    Добавить шаринг*/}
                {/*</button>*/}
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