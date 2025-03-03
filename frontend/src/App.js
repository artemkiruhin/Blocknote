import { BrowserRouter, Routes, Route } from "react-router-dom";
import RootAuthLayout from "./components/layout/RootAuthLayout";
import MainPage from "./pages/MainPage";
import "./styles/App.css"
import NotePage from "./pages/NotePage";
import AuthPage from "./pages/AuthPage";
import Container from "./components/layout/Container";
import SharingsPage from "./pages/SharingsPage";

const App = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<RootAuthLayout><MainPage /></RootAuthLayout>} />
                <Route path="/notes/:id" element={<RootAuthLayout><NotePage /></RootAuthLayout>} />
                <Route path="/notes/new" element={<RootAuthLayout><NotePage /></RootAuthLayout>} />
                <Route path="/sharings" element={<RootAuthLayout><SharingsPage /></RootAuthLayout>} />
                <Route path="/auth" element={<AuthPage />} />
            </Routes>
        </BrowserRouter>
    );
};

export default App;