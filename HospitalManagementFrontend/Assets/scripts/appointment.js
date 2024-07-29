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


var getDoctorSlots = (e) =>{
    var doctorId = 1;
    var date = document.getElementById('date').value;
    fetch(baseUrl + '/Patient/GetDoctorSlots',
        {
            method:'POST',
            headers:{
                'Authorization': `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNSIsIkNvbnRhY3RObyI6Iis5MTkwMDM3NDE3MjEiLCJSb2xlIjoiVXNlciIsImV4cCI6MTcyMjI4MTY0Nn0.HLTj171QP06zc2F7LK4bZpms2xvYyEMSkZsbJoSEoqE`,
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
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        displaySlots(data)
    }).catch( error => {
        console.log(error)
        document.getElementById("bookAppointmentForm").reset();
        // if(error.message === "Unauthorized Access!"){
        //     logOut()
        // }
    });
}

var bookAppointmentByDoctor = (bodyData) =>{
    console.log(bodyData)
    fetch(baseUrl + '/Patient/BookAppointmentByDoctor',
        {
            method:'POST',
            headers:{
                'Authorization': `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNSIsIkNvbnRhY3RObyI6Iis5MTkwMDM3NDE3MjEiLCJSb2xlIjoiVXNlciIsImV4cCI6MTcyMjI4MTY0Nn0.HLTj171QP06zc2F7LK4bZpms2xvYyEMSkZsbJoSEoqE`,
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
    }).catch( error => {
        console.log(error)
        document.getElementById("bookAppointmentForm").reset();
        // if(error.message === "Unauthorized Access!"){
        //     logOut()
        // }
    });
}


document.addEventListener("DOMContentLoaded",()=>{
    var btn = document.getElementById("getSlots");
    btn.addEventListener("click", ()=>{
        getDoctorSlots();
    })

    var bookBtn = document.getElementById("bookAppointment");
    bookBtn.addEventListener("click", ()=>{
        var phone = document.getElementById("phone").value;
        var date = document.getElementById("date").value;
        var mode = document.getElementById("preferredMode").value;
        var type = document.getElementById("preferredType").value;
        var reason = document.getElementById("description").value;
        var slot = document.querySelector(".slotDesign.selected").textContent;
        var bodyData = {
            patientId: 15,
            contactNo: phone,
            appointmentDate: date,
            slot: slot,
            doctorId: 1,
            description: reason,
            appointmentType: type,
            appointmentMode: mode
        }
        bookAppointmentByDoctor(bodyData);
    }) 
})