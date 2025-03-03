
const NoteItem = ({note}) => {
    return (
        <div className="note-item">
            <h3 className={"note-title"}>{note.title}</h3>
            {note.subtitle ? <h4 className={"note-subtitle"}>{note.subtitle}</h4> : null}
            <span className={"note-created"}>Создано: {note.createdAt}</span>
            {note.updatedAt ? <span className={"note-updated"}>Изменено: {note.updatedAt}</span> : null}
        </div>
    )
}

export default NoteItem;