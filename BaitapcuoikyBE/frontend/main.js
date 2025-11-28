const API_BASE = "http://localhost:5005/api"; 

function saveToken(token) {
    localStorage.setItem("token", token);
}

function getToken() {
    return localStorage.getItem("token");
}

function logout() {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}

async function apiGet(url) {
    const res = await fetch(API_BASE + url, {
        headers: {
            "Authorization": "Bearer " + getToken()
        }
    });
    return res;
}

async function apiPost(url, data) {
    const res = await fetch(API_BASE + url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + getToken()
        },
        body: JSON.stringify(data)
    });
    return res;
}

async function apiPut(url, data) {
    const res = await fetch(API_BASE + url, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + getToken()
        },
        body: JSON.stringify(data)
    });
    return res;
}

async function apiDelete(url) {
    const res = await fetch(API_BASE + url, {
        method: "DELETE",
        headers: {
            "Authorization": "Bearer " + getToken()
        }
    });
    return res;
}
