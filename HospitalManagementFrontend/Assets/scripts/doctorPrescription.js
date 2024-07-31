var medicineMapper = {};

function addNewRowForTable() {
    var table = document.getElementById("prescriptionTable").getElementsByTagName('tbody')[0];
    var templateRow = document.getElementById("templateRow");
    var newRow = templateRow.cloneNode(true);
    newRow.style.display = "";
    let inputs = newRow.querySelectorAll('input');
    inputs.forEach(input => {
        input.value = "";
    });
    table.appendChild(newRow);
}

function deleteRow(rowButton) {
    var table = document.getElementById("prescriptionTable");
    var row = rowButton.parentNode.parentNode;
    if (row.rowIndex == 1) {
        let inputs = row.querySelectorAll('input');
        inputs.forEach(input => {
            input.value = "";
        });
        return;
    }
    table.deleteRow(row.rowIndex);
}

var providePrescriptionAPIForPatient = (medications) => {
    var bodyData = {
        doctorId: 1,
        patientId: localStorage.getItem('patientId'),
        patientType: "OutPatient",
        prescriptionFor: localStorage.getItem('appointmentId'),
        prescribedMedicine: medications
    }
    console.log(bodyData)
    fetch('http://localhost:5253/api/Doctor/UploadPrescription',
        {
            method:'POST',
            headers: {
                'Content-Type': 'application/json',
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
            console.log(data)
            alert(data)
        }).catch(error => {
            console.log(error)
            alert(error.message)
        });
        window.location.reload();
}

var addPrescription = () => {
    var medications = [];
    var tableRows = document.getElementsByClassName("tableRows");
    Array.from(tableRows).forEach(row => {
        var cells = row.getElementsByTagName('td');
        var values = {};
        Array.from(cells).forEach(cell => {
            var input = cell.querySelector('input');
            if (input) {
                values[input.id] = input.value;
            }
        });
        medications.push(values);
        values = {};
    });
    console.log(medications);
    providePrescriptionAPIForPatient(medications);
}

var displayDataList = (data) =>{
    var medList = document.getElementById("medicineList");
    data.forEach(med =>{
        medicineMapper[med.medicineName] = med.medicineId;
        medList.innerHTML+=`
            <option value="${med.medicineName}" id="${med.medicineId}">
        `;
    })
}

var fetchMedicineNames = () =>{
    fetch('http://localhost:5253/api/Medicine/GetAllMedicineNames',
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
            displayDataList(data);
        }).catch(error => {
            console.log(error)
            alert(error.message)
        });

}


var displayFormList = (data) =>{
    var formDatalist = document.getElementById("formList");
    data.formsAvailable.forEach(form =>{
        formDatalist.innerHTML+=`
            <option value="${form}">
        `;
    })

    var dosageDatalist = document.getElementById("dosageList");
    data.dosagesAvailable.forEach(dosage =>{
        dosageDatalist.innerHTML+=`
            <option value="${dosage}">
        `;
    })
}


var getDetails = (e) =>{
    var value = e.value.trim();
    fetch(`http://localhost:5253/api/Medicine/GetDetailsOfMedicine?id=${medicineMapper[value]}`,
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
            displayFormList(data);
        }).catch(error => {
            console.log(error)
            alert(error.message)
        });

}

document.addEventListener("DOMContentLoaded", () => {
    var addBtn = document.getElementById("addPrescriptionBtn");
    addBtn.addEventListener("click", () => {
        addPrescription();
    })
    fetchMedicineNames();
})