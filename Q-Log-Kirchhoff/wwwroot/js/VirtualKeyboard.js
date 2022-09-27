
var visible = false;

$(document).on('click', function (e) {
    if ($(e.target).closest("#KeyboardContainer").length === 0) {
        //$("#KeyboardContainer").hide();
        if (visible) {
            hideKeyboard();
            visible = false;
        }
        else
            visible = true;

    }
});

var container = null;
var containerID = "KeyboardContainer";

var curInput = null;

var rows_ =
[
        ["1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "←"],
        ["Q", "W", "E", "R", "T", "Z", "U", "I", "O", "P", "Ü"],
        ["A", "S", "D", "F", "G", "H", "J", "K", "L", "Ö", "Ä"],
        ["Y", "X", "C", "V", "B", "N", "M", "-"], //",", ".",
        ["Space"] // , "🠟"
    ];


var rows =
    [
        ["1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "←"],
        ["Q", "W", "E", "R", "T", "Z", "U", "I", "O", "P", "Ü"],
        ["A", "S", "D", "F", "G", "H", "J", "K", "L", "Ö", "Ä"],
        ["Y", "X", "C", "V", "B", "N", "M", "-", ",", "."],
        ["Space"] // , "🠟"
    ];

//document.onload = function () {
//    document.getElementById(containerID).addEventListener("blur", function () {
//        hideKeyboard();
//        console.log("HIDE!");
//        return;
//    });
//};

function setContainerID(id) {
    containerID = id;
}

function hideKeyboard() {
    if (container === null)
        return;
    while (container.firstChild) {
        container.removeChild(container.firstChild);
    }
}

function setCurInput(inputText) {
    if (curInput !== null && curInput !== undefined) {
        curInput.innerText = inputText;
    }
}

function showKeyboard(input, fieldName = "", charsOnly = false, numbersOnly = false) {
    visible = false;
    if (input === null || input === undefined) {
        console.error("Could not show keyboard. Inputelement was null or undefined.");
        return;
    }
    if (container === null) {
        try {
            container = document.getElementById(containerID);
            
        }
        catch (err) {
            console.error("Could not show keyboard. Error: " + err);
            return;
        }

        if (container === null) {
            console.error("Could not show keyboard. Container wasnt found.");
            return;
        }
    }
    // make sure we have a fresh start
    hideKeyboard();

    var name = document.createElement("li");
    name.innerText = fieldName;
    name.classList += "fieldName";
    container.appendChild(name);

    var br1 = document.createElement("br");
    container.appendChild(br1);

    curInput = document.createElement("li");
    curInput.innerText = input.value;
    curInput.classList += "curInput";
    container.appendChild(curInput);

    var suggestionContainer = document.createElement("div");
    suggestionContainer.id = "SuggestionContainer";
    container.appendChild(suggestionContainer);

    var br2 = document.createElement("br");
    container.appendChild(br2);

    var hide = document.createElement("li");
    hide.innerHTML = "<i class='glyphicon glyphicon-arrow-down'></i>";
    hide.classList += "hideKeyboard keyboardButton";
    hide.addEventListener('click', function () {
        hideKeyboard();
    });
    container.appendChild(hide);


    var numbers = "1234567890";
    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜ-"; //-

    for (var i = 0; i < rows.length; i++) {
        for (var j = 0; j < rows[i].length; j++) {
               
            var li = document.createElement("li");
            li.innerText = rows[i][j];
            switch (li.innerText) {
                case "Space":
                    li.classList += "keyboardSpace ";
                    if (numbersOnly) {
                        li.classList += " keyboardDisabled ";
                    }
                    else {
                        li.addEventListener('click', function () {
                            input.value += " ";
                            input.focus();
                            curInput.innerText = input.value;
                            $(input).trigger("keyup");
                        });
                    }

                    break;
                case "←":
                    li.classList += "keyboardBack ";
                    li.addEventListener('click', function () {
                        if (input.value.length > 0) {
                            input.value = input.value.substring(0, input.value.length - 1);
                        }
                        if (input.id === "ForwardingAgencyInput") {
                            var postalCodeInput = document.getElementById("PostalCodeInput");
                            postalCodeInput.value = "";
                        }
                        curInput.innerText = input.value;
                        input.focus();
                        $(input).trigger("keyup");
                    });
                    break;
                //case "🠟":
                //    li.classList += "hideKeyboard ";
                //    li.addEventListener('click', function () {
                //        hideKeyboard();
                //    });
                //    break;
                default:
                    if (charsOnly && !chars.includes(rows[i][j]) || numbersOnly && !numbers.includes(rows[i][j])) {
                        li.classList += " keyboardDisabled ";
                    }
                    else {
                        li.addEventListener('click', function () {
                            input.value += this.innerText.toUpperCase();
                            curInput.innerText = input.value;
                            input.focus();
                            $(input).trigger("keyup");
                        });
                    }
                    break;
            }

            li.classList += "keyboardButton";
            container.appendChild(li);
        }
        var br = document.createElement("br");
        container.appendChild(br);

      
    }
}