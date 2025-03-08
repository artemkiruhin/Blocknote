import React from "react";
import NoteItem from "./NoteItem";
import "../../styles/NoteList.css"

const NoteList = ({notes, openNoteHandler}) => {
    return (
        <div className="notes-list">
            {
                notes && notes.length > 0 ? (
                    notes.map((note, index) => (
                        <NoteItem key={note.id} note={note} openNoteHandler={openNoteHandler} />
                    ))
                ) : (
                    <p>Заметок нет.</p>
                )
            }
        </div>
    )
}

export default NoteList