import React from 'react';
import '../../styles/NoteItem.css';

const NoteItem = ({note}) => {
    return (
        <div className="note-item">
            <h3 className="note-title">{note.title}</h3>
            {note.subtitle && <h4 className="note-subtitle">{note.subtitle}</h4>}

            <div className="note-meta">
                <span className="note-created">Создано: {note.createdAt}</span>
                {note.updatedAt && <span className="note-updated">Изменено: {note.updatedAt}</span>}
            </div>
        </div>
    )
}

export default NoteItem;