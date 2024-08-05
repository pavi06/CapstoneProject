var loadPrescription = async (appointmentId) =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Patient"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    var patientId=JSON.parse(localStorage.getItem('loggedInUser')).userId;
    await checkForRefresh()
    displayDetailsSKeleton ();
    displayTableRecordsSkeleton();
    fetch(`https://pavihosmanagebeapp.azurewebsites.net/api/Patient/MyPrescriptionForAppointment?patientId=${patientId}&appointmentId=${appointmentId}`,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
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
                throw new Error(`${errorResponse.message}`);
            }
            return await res.json();
        })
        .then(data => {
            displayDetailsSKeletonRemove()
            displayTableRecordsSkeletonRemove();
            displayDetails(data);
        }).catch(error => {
            openModal('alertModal', "Error", error.message);
            if(error.message === "Prescription Not available!"){
                setTimeout(
                    window.location.href="./MyAppointments.html", 50000
                )
                
            }
        });
}

var displayTableRecords = (data) =>{
    var table = document.getElementById("medicineTableBody");
    var tableRecords = "";
        data.prescription.forEach(medicine => {
            tableRecords += `
            <tr>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-800">${medicine.medicineName}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-800">${medicine.form}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-800">${medicine.dosage}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-800">${medicine.intake}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-800">${medicine.intakeTiming}</td>
            </tr>
        `;
    });
    table.innerHTML=tableRecords;
}

var displayTableRecordsSkeleton = () =>{
    document.getElementById("medicineTableBody").innerHTML=`    
        <tr class="bg-white border-b">
    <td class="px-6 py-4 whitespace-nowrap text-sm">
        <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4 whitespace-nowrap text-sm">
        <div class="w-24 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4 whitespace-nowrap text-sm">
        <div class="w-24 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4 whitespace-nowrap text-sm">
        <div class="w-24 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4 whitespace-nowrap text-sm">
        <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
</tr>`;
}

var displayTableRecordsSkeletonRemove = () =>{
    document.getElementById("medicineTableBody").innerHTML="";
}

var displayDetailsSKeleton = () =>{
    document.getElementById("DoctorInfo").innerHTML = `
<div id="DoctorInfo" class="p-4">
    <div class="flex items-center space-x-4">
        <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
        <div class="w-40 h-6 bg-gray-300 animate-pulse rounded"></div>
    </div>
</div>
<div id="PatientInfo" class="p-4">
    <div class="grid grid-cols-2 gap-4">
        <div class="flex items-center space-x-4">
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
        </div>
        <div class="flex items-center space-x-4">
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
        </div>
    </div>
</div>
<div id="PrescriptionInfo" class="p-4">
    <div class="grid grid-cols-2 gap-4">
        <div class="flex items-center space-x-4">
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
        </div>
        <div class="flex items-center space-x-4">
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
        </div>
        <div class="flex items-center space-x-4">
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
            <div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>
        </div>
    </div>
</div>

    `;
}

var displayDetailsSKeletonRemove = () =>{
    document.getElementById("DoctorInfo").innerHTML ="";
}

var displayDetails = (data) =>{
    document.getElementById("DoctorInfo").innerHTML = `
        <p>${data.doctorName}&nbsp;&nbsp;</p> - <p>&nbsp;&nbsp;${data.docSpecialization}</p>
    `;
    document.getElementById("PatientInfo").innerHTML = `
        <div class="grid grid-cols-2">
            <p class="font-semibold">Patient Name:&nbsp;</p>
            <p>${data.patientName}</p>
        </div>
        <div class="grid grid-cols-2">
            <p class="font-semibold">Patient Age:&nbsp;</p>
            <p>${data.age}</p>
        </div>
    `;
    document.getElementById("PrescriptionInfo").innerHTML = `
        <div class="grid grid-cols-2">
            <p class="font-semibold">Id:&nbsp;</p>
            <p>${data.prescriptionId}</p>
        </div>
        <div class="grid grid-cols-2">
            <p class="font-semibold">Prescribed Date:&nbsp;</p>
            <p>${data.date.split('T')[0]}</p>
        </div>
        <div class="grid grid-cols-2">
            <p class="font-semibold">AppointmentId:&nbsp;</p>
            <p>${data.prescriptionFor}</p>
        </div>
    `;
    displayTableRecords(data);   
}

document.addEventListener("DOMContentLoaded", () =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Patient"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    updateForLogInAndOut();
    const urlParams = new URLSearchParams(window.location.search); 
    const appointmentId = urlParams.get('search'); 
    loadPrescription(appointmentId);
})