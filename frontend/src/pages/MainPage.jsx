import NoteList from "../components/main/NotesList";
import { useState } from "react";
import Container from "../components/layout/Container";
import {useNavigate} from "react-router-dom";

const MainPage = () => {
    const [notes, setNotes] = useState([
        {
            id: 1,
            title: "Заголовок заметки",
            subtitle: "Подзаголовок заметки",
            content: "Текст заметки",
            createdAt: new Date().toLocaleString(),
            updatedAt: new Date().toLocaleString(),
        },
        {
            id: 2,
            title: "Заголовок заметки 2",
            content: "Текст заметки 2",
            createdAt: new Date().toLocaleString(),
            updatedAt: new Date().toLocaleString(),
        },
        {
            id: 3,
            title: "Заголовок заметки 3",
            subtitle: "Подзаголовок заметки 3",
            content: "Текст заметки 3",
            createdAt: new Date().toLocaleString(),
        },
    ]);

    //const navigate = useNavigate();

    const openNoteHandler = (noteId) => {
        //navigate(`/notes/${noteId}`);
    }

    return (
        <Container>
            <NoteList notes={notes} openNoteHandler={openNoteHandler} />
        </Container>
    );
};

export default MainPage;
