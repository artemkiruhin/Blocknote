import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Container from '../components/layout/Container';
import '../styles/NotePage.css';
import {createNote, deleteNote, getNoteById, updateNote} from "../api-handlers/notes-handler";
import {exportNotes} from "../api-handlers/export-handler";
import "../styles/MarkdownStyles.css"
import {renderMarkdown} from "../helpers/extensions";

const NotePage = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const [showPreview, setShowPreview] = useState(false);
    const [note, setNote] = useState({
        title: '',
        subtitle: '',
        content: '',
    });


    useEffect(() => {
        if (id && id !== 'new') {
            const fetchNote = async () => {
                const result = await getNoteById(id)
                setNote({
                    title: result.title,
                    subtitle: result.subtitle,
                    content: result.content
                });
            }

            fetchNote();
        } else {
            setNote({
                title: '',
                subtitle: '',
                content: '',
            })
        }
    }, [id]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNote({ ...note, [name]: value });
    };

    const handleSave = async () => {
        if (id && id !== 'new') {
            const result = await updateNote(id, note.title, note.subtitle, note.content)
        } else {
            const result = await createNote(note.title, note.subtitle, note.content)
        }
        navigate('/');
    };

    const handleDelete = async () => {
        if (window.confirm('Вы уверены, что хотите удалить эту заметку?')) {
            const result = await deleteNote(id);
            navigate('/');
        }
    };

    const handleShare = () => {
        navigate(`/sharings/new/${id}`)
    };

    const handleExport = async (format) => {
        let type = 0;
        if (format === 'DOCX') type = 0;
        else if (format === 'Markdown') type = 1;
        else if (format === 'HTML') type = 2;

        const result = await exportNotes(id, type);

        if (result.success) {
            alert(`Файл "${result.fileName}" успешно скачан!`);
        } else {
            alert(`Ошибка при скачивании файла: ${result.error}`);
        }
        // <span className="export-label">Экспорт:</span>
        // <button className="btn-export" onClick={() => handleExport('HTML')}>
        //     HTML
        // </button>
        // <button className="btn-export" onClick={() => handleExport('JSON')}>
        //     JSON
        // </button>
        // <button className="btn-export" onClick={() => handleExport('DOCX')}>
        //     DOCX
        // </button>

    };
    return (
        <Container>
            <div className="edit-note">
                <div className="edit-header">
                    <button className="btn-icon" onClick={() => navigate('/')} title="Назад">
                        ←
                    </button>
                    <div className="header-title">
                        {id && id !== 'new' ? 'Редактировать заметку' : 'Новая заметка'}
                    </div>
                    <div className="header-actions">
                        <button className="btn-action btn-save" onClick={handleSave}>
                            {id && id !== 'new' ? 'Сохранить' : 'Создать'}
                        </button>
                    </div>
                </div>

                <div className="edit-form">
                    <div className="form-group">
                        <input
                            type="text"
                            name="title"
                            className="input-title"
                            placeholder="Заголовок заметки"
                            value={note.title}
                            onChange={handleInputChange}
                            required
                        />
                    </div>

                    <div className="form-group">
                        <input
                            type="text"
                            name="subtitle"
                            className="input-subtitle"
                            placeholder="Подзаголовок (необязательно)"
                            value={note.subtitle || ''}
                            onChange={handleInputChange}
                        />
                    </div>

                    <div className="form-group">
                        <div className="editor-toolbar">
                            <div className="toolbar-section">
                                <button
                                    className={`btn-tab ${!showPreview ? 'active' : ''}`}
                                    onClick={() => setShowPreview(false)}
                                >
                                    Редактировать
                                </button>
                                <button
                                    className={`btn-tab ${showPreview ? 'active' : ''}`}
                                    onClick={() => setShowPreview(true)}
                                >
                                    Предпросмотр
                                </button>
                            </div>
                            <div className="toolbar-section">
                                <button className="btn-icon" title="Жирный" onClick={() => {
                                    const textarea = document.querySelector('textarea[name="content"]');
                                    const start = textarea.selectionStart;
                                    const end = textarea.selectionEnd;
                                    const selectedText = note.content.substring(start, end);
                                    const replacement = `**${selectedText}**`;
                                    setNote({
                                        ...note,
                                        content: note.content.substring(0, start) + replacement + note.content.substring(end)
                                    });
                                }}>
                                    <strong>B</strong>
                                </button>
                                <button className="btn-icon" title="Курсив" onClick={() => {
                                    const textarea = document.querySelector('textarea[name="content"]');
                                    const start = textarea.selectionStart;
                                    const end = textarea.selectionEnd;
                                    const selectedText = note.content.substring(start, end);
                                    const replacement = `*${selectedText}*`;
                                    setNote({
                                        ...note,
                                        content: note.content.substring(0, start) + replacement + note.content.substring(end)
                                    });
                                }}>
                                    <em>I</em>
                                </button>
                                <button className="btn-icon" title="Заголовок" onClick={() => {
                                    const textarea = document.querySelector('textarea[name="content"]');
                                    const start = textarea.selectionStart;
                                    const end = textarea.selectionEnd;
                                    const selectedText = note.content.substring(start, end);
                                    const replacement = `# ${selectedText}`;
                                    setNote({
                                        ...note,
                                        content: note.content.substring(0, start) + replacement + note.content.substring(end)
                                    });
                                }}>
                                    H
                                </button>
                                <button className="btn-icon" title="Список" onClick={() => {
                                    const textarea = document.querySelector('textarea[name="content"]');
                                    const start = textarea.selectionStart;
                                    const end = textarea.selectionEnd;
                                    const selectedText = note.content.substring(start, end);
                                    const replacement = `- ${selectedText}`;
                                    setNote({
                                        ...note,
                                        content: note.content.substring(0, start) + replacement + note.content.substring(end)
                                    });
                                }}>
                                    •
                                </button>
                                <button className="btn-icon" title="Ссылка" onClick={() => {
                                    const textarea = document.querySelector('textarea[name="content"]');
                                    const start = textarea.selectionStart;
                                    const end = textarea.selectionEnd;
                                    const selectedText = note.content.substring(start, end);
                                    const replacement = `[${selectedText}](url)`;
                                    setNote({
                                        ...note,
                                        content: note.content.substring(0, start) + replacement + note.content.substring(end)
                                    });
                                }}>
                                    🔗
                                </button>
                            </div>
                        </div>

                        {!showPreview ? (
                            <textarea
                                name="content"
                                className="input-content"
                                placeholder="Содержание заметки (поддерживается Markdown)"
                                value={note.content || ''}
                                onChange={handleInputChange}
                                rows={15}
                            ></textarea>
                        ) : (
                            <div
                                className="preview-content"
                                dangerouslySetInnerHTML={{ __html: renderMarkdown(note.content) }}
                            ></div>
                        )}

                        <div className="markdown-help">
                            <div className="help-toggle">Подсказка по Markdown</div>
                            <div className="help-content">
                                <table>
                                    <tbody>
                                    <tr>
                                        <td><code># Заголовок</code></td>
                                        <td>Заголовок 1 уровня</td>
                                    </tr>
                                    <tr>
                                        <td><code>## Заголовок</code></td>
                                        <td>Заголовок 2 уровня</td>
                                    </tr>
                                    <tr>
                                        <td><code>**текст**</code></td>
                                        <td>Жирный текст</td>
                                    </tr>
                                    <tr>
                                        <td><code>*текст*</code></td>
                                        <td>Курсив</td>
                                    </tr>
                                    <tr>
                                        <td><code>- элемент</code></td>
                                        <td>Маркированный список</td>
                                    </tr>
                                    <tr>
                                        <td><code>[текст](ссылка)</code></td>
                                        <td>Ссылка</td>
                                    </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                {
                    id && id !== 'new' ?
                        <div className="note-actions">
                            <div className="action-group">
                                <button className="btn-action" onClick={handleShare}>
                                    Поделиться
                                </button>
                                {id && id !== 'new' && (
                                    <button className="btn-action btn-delete" onClick={handleDelete}>
                                        Удалить
                                    </button>
                                )}
                            </div>

                            <div className="action-group">
                                <span className="export-label">Экспорт:</span>
                                <button className="btn-export" onClick={() => handleExport('HTML')}>
                                    HTML
                                </button>
                                <button className="btn-export" onClick={() => handleExport('JSON')}>
                                    JSON
                                </button>
                                <button className="btn-export" onClick={() => handleExport('DOCX')}>
                                    DOCX
                                </button>
                                <button className="btn-export" onClick={() => handleExport('Markdown')}>
                                    Markdown
                                </button>
                            </div>
                        </div>
                        :
                        null

                }

            </div>
        </Container>
    );
};

export default NotePage;