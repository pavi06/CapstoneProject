var UpdateTab = (e, lefttag) =>{
    e.classList.add('addBgColor');
    document.getElementById(lefttag).classList.remove('addBgColor');
    if(e.id === "createPatient"){
        document.getElementById("addDataDynamically").innerHTML = `
            <div id="inPatientCreate">
                    <div class="flex flex-wrap justify-end mr-0 mb-5 mx-auto">
                        <div class="w-20 py-3 px-5 rounded-md ring-1 mr-3 ring-[#e0e0e0]" id="patientIdDisplay"></div>
                        <div class="flex flex-col">
                            <input type="text" name="name" required onblur="validateName('name')" id="name" placeholder="patientName" class="w-40 rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md" />
                            <span id="nametext" class="text-xs ml-3 none mt-0"></span>
                        </div>
                        <div class="flex flex-col">
                            <input type="text" name="contact" required onblur="validatePhone('contact')" id="contact" placeholder="contactNo" class=" mx-2 w-40 rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md" />
                            <span id="contacttext" class="text-xs ml-3 none mt-0"></span>
                        </div>
                        <button type="button" class="mx-2 px-3 bg-indigo-100 rounded-lg" onclick="getPatientId()" style="width: fit-content;">Get Patient</button>
                    </div>
                    <form id="createPatientForm" style="width: 50%;" class="mx-auto">
                        <div class="mb-5">
                            <label for="patientCreateName" class="mb-3 block text-base font-medium text-[#07074D]">Name</label>
                            <input type="text" name="patientCreateName" id="patientCreateName"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                            <span id="nametext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        <div class="mb-5">
                            <label for="createPatientDob" class="mb-3 block text-base font-medium text-[#07074D]">Date Of Birth</label>
                            <input type="date" name="createPatientDob" id="createPatientDob"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                            <span id="createPatientDobtext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        <div class="mb-5">
                            <label for="gender" class="mb-3 block text-base font-medium text-[#07074D]">Gender</label>
                            <input type="text" name="gender" id="gender"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                            <span id="gendertext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        <div class="mb-5">
                            <label for="createPatientContact" class="mb-3 block text-base font-medium text-[#07074D]">Contact Number</label>
                            <input type="text" name="createPatientContact" id="createPatientContact"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                            <span id="createPatientContacttext" class="text-xs ml-3 none mt-0"></span>
                        </div>
                        <div class="mb-5">
                            <div class="mb-5">
                                <label for="address" class="mb-3 block text-base font-medium text-[#07074D]">Address</label>
                                <textarea name="address" id="address" rows="3" class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" placeholder="address here"></textarea>
                                 <span id="addresstext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        </div>
                        <div class="mb-5">
                            <label for="wardType" class="mb-3 block text-base font-medium text-[#07074D]">WardType</label>
                            <input type="text" name="wardType" id="wardType"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                            <span id="wardTypetext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        <div class="mb-5">
                            <label for="noOfDays" class="mb-3 block text-base font-medium text-[#07074D]">NoOfDays</label>
                            <input type="text" name="noOfDays" id="noOfDays"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                            <span id="noOfDaystext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        <div class="mb-5">
                            <label for="description" class="mb-3 block text-base font-medium text-[#07074D]">Description</label>
                            <input type="text" name="description" id="description"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                            <span id="descriptiontext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        <div class="w-full flex justify-center">
                            <button type="button" id="createBtn" onclick="createPatient()"
                                class="hover:bg-white hover:text-[#4A249D] hover:border-[#4A249D] border-2 rounded-md bg-[#4A249D] w-50 py-2 px-6 text-center text-base font-semibold text-white outline-none">
                                Create
                            </button>
                        </div>
                    </form>
                </div>
        `;
    }
    else if(e.id === "updatePatient"){
        document.getElementById("addDataDynamically").innerHTML=`
        <div id="inPatientUpdate" class="w-full flex flex-col justify-center none">
                    <div class="flex flex-wrap flex-end mr-5 mb-5 mx-auto">
                        <div class="w-20 py-3 px-5 rounded-md ring-1 mr-3 ring-[#e0e0e0]" id="admissionIdDisplay"></div>
                        <div class="flex flex-col">
                            <input type="text" name="name" id="name" placeholder="patientName" class="w-40 rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md" />
                            <span id="nametext" class="text-xs ml-3 none mt-0"></span>
                        </div>
                        <div class="flex flex-col">
                            <input type="text" name="contact" id="contact" placeholder="contactNo" class=" mx-2 w-40 rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md" />
                            <span id="contacttext" class="text-xs ml-3 none mt-0"></span>
                        </div>
                        <button type="button" class="mx-2 px-3 bg-indigo-100 rounded-lg" onclick="getAdmissionId()" style="width: fit-content;">Get Patient</button>
                    </div>
                    <form id="inPatientUpdateForm" class="mx-auto text-center" style="width: 40%;">
                        <div class="mb-5">
                            <label for="admissionId" class="mb-3 block text-base font-medium text-[#07074D]">AdmissionId</label>
                            <input type="text" name="admissionId" id="admissionId"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                            <span id="admissionIdtext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        <div class="mb-5">
                            <label for="wardType" class="mb-3 block text-base font-medium text-[#07074D]">WardType</label>
                            <input type="text" name="wardType" id="wardType"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                            <span id="wardTypetext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        <div class="mb-5">
                            <label for="noOfDays" class="mb-3 block text-base font-medium text-[#07074D]">NoOfDays</label>
                            <input type="number" name="noOfDays" id="noOfDays"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                             <span id="noOfDaystext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        <div class="w-full flex justify-center">
                            <button type="button" id="updateBtn" onclick="updatePatient()"
                                class="hover:bg-white hover:text-[#4A249D] hover:border-[#4A249D] border-2 rounded-md bg-[#4A249D] w-50 py-2 px-6 text-center text-base font-semibold text-white outline-none">
                                Update
                            </button>
                        </div>
                    </form>
                </div>
        `;
    }
}

