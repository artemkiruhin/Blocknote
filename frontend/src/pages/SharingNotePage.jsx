import Container from "../components/layout/Container";
import {deleteSharing, getSharingById, updateSharing} from "../api-handlers/sharings-handler";
import {useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import "../styles/ShareNotePage.css"

const ShareNotePage = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const [note, setNote] = useState({
        title: '',
        subtitle: '',
        content: '',
        createdAt: new Date().toLocaleString(),
        expiresAt: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
        accessType: 'public'
    });
    const [originalNote, setOriginalNote] = useState(null);
    const [isEditing, setIsEditing] = useState(false);
    const [shareId, setShareId] = useState('');
    const [isUnlimited, setIsUnlimited] = useState(false);

    useEffect(() => {
        if (id) {
            const fetchSharing = async () => {
                const result = await getSharingById(id);
                console.log(result);
                const note = {
                    id: id,
                    title: result.title,
                    subtitle: result.subtitle,
                    content: result.content,
                    createdAt: result.createdAt.toLocaleString(),
                    expiresAt: result.closeAt.toLocaleString(),
                    accessType: result.accessType,
                };
                setShareId(result.noteId);
                setNote(note);
                setIsUnlimited(!result.hasExpires);
            };

            fetchSharing();
        }
    }, [id]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNote({ ...note, [name]: value });
    };

    const handleStartEditing = () => {
        setOriginalNote({ ...note });
        setIsEditing(true);
    };

    const handleSaveChanges = async () => {
        console.log('Saving changes to shared note:', note);

        try {
            const isPublic = note.accessType === 'public';
            const expiresAt = isUnlimited ? null : (note.expiresAt ? new Date(note.expiresAt).toISOString() : null);
            const hasExpires = !isUnlimited && !!expiresAt;

            console.log(isPublic, expiresAt, hasExpires);

            const result = await updateSharing(id, isPublic, hasExpires, expiresAt);

            if (!result) {
                throw new Error('Не удалось сохранить изменения. Сервер не вернул данные.');
            }

            setNote({
                ...note,
                accessType: isPublic ? 'public' : 'registered',
                expiresAt: expiresAt ? new Date(expiresAt).toLocaleString() : null,
            });

            setIsEditing(false);
            setOriginalNote(null);
            alert('Изменения успешно сохранены!');
        } catch (error) {
            console.error('Ошибка при сохранении изменений:', error);
            alert('Произошла ошибка при сохранении изменений. Попробуйте снова.');
        }
    };

    const handleCancelChanges = () => {
        if (originalNote) {
            setNote({ ...originalNote });
        }
        setIsEditing(false);
        setOriginalNote(null);
    };

    const handleDelete = async () => {
        if (window.confirm('Вы уверены, что хотите удалить эту заметку?')) {
            console.log('Deleting shared note:', note);
            const result = await deleteSharing(id)
            navigate('/sharings');
        }
    };

    const renderMarkdown = (text) => {
        if (!text) return '';

        let html = text
            .replace(/^# (.+)$/gm, '<h1>$1</h1>')
            .replace(/^## (.+)$/gm, '<h2>$1</h2>')
            .replace(/^### (.+)$/gm, '<h3>$1</h3>')
            .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')
            .replace(/\*(.+?)\*/g, '<em>$1</em>')
            .replace(/^- (.+)$/gm, '<li>$1</li>')
            .replace(/\[(.+?)\]\((.+?)\)/g, '<a href="$2">$1</a>')
            .replace(/\n\n/g, '</p><p>');

        html = '<p>' + html + '</p>';
        html = html.replace(/<li>(.+?)<\/li>/g, '<ul><li>$1</li></ul>');
        html = html.replace(/<\/ul><ul>/g, '');

        return html;
    };

    const formatDate = (dateString) => {
        try {
            let maxDate = new Date(9999, 11, 31, 23, 59, 59, 999);
            const currentDate = new Date(dateString);

            if (currentDate.getTime() === maxDate.getTime()) {
                return 'неограниченно';
            }

            if (dateString.includes('T')) {
                const date = new Date(dateString);
                return date.toLocaleString();
            }
            return dateString;
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
                                <div>
                                    <label>
                                        <input
                                            type="checkbox"
                                            checked={isUnlimited}
                                            onChange={(e) => setIsUnlimited(e.target.checked)}
                                        />
                                        Без срока действия
                                    </label>
                                    {!isUnlimited && (
                                        <input
                                            type="date"
                                            name="expiresAt"
                                            value={note.expiresAt}
                                            onChange={handleInputChange}
                                            className="date-input"
                                        />
                                    )}
                                </div>
                            ) : (
                                <span>{isUnlimited ? 'неограниченно' : formatDate(note.expiresAt)}</span>
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