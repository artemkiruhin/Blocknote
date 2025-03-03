import { BrowserRouter, Routes, Route } from "react-router-dom";
import RootAuthLayout from "./components/layout/RootAuthLayout";
import MainPage from "./pages/MainPage";
import "./styles/App.css"
import NotePage from "./pages/NotePage";

const App = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<RootAuthLayout><MainPage /></RootAuthLayout>} />
                <Route path="/notes/:id" element={<RootAuthLayout><NotePage /></RootAuthLayout>} />
                <Route path="/notes/new" element={<RootAuthLayout><NotePage /></RootAuthLayout>} />
            </Routes>
        </BrowserRouter>
    );
};

export default App;