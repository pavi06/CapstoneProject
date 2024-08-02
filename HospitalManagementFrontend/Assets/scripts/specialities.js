var specialityDescription = { "Dermatology": "Skin health diagnosis and treatment.", "Neurology": "nnnn", "Pediatrics": "pppp" ,
     "Cardiology": "nnnn", "Gynacology": "nnnn"}

var redirectToDoctors = (speciality) => {
    console.log(speciality)
    localStorage.setItem("Speciality",speciality);
    window.location.href="./doctors.html";
}

var displaySpecialities = (data) => {
    var specialitiesDiv = document.getElementById("specialitiesDiv");
    data.forEach(speciality => {
        specialitiesDiv.innerHTML += `
            <div class="card bg-white shadow-lg rounded-3xl border-t-4 border-[#009fbd] text-center m-3" onclick="redirectToDoctors('${speciality}')">
                <div class="mx-auto mt-5" style="width: 110px; height: 110px; border-radius: 50%; border: 5px solid #009fbd; overflow: hidden; position: relative;">
                        <img src="../../image.jpg" alt="Image" style="width: 100%; height: 100%; object-fit: cover; display: block;">
                </div>   
                <div class="p-4">
                    <h3 class="text-xl font-bold mb-1" style="color: #4A249D;">${speciality}</h3>
                    <p>${specialityDescription[speciality]}</p>
                </div>
            </div>
        `;
    });
}

var fetchSpecialities = () => {
    fetch('http://localhost:5253/api/DoctorBasic/GetAllSpecialization',
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
            console.log("specialities successfully")
            console.log(data)
            displaySpecialities(data);
        }).catch(error => {
            console.log(error.message)
        });
}


document.addEventListener("DOMContentLoaded", () => {
    updateForLogInAndOut();
    fetchSpecialities();
})