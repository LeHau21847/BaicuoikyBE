const API_BASE = "http://localhost:8090/api";

// Hàm lưu Token VÀ UserId
function saveAuthData(token, role, userId) {
    localStorage.setItem("token", token);
    localStorage.setItem("role", role);
    localStorage.setItem("userId", userId);
}


// Hàm lấy Token
function getToken() {
    return localStorage.getItem("token");
}

// Hàm lấy UserId (quan trọng để đặt hàng)
function getUserId() {
    return Number(localStorage.getItem("userId"));
}

function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    window.location.href = "login.html";
}

// Các hàm gọi API
async function apiGet(url) {
    const res = await fetch(API_BASE + url, {
        headers: { "Authorization": "Bearer " + getToken() }
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

async function apiDelete(url) {
    const res = await fetch(API_BASE + url, {
        method: "DELETE",
        headers: { "Authorization": "Bearer " + getToken() }
    });
    return res;
}