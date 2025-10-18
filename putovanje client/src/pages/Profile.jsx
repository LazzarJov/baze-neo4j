import React, { useEffect, useState } from "react";
import api from "../api/apiClient";
import { useAuth } from "../auth/AuthProvider";

export default function Profile() {
  const { userGuid, logout } = useAuth();
  const [user, setUser] = useState(null);

  useEffect(() => {
    if (!userGuid) return;
    api(`/User/${userGuid}`)
      .then(setUser)
      .catch((e) => {
        console.error(e);
        if (e.message === "Unauthorized") logout();
      });
  }, [userGuid]);

  if (!user) return <div className="container">Učitavanje...</div>;

  return (
    <div className="container">
      <h2>Profil</h2>
      <p><b>Ime:</b> {user.ime}</p>
      <p><b>Email:</b> {user.email}</p>
      <button onClick={logout}>Odjavi se</button>
    </div>
  );
}
