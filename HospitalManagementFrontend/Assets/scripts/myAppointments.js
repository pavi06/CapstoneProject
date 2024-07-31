var url="http://localhost:5253/api/Patient/MyAppointments";
var page = 1;
const itemsperpage = 15;

var displayAppointments = (data) =>{
    var todayAppointment = document.getElementById("todayAppointment");
    var upcomingAppointment = doccument.getElementById("upcomingAppointment");
    var completedAppointment = document.getElementById("completedAppointment");
    const todayDate = new Date();
    const today = `${todayDate.getFullYear()}-${(todayDate.getMonth() + 1).toString().padStart(2, '0')}-${todayDate.getDate().toString().padStart(2, '0')}`;
    data.forEach(appointment => {
        if(appointment.appointmentDate == today){
            todayAppointment.innerHTML+=`
                        <div class="relative h-90 grid-cols-1  m-5 bg-white shadow-lg rounded-lg border">
                            <div class="flex flex-row mt-8">
                                <div class="p-6">
                                    <h2 class="text-lg font-semibold mb-2">${appointment.appointmentId}</h2>
                                    <p>${appointment.appointmentDate}</p>
                                    <p>${appointment.slotConfirmed}</p>
                                    <p>${appointment.description}</p>
                                    <p>${appointment.appointmentStatus}</p>
                                    <p>${appointment.appointmentType}</p>
                                </div>
                                <div class="p-6">
                                    <p>${appointment.doctorName}</p>
                                    <p>${appointment.specialization}</p>
                                </div>
                            </div>
                            <span
                                class="absolute top-0 right-0 bg-green-400 text-xs font-semibold px-10 py-3 rounded-tl-lg rounded-bl-lg shadow-lg">
                                Today
                            </span>
                            <div class="flex flex-row justify-center">
                                <button type="button" class="p-3 m-3 bg-[#4A249D] text-white rounded-lg" onclick="cancelAppointment('${appointment.appointmentId}')">Cancel</button>
                            </div>
                        </div>

            `;
        }
        else if(appointment.status == "scheduled"){
            upcomingAppointment.innerHTML+=`
                        <div class="relative h-90 grid-cols-1  m-5 bg-white shadow-lg rounded-lg border">
                            <div class="flex flex-row mt-8">
                                <div class="p-6">
                                    <h2 class="text-lg font-semibold mb-2">${appointment.appointmentId}</h2>
                                    <p>${appointment.appointmentDate}</p>
                                    <p>${appointment.slotConfirmed}</p>
                                    <p>${appointment.description}</p>
                                    <p>${appointment.appointmentStatus}</p>
                                    <p>${appointment.appointmentType}</p>
                                </div>
                                <div class="p-6">
                                    <p>${appointment.doctorName}</p>
                                    <p>${appointment.specialization}</p>
                                </div>
                            </div>
                            <span
                                class="absolute top-0 right-0 bg-green-400 text-xs font-semibold px-10 py-3 rounded-tl-lg rounded-bl-lg shadow-lg">
                                Upcoming
                            </span>
                            <div class="flex flex-row justify-center">
                                <button type="button" class="p-3 m-3 bg-[#4A249D] text-white rounded-lg" onclick="cancelAppointment('${appointment.appointmentId}')">Cancel</button>
                            </div>
                        </div>
            `;
        }
        else{
            completedAppointment.innerHTML+=`
                        <div class="relative h-90 grid-cols-1  m-5 bg-white shadow-lg rounded-lg border">
                            <div class="flex flex-row mt-8">
                                <div class="p-6">
                                    <h2 class="text-lg font-semibold mb-2">${appointment.appointmentId}</h2>
                                    <p>${appointment.appointmentDate}</p>
                                    <p>${appointment.slotConfirmed}</p>
                                    <p>${appointment.description}</p>
                                    <p>${appointment.appointmentStatus}</p>
                                    <p>${appointment.appointmentType}</p>
                                </div>
                                <div class="p-6">
                                    <p>${appointment.doctorName}</p>
                                    <p>${appointment.specialization}</p>
                                </div>
                            </div>
                            <span
                                class="absolute top-0 right-0 bg-green-400 text-xs font-semibold px-10 py-3 rounded-tl-lg rounded-bl-lg shadow-lg">
                                Completed
                            </span>
                            <div class="flex flex-row justify-center">
                                <button type="button" class="p-3 m-3 bg-[#4A249D] text-white rounded-lg" onclick="viewPrescription('${appointment.appointmentId}')">View Prescription</button>
                            </div>
                        </div>
            `;
        }
    });
}

var cancelAppointment = (appointmentId) =>{
    fetch(`http://localhost:5253/api/Patient/CancelAppointment?appointmentId=${appointmentId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        }
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
    }).catch(error => {
        if (error.message === 'Unauthorized Access!') {
            alert("Unauthorized Access!")
        } else {
            alert(error.message);
        }
    });
}

var viewPrescription = (appointmentId) =>{
    window.location.href=`./prescription.html?search=${appointmentId}`;
}

var getAllAppointments = () =>{
    const skip = (page - 1) * itemsperpage;
    fetch(`${url}?limit=${itemsperpage}&skip=${skip}`,
        {
            method:'GET',
            headers:{
                'Content-Type' : 'application/json',                
            },
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
        displayAppointments(data)
    }).catch( error => {
        console.log(error)
    });    
}

var loadMoreData = () =>{
    page++;
    getAllAppointments();
}

document.addEventListener("DOMContentLoaded", () => {
    page=1;
    getAllAppointments();
})