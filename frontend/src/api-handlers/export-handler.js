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
        
        const blob = await response.blob();

        const contentDisposition = response.headers.get('Content-Disposition');

        let fileName = `note_${noteId}.md`;
        if (contentDisposition && contentDisposition.includes('filename=')) {
            fileName = contentDisposition
                .split('filename=')[1]
                .replace(/['"]/g, '');
        }

        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();

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