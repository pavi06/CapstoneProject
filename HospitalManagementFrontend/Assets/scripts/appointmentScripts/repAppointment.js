var doctorId = 0;

var getPatientId = () =>{
    if(!(validateName('patientName') && validatePhone('contactNo'))){
        alert("Provide valid Patient name and contactno to get patient id!");
        return;
    }
    fetch('http://localhost:5253/api/DoctorBasic/GetPatientId',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json',                
            },
            body:JSON.stringify({
                patientName: document.getElementById("patientName").value,
                contactNumber: document.getElementById("contactNo").value
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
        var id = document.getElementById("idDisplay");
        id.innerHTML=data;
        alert(data)
    }).catch( error => {
        console.log(error)
        alert(error.message)
    });
}


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
    if(!(validateDate('date'))){
        alert("Provide valid date to get slots");
        return;
    }
    fetch('http://localhost:5253/api/Patient/GetDoctorSlots',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json',                
            },
            body:JSON.stringify({
                doctorId: doctorId,
                date: document.getElementById('date').value
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

var bookAppointment = () => {
    var patientId = parseInt(document.getElementById("idDisplay").textContent, 10);
    var slot = document.querySelector(".slotDesign.selected").textContent;
    if(!(patientId)){
        if(!(validateName('name') && validate('dob') && validate('gender') && validatePhone('contactNo') && validate('address') && validateDate('date')
        && validate('preferredType') && validate('description') && slot)){
            alert("provide all patient details to proceed");
            return;
        }
    }
    else{
        if(!(validatePhone('contactNo') && validateDate('date') && validate('preferredType') && validate('description') && slot)){
            alert("Provide all necessary appointment details to proceed");
            return;
        }
    }
    
    var bodyData = {
        patientId: patientId,
        name: document.getElementById("name").value,
        dateOfBirth: document.getElementById("dob").value ? document.getElementById("dob").value : "0001-01-01",
        gender: document.getElementById("gender").value,
        contactNo: document.getElementById("contactNo").value,
        address: document.getElementById("address").value,
        appointmentDate: document.getElementById("date").value,
        slot: slot,
        doctorId: doctorId,
        description: document.getElementById("description").value,
        appointmentType: document.getElementById("preferredType").value,
    } 
    console.log(bodyData)
    fetch('http://localhost:5253/api/Receptionist/BookAppointment',
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
        console.log(data);
        document.getElementById("slotsAvailable").innerHTML="";
        document.getElementById("bookAppointmentForm").reset();
        document.getElementById("patientName").value="";
        document.getElementById("contactNo").value="";
        document.getElementById("idDisplay").innerHTML="";
        window.location.href="./ReceptionistDashboard.html";
    }).catch( error => {
        console.log(error)
        document.getElementById("slotsAvailable").innerHTML="";
        document.getElementById("bookAppointmentForm").reset();
    });
}

document.addEventListener("DOMContentLoaded", () => {
    const urlParams = new URLSearchParams(window.location.search); 
    doctorId = urlParams.get('search'); 
    var appointmentBtn = document.getElementById("bookAppointmentBtn");
    appointmentBtn.addEventListener("click", () => {
        bookAppointment();
    })
})