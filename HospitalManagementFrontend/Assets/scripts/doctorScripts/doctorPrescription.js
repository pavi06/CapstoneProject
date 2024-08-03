var medicineMapper = {};

var checkAllValuesAreFilledForBeforeRow = () =>{
    var rowlength = document.getElementById("prescriptionTable").rows.length;
    var rowCells = document.getElementById("prescriptionTable").rows[rowlength - 1].cells;
    var count = 0;
    for (var i = 0; i < 5; i++) {     
        if (rowCells[i].querySelector('input').value.trim()) {
            count += 1;
        }
    }
    if(count === 5){
        return true;
    }else{
        return false;
    }
}

function addNewRowForTable() {
    var tableBody = document.getElementById("prescriptionTable").getElementsByTagName('tbody')[0];
    if(!checkAllValuesAreFilledForBeforeRow()){
        openModal('alertModal', "Error", "provide the data properly to add new!");
        return;
    };
    var templateRow = document.getElementById("templateRow");
    var newRow = templateRow.cloneNode(true);
    newRow.style.display = "";
    let inputs = newRow.querySelectorAll('input');
    inputs.forEach(input => {
        input.value = "";
    });
    tableBody.appendChild(newRow);
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

var removeAllValuesAdded = () =>{
    var tableRows = document.getElementsByClassName("tableRows");
    Array.from(tableRows).forEach(row => {
        if (row.rowIndex == 1) {
            let inputs = row.querySelectorAll('input');
            inputs.forEach(input => {
                input.value = "";
            });
        }
        else{
            row.parentNode.removeChild(row)
        }
    });
}

var providePrescriptionAPIForPatient =async (medications) => {
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Doctor"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    var bodyData = {
        doctorId: JSON.parse(localStorage.getItem('loggedInUser')).userId,
        patientId: localStorage.getItem('patientId'),
        patientType: "OutPatient",
        prescriptionFor: localStorage.getItem('appointmentId'),
        prescribedMedicine: medications
    }
    await checkForRefresh()
    fetch('http://localhost:5253/api/Doctor/UploadPrescription',
        {
            method:'POST',
            headers: {
                'Content-Type': 'application/json',
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
            openModal('alertModal', "Success", data);
        }).catch(error => {
            openModal('alertModal', "Error", error.message);
        });
        setTimeout(() => {
            removeAllValuesAdded();
        }, 300);
}

var addPrescription = () => {
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Doctor"){
        openModal('alertModal', "Error", "Unauthorized Access!");
        return;
    }
    if(!checkAllValuesAreFilledForBeforeRow()){
        openModal('alertModal', "Error", "provide the data properly to add new!");
        return;
    };
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

var fetchMedicineNames = async () =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Doctor"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    await checkForRefresh()
    fetch('http://localhost:5253/api/Medicine/GetAllMedicineNames',
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
            displayDataList(data);
        }).catch(error => {
            openModal('alertModal', "Error", error.message);
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


var getDetails = async (e) =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Doctor"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    var value = e.value.trim();
    await checkForRefresh()
    fetch(`http://localhost:5253/api/Medicine/GetDetailsOfMedicine?id=${medicineMapper[value]}`,
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
            displayFormList(data);
        }).catch(error => {
            openModal('alertModal', "Error", error.message);
        });

}

document.addEventListener("DOMContentLoaded", () => {
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Doctor"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    var addBtn = document.getElementById("addPrescriptionBtn");
    addBtn.addEventListener("click", () => {
        addPrescription();
    })
    fetchMedicineNames();
})