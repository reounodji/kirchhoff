
var forwardingAgencyList = null;
var maxNumberOfSuggestions = 5;


// --------- SignarR -------------------------------------
var connection = new signalR.HubConnectionBuilder().withUrl("/RegistrationHub").build();

$(connection).bind("onDisconnect", function (e, data) {
    connection.start();
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

connection.on("Error", function (message) {
    alert(message);
});

connection.onclose(function () {
    connection.start();
});

function changeForm(type, companyNameLabel, companyNameInput, loadReferenceLabel, loadReferenceInput, suggestionsLabelContent) {
    var CompanyNameLabel = document.getElementById("CompanyNameLabel")
    var CompanyNameInput = document.getElementById("CompanyNameInput")
    var LoadReferenceLabel = document.getElementById("LoadReferenceLabel")
    var LoadReferenceInput = document.getElementById("LoadReferenceInput")
    var SupplierNumberInputDiv = document.getElementById("SupplierNumberInputDiv")
    var SupplierNumberInput = document.getElementById("SupplierNumberInput")
    var SuggestionContainer = document.getElementById("SuggestionContainer")
    var SuggestionsLabel = document.getElementById("suggestionsLabel")

    CompanyNameLabel.textContent = companyNameLabel;
    CompanyNameInput.placeholder = companyNameInput;
    CompanyNameInput.value = "";
    LoadReferenceLabel.textContent = loadReferenceLabel;
    LoadReferenceInput.placeholder = loadReferenceInput;
    SupplierNumberInput.value = "";
    SuggestionsLabel.innerText = suggestionsLabelContent;

    clearContainer("SupplierNumberInput");

    if (type === "Supplier") {
        SupplierNumberInputDiv.removeAttribute("hidden");
        SuggestionContainer.setAttribute("size", 9);
    }
    else {
        SupplierNumberInputDiv.setAttribute("hidden", true);
        SuggestionContainer.setAttribute("size", 6);
    }

    clearContainer("SuggestionContainer");
}

function validateNumber(evt) {

    var e = evt || window.event;
    var key = e.keyCode || e.which;

    if (!e.shiftKey && !e.altKey && !e.ctrlKey &&
        // numbers   
        key >= 48 && key <= 57 ||
        // Numeric keypad
        key >= 96 && key <= 105 ||
        // Backspace and Tab and Enter
        key === 8 || key === 9 || key === 13 ||
        // Home and End
        key === 35 || key === 36 ||
        // left and right arrows
        key === 37 || key === 39 ||
        // Del and Ins
        key === 46 || key === 45) {
        // input is VALID
    }
    else {
        // input is INVALID
        e.returnValue = false;
        if (e.preventDefault) e.preventDefault();
    }
}


/*
 *  Disables the send button so that it is impossible to send multiple registrations by accident. 
 */
function disableSendButton() {
    var button = document.getElementById("SendButton");
    if (button !== null && button !== undefined) {
        button.disabled = true;
    }
}


//------------------- CompanyNameChanged -----------------------------------------------------------------
/*
 * Is called when the user enters text into the companyName text field
 */
function companyNameKeyUp() {
    if (document.getElementById("CompanyNameInput") === null || document.getElementById("CompanyNameInput") === undefined)
        return;

    var companyNameInput = document.getElementById("CompanyNameInput").value;

    // only check for matches once the user entered at least 2 characters
    if (companyNameInput.length >= 2) {

        var invoke;

        if (document.getElementById("ApproachTypSelectForwardingAgency").checked) {
            invoke = "GetForwardingAgencies";
        }
        if (document.getElementById("ApproachTypSelectSupplier").checked) {
            invoke = "GetSuppliers";

            //connection.invoke("GetSupplierNumbers", companyNameInput).catch(function (err) {
            //    alert("Leider konnten die SupplierNumbers nicht geladen werden.");
            //    return console.error(err.toString());
            //});
            //removeSelectedSupplierNumber();
        }
        if (document.getElementById("ApproachTypSelectParcelService").checked) {
            invoke = "GetParcelServices";
        }
        if (document.getElementById("ApproachTypSelectFitter").checked) {
            invoke = "GetFitters";
        }

        connection.invoke(invoke, companyNameInput).catch(function (err) {
            alert("Leider konnten die Vorschläge nicht geladen werden.");
            return console.error(err.toString());
        });
    }
    else {
        clearContainer("SuggestionContainer");
    }
}

/* 
 * Receive the list of ForwardingAgencies from the server and
 * display the first maxNumberOfSuggestions suggestions in the keyboard.
 */
connection.on("SetSuggestions", function (companyNameList) {

    var suggestionContainer = document.getElementById("SuggestionContainer");
    if (suggestionContainer === null || suggestionContainer === undefined) {
        console.log("Cannot display forwarding agency suggestions. The SuggestionContainer wasnt found.");
        return;
    }

    clearContainer("SuggestionContainer");

    for (var i = 0; i < companyNameList.length; i++) {
        var option = document.createElement("option");
        option.appendChild(document.createTextNode(companyNameList[i].name));
        option.value = companyNameList[i].name;
        option.setAttribute("onclick", "suggestionSelected(this)");
        suggestionContainer.appendChild(option);
    }

});

connection.on("SetSupplierSuggestions", function (supplierList) {

    var suggestionContainer = document.getElementById("SuggestionContainer");
    if (suggestionContainer === null || suggestionContainer === undefined) {
        console.log("Cannot display forwarding agency suggestions. The SuggestionContainer wasnt found.");
        return;
    }

    clearContainer("SuggestionContainer");

    for (var i = 0; i < supplierList.length; i++) {
        var option = document.createElement("option");
        option.appendChild(document.createTextNode(supplierList[i]));
        option.value = supplierList[i];
        option.setAttribute("onclick", "supplierSuggestionSelected(this)");
        suggestionContainer.appendChild(option);
    }

});



function clearContainer(elementID) {
    var container = document.getElementById(elementID);
    if (container !== null && container !== undefined) {
        while (container.firstChild) {
            container.removeChild(container.firstChild);
        }
    }
}

function supplierNumberSelected(item) {
    var SupplierNumber = document.getElementById("SupplierNumber");
    var SupplierNumberInput = document.getElementById("SupplierNumberInput");
    SupplierNumber.value = SupplierNumberInput[item].value;
}

function removeSelectedSupplierNumber() {
    var SupplierNumber = document.getElementById("SupplierNumber");
    SupplierNumber.value = "";
}

/*
 *  Called when the user clicked on one of the suggestions.
 *  Set the values of the input fields to the values of the suggestion and hide the keyboard.
 */
function suggestionSelected(item) {
    if (item === null || item === undefined)
        return;

    var companyNameInput = document.getElementById("CompanyNameInput");
    if (companyNameInput === null || companyNameInput === undefined)
        return;

    companyNameInput.value = item.value;
}

function supplierSuggestionSelected(item) {
    if (item === null || item === undefined)
        return;

    var companyNameInput = document.getElementById("CompanyNameInput");
    if (companyNameInput === null || companyNameInput === undefined)
        return;

    var supplierNumberInput = document.getElementById("SupplierNumberInput");
    if (supplierNumberInput === null || supplierNumberInput === undefined)
        return;

    var supplierNumberASPInput = document.getElementById("supplierNumberASPInput");
    if (supplierNumberASPInput === null || supplierNumberASPInput === undefined)
        return;

    companyNameInput.value = item.value.split(" \u2192 ")[0];
    supplierNumberInput.value = item.value.split(" \u2192 ")[1];
    supplierNumberASPInput.value = item.value.split(" \u2192 ")[1];
}

// ------------------- SmallVehicle -----------------------------------------------------------------------
function setSmallVehicle(special) {
    var bigBox = document.getElementById('bigVehicleCheckBox');
    var smallBox = document.getElementById('smallVehicleCheckBox');

    if (special === true) {
        bigBox.checked = false;
        smallBox.checked = true;
    }
    else {
        bigBox.checked = true;
        smallBox.checked = false;
    }
}
