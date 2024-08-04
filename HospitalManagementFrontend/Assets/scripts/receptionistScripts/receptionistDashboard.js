var doctorUrl = "http://localhost:5253/api/Receptionist/CheckDoctorAvailability";
var page = 1;
var doctorPage=1;
const itemsperpage = 5;

var displayAppointments = (data) => {
    var div = document.getElementById("slider");
    if(data.length === 0 && div.children.length === 0){
        div.innerHTML=`
            <div id="displayInformation" class="mt-5 p-5 mb-5 border-l-4 border-red-400 bg-red-100 rounded-lg shadow-lg" style="width: 80%;">
                <h1 class="font-semibold"><i class='bx bxs-bell-ring bx-lg text-red-500 mr-3'></i>&nbsp;No Appointments available today! &nbsp;</h1>
            </div>
        `;
    }
    data.forEach(appointment => {
        var color="";
        var hideOrNot="";
        if(appointment.appointmentStatus.toLowerCase() === "cancelled"){
            hideOrNot="hidden";
            color="text-red-400"
        }
        div.innerHTML += `
            <div class="relative h-90 m-5 pb-5 bg-white md:w-80 shadow-lg border-t-4 border-[#009fbd] rounded-2xl grow-0 shrink-0 basis-2/5">
                            <div class="grid grid-cols-2 mt-12">
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
                                        <p class="font-bold">Doctor Name</p>
                                        <p>${appointment.doctorName}</p>
                                    </div>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Specialization</p>
                                        <p>${appointment.specialization}</p>
                                    </div>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Patient Name</p>
                                        <p>${appointment.patientName}</p>
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
                                 <div class="flex flex-row justify-center mx-auto my-5" style="width:40%">
                                    <button type="button" class="w-full  py-2 px-4 my-1 bg-[#009fbd] font-bold rounded-lg text-[#f6f5f5] border-2 border-[#009fbd] hover:bg-[#f6f5f5] hover:text-[#009fbd] ${hideOrNot}" onclick="cancelAppointment('${appointment.appointmentId}')">Cancel</button>
                                </div>
                        </div>     
        `;
    });
}

var appointmentsSkeleton = () =>{
    document.getElementById("slider").innerHTML = `
         <div class="relative h-90 m-5 pb-5 bg-white md:w-80 shadow-lg border-t-4 border-[#009fbd] rounded-2xl grow-0 shrink-0 basis-2/5">
    <div class="grid grid-cols-2 mt-12">
        <div class="pl-4 pr-0">
            <div class="grid grid-cols-3">
                <div class="w-16 h-4 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-16 h-4 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-16 h-4 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-16 h-4 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-16 h-4 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-16 h-4 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
        </div>

        <div class="pl-1 pr-4">
            <div class="w-40 h-5 bg-gray-300 animate-pulse mb-2 rounded"></div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
    </div>

    <span class="absolute top-0 right-0 bg-gray-300 text-transparent text-md uppercase font-semibold px-5 py-2 rounded-tl-2xl rounded-br-2xl rounded-tr-2xl shadow-lg">
        <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
    </span>

    <div class="flex flex-row justify-center mx-auto my-5" style="width:40%">
        <button type="button" class="w-full py-2 px-4 my-1 bg-gray-300 font-bold rounded-lg text-transparent border-2 border-gray-300">
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
        </button>
    </div>
    </div>
    `;

}

var removeAppointmentSkeleton = () =>{
    document.getElementById("slider").innerHTML ="";
}

