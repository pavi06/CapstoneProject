const baseUrl ="https://pavihosmanagebeapp.azurewebsites.net/api";
var bookAppointmentBySpeciality = (bodyData) =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Patient"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    console.log(bodyData)
    fetch(baseUrl + '/Patient/BookAppointmentBySpeciality',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json', 
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`              
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
            throw new Error(`${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        openModal('alertModal', "Success", "Appointment Booked Successfully!");
        document.getElementById("bookAppointmentForm").reset();
        document.getElementById("infoModel").style.display='block';
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
    });
}


var redirectToAppointments = () =>{
    window.location.href="./MyAppointments.html";
}


document.addEventListener("DOMContentLoaded",()=>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Patient"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    updateForLogInAndOut()
    var bookBtn = document.getElementById("bookAppointment");
    bookBtn.addEventListener("click", ()=>{
        var patientId = JSON.parse(localStorage.getItem('loggedInUser')).userId;
        if(!(validatePhone('phone') && validateDate('date') && validate('preferredTime') && validate('specialist')
        && validate('preferredMode') && validate('preferredLanguage')&& validate('preferredType')&& validate('description') && patientId)){
            openModal('alertModal', "Error", "provide all details poperly to proceed!");
        }
        var bodyData = {
            patientId : patientId,
            phoneNo : document.getElementById("phone").value,
            appointmentDate : document.getElementById("date").value,
            preferredTime : document.getElementById("preferredTime").value,
            speciality : document.getElementById("specialist").value,
            description : document.getElementById("description").value,
            preferredLanguage : document.getElementById("preferredLanguage").value,
            appointmentType : document.getElementById("preferredType").value,
            appointmentMode : document.getElementById("preferredMode").value
        }
        bookAppointmentBySpeciality(bodyData);
    }) 
})