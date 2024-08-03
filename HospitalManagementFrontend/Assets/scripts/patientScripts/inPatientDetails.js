var fetchDetails = async () =>{
    await checkForRefresh()
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    displayDataSkeleton();
    fetch('http://localhost:5253/api/Receptionist/GetAllInPatientDetails',
        {
            method:'GET',
            headers:{
                'Content-Type' : 'application/json', 
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`               
            }
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
        removeDateSkeleton();
        displayData(data)
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
    });
}

var displayDataSkeleton = () =>{
    document.getElementById("tableBody").innerHTML = `
        <tr class="bg-white border-b">
    <td class="px-6 py-4 font-medium">
        <div class="w-24 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-40 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-24 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-32 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
    <td class="px-6 py-4">
        <div class="w-48 h-4 bg-gray-300 animate-pulse rounded"></div>
    </td>
</tr>
    `;
}

var removeDateSkeleton = () =>{
    document.getElementById("tableBody").innerHTML ="";
}

var displayData = (data) =>{
    var div = document.getElementById("tableBody");
    data.forEach(patient => {
        div.innerHTML+=`
            <tr class="whitespace-nowrap text-sm">
                <td class="px-6 py-4 font-medium">${patient.patientId}</td>
                <td class="px-6 py-4">${patient.patientName}</td>
                <td class="px-6 py-4">${patient.wardType}</td>
                <td class="px-6 py-4">${patient.roomNo}</td>
                <td class="px-6 py-4">${patient.admittedDate.split('T')[0]}</td>
                <td class="px-6 py-4">${patient.description}</td>
            </tr>
        `;
    });
}

var filterTable = () => {
    var name = document.getElementById("name").value;
    if(!validateName('name')){
        document.getElementById("tableBody").innerHTML="";
        fetchDetails();
    }
    var tableRows = document.getElementById("tableBody");
    Array.from(tableRows.rows).forEach(row => {
        row.classList.remove("none");
    })
    var res="";
    Array.from(tableRows.rows).forEach(row => {
        res="";
        const cell = row.cells[1];
        if (cell) {
            res = cell.textContent.toLowerCase().includes(name.toLowerCase())?"":"none";
        }
        if(res){
            row.classList.add(res);
        }
    });    
}

document.addEventListener("DOMContentLoaded", () =>{
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    fetchDetails();
})