var url = "http://localhost:5253/api/DoctorBasic/GetAllDoctorsBySpecializationWithLimit";
var page = 1;
const itemsperpage = 10;

var displayDoctors = (data) =>{
    var doctorDiv = document.getElementById("doctors");
    data.forEach(doctor => {
        var languages = doctor.languagesKnown.join(', ')
        var availaleDays = doctor.availableDaysOfWeek.join(', ')
        doctorDiv.innerHTML+=`
            <div class="bg-white rounded-lg my-10 p-5 border-b-2 doctorCard">
                <div class="relative border-8 border-[#4A249D] rounded-full overflow-hidden doctorImage">
                    <img src="../../image.jpg" alt="doctorImage" class="absolute inset-0 w-full h-full object-cover">
                </div>
                <div class="ml-4 flex-1 p-5 border-t-3 border-b-3 border-l-3 border-blue-900 rounded-tl-lg rounded-bl-lg">
                    <ul> 
                        <li class="p-2">
                            <div class="grid grid-cols-2">
                                <p class="font-semibold">Name</p>
                                <p>Dr. ${doctor.doctorName}</p>
                            </div>
                        </li>
                        <li class="p-2">
                            <div class="grid grid-cols-2">
                                <p class="font-semibold">Specialization</p>
                                <p>${doctor.specialization}</p>
                            </div>
                         </li>
                        <li class="p-2">
                            <div class="grid grid-cols-2">
                                <p class="font-semibold">Experience</p>
                                <p>${doctor.experience} years</p>
                            </div>
                        </li>
                        <li class="p-2">
                            <div class="grid grid-cols-2">
                                <p class="font-semibold">Languages known</p>
                                <p>${languages}</p>
                            </div>
                         </li>
                         <li class="p-2">
                             <div class="grid grid-cols-2">
                                <p class="font-semibold">Available days</p>
                                <p>${availaleDays}</p>
                            </div>
                        </li>
                    </ul>
                </div>
                <div>
                    <button class="p-2 mt-5 border-2 border-[#4A249D] text-[#4A249D] uppercase text-center font-bold hover:bg-[#4A249D] hover:text-white cursor-pointer transition-colors mx-auto" style="width: fit-content;" onclick="bookAppointmentRedirect(${doctor.doctorId})">Book Appointment</button>
                </div>
            </div>
        `;
    });
}

var fetchDoctors = () =>{
    const skip =  (page - 1) * itemsperpage;
        fetch(`${url}?limit=${itemsperpage}&skip=${skip}`, {
            method: 'POST',
            headers: {'Content-Type':'application/json'},
            body:JSON.stringify(localStorage.getItem('Speciality')),
        }).then(async(res) => {
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
                if(error.message == "No Doctors are available!"){
                    document.getElementById('loadMoreDiv').classList.add("hide");
                }
            }
        });
    
}

var loadMoreData = () =>{
    page++;
    fetchDoctors();
}

document.addEventListener("DOMContentLoaded",()=>{
    updateForLogInAndOut();
    document.getElementById("specialityHeader").innerHTML=localStorage.getItem("Speciality");
    page=1;
    document.getElementById("doctors").innerHTML="";
    fetchDoctors();
})