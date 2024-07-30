var url="http://localhost:5253/api/Receptionist/GetTodayAppointmentDetails";
var doctorUrl = "http://localhost:5253/api/Receptionist/CheckDoctorAvailability";
var page = 1;
var doctorPage=1;
const itemsperpage = 5;

var displayAppointments = (data) => {
    var div = document.getElementById("slider");
    data.forEach(appointment => {
        div.innerHTML += `
            <div class="relative h-90 grid-cols-1  m-5 bg-white shadow-lg rounded-lg border">
                            <div class="flex flex-row mt-8">
                                <div class="p-6">
                                    <h2 class="text-lg font-semibold mb-2">${appointment.appointmentId}</h2>
                                    <p>${appointment.appointmentDate}</p>
                                    <p>${appointment.slot}</p>
                                    <p>${appointment.description}</p>
                                    <p>${appointment.appointmentStatus}</p>
                                    <p>${appointment.appointmentType}</p>
                                </div>
                                <div class="p-6">
                                    <p>${appointment.doctorName}</p>
                                    <p>${appointment.specialization}</p>
                                    <p>${appointment.patientName}</p>
                                    <p>${appointment.contactNo}</p>
                                </div>
                            </div>
                            <span
                                class="absolute top-0 right-0 bg-green-400 text-xs font-semibold px-10 py-3 rounded-tl-lg rounded-bl-lg shadow-lg">
                                Today
                            </span>
                            <div class="flex flex-row justify-center">
                                <button class="p-3 m-3 bg-[#4A249D]">Cancel</button>
                            </div>
                        </div>
        `;
    });
}

var fetchAppointments = () => {
    const skip = (page - 1) * itemsperpage;
    fetch(`${url}?limit=${itemsperpage}&skip=${skip}`, {
        method: 'GET',
        headers:{
            'Content-Type' : 'application/json',                
        },
    }).then(async (res) => {
        if (!res.ok) {
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        displayAppointments(data);
    }).catch(error => {
        if (error.message === 'Unauthorized Access!') {
            alert("Unauthorized Access!")
        } else {
            alert(error.message);
            if (error.message == "No Appointments are available!") {
                document.getElementById('loadMoreDiv').classList.add("hide");
            }
        }
    });
}

var loadMoreData = () => {
    page++;
    fetchAppointments();
}

var displayRoomAvailability = (data) =>{
    var div = document.getElementById("roomsAvailability");
    Object.entries(data).forEach(([wardType, count]) => {
        //based on percentage change color
        div.innerHTML+=`
            <div class="w-40 h-40 border-[#4A249D] opacity-80 p-5 rounded-full" style="border-width: 15px;">
                    <div class="flex flex-col align-middle text-center w-full">
                        <h1 class="text-3xl text-indigo-900">${count}</h1>
                        <p>${wardType} ward Rooms</p>
                    </div>
            </div>
        `;
    });
}

var getRoomAvailabilityStatictics = () => {
    fetch('http://localhost:5253/api/Receptionist/CheckBedAvilability',
        {
            method:'GET',
            headers:{
                'Content-Type' : 'application/json',                
            }
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
        displayRoomAvailability(data)
    }).catch( error => {
        console.log(error)
    });
}


var displayDoctors = (data) => {
    var div = document.getElementById("doctorsList");
    data.forEach(doctor => {
        var languages = doctor.knownLanguages.join(', ');
        var availableDays = doctor.availableDays.join(",");
        var availableSlots = doctor.availableSlots.join(", ");
        //check for slots available!
        div.innerHTML += `
            <div class="relative h-90 grid-cols-1  m-5 bg-white shadow-lg rounded-lg border">
                    <div class="p-6 mt-8 text-center mx-auto">
                        <div class="relative mx-auto border-8 border-blue-900 rounded-full overflow-hidden doctorImageCard">
                            <img src="../image.jpg" alt="doctorImage"
                                class="absolute inset-0 w-full h-full object-cover">
                        </div>
                        <h2 class="text-lg font-semibold mb-2">${doctor.name}</h2>
                        <p>${doctor.doctorId}</p>
                        <p>${doctor.specialization}</p>
                        <p>${languages}</p>
                        <p>${availableDays}</p>
                        <p>Available slots : ${availableSlots}</p>
                    </div>
                    <span
                        class="absolute top-0 right-0 bg-blue-400 text-xs font-semibold px-10 py-3 rounded-tl-lg rounded-bl-lg shadow-lg">
                        ${doctor.specialization}
                    </span>
             </div>
        `;
    });
}


var getDoctors = () =>{
    var speciality = document.getElementById("speciality").value;
    const skip = (doctorPage - 1) * itemsperpage;
    fetch(`${doctorUrl}?limit=${itemsperpage}&skip=${skip}`, {
        method: 'POST',
        headers:{
            'Content-Type' : 'application/json',                
        },
        body:JSON.stringify(speciality)
    }).then(async (res) => {
        if (!res.ok) {
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.message}`);
        }
        return await res.json();
    }).then(data => {
        displayDoctors(data);
    }).catch(error => {
        if (error.message === 'Unauthorized Access!') {
            alert("Unauthorized Access!")
        } else {
            alert(error.message);
            if (error.message == "No Doctors are available!") {
                document.getElementById('loadMoreDoctor').classList.add("hide");
            }
        }
    });
}


var loadMoreDoctors = () => {
    doctorPage++;
    getDoctors();
}


var redirectToBill = () =>{
    window.location.href="./bill.html";
}

document.addEventListener("DOMContentLoaded", () => {
    page = 1;
    doctorPage=1;
    document.getElementById("slider").innerHTML = "";
    fetchAppointments();
    getRoomAvailabilityStatictics();
})



// 

// {
//     "General": 4
//   }



// http://localhost:5253/api/Receptionist/BookAppointment
// {
//     "patientId": 0,
//     "contactNo": "string",
//     "appointmentDate": "2024-07-30T09:59:52.289Z",
//     "slot": "string",
//     "doctorId": 0,
//     "description": "string",
//     "appointmentType": "string",
//     "appointmentMode": "string"
//   }