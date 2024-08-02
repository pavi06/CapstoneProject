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
        alert("provide the data properly to add new!");
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

var providePrescriptionAPIForPatient = (medications) => {
    var bodyData = {
        doctorId: JSON.parse(localStorage.getItem('loggedInUser')).userId,
        patientId: localStorage.getItem('patientId'),
        patientType: "OutPatient",
        prescriptionFor: localStorage.getItem('appointmentId'),
        prescribedMedicine: medications
    }
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
            console.log("prescription added successfully")
            alert(data)
        }).catch(error => {
            console.log(error)
            alert(error.message)
        });
        setTimeout(() => {
            removeAllValuesAdded();
        }, 300);
}

var addPrescription = () => {
    if(!checkAllValuesAreFilledForBeforeRow()){
        alert("provide the data properly to add new!");
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
    // setTimeout(() => {
    //     window.location.href="./doctorHome.html";
    // }, 500);
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