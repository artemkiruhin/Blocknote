import NoteList from "../components/main/NotesList";
import {useState} from "react";

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
            id: 1,
            title: "Заголовок заметки",
            subtitle: "Подзаголовок заметки",
            content: "Текст заметки",
            createdAt: new Date().toLocaleString(),
            updatedAt: new Date().toLocaleString(),
        },
        {
            id: 1,
            title: "Заголовок заметки",
            subtitle: "Подзаголовок заметки",
            content: "Текст заметки",
            createdAt: new Date().toLocaleString(),
            updatedAt: new Date().toLocaleString(),
        },
        {
            id: 1,
            title: "Заголовок заметки",
            subtitle: "Подзаголовок заметки",
            content: "Текст заметки",
            createdAt: new Date().toLocaleString(),
            updatedAt: new Date().toLocaleString(),
        },
        {
            id: 1,
            title: "Заголовок заметки",
            subtitle: "Подзаголовок заметки",
            content: "Текст заметки",
            createdAt: new Date().toLocaleString(),
            updatedAt: new Date().toLocaleString(),
        },
        {
            id: 1,
            title: "Заголовок заметки",
            subtitle: "Подзаголовок заметки",
            content: "Текст заметки",
            createdAt: new Date().toLocaleString(),
            updatedAt: new Date().toLocaleString(),
        },
        {
            id: 1,
            title: "Заголовок заметки",
            subtitle: "Подзаголовок заметки",
            content: "Текст заметки",
            createdAt: new Date().toLocaleString(),
            updatedAt: new Date().toLocaleString(),
        },

    ]);


    return (
        <div>
            <NoteList notes={notes}/>
        </div>
    )
}

export default MainPage;