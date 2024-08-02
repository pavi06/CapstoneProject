var url="http://localhost:5253/api/Patient/MyAppointments";
var page = 1;
const itemsperpage = 15;

var displayAppointments = (data) =>{
    var todayAppointment = document.getElementById("todayAppointment");
    var upcomingAppointment = document.getElementById("upcomingAppointment");
    var completedAppointment = document.getElementById("completedAppointment");
    const todayDate = new Date();
    const today = `${todayDate.getFullYear()}-${(todayDate.getMonth() + 1).toString().padStart(2, '0')}-${todayDate.getDate().toString().padStart(2, '0')}`;
    data.forEach(appointment => {
        var hideOrNot="";
        var color="";
        if(appointment.appointmentStatus.toLowerCase() === "cancelled"){
            hideOrNot="hidden";
            color="text-red-400"
        }
        console.log(hideOrNot)
        if(appointment.appointmentDate.split('T')[0] === today){
            todayAppointment.innerHTML+=`
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
                                        <p class="col-span-2">${appointment.slotConfirmed}</p>
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
                                    <p class="font-semibold text-lg">Doctor Details</p>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Name</p>
                                        <p>${appointment.doctorName}</p>
                                    </div>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Age</p>
                                        <p>${appointment.specialization}</p>
                                    </div>
                                </div>
                            </div>
                            <span
                                class="absolute top-0 right-0 bg-[#009fbd] text-md uppercase font-semibold px-5 py-2 rounded-tl-2xl rounded-br-2xl rounded-tr-2xl shadow-lg">
                                Today
                            </span>
                            <div class="flex flex-row justify-center mx-auto mt-3 mb-2" style=width:20%>
                                <button type="button" class="w-full  py-2 px-4 my-1 bg-[#009fbd] font-bold rounded-lg text-[#f6f5f5] border-2 border-[#009fbd] hover:bg-[#f6f5f5] hover:text-[#009fbd]" onclick="cancelAppointment('${appointment.appointmentId}')">Cancel</button>
                            </div>
                        </div> 
            `;
        }
        else if(appointment.appointmentStatus.toLowerCase() === "scheduled" && appointment.appointmentDate.split('T')[0] > today){
            console.log("here")
            upcomingAppointment.innerHTML+=`
                    <div class="relative h-90 m-5 pb-5 bg-white md:w-80 shadow-lg border-t-4 border-[#e9c46a] rounded-2xl grow-0 shrink-0 basis-2/5">
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
                                        <p class="col-span-2">${appointment.slotConfirmed}</p>
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
                                    <p class="font-semibold text-lg">Doctor Details</p>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Name</p>
                                        <p>${appointment.doctorName}</p>
                                    </div>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Age</p>
                                        <p>${appointment.specialization}</p>
                                    </div>
                                </div>
                            </div>
                            <span
                                class="absolute top-0 right-0 bg-[#e9c46a] text-md uppercase font-semibold px-5 py-2 rounded-tl-2xl rounded-br-2xl rounded-tr-2xl shadow-lg">
                                Upcoming
                            </span>
                            <div class="flex flex-row justify-center mx-auto mt-3 mb-2" style=width:20%>
                                <button type="button" class="w-full  py-2 px-4 my-1 bg-[#e9c46a] font-bold rounded-lg border-2 border-[#e9c46a] hover:bg-[#f6f5f5] hover:text-[#e9c46a]" onclick="cancelAppointment('${appointment.appointmentId}')">Cancel</button>
                            </div>
                        </div>                          
            `;
        }
        else{
            completedAppointment.innerHTML+=`
                        <div class="relative h-90 m-5 pb-5 bg-white md:w-80 shadow-lg border-t-4 border-[#36ba98] rounded-2xl grow-0 shrink-0 basis-2/5">
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
                                        <p class="col-span-2">${appointment.slotConfirmed}</p>
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
                                    <p class="font-semibold text-lg">Doctor Details</p>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Name</p>
                                        <p>${appointment.doctorName}</p>
                                    </div>
                                    <div class="grid grid-cols-2 mt-2">
                                        <p class="font-bold">Age</p>
                                        <p>${appointment.specialization}</p>
                                    </div>
                                </div>
                            </div>
                            <span
                                class="absolute top-0 right-0 bg-[#36ba98] text-md uppercase font-semibold px-5 py-2 rounded-tl-2xl rounded-br-2xl rounded-tr-2xl shadow-lg">
                                Completed
                            </span>
                            <div class="flex flex-row justify-center mx-auto mt-3 mb-2" style=width:50%>
                                <button type="button" class="w-full  py-2 px-4 my-1 bg-[#36ba98] font-bold rounded-lg text-[#f6f5f5] ${hideOrNot}  border-2 border-[#36ba98] hover:bg-[#f6f5f5] hover:text-[#36ba98]" onclick="viewPrescription('${appointment.appointmentId}')">View Prescription</button>
                            </div>
                        </div>                        
            `;
        }
        if(todayAppointment.children.length < 1){
            document.getElementById("displayInformationToday").classList.remove('none');
        }
        console.log(upcomingAppointment.childNodes.length)
        if(upcomingAppointment.childNodes.length <1 ){
            document.getElementById("displayInformationUpcoming").classList.remove('none');
        }
        if(completedAppointment.childNodes.length <1){
            document.getElementById("displayInformationCompleted").classList.remove('none');
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
    getAllAppointments();
}

var viewPrescription = (appointmentId) =>{
    window.location.href=`./prescription.html?search=${appointmentId}`;
}

var getAllAppointments = () =>{
    const skip = (page - 1) * itemsperpage;
    var patientId = JSON.parse(localStorage.getItem('loggedInUser')).userId; 
    fetch(`${url}?patientId=${patientId}&limit=${itemsperpage}&skip=${skip}`,
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

document.addEventListener("DOMContentLoaded",() => {
    updateForLogInAndOut();
    page=1;
    getAllAppointments();
})