var UpdateTab = (e) =>{
    console.log(e)
    e.setAttribute("aria-selected", true)
    e.classList.add("active")
    document.querySelectorAll('[role="tab"]').forEach(tab => {
        if (tab !== e) {
            tab.classList.remove("active");
            tab.setAttribute("aria-selected", "false");
        }
    });
}

var getPatientId = () =>{
    var name = document.getElementById("patientName").value;
    var contactNo = document.getElementById("contactNo").value;
    fetch('http://localhost:5253/api/DoctorBasic/GetPatientId',
        {
            method:'POST',
            headers:{
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


var createPatient = () =>{
    var id = document.getElementById("idDisplay").textContent;
    var name = document.getElementById("name").value;
    var dob = document.getElementById("dob").value;
    var gender = document.getElementById("gender").value;
    var contactNo = document.getElementById("contactNo").value;
    var address = document.getElementById("address").value;
    var wardType = document.getElementById("wardType").value;
    var noOfDays = document.getElementById("noOfDays").value;
    var description = document.getElementById("description").value;
    var bodyData = {
        patientId: id,
        name: name,
        dateOfBirth: dob,
        gender: gender,
        contactNo: contactNo,
        address: address,
        wardType: wardType,
        noOfDays: noOfDays,
        description: description
    }
    fetch('http://localhost:5253/api/Receptionist/AdmissionForPatient',
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
            throw new Error(`${errorResponse.message}`);
        }
        return await res.text();
    })
    .then(data => {
        alert(data)
    }).catch( error => {
        console.log(error)
        alert(error.message)
    });
}


var updatePatient = () =>{
    var admissionId = document.getElementById("admissionIdDisplay").textContent;
    var wardType = document.getElementById("wardType").value;
    var noOfDays = document.getElementById("noOfDays").value;
    var bodyData = {
        admissionId:admissionId,
        wardType:wardType,
        noOfDays:noOfDays
    }
    fetch('http://localhost:5253/api/Receptionist/UpdateInPatient',
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
            throw new Error(`${errorResponse.message}`);
        }
        return await res.text();
    })
    .then(data => {
        alert(data)
    }).catch( error => {
        console.log(error)
        alert(error.message)
    });
}

var getAdmissionId = () => {
    var name = document.getElementById("name").value;
    var contactNo = document.getElementById("contact").value;
    fetch('http://localhost:5253/api/DoctorBasic/GetAdmissionId',
        {
            method:'POST',
            headers:{
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
        var id = document.getElementById("admissionIdDisplay");
        id.innerHTML=data;
        alert(data)
    }).catch( error => {
        console.log(error)
        alert(error.message)
    });
}


document.addEventListener("DOMContentLoaded", () =>{
    var createbtn = document.getElementById("createPatient");
    createbtn.addEventListener("click", () =>{
        createPatient();
    })

    var updatebtn = document.getElementById("updatePatient");
    updatebtn.addEventListener("click", () =>{
        updatePatient();
    })
})
