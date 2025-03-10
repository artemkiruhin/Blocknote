import { BrowserRouter, Routes, Route } from "react-router-dom";
import RootAuthLayout from "./components/layout/RootAuthLayout";
import MainPage from "./pages/MainPage";
import "./styles/App.css"
import NotePage from "./pages/NotePage";
import AuthPage from "./pages/AuthPage";
import Container from "./components/layout/Container";
import SharingsPage from "./pages/SharingsPage";
import ShareNotePage from "./pages/SharingNotePage";
import CreateShareNotePage from "./pages/CreateShareNotePage";
import SharingByCodePage from "./pages/SharingByCodePage";

const App = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<RootAuthLayout><MainPage /></RootAuthLayout>} />
                <Route path="/notes/:id" element={<RootAuthLayout><NotePage /></RootAuthLayout>} />
                <Route path="/notes/new" element={<RootAuthLayout><NotePage /></RootAuthLayout>} />
                <Route path="/sharings" element={<RootAuthLayout><SharingsPage /></RootAuthLayout>} />
                <Route path="/sharings/:id" element={<RootAuthLayout><ShareNotePage /></RootAuthLayout>} />
                <Route path="/sharings/new/:id" element={<RootAuthLayout><CreateShareNotePage /></RootAuthLayout>} />
                <Route path="/sharings/code/:code" element={<SharingByCodePage />} />
                <Route path="/auth" element={<AuthPage />} />
            </Routes>
        </BrowserRouter>
    );
};

export default App;