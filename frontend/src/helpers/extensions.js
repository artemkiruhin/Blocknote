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


export {
    renderMarkdown
}