

var fetchAppointments = () => {
    var doctorId = 1;
    fetch(`http://localhost:5253/api/Doctor/GetTodayAppointment?doctorId=${doctorId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
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
        }
    });
}

var displayAppointments = (data) => {
    var div = document.getElementById("appointmentSlider");
    data.forEach(appointment => {
        var hideOrNot = appointment.appointmentStatus=="Cancelled"?"hide":"";
        div.innerHTML += `
            <div class="relative h-90 grid-cols-2  m-5 shadow-lg rounded-lg border">
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
                                    <p>${appointment.patientId}</p>
                                    <p>${appointment.patientName}</p>
                                    <p>${appointment.age}</p>
                                    <p>${appointment.contactNo}</p>
                                </div>
                            </div>
                            <span
                                class="absolute top-0 right-0 bg-green-400 text-xs font-semibold px-10 py-3 rounded-tl-lg rounded-bl-lg shadow-lg">
                                Today
                            </span>
                            <div class="flex flex-wrap">
                                <div class="flex flex-row justify-center">
                                <button type="button" class="p-3 m-3 bg-[#4A249D] text-white rounded-lg" onclick="cancelAppointment('${appointment.appointmentId}')">Cancel</button>
                                </div>
                                <div class="flex flex-row justify-center">
                                    <button id="createPrescriptionBtn" type="button" class="p-3 m-3 bg-[#4A249D] text-white rounded-lg" onclick="redirectToPrescription('${appointment.appointmentId}', '${appointment.patientId}')">Add Prescription</button>
                                </div>
                                <div class="flex flex-row justify-center">
                                    <button type="button" class="p-3 m-3 bg-[#4A249D] text-white rounded-lg hide" onclick="redirectToPrescription('${appointment.appointmentId}')">view Prescription</button>
                                </div>
                                <div class="flex flex-row justify-center">
                                    <button type="button" class="p-3 m-3 bg-[#4A249D] text-white rounded-lg" onclick="redirectToMedicalRecord('${appointment.appointmentId}','${appointment.patientId}')">Medical Record</button>
                                </div>
                            </div>
                            <div class="absolute top-0 w-full h-full bg-gray-500 opacity-70 ${hideOrNot}" id="backgroundDiv${appointment.appointmentId}"></div>
                            <span id="cancelTag${appointment.appointmentId}"
                                class="absolute opacity-100 ${hideOrNot} uppercase font-extrabold text-center text-xl top-14 z-10 w-full bg-[#4A249D] text-white px-10 py-8 shadow-lg">
                                Cancelled
                            </span>
                        </div>
        `;
    });
}


var redirectToPrescription = (appointmentId, patientId) =>{
    localStorage.setItem("appointmentId", appointmentId)
    localStorage.setItem("patientId", patientId)
    window.location.href="./providePrescription.html"
}

var redirectToMedicalRecord = (appointmentId,patientId) => {
    localStorage.setItem("appointmentId", appointmentId)
    localStorage.setItem("patientId", patientId)
    window.location.href="./AddMedicalRecord.html";
}


var cancelAppointment = (appointmentId) =>{
    fetch(`http://localhost:5253/api/Doctor/CancelAppointment?appointmentId=${appointmentId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
    }).then(async (res) => {
        if (!res.ok) {
            if (res.status === 401) {
                throw new Error('Unauthorized Access!');
            }
            const errorResponse = await res.json();
            throw new Error(`${errorResponse.message}`);
        }
        return await res.text();
    }).then(data => {
        alert(data);
        document.getElementById(`cancelTag${appointmentId}`).classList.remove('hide');
        document.getElementById(`backgroundDiv${appointmentId}`).classList.remove('hide');
    }).catch(error => {
        if (error.message === 'Unauthorized Access!') {
            alert("Unauthorized Access!")
        } else {
            alert(error.message);
        }
    });
}


document.addEventListener("DOMContentLoaded", () => {
    fetchAppointments();
})