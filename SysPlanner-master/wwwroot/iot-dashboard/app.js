// ALTERE AQUI A URL DO SEU BACKEND (http ou https conforme seu launchSettings)
const API = window.location.origin; // assume mesmo host/porta que serve o html

// ENVIAR LOCALIZAÇÃO
async function sendLocation() {
    const lat = parseFloat(document.getElementById("lat").value || "0");
    const lng = parseFloat(document.getElementById("lng").value || "0");
    const userIdStr = document.getElementById("userId").value.trim();

    if (!userIdStr) {
        document.getElementById("locResult").innerHTML = "Informe o ID do usuário.";
        return;
    }

    const body = {
        usuarioId: userIdStr, // nome do campo no controller é UsuarioId / Localizacao
        UsuarioId: userIdStr,
        Latitude: lat,
        Longitude: lng
    };

    try {
        // convert user id to guid if needed
        const userId = userIdStr;
        const payload = { usuarioId: userId, latitude: lat, longitude: lng, UsuarioId: userId };
        // endpoint: POST /api/v1/location/save
        const res = await fetch(`${API}/api/v1/location/save`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ UsuarioId: userId, Latitude: lat, Longitude: lng })
        });

        if (!res.ok) throw new Error("Erro");

        document.getElementById("locResult").innerHTML = "📍 Localização enviada com sucesso!";
    } catch (e) {
        console.error(e);
        document.getElementById("locResult").innerHTML = "❌ Erro ao enviar localização.";
    }
}

// SIMULAR CHEGADA EM CASA
async function simulateHome() {
    const userIdStr = document.getElementById("userId").value.trim();
    if (!userIdStr) {
        document.getElementById("simResult").innerHTML = "Informe o ID do usuário.";
        return;
    }

    try {
        const body = {
            UserId: userIdStr,
            Latitude: parseFloat(document.getElementById("lat").value || "0"),
            Longitude: parseFloat(document.getElementById("lng").value || "0")
        };

        const res = await fetch(`${API}/api/v1/iot/simulate-home`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });

        if (!res.ok) {
            const txt = await res.text();
            throw new Error(txt || "Erro");
        }

        const data = await res.json();
        document.getElementById("simResult").innerHTML = "🏡 Evento simulado: " + (data.message ?? "OK");
    } catch (e) {
        console.error(e);
        document.getElementById("simResult").innerHTML = "❌ Não foi possível simular IoT.";
    }
}

// CARREGAR LEMBRETES
async function loadReminders() {
    const list = document.getElementById("remindersList");
    list.innerHTML = "";

    try {
        const response = await fetch(`${API}/api/v1/reminder/list`);
        const data = await response.json();

        if (!Array.isArray(data) || data.length === 0) {
            list.innerHTML = "<li>Nenhum lembrete.</li>";
            return;
        }

        data.forEach(r => {
            const li = document.createElement("li");
            li.textContent = r.title ? `${r.title} - ${r.message}` : r.message;
            list.appendChild(li);
        });
    } catch (e) {
        console.error(e);
        list.innerHTML = "<li>❌ Erro ao carregar lembretes.</li>";
    }
}
