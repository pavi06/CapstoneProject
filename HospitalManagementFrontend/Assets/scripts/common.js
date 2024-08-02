let navbar = document.querySelector(".navbar");

let navLinks = document.querySelector(".nav-links");
let menuOpenBtn = document.querySelector(".navbar .bx-menu");
let menuCloseBtn = document.querySelector(".nav-links .bx-x");
menuOpenBtn.onclick = function() {
navLinks.style.left = "0";
}
menuCloseBtn.onclick = function() {
navLinks.style.left = "-100%";
}


let htmlcssArrow = document.querySelector(".htmlcss-arrow");
htmlcssArrow.onclick = function() {
 navLinks.classList.toggle("show1");
}
let moreArrow = document.querySelector(".more-arrow");
moreArrow.onclick = function() {
 navLinks.classList.toggle("show2");
}
let jsArrow = document.querySelector(".js-arrow");
jsArrow.onclick = function() {
 navLinks.classList.toggle("show3");
}

function signUp(){
    if(!(validateEmail('email') && validatePassword('password'))){
        alert("provide all values properly");
        return;
    }
    var bodyData={
        email:document.getElementById("email").value,
        password:document.getElementById("password").value
    }
    fetch('http://localhost:5253/api/User/Login',
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
            console.log("login successfully")
            localStorage.setItem('loggedInUser',JSON.stringify(data));
            localStorage.setItem('isLoggedIn',true)
            if(data.role==="Receptionist"){
                window.location.href="./receptionistTemplates/ReceptionistDashboard.html"
            }else if(data.role === "Doctor"){
                window.location.href="./doctorTemplates/doctorHome.html"
            }
    }).catch(error => {
            console.log(error.message)
    });
}

function externalSignUp(){
    console.log("inside chcek")
    if(!(validateName('name') && validatePhone('contactno'))){
        alert("provide all values properly");
        return;
    }
    var bodyData={
        userName: document.getElementById("name").value,
        contactNumber: document.getElementById("contactno").value
    }
    console.log("herer")
    fetch('http://localhost:5253/api/User/ExternalLogin',
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
        console.log(data)
        document.getElementById('otpModel').style.display = 'block'
    }).catch(error => {
            console.log(error.message)
    }); 
}

function openModal(modalId) {
    document.getElementById(modalId).classList.remove('hidden')
    document.getElementById(modalId).classList.add('block')
}

function closeModal(modalId) {
    document.getElementById(modalId).classList.add('hidden')
    document.getElementById(modalId).classList.remove('block')
}

function externalLoginVerification(){
    var otp = document.getElementById("otp").value;
    fetch('http://localhost:5253/api/User/VerifyOTPForExternalLogin',
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
        console.log(data)
        localStorage.setItem('loggedInUser',JSON.stringify(data));
        localStorage.setItem('isLoggedIn',true)
        if(localStorage.getItem("redirectionURL")){
            window.location.href = localStorage.getItem("redirectionURL");
        }
        else{
            window.location.href="./patientTemplates/index.html";
        }
    }).catch(error => {
        console.log(error.message)
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

function logOut(){
    if(JSON.parse(localStorage.getItem('loggedInUser')).role === "Patient"){
        localStorage.clear();
        window.location.href="./index.html"               
    }
    else{
        localStorage.clear();
        window.location.href="../login.html"
    }   
    history.replaceState(null, '', '');
}
