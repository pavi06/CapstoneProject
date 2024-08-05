function menu(element) {
    let list = document.querySelector('ul');
    element.name === 'menu' ? (element.name = "close", list.classList.add('top-[80px]'), list.classList.add('opacity-100')) : (element.name = "menu", list.classList.remove('top-[80px]'), list.classList.remove('opacity-100'))
}

function encryptNumber(key, number) {
    const encrypted = sjcl.encrypt(key, number.toString());
    return encrypted;
}

function decryptNumber(key, encrypted) {
    const decrypted = sjcl.decrypt(key, encrypted);
    return parseInt(decrypted, 10);
}

const key = "This is the demo key for prosessing"

function signIn(){
    if(!(validateEmail('email') && validatePassword('password'))){
        alert("provide all values properly");
        return;
    }
    var bodyData={
        email:document.getElementById("email").value,
        password:document.getElementById("password").value
    }
    fetch('https://pavihosmanagebeapp.azurewebsites.net/api/User/Login',
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body:JSON.stringify(bodyData)
        }
    )
    .then(async (res) => {
            if (!res.ok) {
                if (res.status === 401) {
                    throw new Error('Unauthorized Access!');
                }
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.message}`);
            }
            return await res.json();
    })
    .then(data => {
            localStorage.setItem('loggedInUser',JSON.stringify(data));
            localStorage.setItem('isLoggedIn',true)
            if(data.role==="Receptionist"){
                window.location.href="./receptionistTemplates/ReceptionistDashboard.html"
            }else if(data.role === "Doctor"){
                window.location.href="./doctorTemplates/doctorHome.html"
            }
    }).catch(error => {
            openModal('alertModal', "Error", error.message);
    });
}

function signUp(){
    if(!(validateName('name') && validateDate('dob') && validate('gender') && validateEmail('email') && validatePhone('phone')
    && validate('address') && validatePassword('password') && validateConfirmPassword('confirmpassword', 'password'))){
        alert("provide all values properly");
        return;
    }
    var bodyData={
        name: document.getElementById("name").value,
        dateOfBirth: document.getElementById("dob").value,
        gender: document.getElementById("gender").value,
        emailId: document.getElementById("email").value,
        contactNo: document.getElementById("phone").value,
        address: document.getElementById("address").value,
        password: document.getElementById("password").value
    }
    fetch('https://pavihosmanagebeapp.azurewebsites.net/api/User/Register',
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body:JSON.stringify(bodyData)
        }
    )
    .then(async (res) => {
            if (!res.ok) {
                if (res.status === 401) {
                    throw new Error('Unauthorized Access!');
                }
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.message}`);
            }
            return await res.text();
    })
    .then(data => {
        localStorage.setItem('loggedInUser',JSON.stringify(data));
        localStorage.setItem('isLoggedIn',true)
        if(localStorage.getItem("redirectionURL")){
            window.location.href = localStorage.getItem("redirectionURL");
        }
        else{
            window.location.href="./patientTemplates/index.html";
        }
    }).catch(error => {
            openModal('alertModal', "Error", error.message);
    });
}


