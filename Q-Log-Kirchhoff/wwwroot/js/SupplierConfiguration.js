function addSupplierNumber() {
    var supplierNumberSelect = document.getElementById("supplierNumberSelect");
    var supplierNumbers = document.getElementById("FormNumbersInput");
    var supplierNumberInput = document.getElementById("SupplierNumberInput");

    if (supplierNumbers.value.includes(supplierNumberInput.value)) {
        return;
    }

    var option = document.createElement("option");
    option.appendChild(document.createTextNode(supplierNumberInput.value));
    option.value = supplierNumberInput.value;
    option.setAttribute("onclick", "enableRemoveButton()");
    supplierNumberSelect.appendChild(option);

    supplierNumbers.value += supplierNumberInput.value + ',';
    supplierNumberInput.value = "";
}

function enableRemoveButton() {
    document.getElementById("removeButton").disabled = false;
}

function removeSupplierNumber() {
    var supplierNumberSelect = document.getElementById("supplierNumberSelect");
    var supplierNumbers = document.getElementById("FormNumbersInput");
    var supplierNumberToRemove = supplierNumberSelect.options[supplierNumberSelect.selectedIndex].text;

    supplierNumbers.value = supplierNumbers.value.replace(supplierNumberToRemove + ",", " ");
    supplierNumberSelect.removeChild(supplierNumberSelect.options[supplierNumberSelect.selectedIndex]);

    document.getElementById("removeButton").disabled = true;
}