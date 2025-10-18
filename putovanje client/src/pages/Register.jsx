import React, { useState } from "react";
import api from "../api/apiClient";
import { useNavigate } from "react-router-dom";

export default function Register() {
  const [form, setForm] = useState({ ime: "", email: "", password: "" });
  const [msg, setMsg] = useState(null);
  const nav = useNavigate();

  async function submit(e) {
    e.preventDefault();
    setMsg(null);
    try {
      await api("/Auth/register", {
        method: "POST",
        body: JSON.stringify({
          Ime: form.ime,
          Email: form.email,
          Password: form.password
        }),
        auth: false
      });
      setMsg("Uspešno registrovan. Preusmeravam na login...");
      setTimeout(() => nav("/login"), 1200);
    } catch (err) {
      setMsg(err.message || "Greška pri registraciji");
    }
  }

  return (
    <div className="container" style={{ maxWidth: 420 }}>
      <h2>Register</h2>
      {msg && <p>{msg}</p>}
      <form onSubmit={submit}>
        <input placeholder="Ime" value={form.ime} onChange={(e) => setForm({...form, ime: e.target.value})} style={{ marginBottom: 8 }} />
        <input placeholder="Email" value={form.email} onChange={(e) => setForm({...form, email: e.target.value})} style={{ marginBottom: 8 }} />
        <input placeholder="Password" type="password" value={form.password} onChange={(e) => setForm({...form, password: e.target.value})} style={{ marginBottom: 8 }} />
        <button type="submit">Register</button>
      </form>
    </div>
  );
}
