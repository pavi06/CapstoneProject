// function functionAddValidEffects(element) {
//     var name = element.name;
//     element.classList.remove("is-invalid");
//     element.classList.add("is-valid");
//     document.getElementById(`${name}Valid`).innerHTML = "valid input!";
//     document.getElementById(`${name}Invalid`).innerHTML = "";
//     return true;
// }

// function functionAddInValidEffects(element) {
//     var name = element.name;
//     element.classList.remove("is-valid");
//     element.classList.add("is-invalid");
//     document.getElementById(`${name}Valid`).innerHTML = "";
//     document.getElementById(`${name}Invalid`).innerHTML = `Please provide the valid ${name}!`;
//     return false;
// }

function validate(id){
    var element = document.getElementById(id);
    if(element.value){
        return true;
    }
    return false;
}

function validateName(id){
    var regex = /^[A-Za-z]+$/;
    var element = document.getElementById(id);
    if(element.value && element.value.match(regex)){
        return true;
    }
    else{
        return false;
    }
}

function validateEmail(id) {
    var regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    var element = document.getElementById(id);
    if (element.value && element.value.match(regex)) {
        return true;
    }
    else {
        return false;
    }
}

function validatePassword(id) {
    var regexExpression = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$&*_])(?=.*[0-9]).{6,}$/;
    var element = document.getElementById(id);
    if (element.value && element.value.match(regexExpression)) {
        return true;
    }
    else {
        return false;
    }
}

function validateDate(id) {
    var dateElement = document.getElementById(id);
    var today = new Date();
    let formattedDate = `${today.getFullYear()}-${(today.getMonth() + 1).toString().padStart(2, '0')}-${today.getDate().toString().padStart(2, '0')}`;
    if (dateElement.value && Date.parse(dateElement.value) >= Date.parse(formattedDate)) {
        return true;
    } else {
        return false;
    }
}

function validateNumber(id) {
    var data = document.getElementById(id);
    var reg = /^\d+$/;
    if (data.value.match(reg)) {
        return true;
    }
    else {
        return false;
    }
}

function validatePhone(id) {
    var element = document.getElementById(id);
    var regPhone = /^[+]{1}(?:[0-9\-\\(\\)\\/.]\s?){6,15}[0-9]{1}$/;
    if (element.value && element.value.match(regPhone)) {
        return true;
    }
    else {
        return false;
    }
}

function validateConfirmPassword(id1, id2) {
    var element = document.getElementById(id1);
    var element2 = document.getElementById(id2);
    if (element.value && element.value === element2.value) {
        return true;
    }
    else {
        return false;
    }
}