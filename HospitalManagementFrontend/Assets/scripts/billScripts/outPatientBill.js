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

var displayBillSkeleton = () =>{
    document.getElementById("billId").innerHTML=`<div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>`;
    document.getElementById("patientName").innerHTML=`<div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>`;
    document.getElementById("age").innerHTML=`<div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>`;
    document.getElementById("contactNumber").innerHTML = `<div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>`;
    document.getElementById("doctorName").innerHTML = `<div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>`;
    document.getElementById("doctorSpecialization").innerHTML = `<div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>`;
    document.getElementById("Amount").innerHTML=`<div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>`;
    document.getElementById("totalAmount").innerHTML = `<div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>`;
    document.getElementById("tax").innerHTML = `<div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>`;
    document.getElementById("grandTotal").innerHTML = `<div class="w-32 h-6 bg-gray-300 animate-pulse rounded"></div>`;
}

var  displayBillsSkeletonRemove = () =>{
    document.getElementById("billId").innerHTML="";
    document.getElementById("patientName").innerHTML="";
    document.getElementById("age").innerHTML="";
    document.getElementById("contactNumber").innerHTML = "";
    document.getElementById("doctorName").innerHTML = "";
    document.getElementById("doctorSpecialization").innerHTML = "";
    document.getElementById("Amount").innerHTML="";
    document.getElementById("totalAmount").innerHTML = "";
    document.getElementById("tax").innerHTML = "";
}

var generateBill = async () =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    if(!(validateNumber('appointmentId'))){
        openModal('alertModal', "Error", "Provide a valid id!");
        return;
    }
    var appointmentId = document.getElementById("appointmentId").value;
    await checkForRefresh()
    displayBillSkeleton();
    fetch(`https://pavihosmanagebeapp.azurewebsites.net/api/Receptionist/BillForOutPatient?appointmentId=${appointmentId}`,
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
        displayBillsSkeletonRemove();
        displayBill(data)
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
    });
}