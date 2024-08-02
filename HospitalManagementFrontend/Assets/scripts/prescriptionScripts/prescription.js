var loadPrescription = (appointmentId) =>{
    var patientId=JSON.parse(localStorage.getItem('loggedInUser')).userId;
    fetch(`http://localhost:5253/api/Patient/MyPrescriptionForAppointment?patientId=${patientId}&appointmentId=${appointmentId}`,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
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
            displayDetails(data);
        }).catch(error => {
            console.log(error)
            alert(error.message)
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
    updateForLogInAndOut();
    const urlParams = new URLSearchParams(window.location.search); 
    const appointmentId = urlParams.get('search'); 
    loadPrescription(appointmentId);
})