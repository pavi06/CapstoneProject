var getAllAppointments = () =>{
    // http://localhost:5253/api/Patient/MyAppointments?patientId=15&limit=5&skip=0
    fetch(baseUrl + '',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json',                
            },
            body:JSON.stringify({
                doctorId: doctorId,
                date: date
            })
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
        displaySlots(data)
    }).catch( error => {
        console.log(error)
        document.getElementById("bookAppointmentForm").reset();
        // if(error.message === "Unauthorized Access!"){
        //     logOut()
        // }
    });
    var todayAppointments = document.getElementById("slider");
    
}


document.addEventListener("DOMContentLoaded", () => {
    // var slider = document.getElementById("slider");
    // var slider1 = document.getElementById("slider1");
    // var slider2 = document.getElementById("slider2");
    // var nextBtn = document.getElementById("nxtBtn");
    // var prevBtn = document.getElementById("prevBtn");
    // var nextBtn1 = document.getElementById("nxtBtn1");
    // var prevBtn1 = document.getElementById("prevBtn1");
    // var nextBtn2 = document.getElementById("nxtBtn2");
    // var prevBtn2 = document.getElementById("prevBtn2");
    // nextBtn.addEventListener("click", () => {
    //     slider.style.scrollBehavior = "smooth";
    //     slider.scrollLeft += 350;
    // });
    // prevBtn.addEventListener("click", () => {
    //     slider.style.scrollBehavior = "smooth";
    //     slider.scrollLeft -= 350;
    // });
    // nextBtn1.addEventListener("click", () => {
    //     slider1.style.scrollBehavior = "smooth";
    //     slider1.scrollLeft += 350;
    // });
    // prevBtn1.addEventListener("click", () => {
    //     slider1.style.scrollBehavior = "smooth";
    //     slider1.scrollLeft -= 350;
    // });
    // nextBtn2.addEventListener("click", () => {
    //     slider2.style.scrollBehavior = "smooth";
    //     slider2.scrollLeft += 350;
    // });
    // prevBtn2.addEventListener("click", () => {
    //     slider2.style.scrollBehavior = "smooth";
    //     slider2.scrollLeft -= 350;
    // });

    getAllAppointments();
})