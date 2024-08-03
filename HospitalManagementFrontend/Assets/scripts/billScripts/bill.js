var displayBills = (data) =>{  
    var table = document.getElementById('pendingBillsTable').getElementsByTagName('tbody')[0];
    var fragment = document.createDocumentFragment();
    data.forEach(bill => {
        var row = document.createElement('tr');
        row.classList.add("whitespace-nowrap,text-sm")
        row.innerHTML = `
            <td class="px-6 py-4">${bill.patientId}</td>
            <td class="px-6 py-4">${bill.patientName}</td>
            <td class="px-6 py-4">${bill.contactNo}</td>
            <td class="px-6 py-4">${bill.billId}</td>
            <td class="px-6 py-4">${bill.billIssueDate.split('T')[0]}</td>
            <td class="px-6 py-4">${bill.totalAmount}</td>
            <td class="px-6 py-4">${bill.balanceAmount}</td>
            <td class="px-6 py-4">${bill.paymentStatus}</td>
        `;
        fragment.appendChild(row);
    });
    table.appendChild(fragment); 
}

var displayBillsSkeleton = () =>{
    document.getElementById('pendingBillsTable').getElementsByTagName('tbody').innerHTML=`
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

var displayBillsSkeletonRemove = () =>{
    document.getElementById('pendingBillsTable').getElementsByTagName('tbody').innerHTML="";
}


var loadPendingBills = async () =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error","Unauthorized Access!");
        return;
    }
    await checkForRefresh()
    displayBillsSkeleton();
    fetch('http://localhost:5253/api/Receptionist/GetPendingBills',
        {
            method:'GET',
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
        displayBills(data)
    }).catch( error => {
        openModal('alertModal', "Error",error.message);
    });
}

var redirectToOutPatientBill = () =>{
    window.location.href="./outPatientBill.html";
}

var redirectToInPatientBill = () =>{
    window.location.href="./inPatientBill.html";
}


document.addEventListener("DOMContentLoaded",()=>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error","Unauthorized Access!");
        return;
    }
    loadPendingBills();
})