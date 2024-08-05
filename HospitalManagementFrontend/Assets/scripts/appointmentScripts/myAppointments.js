var url="https://pavihosmanagebeapp.azurewebsites.net/api/Patient/MyAppointments";
var page = 1;
const itemsperpage = 15;

var displayAppointmentsSkeleton = () =>{
    document.getElementById("todayAppointment").innerHTML=`
         <div class="relative h-90 m-5 pb-5 bg-white md:w-80 shadow-lg border-t-4 border-[#009fbd] rounded-2xl grow-0 shrink-0 basis-2/5">
    <div class="grid grid-cols-2 mt-8">
        <div class="pl-4 pr-0">
            <div class="grid grid-cols-3">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
        </div>
        <div class="pl-1 pr-4">
            <div class="font-semibold text-lg">
                <div class="w-48 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
    </div>
    <div class="absolute top-0 right-0 bg-[#009fbd] text-md uppercase font-semibold px-5 py-2 rounded-tl-2xl rounded-br-2xl rounded-tr-2xl shadow-lg">
        <div class="w-20 h-4 bg-gray-300 animate-pulse rounded"></div>
    </div>
    <div class="flex flex-row flex-wrap justify-evenly mt-5">
        <div class="flex flex-row justify-center mx-auto">
            <div class="w-full py-2 px-4 my-1 bg-[#009fbd] text-[#f6f5f5] font-bold rounded-lg border-2 border-[#009fbd] hover:bg-[#f6f5f5] hover:text-[#009fbd] cursor-pointer">
                <div class="w-32 h-8 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
        <div class="flex flex-row justify-center mx-auto">
            <div class="w-full px-4 py-2 my-1 bg-[#009fbd] text-[#f6f5f5] border-[#009fbd] border-2 font-bold rounded-lg hover:bg-[#f6f5f5] hover:text-[#009fbd] cursor-pointer">
                <div class="w-32 h-8 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
        <div class="flex flex-row justify-center mx-auto">
            <div class="w-full py-2 px-4 my-1 bg-[#009fbd] text-[#f6f5f5] border-[#009fbd] border-2 rounded-lg cursor-pointer">
                <div class="w-32 h-8 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
    </div>
</div>
    `;

    document.getElementById("upcomingAppointment").innerHTML=`
         <div class="relative h-90 m-5 pb-5 bg-white md:w-80 shadow-lg border-t-4 border-[#009fbd] rounded-2xl grow-0 shrink-0 basis-2/5">
    <div class="grid grid-cols-2 mt-8">
        <div class="pl-4 pr-0">
            <div class="grid grid-cols-3">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
        </div>
        <div class="pl-1 pr-4">
            <div class="font-semibold text-lg">
                <div class="w-48 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
    </div>
    <div class="absolute top-0 right-0 bg-[#009fbd] text-md uppercase font-semibold px-5 py-2 rounded-tl-2xl rounded-br-2xl rounded-tr-2xl shadow-lg">
        <div class="w-20 h-4 bg-gray-300 animate-pulse rounded"></div>
    </div>
    <div class="flex flex-row flex-wrap justify-evenly mt-5">
        <div class="flex flex-row justify-center mx-auto">
            <div class="w-full py-2 px-4 my-1 bg-[#009fbd] text-[#f6f5f5] font-bold rounded-lg border-2 border-[#009fbd] hover:bg-[#f6f5f5] hover:text-[#009fbd] cursor-pointer">
                <div class="w-32 h-8 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
        <div class="flex flex-row justify-center mx-auto">
            <div class="w-full px-4 py-2 my-1 bg-[#009fbd] text-[#f6f5f5] border-[#009fbd] border-2 font-bold rounded-lg hover:bg-[#f6f5f5] hover:text-[#009fbd] cursor-pointer">
                <div class="w-32 h-8 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
        <div class="flex flex-row justify-center mx-auto">
            <div class="w-full py-2 px-4 my-1 bg-[#009fbd] text-[#f6f5f5] border-[#009fbd] border-2 rounded-lg cursor-pointer">
                <div class="w-32 h-8 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
    </div>
</div>
    `;

    document.getElementById("completedAppointment").innerHTML=`
         <div class="relative h-90 m-5 pb-5 bg-white md:w-80 shadow-lg border-t-4 border-[#009fbd] rounded-2xl grow-0 shrink-0 basis-2/5">
    <div class="grid grid-cols-2 mt-8">
        <div class="pl-4 pr-0">
            <div class="grid grid-cols-3">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
            <div class="grid grid-cols-3 mt-1">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="col-span-2">
                    <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
                </div>
            </div>
        </div>
        <div class="pl-1 pr-4">
            <div class="font-semibold text-lg">
                <div class="w-48 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
            <div class="grid grid-cols-2 mt-2">
                <div class="w-24 h-6 bg-gray-300 animate-pulse rounded"></div>
                <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
    </div>
    <div class="absolute top-0 right-0 bg-[#009fbd] text-md uppercase font-semibold px-5 py-2 rounded-tl-2xl rounded-br-2xl rounded-tr-2xl shadow-lg">
        <div class="w-20 h-4 bg-gray-300 animate-pulse rounded"></div>
    </div>
    <div class="flex flex-row flex-wrap justify-evenly mt-5">
        <div class="flex flex-row justify-center mx-auto">
            <div class="w-full py-2 px-4 my-1 bg-[#009fbd] text-[#f6f5f5] font-bold rounded-lg border-2 border-[#009fbd] hover:bg-[#f6f5f5] hover:text-[#009fbd] cursor-pointer">
                <div class="w-32 h-8 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
        <div class="flex flex-row justify-center mx-auto">
            <div class="w-full px-4 py-2 my-1 bg-[#009fbd] text-[#f6f5f5] border-[#009fbd] border-2 font-bold rounded-lg hover:bg-[#f6f5f5] hover:text-[#009fbd] cursor-pointer">
                <div class="w-32 h-8 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
        <div class="flex flex-row justify-center mx-auto">
            <div class="w-full py-2 px-4 my-1 bg-[#009fbd] text-[#f6f5f5] border-[#009fbd] border-2 rounded-lg cursor-pointer">
                <div class="w-32 h-8 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
    </div>
</div>
    `;
}