var Update = (e) =>{
    e.classList.add('addBgColor');
    document.getElementById("addDataDynamically").innerHTML=`
        <div id="inPatientDoctorUpdate" class="w-full flex flex-col justify-center none">
                    <form id="inPatientDoctorUpdateForm" class="mx-auto text-center" style="width: 40%;">
                        <div class="mb-5">
                            <label for="admissionId" class="mb-3 block text-base font-medium text-[#07074D]">AdmissionId</label>
                            <input type="text" name="admissionId" id="admissionId"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                            <span id="admissionIdtext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        <div class="mb-5">
                            <label for="doctorId" class="mb-3 block text-base font-medium text-[#07074D]">DoctorId</label>
                            <input type="text" name="doctorId" id="doctorId"
                                class="w-full rounded-md border border-[#e0e0e0] bg-white py-3 px-6 text-base font-medium text-[#6B7280] outline-none focus:border-[#6A64F1] focus:shadow-md break-words" />
                            <span id="doctorIdtext" class="text-xs ml-3 none mt-0"></span>
                                </div>
                        <div class="w-full flex justify-center">
                            <button type="button" id="updateBtn" onclick="updateDoctor()"
                                class="hover:bg-white hover:text-[#4A249D] hover:border-[#4A249D] border-2 rounded-md bg-[#4A249D] w-50 py-2 px-6 text-center text-base font-semibold text-white outline-none">
                                Update
                            </button>
                        </div>
                    </form>
                </div>
        `;
}

