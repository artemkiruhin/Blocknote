import {API_URL} from "./base-handler";

const exportNotes = async (noteId, type) => {
    try {
        const response = await fetch(`${API_URL}/export?noteId=${noteId}&type=${type}`, {
            method: 'GET',
            credentials: 'include',
        });

        if (!response.ok) {
            throw new Error(`Ошибка: ${response.statusText}`);
        }

        // Получаем бинарные данные (файл)
        const blob = await response.blob();

        // Определяем имя файла из заголовка Content-Disposition
        const contentDisposition = response.headers.get('Content-Disposition');
        let fileName = `note_${noteId}`; // Имя по умолчанию
        if (contentDisposition && contentDisposition.includes('filename=')) {
            fileName = contentDisposition
                .split('filename=')[1]
                .replace(/['"]/g, ''); // Убираем кавычки, если они есть
        }

        // Создаем ссылку для скачивания
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName; // Указываем имя файла
        document.body.appendChild(a);
        a.click(); // Инициируем скачивание

        // Очищаем ссылку
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);

        return { success: true, fileName };
    } catch (e) {
        console.error('Ошибка при экспорте заметки:', e);
        return { success: false, error: e.message };
    }
};

export {
    exportNotes,
}