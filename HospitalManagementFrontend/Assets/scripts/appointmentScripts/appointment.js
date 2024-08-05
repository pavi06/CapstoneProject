const baseUrl ="https://pavihosmanagebeapp.azurewebsites.net/api";

var displaySlots = (data) =>{
    var div = document.getElementById("slotsAvailable");
    div.innerHTML="";
    Object.entries(data).forEach(([slot, slotAvailability]) => {
        const slotDiv = document.createElement("div");
        slotDiv.className = `slotDesign ${slotAvailability ? '' : 'notAvailable'}`;
        slotDiv.textContent = slot; 
        slotDiv.addEventListener("click", () => {
            Array.from(div.children).forEach(child => {
                child.classList.remove("selected");
            });
            slotDiv.classList.add("selected");
        });
        div.appendChild(slotDiv);
    });
}


var getDoctorSlots = () =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Patient"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    var doctorId = localStorage.getItem('currentDoctorId');
    var date = document.getElementById('date').value;
    if(!(doctorId && validateDate('date'))){
        openModal('alertModal', "Error", "Verify you selected the doctor and provided the date to get slots!");
        return;
    }
    fetch(baseUrl + '/Patient/GetDoctorSlots',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json',  
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`              
            },
            body:JSON.stringify({
                doctorId: doctorId,
                date: date
            })
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
        displaySlots(data)
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
    });
}

var bookAppointmentByDoctor = (bodyData) =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Patient"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    fetch(baseUrl + '/Patient/BookAppointmentByDoctor',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json', 
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`               
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
        document.getElementById("slotsAvailable").innerHTML="";
        document.getElementById("bookAppointmentForm").reset();
        document.getElementById("infoModel").style.display='block';
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
        document.getElementById("slotsAvailable").innerHTML="";
        document.getElementById("bookAppointmentForm").reset();
    });
}

var redirectToAppointments = () =>{
    window.location.href="./MyAppointments.html";
}


document.addEventListener("DOMContentLoaded",()=>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Patient"){
        openModal('alertModal', "Error", "UnAuthorized Success!");
        return;
    }
    updateForLogInAndOut();
    if(!(JSON.parse(localStorage.getItem('loggedInUser')).role === "Patient")){
        document.getElementById("loginInfoModel").classList.remove("hidden");
    }
    var btn = document.getElementById("getSlots");
    btn.addEventListener("click", ()=>{
        getDoctorSlots();
    })

    var bookBtn = document.getElementById("bookAppointment");
    bookBtn.addEventListener("click", ()=>{
        var slot = document.querySelector(".slotDesign.selected").textContent;
        var patientId = JSON.parse(localStorage.getItem('loggedInUser')).userId;
        if(!(validatePhone('phone') && validateDate('date') && validate('preferredMode') && validate('preferredType')
        && validate('description') && slot && patientId)){
            openModal('alertModal', "Error", "Provide all details properly!");
            return;
        }
        var bodyData = {
            patientId: patientId,
            contactNo: document.getElementById("phone").value,
            appointmentDate: document.getElementById("date").value,
            slot: slot,
            doctorId: localStorage.getItem("currentDoctorId"),
            description: document.getElementById("description").value,
            appointmentType: document.getElementById("preferredType").value,
            appointmentMode: document.getElementById("preferredMode").value
        }
        bookAppointmentByDoctor(bodyData);
    }) 
})