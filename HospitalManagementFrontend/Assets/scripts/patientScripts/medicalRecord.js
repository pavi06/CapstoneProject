var getPatientId = async () => {
    console.log(!(validateName('patientName') && validatePhone('contactNo')))
    if (!(validateName('patientName') && validatePhone('contactNo'))) {
        alert("Please provide all the necessary details to proceed");
        return;
    }
    var name = document.getElementById("patientName").value;
    var contactNo = document.getElementById("contactNo").value;
    await checkForRefresh()
    fetch('http://localhost:5253/api/DoctorBasic/GetPatientId',
        {
            method: 'POST',
            headers: {
                // 'Authorization': `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNSIsIkNvbnRhY3RObyI6Iis5MTkwMDM3NDE3MjEiLCJSb2xlIjoiVXNlciIsImV4cCI6MTcyMjI4MTY0Nn0.HLTj171QP06zc2F7LK4bZpms2xvYyEMSkZsbJoSEoqE`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                patientName: name,
                contactNumber: contactNo
            })
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
            console.log(data)
            var id = document.getElementById("idDisplay");
            id.innerHTML = data;
            alert(data)
        }).catch(error => {
            console.log(error)
            alert(error.message)
        });
}

var addRecord = async () => {
    if (!validate('diagnosis') && validate('treatment') && validate('treatStatus')) {
        alert("provide all details properly!")
        return;
    }
    var diagnosis = document.getElementById("diagnosis").value;
    var treatment = document.getElementById("treatment").value;
    var treatmentStatus = document.getElementById("treatStatus").value;
    await checkForRefresh()
    fetch('http://localhost:5253/api/Doctor/CreateMedicalRecord',
        {
            method: 'POST',
            headers: {
                // 'Authorization': `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNSIsIkNvbnRhY3RObyI6Iis5MTkwMDM3NDE3MjEiLCJSb2xlIjoiVXNlciIsImV4cCI6MTcyMjI4MTY0Nn0.HLTj171QP06zc2F7LK4bZpms2xvYyEMSkZsbJoSEoqE`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                appointmentId: localStorage.getItem('appointmentId'),
                patientId: localStorage.getItem('patientId'),
                patientType: "OutPatient",
                doctorId: JSON.parse(localStorage.getItem('loggedInUser')).userId,
                diagnosis: diagnosis,
                treatment: treatment,
                treatmentStatus: treatmentStatus
            })
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
            return await res.text();
        })
        .then(data => {
            console.log(data)
            alert(data)
            window.location.href="./MedicalRecords.html";
        }).catch(error => {
            if(error.message==="Prescription Not available!"){
                alert(error.message+"Add prescription and then add the medical record!");
            }
            console.log(error)
            alert(error.message)
        document.getElementById("addMedicalRecordForm").reset();
    });
}

var GetRecords = async () => {
    var doctorId = JSON.parse(localStorage.getItem('loggedInUser')).userId;
    if(!validateNumber('patientId')){
        alert("Provide a valid id");
        return;
    }
    var patientId = document.getElementById("patientId").value;
    await checkForRefresh()
    fetch(`http://localhost:5253/api/Doctor/GetMedicalRecord?doctorId=${doctorId}&patientId=${patientId}`,
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
            console.log(data)
            document.getElementById("medicalRecord").innerHTML="";
            displayRecords(data);
        }).catch(error => {
            console.log(error)
            alert(error.message)
        });
}

var displayRecords = (data) => {
    var recordDiv = document.getElementById("medicalRecord");
    if(data.length === 0){
        recordDiv.innerHTML=`
            <div id="displayInformation" class="mt-10 p-5 mx-auto rounded-lg border-t-4 border-red-200 shadow-lg" style="width: 50%;">
                <h1 class="text-red-400 font-semibold text-xl text-center">&nbsp;No Records Are available! &nbsp;</h1>
            </div>
        `;
        return;
    }
    data.forEach(record => {
        var tableRecords = "";
        console.log(record.medications)
        record.medications.forEach(medicine => {
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
        recordDiv.innerHTML += `
            <div class="bg-white rounded-3xl border-t-4 border-[#009fbd] shadow-lg my-10 p-5  mx-auto" style="width: 80%;">
                <div class="flex font-bold text-xl text-[#009fbd] ml-5 mb-3">
                    <h4>RecordId&nbsp;&nbsp;</h4>
                    <h4> ${record.recordId} </h4>
                </div>
                    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 mx-5 sm:grid-cols-1">
                        <div class="grid grid-cols-2">
                            <p class="font-semibold">PatientName</p>
                            <p> ${record.patientName} </p>
                        </div>
                        <div class="grid grid-cols-2">
                            <p class="font-semibold">ContactNumber</p>
                            <p> ${record.contactNo} </p>
                        </div>
                        <div class="grid grid-cols-2">
                                <p class="font-semibold">Recorded Date</p>
                                <p>${record.date.split('T')[0]}</p>
                        </div>
                        <div class="grid grid-cols-2">
                                <p class="font-semibold">Diagnosis</p>
                                <p>${record.diagnosis}</p>
                        </div>
                        <div class="grid grid-cols-2">
                                <p class="font-semibold">Treatment</p>
                                <p>${record.treatment}</p>
                        </div>
                        <div class="grid grid-cols-2">
                                <p class="font-semibold">Status</p>
                                <p>${record.treatmentStatus}</p>
                        </div>
                    </div>
                    <div id="medications" class="overflow-y-auto h-50 mt-5 mx-auto">
                        <table class="min-w-full divide-y" id="prescriptionTable">
                            <thead class="bg-[#009fbd]">
                                <tr class="text-center text-black">
                                    <th scope="col" class="p-3 uppercase font-semibold text-[#f6f5f5] text-sm font-medium">Medicine</th>
                                    <th scope="col" class="p-3 uppercase font-semibold text-[#f6f5f5] text-sm font-medium">Form</th>
                                    <th scope="col" class="p-3 uppercase font-semibold text-[#f6f5f5] text-sm font-medium">Dosage</th>
                                    <th scope="col" class="p-3 uppercase font-semibold text-[#f6f5f5] text-sm font-medium">Intake</th>
                                    <th scope="col" class="p-3 uppercase font-semibold text-[#f6f5f5] text-sm font-medium">IntakeTiming</th>
                                </tr>
                            </thead>
                            <tbody class="divide-y divide-gray-200">${tableRecords}                             
                            </tbody>
                        </table>
                    </div>
            </div>
        `;
    });
}

document.addEventListener("DOMContentLoaded", () => {
    if (document.getElementById("createRecordBtn")) {
        var btn = document.getElementById("createRecordBtn");
        btn.addEventListener("click", () => {
            addRecord();
        })
    }
})




