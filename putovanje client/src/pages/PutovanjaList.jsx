import React, { useEffect, useState } from "react";
import api from "../api/apiClient";

export default function PutovanjaList() {
  const [putovanja, setPutovanja] = useState([]);
  const [err, setErr] = useState(null);

  useEffect(() => {
    api("/Putovanje", { method: "GET" })
      .then((data) => setPutovanja(Array.isArray(data) ? data : []))
      .catch((e) => setErr(e.message || "Greška pri učitavanju"));
  }, []);

  return (
    <div className="container">
      <h2>Lista putovanja</h2>
      {err && <p style={{ color: "red" }}>{err}</p>}
      <ul>
        {putovanja.map((p) => (
          <li key={p.guid || p.Guid || p.id}>
            <b>{p.naziv || p.Naziv || "Naziv"}</b>
            <div style={{ fontSize: 12, color: "#666" }}>{p.opis || p.Opis}</div>
          </li>
        ))}
      </ul>
    </div>
  );
}
