const API_BASE = "https://localhost:5001/api"; // Thay đổi nếu API của bạn khác

// Lưu token
function saveToken(token) {
    localStorage.setItem("token", token);
}

// Lấy token
function getToken() {
    return localStorage.getItem("token");
}

// Xóa token (logout)
function logout() {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}

// Gửi request có JWT
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