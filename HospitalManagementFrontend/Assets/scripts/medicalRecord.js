var getPatientId = () =>{
    var name = document.getElementById("patientName").value;
    var contactNo = document.getElementById("contactNo").value;
    fetch('http://localhost:5253/api/DoctorBasic/GetPatientId',
        {
            method:'POST',
            headers:{
                // 'Authorization': `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNSIsIkNvbnRhY3RObyI6Iis5MTkwMDM3NDE3MjEiLCJSb2xlIjoiVXNlciIsImV4cCI6MTcyMjI4MTY0Nn0.HLTj171QP06zc2F7LK4bZpms2xvYyEMSkZsbJoSEoqE`,
                'Content-Type' : 'application/json',                
            },
            body:JSON.stringify({
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
        id.innerHTML=data;
        alert(data)
    }).catch( error => {
        console.log(error)
        alert(error.message)
    });
}

var addRecord = () =>{
    var diagnosis = document.getElementById("diagnosis").value;
    var treatment = document.getElementById("treatment").value;
    var treatmentStatus = document.getElementById("treatStatus").value;
    fetch('http://localhost:5253/api/Doctor/CreateMedicalRecord',
        {
            method:'POST',
            headers:{
                // 'Authorization': `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNSIsIkNvbnRhY3RObyI6Iis5MTkwMDM3NDE3MjEiLCJSb2xlIjoiVXNlciIsImV4cCI6MTcyMjI4MTY0Nn0.HLTj171QP06zc2F7LK4bZpms2xvYyEMSkZsbJoSEoqE`,
                'Content-Type' : 'application/json',                
            },
            body:JSON.stringify({
                //check if not available in localstorage
                appointmentId: localStorage.getItem('appointmentId'),
                patientId: localStorage.getItem('patientId'),
                patientType: "OutPatient",
                doctorId: 1,
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
    }).catch( error => {
        console.log(error)
        alert(error.message)
    });
}

var GetRecords = () => {
    var doctorId = 1;
    var patientId = document.getElementById("patientId").value;
    fetch(`http://localhost:5253/api/Doctor/GetMedicalRecord?doctorId=${doctorId}&patientId=${patientId}`,
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
            throw new Error(`${errorResponse.message}`);
        }
        return await res.json();
    })
    .then(data => {
        console.log(data)
        displayRecords(data);
    }).catch( error => {
        console.log(error)
        alert(error.message)
    });
}

var displayRecords = (data) =>{
    var recordDiv = document.getElementById("medicalRecord");
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
        recordDiv.innerHTML+=`
            <div class="bg-white rounded-lg my-10 p-5 border-2 mx-auto" style="width: 80%;">
                <div class="p-2 flex flex-row flex-wrap justify-between recordDiv">
                    <div id="details" class="px-2 mx-auto detailsDiv">
                        <div class="grid grid-cols-2">
                            <h4 class="font-bold text-lg">RecordId</h4>
                            <h4> ${record.recordId} </h4>
                        </div>
                        <div class="grid grid-cols-2 mt-5">
                            <p>PatientName</p>
                            <p> ${record.patientName} </p>
                        </div>
                        <div class="grid grid-cols-2">
                            <p>ContactNumber</p>
                            <p> ${record.contactNo} </p>
                        </div>
                        <ul class="mt-2 text-wrap">
                            <div class="grid grid-cols-2">
                                <li>Recorded Date</li>
                                <li>${record.date}</li>
                            </div>
                            <div class="grid grid-cols-2">
                                <li>Diagnosis</li>
                                <li>${record.diagnosis}</li>
                            </div>
                            <div class="grid grid-cols-2">
                                <li>Treatment</li>
                                <li>${record.treatment}</li>
                            </div>
                            <div class="grid grid-cols-2">
                                <li>Status</li>
                                <li>${record.treatmentStatus}</li>
                            </div>
                        </ul>
                    </div>
                    <div id="medications" class="overflow-y-auto h-64 mx-auto tableDiv">
                        <table class="min-w-full divide-y" id="prescriptionTable">
                            <thead class="bg-indigo-100">
                                <tr class="text-center text-black">
                                    <th scope="col" class="p-3 text-sm font-medium">Medicine</th>
                                    <th scope="col" class="p-3 text-sm font-medium">Form</th>
                                    <th scope="col" class="p-3 text-sm font-medium">Dosage</th>
                                    <th scope="col" class="p-3 text-sm font-medium">Intake</th>
                                    <th scope="col" class="p-3 text-sm font-medium">IntakeTiming</th>
                                </tr>
                            </thead>
                            <tbody class="divide-y divide-gray-200">${tableRecords}                             
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        `;
    });
}

document.addEventListener("DOMContentLoaded",()=>{
    if(document.getElementById("createRecordBtn")){
        var btn = document.getElementById("createRecordBtn");
        btn.addEventListener("click",()=>{
            addRecord();
        })
    }    
})




