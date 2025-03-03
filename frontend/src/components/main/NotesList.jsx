import React from "react";
import NoteItem from "./NoteItem";

const NoteList = ({notes}) => {
    return (
        <div className="notes-list">
            {
                notes.map((note, index) => (
                    <NoteItem note={note} />
                ))
            }
        </div>
    )
}

export default NoteList