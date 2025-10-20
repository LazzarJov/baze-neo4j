import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthProvider";

export default function NavBar() {
  const { token, logout, user } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  // helper za prikaz imena (fallback na više mogućih property-ja)
  const displayName = user?.ime || user?.Ime || user?.name || user?.Name || "Moj profil";

  return (
    <nav style={{
      padding: 10,
      borderBottom: "1px solid #ddd",
      display: "flex",
      gap: 12,
      alignItems: "center",
      flexWrap: "wrap"
    }}>
      <Link to="/">Home</Link>

      {/* javne veze */}
      {!token && (
        <>
          <Link to="/login">Login</Link>
          <Link to="/register">Register</Link>
        </>
      )}

      {/* privatne veze */}
      {token && (
        <>
          {/* Prikaži ime korisnika kao link na profil */}
          <Link to="/profile" style={{ fontWeight: 600 }}>
            {displayName}
          </Link>

          <button
            onClick={handleLogout}
            style={{
              marginLeft: 8,
              padding: "6px 10px",
              cursor: "pointer",
              borderRadius: 4,
              border: "1px solid #ddd",
              background: "#fff"
            }}
          >
            Odjavi se
          </button>
        </>
      )}
    </nav>
  );
}