var fetchAppointments = async (skeletonStatus) => {
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    const skip = (page - 1) * itemsperpage;
    await checkForRefresh()
    if(skeletonStatus){
        appointmentsSkeleton();
    }
    fetch(`http://localhost:5253/api/Receptionist/GetTodayAppointmentDetails?limit=${itemsperpage}&skip=${skip}`, {
        method: 'GET',
        headers:{
            'Content-Type' : 'application/json',  
            'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`              
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
        if(skeletonStatus){
            removeAppointmentSkeleton();
        }
        displayAppointments(data);
    }).catch(error => {
        if(skeletonStatus){
            removeAppointmentSkeleton();
        }
        if (error.message === 'Unauthorized Access!') {
            openModal('alertModal', "Error", "Unauthorized Access!");
        } else {
            openModal('alertModal', "Error", error.message);
            if (error.message == "No Appointments are available!") {
                document.getElementById('loadMoreDiv').classList.add("hide");
                if(document.getElementById("slider").children.length===0){
                    document.getElementById("slider").innerHTML=`
                    <div id="displayInformation" class="mt-5 p-5 mb-5 border-l-4 border-red-400 mx-auto bg-red-100 rounded-lg shadow-lg" style="width: 80%;">
                        <h1 class="font-semibold"><i class='bx bxs-bell-ring bx-lg text-red-500 mr-3'></i>&nbsp;No Appointments available today! &nbsp;</h1>
                    </div>
                `;
                }
            }
        }
    });
}

var loadMoreData = () => {
    page++;
    fetchAppointments(false);
}

var displayRoomAvailability = (data) =>{
    var div = document.getElementById("roomsAvailability");
    Object.entries(data).forEach(([wardType, count]) => {
        var color = wardType === "General" ? "[#009fbd]" : wardType === "ICU" ? "[#36ba98]" : "[#e9c46a]";
        div.innerHTML+=`
            <div class="w-40 h-40 border-${color} opacity-80 p-5 rounded-full" style="border-width: 10px;">
                    <div class="flex flex-col align-middle text-center w-full">
                        <h1 class="text-4xl font-bold text-${color}">${count}</h1>
                        <p class="font-semibold">${wardType} ward Rooms</p>
                    </div>
            </div>
        `;
    });
}

var getRoomAvailabilityStatictics = async () => {
    await checkForRefresh()
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    fetch('http://localhost:5253/api/Receptionist/CheckBedAvilability',
        {
            method:'GET',
            headers:{
                'Content-Type' : 'application/json',  
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`              
            }
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
        displayRoomAvailability(data)
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
    });
}


var displayDoctors = (data) => {
    var div = document.getElementById("doctorsList");
    data.forEach(doctor => {
        var languages = doctor.knownLanguages.join(', ');
        var availableDays = doctor.availableDays.join(",");
        var availableSlots = doctor.availableSlots.join(", ");
        div.innerHTML += `
            <div class="relative h-90 m-5 bg-white shadow-lg rounded-lg border" style="width: 400px;">
                    <div class="p-6 mt-10 mx-auto">
                        <div class="relative mx-auto border-8 border-[#009fbd] rounded-full overflow-hidden doctorImageCard">
                            <img src="../../image.jpg" alt="doctorImage"
                                class="absolute inset-0 w-full h-full object-cover">
                        </div>
                        <div class="mx-auto w-full">
                            <div class="grid grid-cols-2 mt-1">
                                <p class="font-bold">DoctorName:</p>
                                <p>${doctor.name}</p>
                            </div>
                            <div class="grid grid-cols-2 mt-1">
                                <p class="font-bold">DoctorId:</p>
                                <p>${doctor.doctorId}</p>
                            </div>
                            <div class="grid grid-cols-2 mt-1">
                                <p class="font-bold">Specialization:</p>
                                <p>${doctor.specialization}</p>
                            </div>
                            <div class="grid grid-cols-2 mt-1">
                                <p class="font-bold">Languages Known:</p>
                                <p>${languages}</p>
                            </div>
                            <div class="grid grid-cols-2 mt-1">
                                <p class="font-bold">Available Days:</p>
                                <p>${availableDays}</p>
                            </div>
                            <div class="grid grid-cols-2 mt-1">
                                <p class="font-bold">Available Slots:</p>
                                <p>${availableSlots}</p>
                            </div>
                        </div>
                    </div>
                    <span
                        class="absolute top-0 right-0 bg-[#009fbd] text-[#f6f5f5] text-md font-semibold px-10 py-3 rounded-tl-3xl rounded-br-3xl shadow-lg">
                        ${doctor.specialization}
                    </span>
                    <div class="w-full flex justify-center mb-5"> 
                        <button type="button" onclick="bookAppointment(${doctor.doctorId})"
                        class="hover:bg-white border-2 mx-5 hover:border-[#009fbd] hover:text-[#009fbd] rounded-md bg-[#009fbd] text-[#f6f5f5] w-25 py-2 px-5 text-black-900 text-center text-base font-semibold outline-none">
                        Book Appointment
                        </button>
                    </div>
                </div> 
        `;
    });
}

var displayDoctorsSkeleton = () =>{
    var div = document.getElementById("doctorsList");
    div.innerHTML +=`
        <div class="relative h-90 m-5 bg-white shadow-lg rounded-lg border" style="width: 400px;">
    <div class="p-6 mt-10 mx-auto">
        <div class="relative mx-auto border-8 border-[#009fbd] rounded-full overflow-hidden doctorImageCard">
            <div class="w-full h-full bg-gray-300 animate-pulse"></div>
        </div>

        <div class="mx-auto w-full mt-4">
            <div class="grid grid-cols-2 mt-1">
                <p class="font-bold bg-gray-300 animate-pulse w-24 h-4 rounded"></p>
                <p class="bg-gray-300 animate-pulse w-32 h-4 rounded"></p>
            </div>
            <div class="grid grid-cols-2 mt-1">
                <p class="font-bold bg-gray-300 animate-pulse w-24 h-4 rounded"></p>
                <p class="bg-gray-300 animate-pulse w-32 h-4 rounded"></p>
            </div>
            <div class="grid grid-cols-2 mt-1">
                <p class="font-bold bg-gray-300 animate-pulse w-24 h-4 rounded"></p>
                <p class="bg-gray-300 animate-pulse w-32 h-4 rounded"></p>
            </div>
            <div class="grid grid-cols-2 mt-1">
                <p class="font-bold bg-gray-300 animate-pulse w-24 h-4 rounded"></p>
                <p class="bg-gray-300 animate-pulse w-32 h-4 rounded"></p>
            </div>
            <div class="grid grid-cols-2 mt-1">
                <p class="font-bold bg-gray-300 animate-pulse w-24 h-4 rounded"></p>
                <p class="bg-gray-300 animate-pulse w-32 h-4 rounded"></p>
            </div>
            <div class="grid grid-cols-2 mt-1">
                <p class="font-bold bg-gray-300 animate-pulse w-24 h-4 rounded"></p>
                <p class="bg-gray-300 animate-pulse w-32 h-4 rounded"></p>
            </div>
        </div>
    </div>

    <span class="absolute top-0 right-0 bg-gray-300 text-transparent text-md font-semibold px-10 py-3 rounded-tl-3xl rounded-br-3xl shadow-lg">
        <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
    </span>

    <div class="w-full flex justify-center mb-5">
        <button type="button" class="bg-gray-300 text-transparent w-25 py-2 px-5 rounded-md text-base font-semibold">
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
        </button>
    </div>
    </div>    
    `;
}

var removeDoctorSkeleton = () =>{
    document.getElementById("doctorsList").innerHTML="";
}

var getDoctors = async (skeletonStatus) =>{
    if(!validate('speciality')){
        openModal('alertModal', "Error", "Choose the valid speciality");
        return;
    }
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    if(skeletonStatus){
        displayDoctorsSkeleton();
    }
    const skip = (doctorPage - 1) * itemsperpage;
    await checkForRefresh()
    fetch(`${doctorUrl}?limit=${itemsperpage}&skip=${skip}`, {
        method: 'POST',
        headers:{
            'Content-Type' : 'application/json',  
            'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`              
        },
        body:JSON.stringify(document.getElementById("speciality").value)
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
        if(skeletonStatus){
            removeDoctorSkeleton();
        }
        displayDoctors(data);
    }).catch(error => {
        if(skeletonStatus){
            displayDoctorsSkeleton();
        }
        if (error.message === 'Unauthorized Access!') {
            openModal('alertModal', "Error", "Unauthorized Access!");
        } else {
            openModal('alertModal', "Error", error.message);
            if (error.message === "No Doctor are available!") {
                document.getElementById('loadMoreDoctor').classList.add("hide");
                if(document.getElementById("doctorsList").children.length===0){
                    document.getElementById("doctorsList").innerHTML=`
                    <div id="displayInformation" class="mt-5 p-5 mb-5 border-l-4 border-red-400 mx-auto bg-red-100 rounded-lg shadow-lg" style="width: 80%;">
                        <h1 class="font-semibold"><i class='bx bxs-bell-ring bx-lg text-red-500 mr-3'></i>&nbsp;No doctors available! &nbsp;</h1>
                    </div>
                `;
                }
            }
        }
    });
}


var loadMoreDoctors = () => {
    doctorPage++;
    getDoctors(false);
}

var redirectToInPatientManagement = () =>{
    window.location.href="./inPatient.html";
}

var redirectToBill = () =>{
    window.location.href="./billDetails.html";
}

var bookAppointment = (doctorId) => {
    window.location.href=`./RepBookAppointment.html?search=${doctorId}`;
}

document.addEventListener("DOMContentLoaded", () => {
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    page = 1;
    doctorPage=1;
    document.getElementById("slider").innerHTML = "";
    fetchAppointments(true);
    getRoomAvailabilityStatictics();
})
