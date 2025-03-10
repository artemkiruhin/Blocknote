import {useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {getSharingByCode} from "../api-handlers/sharings-handler";
import Container from "../components/layout/Container";
import "../styles/MarkdownStyles.css"

const SharingByCodePage = () => {
    const navigate = useNavigate();
    const {code} = useParams();

    const [sharingNote, setSharingNote] = useState({
        code: code,
        title: '',
        subtitle: '',
        content: ''
    });

    useEffect(() => {
        const fetchSharing = async () => {
            const result = await getSharingByCode(code);
            setSharingNote({
                code: code,
                title: result.title,
                subtitle: result.subtitle,
                content: result.content
            });
        }
        fetchSharing();
    }, [code])

    const renderMarkdown = (text) => {
        if (!text) return '';

        let html = text
            .replace(/^# (.+)$/gm, '<h1>$1</h1>') // Заголовки
            .replace(/^## (.+)$/gm, '<h2>$1</h2>')
            .replace(/^### (.+)$/gm, '<h3>$1</h3>')
            .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>') // Жирный текст
            .replace(/\*(.+?)\*/g, '<em>$1</em>') // Курсив
            .replace(/^- (.+)$/gm, '<li>$1</li>') // Списки
            .replace(/\[(.+?)\]\((.+?)\)/g, '<a href="$2">$1</a>') // Ссылки
            .replace(/```([\s\S]*?)```/g, '<pre><code>$1</code></pre>') // Блоки кода
            .replace(/`(.+?)`/g, '<code>$1</code>') // Встроенный код
            .replace(/\n\n/g, '</p><p>'); // Параграфы

        html = '<p>' + html + '</p>';
        html = html.replace(/<li>(.+?)<\/li>/g, '<ul><li>$1</li></ul>'); // Списки
        html = html.replace(/<\/ul><ul>/g, ''); // Убираем лишние теги

        return html;
    };

    return (
        <Container>
            <div className="share-note">
                <div className="share-header">
                    <button className="btn-icon" onClick={() => navigate('/')} title="Назад">
                        ←
                    </button>
                    <div className="header-title">
                        Заметка
                    </div>

                </div>
                <div className="share-content-wrapper">
                    <div className="share-note-header">
                        <h1 className="note-title">{sharingNote.title}</h1>
                        {sharingNote.subtitle && <h2 className="note-subtitle">{sharingNote.subtitle}</h2>}
                    </div>

                    <div
                        className="share-note-content"
                        dangerouslySetInnerHTML={{__html: renderMarkdown(sharingNote.content)}}
                    ></div>
                </div>

            </div>
        </Container>
    )
}

export default SharingByCodePage;