
const NoteList = ({notes}) => {
    return (
        <div className="notes-list">
            {
                notes.map((note, index) => (
                    <div className="note-item">
                        <h3 className={"note-title"}>{note.title}</h3>
                        <h4 className={"note-subtitle"}>{note.subtitle}</h4>
                        <span className={"note-created"}>Создано: {note.createdAt}</span>
                        <span className={"note-updated"}>Изменено: {note.updatedAt}</span>
                    </div>
                ))
            }
        </div>
    )
}

export default NoteList