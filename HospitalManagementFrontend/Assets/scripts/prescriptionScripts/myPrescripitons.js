var url="http://localhost:5253/api/Patient/MyPrescriptions";
var page = 1;
const itemsperpage = 10;

var viewPrescription = (appointmentId) => {
    window.location.href=`./prescription.html?search=${appointmentId}`;
}

var fetchPrescriptions = () =>{
    var patientId = JSON.parse(localStorage.getItem('loggedInUser')).userId;
    const skip = (page - 1) * itemsperpage;
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
        displayPrescriptions(data)
    }).catch( error => {
        console.log(error)
    }); 
}

var loadMoreData = () =>{
    page++;
    fetchPrescriptions();
}

var displayPrescriptions = (data) =>{
    var prescriptionDiv = document.getElementById("prescriptions");
    data.forEach(prescription => {
        console.log(prescription)
        prescriptionDiv.innerHTML+=`
        <div class="mx-auto mt-5" style="width: 80%;">
                <div class="grid grid-cols-5 hover:bg-[#f6f5f5] text-wrap text-center mx-5 p-3 bg-white border-t-4 border-[#009fbd] rounded-xl shadow-lg overflow-hidden items-center justify-start">
                    <div class="flex flex-row flex-wrap text-center">
                        <p class="text-wrap font-semibold">PrescriptionId : </p>
                        <p>&nbsp;${prescription.prescriptionId}</p>
                    </div>
                    <div class="flex flex-wrap text-center col-span-2">
                        <p class="text-wrap font-semibold">PrescribedDate : </p>
                        <p>&nbsp;${prescription.prescribedDate.split('T')[0]}</p>
                    </div>
                    <div class="flex flex-wrap text-center">
                        <p class="text-wrap font-semibold">AppointmentId : </p>
                        <p>&nbsp;${prescription.prescriptionFor}</p>
                    </div>
                    <div class="text-center">
                        <button type="button" class="px-3 py-2 bg-[#009fbd] text-white font-semibold rounded-lg hover:bg-[#f6f5f5] border-2 border-[#009fbd] hover:text-[#009fbd]" onclick="viewPrescription(${prescription.prescriptionFor})">View</button>
                    </div>
                </div>
        </div> 
    `;
    });
}

document.addEventListener("DOMContentLoaded", () => {
    updateForLogInAndOut();
    page=1;
    document.getElementById("prescriptions").innerHTML="";
    fetchPrescriptions();
})