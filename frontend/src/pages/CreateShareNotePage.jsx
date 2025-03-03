import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Container from '../components/layout/Container';
import '../styles/CreateShareNotePage.css';

const CreateShareNotePage = () => {
    const navigate = useNavigate();
    const [note, setNote] = useState({
        createdAt: new Date().toLocaleString(),
        expiresAt: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0], // Default: 30 days from now
        accessType: 'public', // Default: public access (for all)
        durationType: 'limited' // Default: limited (with expiration)
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNote({ ...note, [name]: value });

        // Clear expiration date if unlimited duration is selected
        if (name === 'durationType' && value === 'unlimited') {
            setNote(prev => ({ ...prev, [name]: value, expiresAt: '' }));
        } else if (name === 'durationType' && value === 'limited' && !note.expiresAt) {
            // Set default expiration date when switching back to limited
            setNote(prev => ({
                ...prev,
                [name]: value,
                expiresAt: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]
            }));
        }
    };

    const handleCreateSharing = () => {
        console.log('Creating shared note:', note);
        // Here you would send the note to your backend

        // For demo purposes, generate a random ID
        const newShareId = Math.floor(1000 + Math.random() * 9000);
        navigate(`/sharings/${newShareId}`);
    };

    const handleCancel = () => {
        navigate('/sharings');
    };

    return (
        <Container>
            <div className="create-share-note">
                <div className="share-header">
                    <button className="btn-icon" onClick={handleCancel} title="Назад">
                        ←
                    </button>
                    <div className="header-title">
                        Создание общего доступа к заметке
                    </div>
                    <div className="header-actions">
                        <button className="btn-action btn-cancel" onClick={handleCancel}>
                            Отменить
                        </button>
                        <button
                            className="btn-action btn-save"
                            onClick={handleCreateSharing}
                        >
                            Создать
                        </button>
                    </div>
                </div>

                <div className="share-form">
                    <div className="share-settings">
                        <h3 className="settings-title">Настройки доступа</h3>

                        <div className="settings-group">
                            <label className="settings-label">Тип доступа:</label>
                            <div className="radio-group">
                                <label className="radio-label">
                                    <input
                                        type="radio"
                                        name="accessType"
                                        value="public"
                                        checked={note.accessType === 'public'}
                                        onChange={handleInputChange}
                                    />
                                    <span>Для всех</span>
                                </label>

                                <label className="radio-label">
                                    <input
                                        type="radio"
                                        name="accessType"
                                        value="registered"
                                        checked={note.accessType === 'registered'}
                                        onChange={handleInputChange}
                                    />
                                    <span>Только для зарегистрированных</span>
                                </label>
                            </div>
                        </div>

                        <div className="settings-group">
                            <label className="settings-label">Вид доступа:</label>
                            <div className="radio-group">
                                <label className="radio-label">
                                    <input
                                        type="radio"
                                        name="durationType"
                                        value="limited"
                                        checked={note.durationType === 'limited'}
                                        onChange={handleInputChange}
                                    />
                                    <span>С завершением</span>
                                </label>

                                <label className="radio-label">
                                    <input
                                        type="radio"
                                        name="durationType"
                                        value="unlimited"
                                        checked={note.durationType === 'unlimited'}
                                        onChange={handleInputChange}
                                    />
                                    <span>Бессрочный</span>
                                </label>
                            </div>
                        </div>

                        {note.durationType === 'limited' && (
                            <div className="expiration-date-group">
                                <label className="settings-label">Дата завершения:</label>
                                <input
                                    type="date"
                                    name="expiresAt"
                                    value={note.expiresAt}
                                    onChange={handleInputChange}
                                    min={new Date().toISOString().split('T')[0]}
                                    className="date-input"
                                    required
                                />
                            </div>
                        )}

                        <div className="settings-info">
                            <p>
                                <strong>Тип доступа:</strong> Заметка с доступом "Для всех" будет видна любому пользователю с ссылкой.
                                Доступ "Только для зарегистрированных" потребует авторизации для просмотра заметки.
                            </p>
                            <p>
                                <strong>Вид доступа:</strong> Выберите "С завершением", чтобы установить дату истечения срока действия ссылки,
                                или "Бессрочный" для постоянного доступа.
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </Container>
    );
};

export default CreateShareNotePage;