function externalSignIn(){
    if(!(validateName('name') && validatePhone('contactno'))){
        alert("provide all values properly");
        return;
    }
    var bodyData={
        userName: document.getElementById("name").value,
        contactNumber: document.getElementById("contactno").value
    }
    fetch('https://pavihosmanagebeapp.azurewebsites.net/api/User/ExternalLogin',
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body:JSON.stringify(bodyData)
        }
    )
    .then(async (res) => {
            if (!res.ok) {
                if (res.status === 401) {
                    throw new Error('Unauthorized Access!');
                }
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.message}`);
            }
            return await res.text();
    })
    .then(data => {
        document.getElementById('otpModel').style.display = 'block'
    }).catch(error => {
         openModal('alertModal', "Error", error.message);
    }); 
}

function openModal(modalId, status, message) {
    document.getElementById("headerMessage").innerHTML=status;
    if(status === "Error"){
        document.getElementById("headerMessage").style.color="Red";
    }else{
        document.getElementById("headerMessage").style.color="Green";
    }
    document.getElementById("message").innerHTML=message;
    document.getElementById(modalId).style.display = 'block'
    document.getElementById(modalId).classList.remove('hidden')
}

function closeModal(modalId) {
    document.getElementById(modalId).style.display = 'none'
    document.getElementById(modalId).classList.add('hidden')
}

function externalLoginVerification(){
    var otp = document.getElementById("otp").value;
    fetch('https://pavihosmanagebeapp.azurewebsites.net/api/User/VerifyOTPForExternalLogin',
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body:JSON.stringify(otp)
        }
    )
    .then(async (res) => {
            if (!res.ok) {
                if (res.status === 401) {
                    throw new Error('Unauthorized Access!');
                }
                const errorResponse = await res.json();
                throw new Error(`${errorResponse.message}`);
            }
            return await res.json();
    })
    .then(data => {
        localStorage.setItem('loggedInUser',JSON.stringify(data));
        localStorage.setItem('isLoggedIn',true)
        if(localStorage.getItem("redirectionURL")){
            window.location.href = localStorage.getItem("redirectionURL");
        }
        else{
            window.location.href="./patientTemplates/index.html";
        }
    }).catch(error => {
        openModal('alertModal', "Error", error.message);
    }); 
}

function updateForLogInAndOut(){
    if(localStorage.getItem('loggedInUser')== null){
        const elements = document.getElementsByClassName("logOutNav");
        for (let i = 0; i < elements.length; i++) {
            elements[i].classList.remove("hidden");
        } 
        const logInElements = document.getElementsByClassName("logInNav");
        for (let i = 0; i < logInElements.length; i++) {
            logInElements[i].classList.add("hidden");
        } 
    }
    else{
        const elements = document.getElementsByClassName("logOutNav");
        for (let i = 0; i < elements.length; i++) {
            elements[i].classList.add("hidden");
        } 
        const logInElements = document.getElementsByClassName("logInNav");
        for (let i = 0; i < logInElements.length; i++) {
            logInElements[i].classList.remove("hidden");
        }
    }
}

function redirectToSpecializations(){
    window.location.href="./specialities.html"
}

function redirectToBookAppointment(){
    if(localStorage.getItem('loggedInUser') == null){
        document.getElementById("loginInAlert").style.display = 'block';
    }
    else{
        window.location.href="./AppointmentSpec.html";
    }
}

function bookAppointmentRedirect(doctorId){
    if(localStorage.getItem('loggedInUser') == null){
        document.getElementById("loginInAlert").style.display = 'block';
    }
    else{
        localStorage.setItem('currentDoctorId', doctorId);
        window.location.href="./Appointment.html";
    }
}

function redirectToExternalLogin (){
    localStorage.setItem("redirectionURL", window.location.href)
    window.location.href="../PatientLogin.html";
}

function preventBack(){
    window.history.forward();
}

function logOut(){
    if(JSON.parse(localStorage.getItem('loggedInUser')) && JSON.parse(localStorage.getItem('loggedInUser')).role === "Patient"){
        localStorage.clear();
        window.location.href="./index.html"             
    }
    else{
        localStorage.clear();
        window.location.href="../login.html";
    }   
}


function checkTokenAboutToExpiry(accessToken){
    if(!accessToken){
       return "Invalid accessToken!";
    }
    const payload = parseJwt(accessToken);
    if(!payload){
        return "Invalid accessToken!";
    }
    const currentTimestamp = Math.floor(Date.now() / 1000);
    const timeUntilExpiry = payload.exp - currentTimestamp; //diff in sec
    if (timeUntilExpiry <= 300) {
        return "Refresh";
    } else {
        return "Not about to expire!";
    }
}


function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1]; //getting payload
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
    } catch (e) {
        return null;
    }
}


function refreshToken(){
    fetch('https://pavihosmanagebeapp.azurewebsites.net/api/Token/refreshToken', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            accessToken: `${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`,
            refreshToken: `${JSON.parse(localStorage.getItem('loggedInUser')).refreshToken}`
        })
    })
    .then(async(res) => {
        if (!res.ok) {
            throw new Error('Unauthorized Access!');
        }
        return await res.json();
    })
    .then(data => {
        let loggedInUser = JSON.parse(localStorage.getItem('loggedInUser')) || {};
        if(Object.keys(loggedInUser).length === 0){
            throw new Error('Unauthorized Access!');
        }
        loggedInUser.accessToken = data.accessToken;
        loggedInUser.refreshToken = data.refreshToken;
        localStorage.setItem('loggedInUser', JSON.stringify(loggedInUser));
    }).catch(error => {
        openModal('alertModal', "Error", error.message);
        setTimeout((error)=>{
            if(error.message === "Unauthorized Access!"){
                logOut();
            }
        },5000)
    });    
}

async function checkForRefresh(){
    if(localStorage.getItem('loggedInUser')){
        var res = await checkTokenAboutToExpiry(JSON.parse(localStorage.getItem('loggedInUser')).accessToken);
        if (res === "Refresh") {
            await refreshToken();
        } else if (res === "Invalid accessToken!") {
            alert("Invalid AccessToken!");
            return;
        }
    }
}

function redirectToPatientLoginPage(){
    window.location.href="./PatientLogin.html";
}

function redirectToPatientRegisterPage(){
    window.location.href="./register.html";
}


function redirectToDoctors(){
    window.location.href="./specialities.html";
}

function redirectToBookAppointment(){
    window.location.href="./AppointmentSpec.html";
}
