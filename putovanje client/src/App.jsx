import React from "react";
import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import { AuthProvider } from "./auth/AuthProvider";
import PrivateRoute from "./components/PrivateRoute";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Profile from "./pages/Profile";
import NotFound from "./pages/NotFound";

// Placeholder Home komponenta
function Home() {
  return <div style={{ padding: 20 }}><h2>Dobrodošli!</h2><p>Home stranica je u pripremi.</p></div>;
}

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
         <nav style={{ padding: 10, borderBottom: "1px solid #ddd" }}>
-          <Link to="/" style={{ marginRight: 10 }}>Home</Link>
-          <Link to="/profile" style={{ marginRight: 10 }}>Profil</Link>
-          <Link to="/login" style={{ marginRight: 10 }}>Login</Link>
-          <Link to="/register">Register</Link>
-        </nav>

        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/profile" element={
            <PrivateRoute>
              <Profile />
            </PrivateRoute>
          } />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}
