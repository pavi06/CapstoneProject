var specialityDescription = { "Dermatology": "Skin health diagnosis and treatment.", "Neurology": "Nervous system diagnosis and treatment.", "Pediatrics": "Child health diagnosis and treatment." ,
     "Cardiology": "Heart health diagnosis and treatment.", "Gynacology": "Reproductive health diagnosis and treatment."}

var redirectToDoctors = (speciality) => {
    localStorage.setItem("Speciality",speciality);
    window.location.href="./doctors.html";
}

var displaySpecialities = (data) => {
    var specialitiesDiv = document.getElementById("specialitiesDiv");
    data.forEach(speciality => {
        specialitiesDiv.innerHTML += `
            <div class="card bg-white shadow-lg rounded-3xl border-t-4 border-[#009fbd] text-center m-3" onclick="redirectToDoctors('${speciality}')">
                <div class="mx-auto mt-5" style="width: 110px; height: 110px; border-radius: 50%; border: 5px solid #009fbd; overflow: hidden; position: relative;">
                        <img src="../../Assets/images/${speciality.toLowerCase()}.jpg" alt="Image" style="width: 100%; height: 100%; object-fit: cover; display: block;">
                </div>   
                <div class="p-4">
                    <h3 class="text-xl font-bold mb-1" style="color: #4A249D;">${speciality}</h3>
                    <p>${specialityDescription[speciality]}</p>
                </div>
            </div>
        `;
    });
}

var displaySpecialitiesSkeleton = () => {
    var specialitiesDiv = document.getElementById("specialitiesDiv");
        specialitiesDiv.innerHTML = `
            <div class="card bg-white shadow-lg rounded-3xl border-t-4 border-[#009fbd] text-center m-3">
            <div class="mx-auto mt-5" style="width: 110px; height: 110px; border-radius: 50%; border: 5px solid #009fbd; overflow: hidden; position: relative;">
                <div class="w-full h-full bg-gray-300 animate-pulse" style="border-radius: 50%;"></div>
            </div>   
            
            <div class="p-4">
                <div class="w-3/4 h-6 bg-gray-300 animate-pulse mb-2 rounded"></div>
                <div class="w-full h-4 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
         <div class="card bg-white shadow-lg rounded-3xl border-t-4 border-[#009fbd] text-center m-3">
            <div class="mx-auto mt-5" style="width: 110px; height: 110px; border-radius: 50%; border: 5px solid #009fbd; overflow: hidden; position: relative;">
                <div class="w-full h-full bg-gray-300 animate-pulse" style="border-radius: 50%;"></div>
            </div>   
            
            <div class="p-4">
                <div class="w-3/4 h-6 bg-gray-300 animate-pulse mb-2 rounded"></div>
                <div class="w-full h-4 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
         <div class="card bg-white shadow-lg rounded-3xl border-t-4 border-[#009fbd] text-center m-3">
            <div class="mx-auto mt-5" style="width: 110px; height: 110px; border-radius: 50%; border: 5px solid #009fbd; overflow: hidden; position: relative;">
                <div class="w-full h-full bg-gray-300 animate-pulse" style="border-radius: 50%;"></div>
            </div>   
            
            <div class="p-4">
                <div class="w-3/4 h-6 bg-gray-300 animate-pulse mb-2 rounded"></div>
                <div class="w-full h-4 bg-gray-300 animate-pulse rounded"></div>
            </div>
        </div>
        `;
}

var removeDisplaySpecialitiesSkeleton = () =>{
    document.getElementById("specialitiesDiv").innerHTML="";
}

var fetchSpecialities = async () => {
    await checkForRefresh()
    displaySpecialitiesSkeleton()
    fetch('http://localhost:5253/api/DoctorBasic/GetAllSpecialization',
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        }
        )
        .then(async (res) => {
            removeDisplaySpecialitiesSkeleton()
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
            displaySpecialities(data);
        }).catch(error => {
            openModal('alertModal', "Error", error.message);
        });
}


document.addEventListener("DOMContentLoaded", () => {
    updateForLogInAndOut();
    fetchSpecialities();
})