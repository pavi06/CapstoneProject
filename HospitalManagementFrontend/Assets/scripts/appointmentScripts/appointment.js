const baseUrl ="http://localhost:5253/api";

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
    var doctorId = localStorage.getItem('currentDoctorId');
    var date = document.getElementById('date').value;
    console.log(validateDate('date'))
    console.log(doctorId)
    if(!(doctorId && validateDate('date'))){
        alert("Verify you selected the doctor and provided the date to get slots!");
        return;
    }
    fetch(baseUrl + '/Patient/GetDoctorSlots',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json',                
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
        console.log(data)
        displaySlots(data)
    }).catch( error => {
        console.log(error)
        alert(error.message)
    });
}

var bookAppointmentByDoctor = (bodyData) =>{
    fetch(baseUrl + '/Patient/BookAppointmentByDoctor',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json',                
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
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        console.log("booked successfully")
        document.getElementById("slotsAvailable").innerHTML="";
        document.getElementById("bookAppointmentForm").reset();
        document.getElementById("infoModel").style.display='block';
    }).catch( error => {
        console.log(error)
        document.getElementById("slotsAvailable").innerHTML="";
        document.getElementById("bookAppointmentForm").reset();
    });
}

var redirectToAppointments = () =>{
    window.location.href="./MyAppointments.html";
}


document.addEventListener("DOMContentLoaded",()=>{
    updateForLogInAndOut();
    if(!(JSON.parse(localStorage.getItem('loggedInUser')).role === "Patient")){
        document.getElementById("loginInfoModel").classList.remove("hidden");
    }
    var btn = document.getElementById("getSlots");
    btn.addEventListener("click", ()=>{
        console.log("buttonclicked")
        getDoctorSlots();
    })

    var bookBtn = document.getElementById("bookAppointment");
    bookBtn.addEventListener("click", ()=>{
        var slot = document.querySelector(".slotDesign.selected").textContent;
        var patientId = JSON.parse(localStorage.getItem('loggedInUser')).userId;
        console.log(validatePhone('phone'))
        console.log(validateDate('date'))
        console.log(validate('preferredMode'))
        console.log(validate('preferredType'))
        console.log(validate('description'))
        console.log(slot)
        console.log(patientId)
        if(!(validatePhone('phone') && validateDate('date') && validate('preferredMode') && validate('preferredType')
        && validate('description') && slot && patientId)){
            alert("Provide all details properly!");
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