var displayAppointmentsSkeletonRemove = () =>{
    document.getElementById("completedAppointment").innerHTML="";
    document.getElementById("todayAppointment").innerHTML="";
    document.getElementById("upcomingAppointment").innerHTML="";
};

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
        if(appointment.appointmentDate.split('T')[0] === today){
            todayAppointment.innerHTML+=`
                        <div class="relative h-90 m-5 pb-5 bg-white shadow-lg border-t-4 border-[#009fbd] rounded-2xl grow-0 shrink-0 lg:basis-2/5 sm:2/5 md:3/5">
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
            upcomingAppointment.innerHTML+=`
                    <div class="relative h-90 m-5 pb-5 bg-white shadow-lg border-t-4 border-[#e9c46a] rounded-2xl grow-0 shrink-0 lg:basis-2/5 sm:2/5 md:3/5">
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
                        <div class="relative h-90 m-5 pb-5 bg-white shadow-lg border-t-4 border-[#36ba98] rounded-2xl grow-0 shrink-0 lg:basis-2/5 sm:2/5 md:3/5">
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
    });
    if(todayAppointment.children.length === 0){
        todayAppointment.innerHTML=`
        <div id="displayInformation" class="mt-5 p-5 mx-auto rounded-lg shadow-lg" style="width: 50%;">
            <h1 class="text-red-400 font-semibold text-xl text-center"><i class='bx bxs-bell-ring'></i> &nbsp;No Appointments Are available today! &nbsp;<i class='bx bxs-bell-ring'></i></h1>
        </div>`;
    }
    if(upcomingAppointment.childNodes.length === 0 ){
        upcomingAppointment.innerHTML=`
        <div id="displayInformation" class="mt-5 p-5 mx-auto rounded-lg shadow-lg" style="width: 50%;">
            <h1 class="text-red-400 font-semibold text-xl text-center"><i class='bx bxs-bell-ring'></i> &nbsp;No Appointments Are available today! &nbsp;<i class='bx bxs-bell-ring'></i></h1>
        </div>
        `;
    }
    if(completedAppointment.childNodes.length === 0){
        completedAppointment.innerHTML=`
            <div id="displayInformation" class="mt-5 p-5 mx-auto rounded-lg shadow-lg" style="width: 50%;">
            <h1 class="text-red-400 font-semibold text-xl text-center"><i class='bx bxs-bell-ring'></i> &nbsp;No Appointments Are available today! &nbsp;<i class='bx bxs-bell-ring'></i></h1>
        </div>
        `;
    }
}

var cancelAppointment = (appointmentId) =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Patient"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    fetch(`https://pavihosmanagebeapp.azurewebsites.net/api/Patient/CancelAppointment?appointmentId=${appointmentId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`
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
        openModal('alertModal', "Success", data);
    }).catch(error => {
        console.log(error.message)
        if (error.message === 'Unauthorized Access!') {
            openModal('alertModal', "Error", "Unauthorized Access!");
        } else {
            openModal('alertModal', "Error", error.message);
        }
    });
    window.location.reload()
}

var viewPrescription = (appointmentId) =>{
    window.location.href=`./prescription.html?search=${appointmentId}`;
}

var getAllAppointments = (skeletonStatus) =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Patient"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    const skip = (page - 1) * itemsperpage;
    var patientId = JSON.parse(localStorage.getItem('loggedInUser')).userId; 
    if(skeletonStatus){
        displayAppointmentsSkeleton();
    }
    fetch(`${url}?patientId=${patientId}&limit=${itemsperpage}&skip=${skip}`,
        {
            method:'GET',
            headers:{
                'Content-Type' : 'application/json', 
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`               
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
        if(skeletonStatus){
            displayAppointmentsSkeletonRemove();
        }
        if(data.length === 0){
            openModal('alertModal', "Information", "No more appointments available!");
        }
        displayAppointments(data)
    }).catch( error => {
        console.log(error.message)
        openModal('alertModal', "Error", error.message);
    });    
}

var loadMoreData = () =>{
    page++;
    getAllAppointments(false);
}

document.addEventListener("DOMContentLoaded",() => {
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Patient"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    updateForLogInAndOut();
    page=1;
    getAllAppointments(true);
})