var getPatientId = async () =>{
    if(!(validateName('name') && validatePhone('contact'))){
        openModal('alertModal', "Error", "Provide all values properly!");
        return;
    }
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    var name = document.getElementById("name").value;
    var contactNo = document.getElementById("contact").value;
    await checkForRefresh();
    fetch('https://pavihosmanagebeapp.azurewebsites.net/api/DoctorBasic/GetPatientId',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json',  
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`              
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
        var id = document.getElementById("patientIdDisplay");
        id.innerHTML=data;
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
    });
}


var createPatient = async () =>{
    if(!(validate('wardType') && validateNumber('noOfDays') && validate('description'))){
        openModal('alertModal', "Error", "Provide necessary values to proceed!");
        return;
    }
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    var patientId=parseInt(document.getElementById("patientIdDisplay").textContent,10);
    var bodyData = {
        patientId: Number.isNaN(patientId) ? 0 : patientId,
        name: document.getElementById("patientCreateName").value,
        dateOfBirth: document.getElementById("createPatientDob").value ? "" : "0001-01-01",
        gender: document.getElementById("gender").value,
        contactNo: document.getElementById("createPatientContact").value,
        address: document.getElementById("address").value,
        wardType: document.getElementById("wardType").value,
        noOfDays: parseInt(document.getElementById("noOfDays").value ,10),
        description: document.getElementById("description").value
    }
    console.log(bodyData)
    await checkForRefresh()
    fetch('https://pavihosmanagebeapp.azurewebsites.net/api/Receptionist/AdmissionForPatient',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json', 
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`               
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
        openModal('alertModal',"Success",data)
        window.location.href="./InPatientDetails.html";
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
    });
    document.getElementById("createPatientForm").reset();
    document.getElementById("name").value="";
    document.getElementById("contact").value="";
    document.getElementById("patientIdDisplay").textContent="";
}


var updatePatient = async () =>{
    if(!(validateNumber('admissionId') && validate('wardType') && validate('noOfDays'))){
        openModal('alertModal', "Error", "Provide all values properly!");
        return;
    }
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "UnAuthorized access!");
        return;
    }
    var bodyData = {
        admissionId:document.getElementById("admissionId").value,
        wardType:document.getElementById("wardType").value,
        noOfDays:document.getElementById("noOfDays").value
    }
    await checkForRefresh()
    fetch('https://pavihosmanagebeapp.azurewebsites.net/api/Receptionist/UpdateInPatient',
        {
            method:'PUT',
            headers:{
                'Content-Type' : 'application/json',  
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`              
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
        openModal('alertModal', "Success", data);
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
    });
    document.getElementById("inPatientUpdateForm").reset();
    window.location.href="./InPatientDetails.html";
}

var getAdmissionId = async () => {
    if(!(validateName('name') && validatePhone('contact'))){
        openModal('alertModal', "Error", "Provide name and contactno properly to get admission id");
        return;
    }
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    await checkForRefresh()
    fetch('https://pavihosmanagebeapp.azurewebsites.net/api/DoctorBasic/GetAdmissionId',
        {
            method:'POST',
            headers:{
                'Content-Type' : 'application/json',  
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`              
            },
            body:JSON.stringify({
                patientName: document.getElementById("name").value,
                contactNumber: document.getElementById("contact").value
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
        var id = document.getElementById("admissionIdDisplay");
        id.innerHTML=data;
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
    });
}


var updateDoctor = async () =>{
    if(!(validateNumber('admissionId') && validateNumber('doctorId'))){
        openModal('alertModal', "Error", "Provide all values properly!");
        return;
    }
    if(JSON.parse(localStorage.getItem('loggedInUser')).role != "Receptionist"){
        openModal('alertModal', "Error", "UnAuthorized Access!");
        return;
    }
    var bodyData = {
        admissionId:document.getElementById("admissionId").value,
        doctorId:document.getElementById("doctorId").value,
    }
    await checkForRefresh()
    fetch('https://pavihosmanagebeapp.azurewebsites.net/api/Receptionist/AddDoctorForPatient',
        {
            method:'PUT',
            headers:{
                'Content-Type' : 'application/json', 
                'Authorization': `Bearer ${JSON.parse(localStorage.getItem('loggedInUser')).accessToken}`               
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
        openModal('alertModal', "Success", data);
    }).catch( error => {
        openModal('alertModal', "Error", error.message);
    });
    document.getElementById("inPatientDoctorUpdateForm").reset();
}