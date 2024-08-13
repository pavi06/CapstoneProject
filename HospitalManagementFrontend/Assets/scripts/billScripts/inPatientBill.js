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
    for (const key in data) {
        if (data.hasOwnProperty(key)) {
            const roomRateDTO = data[key];
            roomDiv.innerHTML+=`
            <tr class="whitespace-nowrap text-sm">
                <td class="px-6 py-4 font-medium">${key}</td>
                <td class="px-6 py-4">${roomRateDTO.noOfDays}</td>
                <td class="px-6 py-4">${roomRateDTO.costPerDay}</td>
                <td class="px-6 py-4">${roomRateDTO.noOfDays * roomRateDTO.costPerDay}</td>                       
            </tr>
        `;
        }
    }
}

var displayDataSkeleton = () =>{
    document.getElementById("tableBody").innerHTML=`
          <tr class="whitespace-nowrap text-sm">
    <td class="px-6 py-4">
        <div class="w-20 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-24 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-20 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-28 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-24 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-24 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
</tr> 
    `;
}

var displayDataSkeletonRemove = () =>{
    document.getElementById("tableBody").innerHTML="";
}

var generateBill = async () => {
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error","Unauthorized Access!");
        return;
    }
    if(!validateNumber('patientId')){
        openModal('alertModal', "Error","Provide a valid id!");
        return;
    }
    var patientId = document.getElementById("patientId").value;
    displayDataSkeleton()
    await checkForRefresh()
    console.log(patientId)
    fetch(`http://localhost:5253/api/Receptionist/BillForInPatient?inPatientid=${patientId}`,
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json',  
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
        displayDataSkeletonRemove()
        displayBill(data)
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
    });
}