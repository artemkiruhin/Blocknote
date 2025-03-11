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

const formatSharingDate = (dateString) => {
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


export {
    renderMarkdown,
    formatDateTime,
    formatSharingDate
}