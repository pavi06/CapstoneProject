var doctorId = 1;

var getPatientId = () =>{
    var name = document.getElementById("patientName").value;
    var contactNo = document.getElementById("contactNo").value;
    fetch('http://localhost:5253/api/DoctorBasic/GetPatientId',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json',                
            },
            body:JSON.stringify({
                patientName: name,
                contactNumber: contactNo
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
    var date = document.getElementById('date').value;
    fetch('http://localhost:5253/api/Patient/GetDoctorSlots',
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

var bookAppointment = () => {
    console.log("book appointment")
    var patientId = parseInt(document.getElementById("idDisplay").textContent, 10);;
    var patientName = document.getElementById("name").value;
    var dob = document.getElementById("dob").value ? document.getElementById("dob").value : "0001-01-01";
    var gender = document.getElementById("gender").value;
    var contactNo = document.getElementById("contactNo").value;
    var address = document.getElementById("address").value;
    var appointmentDate = document.getElementById("date").value;
    var preferredType = document.getElementById("preferredType").value;
    var slot = document.querySelector(".slotDesign.selected").textContent;
    var description = document.getElementById("description").value;
    var bodyData = {
        patientId: patientId,
        name: patientName,
        dateOfBirth: dob,
        gender: gender,
        contactNo: contactNo,
        address: address,
        appointmentDate: appointmentDate,
        slot: slot,
        doctorId: doctorId,
        description: description,
        appointmentType: preferredType,
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
        //redirect to my appointments page
    }).catch( error => {
        console.log(error)
        document.getElementById("slotsAvailable").innerHTML="";
        document.getElementById("bookAppointmentForm").reset();
    });
}

document.addEventListener("DOMContentLoaded", () => {
    // const urlParams = new URLSearchParams(window.location.search); 
    // doctorId = urlParams.get('search'); 
    var appointmentBtn = document.getElementById("bookAppointmentBtn");
    appointmentBtn.addEventListener("click", () => {
        bookAppointment();
    })
})