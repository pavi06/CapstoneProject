var displayBill = (data) =>{
    document.getElementById("billId").innerHTML=data.billId;
    document.getElementById("patientName").innerHTML=data.patientName;
    document.getElementById("age").innerHTML=data.age;
    document.getElementById("contactNumber").innerHTML = data.contactNo;
    document.getElementById("doctorName").innerHTML = data.doctorName;
    document.getElementById("doctorSpecialization").innerHTML = data.doctorSpecialization;
    document.getElementById("Amount").innerHTML=data.doctorFee;
    document.getElementById("totalAmount").innerHTML = data.totalAmount;
    document.getElementById("tax").innerHTML = "2%";
    document.getElementById("grandTotal").innerHTML = data.totalAmount + (0.02*data.totalAmount);
}

var generateBill = () =>{
    var appointmentId = document.getElementById("appointmentId").value;
    fetch(`http://localhost:5253/api/Receptionist/BillForOutPatient?appointmentId=${appointmentId}`,
        {
            method:'POST',
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
        displayBill(data)
    }).catch( error => {
        console.log(error)
        alert(error.message)
    });
}