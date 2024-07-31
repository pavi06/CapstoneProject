var displayBill = (data) => {
    document.getElementById("receiptId").innerHTML=data.billId;
    document.getElementById("patientName").innerHTML=data.patientName;
    document.getElementById("patientAge").innerHTML=data.age;
    document.getElementById("contactNumber").innerHTML = data.contactNo;
    document.getElementById("date").innerHTML = data.date;
    document.getElementById("admittedDate").innerHTML = data.admittedDate;
    document.getElementById("noOfDays").innerHTML = data.noOfDaysIn;
    displaytableData(data.roomDetails);   
    document.getElementById("totalAmount").innerHTML = data.totalAmount;
    document.getElementById('doctorFee').innerHTML = data.doctorFee;
    document.getElementById("tax").innerHTML = "2%";
    document.getElementById("grandTotal").innerHTML = data.totalAmount+data.doctorFee + (0.02*(data.totalAmount+data.doctorFee));
}

var displaytableData = (data) =>{
    var roomDiv = document.getElementById("tableBody");
    data.forEach(room => {
        roomDiv.innerHTML+=`
            <tr class="whitespace-nowrap text-sm">
                <td class="px-6 py-4 font-medium">${room.roomType}</td>
                <td class="px-6 py-4">${room.noOfDays}</td>
                <td class="px-6 py-4">${room.costPerDay}</td>
                <td class="px-6 py-4">${room.noOfDays * room.costPerDay}</td>                       
            </tr>
        `;
    });
}

var generateBill = () => {
    var patientId = document.getElementById("patientId").value;
    fetch(`http://localhost:5253/api/Receptionist/BillForInPatient?inPatientid=${patientId}`,
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