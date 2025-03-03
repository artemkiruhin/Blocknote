import { BrowserRouter, Routes, Route } from "react-router-dom";
import RootAuthLayout from "./components/layout/RootAuthLayout";
import MainPage from "./pages/MainPage";
import "./styles/App.css"

const App = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<RootAuthLayout><MainPage /></RootAuthLayout>} />
                {/*<Route path="/notes/:noteId" element={<RootAuthLayout><NotePage /></RootAuthLayout>} />*/}
            </Routes>
        </BrowserRouter>
    );
};

export default App;