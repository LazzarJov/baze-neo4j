# Putovanje Client (React + Vite)

## Setup
1. `cd client`
2. `npm install`
3. `npm run dev`

Frontend će biti na `http://localhost:5173` po default-u.

## Config
- API base URL je u `.env` (VITE_API_BASE). Podrazumevano: `https://localhost:7235/api`.

## Napomene
- Auth token se čuva u `localStorage` (key: `token`).
- Koristi `AuthProvider` i `PrivateRoute` za zaštićene rute.
