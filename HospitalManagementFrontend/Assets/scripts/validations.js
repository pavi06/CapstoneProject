function functionAddValidEffects(id) {
    document.getElementById(id).style.border="1px solid green";
    document.getElementById(id+'text').innerHTML="<i class='bx bx-check'></i>&nbsp;Looks Good!"
    document.getElementById(id+'text').style.color="green";
    document.getElementById(id+'text').classList.remove('none');
    return true;
}

function functionAddInValidEffects(id) {
    if(id === "confirmpassword"){
        document.getElementById(id+'text').innerHTML="<i class='bx bxs-error'></i>&nbsp;Passwords are not same!"
    }
    else{
        document.getElementById(id+'text').innerHTML="<i class='bx bxs-error'></i>&nbsp;Provide valid value!"
    }
    document.getElementById(id).style.border="1px solid red";    
    document.getElementById(id+'text').style.color="red";
    document.getElementById(id+'text').classList.remove('none');
    return true;
}

function validate(id){
    var element = document.getElementById(id);
    if(element.value){
        return functionAddValidEffects(id);
    }
    return functionAddInValidEffects(id);
}

function validateName(id){
    var regex = /^[A-Za-z]+$/;
    var element = document.getElementById(id);
    if(element.value && element.value.match(regex)){
        return functionAddValidEffects(id);
    }
    else{
        return functionAddInValidEffects(id);
    }
}

function validateEmail(id) {
    var regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    var element = document.getElementById(id);
    if (element.value && element.value.match(regex)) {
        return functionAddValidEffects(id);
    }
    else {
        return functionAddInValidEffects(id);
    }
}

function validatePassword(id) {
    var regexExpression = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$&*_])(?=.*[0-9]).{6,}$/;
    var element = document.getElementById(id);
    if (element.value && element.value.match(regexExpression)) {
        return functionAddValidEffects(id);
    }
    else {
        return functionAddInValidEffects(id);
    }
}

function validateDate(id) {
    var dateElement = document.getElementById(id);
    var today = new Date();
    let formattedDate = `${today.getFullYear()}-${(today.getMonth() + 1).toString().padStart(2, '0')}-${today.getDate().toString().padStart(2, '0')}`;
    if (dateElement.value && Date.parse(dateElement.value) >= Date.parse(formattedDate)) {
        return functionAddValidEffects(id);
    } else {
        return functionAddInValidEffects(id);
    }
}

function validateNumber(id) {
    var data = document.getElementById(id);
    var reg = /^\d+$/;
    if (data.value.match(reg)) {
        return functionAddValidEffects(id);
    }
    else {
        return functionAddInValidEffects(id);
    }
}

function validatePhone(id) {
    var element = document.getElementById(id);
    var regPhone = /^[+]{1}(?:[0-9\-\\(\\)\\/.]\s?){6,15}[0-9]{1}$/;
    if (element.value && element.value.match(regPhone)) {
        return functionAddValidEffects(id);
    }
    else {
        return functionAddInValidEffects(id);
    }
}

function validateConfirmPassword(id1, id2) {
    var element = document.getElementById(id1);
    var element2 = document.getElementById(id2);
    if (element.value && element.value === element2.value) {
        return functionAddValidEffects(id1);
    }
    else {
        return functionAddInValidEffects(id1);
    }
}


function passwordInfo(id){
    var password = document.getElementById(id+'text');
    password.style.color="red";
    password.innerHTML="Should contain atleast a uppercase, lowercase, numeric and any of these special characte( @ $ * _)"
}