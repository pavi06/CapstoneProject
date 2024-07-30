var url = "http://localhost:5253/api/DoctorBasic/GetAllDoctorsBySpecializationWithLimit";
var page = 1;
const itemsperpage = 10;

var bookAppointmentRedirect = (doctorId) =>{
    localStorage.setItem('currentDoctorId', doctorId);
    window.location.href="./Appointment.html";
}

var displayDoctors = (data) =>{
    var doctorDiv = document.getElementById("doctors");
    data.forEach(doctor => {
        var languages = doctor.languagesKnown.join(', ')
        var availaleDays = doctor.availableDaysOfWeek.join(', ')
        doctorDiv.innerHTML+=`
            <div class="bg-white rounded-lg my-10 p-5 bg-blue-500 border-b-2 doctorCard">
                <div class="relative border-8 border-blue-900 rounded-full overflow-hidden doctorImage">
                    <img src="../image.jpg" alt="doctorImage" class="absolute inset-0 w-full h-full object-cover">
                </div>
                <div class="ml-4 flex-1 p-5 border-t-3 border-b-3 border-l-3 border-blue-900 rounded-tl-lg rounded-bl-lg">
                    <ul>
                        <li class="p-2">Dr. ${doctor.doctorName}</li>
                        <li class="p-2">${doctor.specialization}</li>
                        <li class="p-2">${doctor.experience} years Experience</li>
                        <li class="p-2">Languages known : ${languages}</li>
                         <li class="p-2">Available days : ${availaleDays}</li>
                        <li class="p-3 mt-5 border-2 border-blue-900 text-blue-900 uppercase text-center font-bold hover:bg-blue-900 hover:text-white cursor-pointer transition-colors" style="width: fit-content;" onclick="bookAppointmentRedirect(${doctor.doctorId})">Book Appointment</li>
                    </ul>
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
    document.getElementById("specialityHeader").innerHTML=localStorage.getItem("Speciality");
    page=1;
    document.getElementById("doctors").innerHTML="";
    fetchDoctors();
})