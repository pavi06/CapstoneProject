const SESSION_EXPIRATION_TIME = 5 * 60;
const CHECK_ACTIVITY_INTERVAL = 30;
let logoutTimer;


function startSession() {
    clearTimeout(logoutTimer);
    logoutTimer = setTimeout(this.logout, SESSION_EXPIRATION_TIME * 1000);
}

function resetSession() {
    clearTimeout(logoutTimer);
    startSession();
}

function logout() {
    if(JSON.parse(localStorage.getItem('loggedInUser')) && JSON.parse(localStorage.getItem('loggedInUser')).role === "Patient"){
        localStorage.clear();
        window.location.reload();
        window.location.href="./index.html"             
    }
    else{
        localStorage.clear();
        window.location.reload();
        window.location.href="../login.html";
    }
}

document.addEventListener('mousemove', resetSession);
document.addEventListener('keypress', resetSession);