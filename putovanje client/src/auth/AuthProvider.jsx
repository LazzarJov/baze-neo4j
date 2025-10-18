import React, { createContext, useContext, useState } from "react";
import api from "../api/apiClient";

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [token, setToken] = useState(() => localStorage.getItem("token"));
  const [userGuid, setUserGuid] = useState(() => localStorage.getItem("userGuid"));

  async function login(email, password) {
    const res = await api("/Auth/login", {
      method: "POST",
      body: JSON.stringify({ email, password }),
      auth: false
    });

    // backend može vratiti { Token: "..."} ili { token: "..." }
    const jwt = res.token || res.Token || res.jwt || res;
    if (!jwt) throw new Error("No token returned from server");
    localStorage.setItem("token", jwt);
    setToken(jwt);

    try {
      const me = await api("/User/me");
      const guid = me.guid || me.Guid;
      if (guid) {
        localStorage.setItem("userGuid", guid);
        setUserGuid(guid);
      }
    } catch (err) {
      console.warn("Could not fetch /User/me after login", err);
    }
  }

  function logout() {
    setToken(null);
    setUserGuid(null);
    localStorage.removeItem("token");
    localStorage.removeItem("userGuid");
  }

  return (
    <AuthContext.Provider value={{ token, login, logout, userGuid }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
