import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Container from '../components/layout/Container';
import '../styles/ShareNotePage.css';

const ShareNotePage = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const [note, setNote] = useState({
        title: '',
        subtitle: '',
        content: '',
        createdAt: new Date().toLocaleString(),
        expiresAt: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0], // Default: 30 days from now
        accessType: 'public' // Default: public access (for all)
    });
    const [originalNote, setOriginalNote] = useState(null); // Store original note state
    const [isEditing, setIsEditing] = useState(false);
    const [shareId, setShareId] = useState('');

    useEffect(() => {
        if (id) {
            // Generate a random share ID for demonstration
            setShareId(`share_${Math.random().toString(36).substring(2, 10)}`);

            // Stub: Fetch shared note data
            const fetchedNote = {
                id: parseInt(id),
                title: `Заголовок заметки ${id}`,
                subtitle: `Подзаголовок заметки ${id}`,
                content: `# Это заголовок\n\nЭто **жирный** текст и *курсив*.\n\n- Пункт списка 1\n- Пункт списка 2\n\n## Подзаголовок\n\nЭто [ссылка](https://example.com)`,
                createdAt: new Date().toLocaleString(),
                expiresAt: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
                accessType: 'public'
            };

            setNote(fetchedNote);
        }
    }, [id]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNote({ ...note, [name]: value });
    };

    const handleStartEditing = () => {
        // Save the original note state when starting to edit
        setOriginalNote({...note});
        setIsEditing(true);
    };

    const handleSaveChanges = () => {
        console.log('Saving changes to shared note:', note);
        setIsEditing(false);
        setOriginalNote(null); // Clear original state after saving
        // Here you would update the note in your backend
    };

    const handleCancelChanges = () => {
        // Restore the original note state
        if (originalNote) {
            setNote({...originalNote});
        }
        setIsEditing(false);
        setOriginalNote(null); // Clear original state after cancelling
    };

    const handleDelete = () => {
        if (window.confirm('Вы уверены, что хотите удалить эту заметку?')) {
            console.log('Deleting shared note:', note);
            navigate('/sharings');
        }
    };

    // Convert Markdown to HTML for display
    const renderMarkdown = (text) => {
        if (!text) return '';

        let html = text
            // Headers
            .replace(/^# (.+)$/gm, '<h1>$1</h1>')
            .replace(/^## (.+)$/gm, '<h2>$1</h2>')
            .replace(/^### (.+)$/gm, '<h3>$1</h3>')
            // Bold and Italic
            .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')
            .replace(/\*(.+?)\*/g, '<em>$1</em>')
            // Lists
            .replace(/^- (.+)$/gm, '<li>$1</li>')
            // Links
            .replace(/\[(.+?)\]\((.+?)\)/g, '<a href="$2">$1</a>')
            // Paragraphs
            .replace(/\n\n/g, '</p><p>');

        // Wrap in paragraphs
        html = '<p>' + html + '</p>';
        // Fix lists
        html = html.replace(/<li>(.+?)<\/li>/g, '<ul><li>$1</li></ul>');
        html = html.replace(/<\/ul><ul>/g, '');

        return html;
    };

    // Format date for display
    const formatDate = (dateString) => {
        try {
            if (dateString.includes('T')) {
                // If it's ISO format, convert to readable date
                const date = new Date(dateString);
                return date.toLocaleString();
            }
            return dateString; // Return as is if it's already formatted
        } catch (e) {
            return dateString;
        }
    };

    return (
        <Container>
            <div className="share-note">
                <div className="share-header">
                    <button className="btn-icon" onClick={() => navigate('/sharings')} title="Назад">
                        ←
                    </button>
                    <div className="header-title">
                        Общий доступ к заметке
                    </div>
                    <div className="header-actions">
                        {isEditing ? (
                            <>
                                <button className="btn-action btn-cancel" onClick={handleCancelChanges}>
                                    Отменить изменения
                                </button>
                                <button className="btn-action btn-save" onClick={handleSaveChanges}>
                                    Сохранить изменения
                                </button>
                            </>
                        ) : (
                            <button className="btn-action" onClick={handleStartEditing}>
                                Изменить
                            </button>
                        )}
                    </div>
                </div>

                <div className="share-info">
                    <div className="share-id">
                        Идентификатор: <span className="highlight">{shareId}</span>
                    </div>

                    <div className="share-dates">
                        <div className="date-field">
                            <label>Создано:</label>
                            <span>{formatDate(note.createdAt)}</span>
                        </div>

                        <div className="date-field">
                            <label>Срок действия до:</label>
                            {isEditing ? (
                                <input
                                    type="date"
                                    name="expiresAt"
                                    value={note.expiresAt}
                                    onChange={handleInputChange}
                                    className="date-input"
                                />
                            ) : (
                                <span>{formatDate(note.expiresAt)}</span>
                            )}
                        </div>

                        <div className="date-field">
                            <label>Доступ:</label>
                            {isEditing ? (
                                <select
                                    name="accessType"
                                    value={note.accessType}
                                    onChange={handleInputChange}
                                    className="access-select"
                                >
                                    <option value="public">Для всех</option>
                                    <option value="registered">Только для зарегистрированных</option>
                                </select>
                            ) : (
                                <span className="access-type">
                                    {note.accessType === 'public' ? 'Для всех' : 'Только для зарегистрированных'}
                                </span>
                            )}
                        </div>
                    </div>

                    {isEditing && (
                        <div className="share-settings-help">
                            <p>
                                Установите дату истечения срока действия ссылки и выберите тип доступа для вашей заметки.
                                Заметка с доступом "Для всех" будет видна любому пользователю с ссылкой.
                            </p>
                        </div>
                    )}
                </div>

                <div className="share-content-wrapper">
                    <div className="share-note-header">
                        <h1 className="note-title">{note.title}</h1>
                        {note.subtitle && <h2 className="note-subtitle">{note.subtitle}</h2>}
                    </div>

                    <div
                        className="share-note-content"
                        dangerouslySetInnerHTML={{ __html: renderMarkdown(note.content) }}
                    ></div>
                </div>

                <div className="share-actions">
                    <button className="btn-action btn-delete" onClick={handleDelete}>
                        Удалить
                    </button>
                </div>
            </div>
        </Container>
    );
};

export default ShareNotePage;