var fetchAppointments = async () => {
    await checkForRefresh()
    var doctorId = JSON.parse(localStorage.getItem('loggedInUser')).userId;
    console.log(doctorId)
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
    console.log(data)
    var todayDiv = document.getElementById("todayAppointment");
    var upcomingDiv = document.getElementById("upcomingAppointment");
    const todayDate = new Date();
    const today = `${todayDate.getFullYear()}-${(todayDate.getMonth() + 1).toString().padStart(2, '0')}-${todayDate.getDate().toString().padStart(2, '0')}`;
    console.log(today)
    data.forEach(appointment => {
        var negated = appointment.prescriptionAddedOrNot ? "none" : ""; 
        var color = appointment.appointmentStatus.toLowerCase() === "cancelled"?"text-red-400" : "" 
        var hideOrNot = appointment.appointmentStatus.toLowerCase() === "cancelled"? "hidden":"";    
        if(appointment.appointmentDate.split('T')[0] === today){
            todayDiv.innerHTML += `
                        <div class="relative h-90 m-5 pb-5 bg-white md:w-80 shadow-lg border-t-4 border-[#009fbd] rounded-2xl grow-0 shrink-0 basis-2/5">
                            <div class="grid grid-cols-2 mt-8">
                                <div class="pl-4 pr-0">
                                    <div class="grid grid-cols-3">
                                        <h4 class="font-bold text-lg">Id</h4>
                                        <h4 class="col-span-2">${appointment.appointmentId} </h4>
                                    </div>
                                    <div class="grid grid-cols-3 mt-1">
                                        <p class="font-bold">Date</p>
                                        <p class="col-span-2">${appointment.appointmentDate.split('T')[0]}</p>
                                    </div>
                                    <div class="grid grid-cols-3 mt-1">
                                        <p class="font-bold">Slot</p>
                                        <p class="col-span-2">${appointment.slot}</p>
                                    </div>
                                    <div class="grid grid-cols-3 mt-1">
                                        <p class="font-bold">Status</p>
                                        <p class="col-span-2 ${color}">${appointment.appointmentStatus}</p>
                                    </div>
                                    <div class="grid grid-cols-3 mt-1">
                                        <p class="font-bold">Type</p>
                                        <p class="col-span-2">${appointment.appointmentType}</p>
                                    </div>
                                    <div class="grid grid-cols-3 mt-1">
                                        <p class="font-bold">Reason</p>
                                        <p class="col-span-2">${appointment.description}</p>
                                    </div>
                                </div>
                                <div class="pl-1 pr-4">
                                    <p class="font-semibold text-lg">Patient Details</p>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">ID</p>
                                        <p>${appointment.patientId}</p>
                                    </div>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Name</p>
                                        <p>${appointment.patientName}</p>
                                    </div>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Age</p>
                                        <p>${appointment.age}</p>
                                    </div>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">ContactNo</p>
                                        <p>${appointment.contactNo}</p>
                                    </div>
                                </div>
                            </div>
                            <span
                                class="absolute top-0 right-0 bg-[#009fbd] text-md uppercase font-semibold px-5 py-2 rounded-tl-2xl rounded-br-2xl rounded-tr-2xl shadow-lg">
                                Today
                            </span>
                            <div class="flex flex-row flex-wrap justify-evenly">
                                 <div class="flex flex-row justify-center mx-auto">
                                <button type="button" class="w-full  py-2 px-4 my-1 bg-[#009fbd] font-bold rounded-lg text-[#f6f5f5] border-2 border-[#009fbd] hover:bg-[#f6f5f5] hover:text-[#009fbd] ${hideOrNot}" onclick="cancelAppointment('${appointment.appointmentId}')">Cancel</button>
                                </div>
                                <div class="flex flex-row justify-center mx-auto">
                                        <button type="button"  class="w-full px-4 py-2 my-1 bg-[#009fbd] text-[#f6f5f5] border-[#009fbd] border-2 font-bold rounded-lg ${negated} ${hideOrNot} hover:bg-[#f6f5f5] hover:text-[#009fbd]" onclick="redirectToPrescription('${appointment.appointmentId}', '${appointment.patientId}')">Add Prescription</button>
                                </div>
                                <div class="flex flex-row justify-center mx-auto">
                                    <button type="button"  class="w-full py-2 px-4 my-1 bg-[#009fbd] font-bold border-[#009fbd] border-2 rounded-lg text-[#f6f5f5] ${hideOrNot} hover:bg-[#f6f5f5] hover:text-[#009fbd]"  onclick="redirectToMedicalRecord('${appointment.appointmentId}','${appointment.patientId}')">Medical Record</button>
                                </div>
                            </div>
                        </div> 
        `;
        }else{
            upcomingDiv.innerHTML+=`
                <div class="relative h-90 m-5 pb-5 bg-white md:w-80 shadow-lg border-t-4 border-[#e9c46a] rounded-2xl grow-0 shrink-0 basis-2/5">
                            <div class="grid grid-cols-2 mt-8">
                                <div class="pl-4 pr-0">
                                    <div class="grid grid-cols-3">
                                        <h4 class="font-bold text-lg">Id</h4>
                                        <h4 class="col-span-2">${appointment.appointmentId} </h4>
                                    </div>
                                    <div class="grid grid-cols-3 mt-1">
                                        <p class="font-bold">Date</p>
                                        <p class="col-span-2">${appointment.appointmentDate.split('T')[0]}</p>
                                    </div>
                                    <div class="grid grid-cols-3 mt-1">
                                        <p class="font-bold">Slot</p>
                                        <p class="col-span-2">${appointment.slot}</p>
                                    </div>
                                    <div class="grid grid-cols-3 mt-1">
                                        <p class="font-bold">Status</p>
                                        <p class="col-span-2 ${color}">${appointment.appointmentStatus}</p>
                                    </div>
                                    <div class="grid grid-cols-3 mt-1">
                                        <p class="font-bold">Type</p>
                                        <p class="col-span-2">${appointment.appointmentType}</p>
                                    </div>
                                    <div class="grid grid-cols-3 mt-1">
                                        <p class="font-bold">Reason</p>
                                        <p class="col-span-2">${appointment.description}</p>
                                    </div>
                                </div>
                                <div class="pl-1 pr-4">
                                    <p class="font-semibold text-lg">Patient Details</p>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">ID</p>
                                        <p>${appointment.patientId}</p>
                                    </div>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Name</p>
                                        <p>${appointment.patientName}</p>
                                    </div>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Age</p>
                                        <p>${appointment.age}</p>
                                    </div>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">ContactNo</p>
                                        <p>${appointment.contactNo}</p>
                                    </div>
                                </div>
                            </div>
                            <span
                                class="absolute top-0 right-0 bg-[#e9c46a] text-md uppercase font-semibold px-5 py-2 rounded-tl-2xl rounded-br-2xl rounded-tr-2xl shadow-lg">
                                Today
                            </span>
                                 <div class="flex flex-row justify-center mx-auto mt-5" style="width:30%">
                                <button type="button" class="w-full  py-2 px-4 my-1 bg-[#e9c46a] font-bold rounded-lg border-2 border-[#e9c46a] ${hideOrNot} hover:bg-[#f6f5f5] hover:text-[#e9c46a]" onclick="cancelAppointment('${appointment.appointmentId}')">Cancel</button>
                                </div>
                        </div> 
            `;
        }
        if(todayDiv.children.length < 2){
            document.getElementById("displayInformationToday").classList.remove('none');
        }
        if(upcomingDiv.childNodes.length < 2){
            document.getElementById("displayInformationUpcoming").classList.remove('none');
        }
        
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


var cancelAppointment = async (appointmentId) =>{
    await checkForRefresh()
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