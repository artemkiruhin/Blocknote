import Navbar from "./Navbar";
import Container from "./Container";

const RootAuthLayout = ({ children }) => {
    return (
        <>
            <Navbar />
            <Container>
                {children}
            </Container>
        </>
    )
}

export default RootAuthLayout;