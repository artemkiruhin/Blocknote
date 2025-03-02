import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import './styles/App.css';
import MainPage from "./pages/MainPage";
import Navbar from "./components/layout/Navbar";

const App = () => {
    return (
        <div>
            <Navbar/>
            <MainPage/>
        </div>
    );
};

export default App;