function addNewRowForTable() {
    var table = document.getElementById("prescriptionTable").getElementsByTagName('tbody')[0];
    var templateRow = document.getElementById("templateRow");
    var newRow = templateRow.cloneNode(true);
    newRow.style.display = "";
    let inputs = newRow.querySelectorAll('input');
    inputs.forEach(input => {
        input.value = "";
    });
    table.appendChild(newRow);
}

function deleteRow(rowButton) {
    var table = document.getElementById("prescriptionTable");
    var row = rowButton.parentNode.parentNode;
    if(row.rowIndex==1){
        let inputs = row.querySelectorAll('input');
        inputs.forEach(input => {
            input.value = "";
        });
        return;
    }
    table.deleteRow(row.rowIndex);
}
