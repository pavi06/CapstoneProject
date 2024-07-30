const baseUrl ="http://localhost:5253/api";
var bookAppointmentBySpeciality = (bodyData) =>{
    console.log(bodyData)
    fetch(baseUrl + '/Patient/BookAppointmentBySpeciality',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json',                
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
            throw new Error(`${errorResponse.errorCode} Error! - ${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        console.log("booked successfully")
        console.log(data)
        document.getElementById("bookAppointmentForm").reset();
    }).catch( error => {
        console.log(error)
        document.getElementById("bookAppointmentForm").reset();
        // if(error.message === "Unauthorized Access!"){
        //     logOut()
        // }
    });
}



document.addEventListener("DOMContentLoaded",()=>{
    var bookBtn = document.getElementById("bookAppointment");
    bookBtn.addEventListener("click", ()=>{
        // var name = document.getElementById("name").value;
        var phone = document.getElementById("phone").value;
        var date = document.getElementById("date").value;
        var time = document.getElementById("preferredTime").value;
        var speciality = document.getElementById("specialist").value;
        var mode = document.getElementById("preferredMode").value;
        var language = document.getElementById("preferredLanguage").value;
        var type = document.getElementById("preferredType").value;
        var reason = document.getElementById("description").value;
        var bodyData = {
            patientId : 15,
            phoneNo : phone,
            appointmentDate : date,
            preferredTime : time,
            speciality : speciality,
            description : reason,
            preferredLanguage : language,
            appointmentType : type,
            appointmentMode : mode
        }
        bookAppointmentBySpeciality(bodyData);
    }) 
})