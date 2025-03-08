import NoteList from "../components/main/NotesList";
import {useEffect, useState} from "react";
import Container from "../components/layout/Container";
import {useNavigate} from "react-router-dom";
import {getAllNotes} from "../api-handlers/notes-handler";

const MainPage = () => {
    const [notes, setNotes] = useState([]);

    useEffect(() => {
        const fetchNotes = async () => {
            const result = await getAllNotes();
            setNotes(result);
        }

        fetchNotes();
    }, []);

    const navigate = useNavigate();

    const openNoteHandler = (noteId) => {
        navigate(`/notes/${noteId}`);
    }

    return (
        <NoteList notes={notes} openNoteHandler={openNoteHandler} />
    );
};

export default MainPage;
