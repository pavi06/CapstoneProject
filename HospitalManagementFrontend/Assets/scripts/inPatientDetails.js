var fetchDetails = () =>{
    fetch('http://localhost:5253/api/Receptionist/GetAllInPatientDetails',
        {
            method:'GET',
            headers:{
                'Content-Type' : 'application/json',                
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
        displayData(data)
    }).catch( error => {
        console.log(error)
        alert(error.message)
    });
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
                <td class="px-6 py-4">${patient.admittedDate}</td>
                <td class="px-6 py-4">${patient.description}</td>
            </tr>
        `;
    });
}

var filterTable = () => {
    var name = document.getElementById("name").value;
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
    fetchDetails();
})