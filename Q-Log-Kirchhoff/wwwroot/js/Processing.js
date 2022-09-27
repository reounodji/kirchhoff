
// Vars
var ExceededWaitTimeColorCode = "";
var ExitColorCode = "";
var HoverColorCode = "";
var NewEntryColorCode = "";
var RecentChangeColorCode = "";

var table = null;

const timeOfRegistrationCol = 0;
const timeSinceRegistrationCol = 1;
const companyNameCol = 2;
const supplierNumberCol = 3;
const numberOfPeopleCol = 4;
const smallVehicleCol = 5;
const bigVehicleCol = 6;

const approachTypCol = 9;
const targetCol = 10;
const loadingStationCol = 11;

const releaseCol = 13;
const timeOfCallCol = 14;
const timeOfEntryCol = 15;
const processStartCol = 16;
const processEndCol = 17;


const forwarderCol = 0;
const customerCol = 1;
const nameCol = 2;
const phonenumberCol = 3;
const commentCol = 4;
const licensePlateCol = 5;
const shortTimeOfRegistCol = 6;
const minSinceRegistCol = 7;
const gateCol = 8;
const inactiveSinceCol = 9;
const commandCol = 10;
const timeOfExitCol = 10;



// SignalR
var connection = new signalR.HubConnectionBuilder().withUrl("/ProcessingHub").build();
//var connection = new signalR.HubConnectionBuilder().withUrl(" https://localhost:44318/ProcessingHub").build();

connection.onclose(async () => {
    await start();
});

async function start() {
    try {
        await connection.start();
        console.log("connected");
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

connection.start({ pingInterval: 6000 }).catch(function (err) {
    console.log("trying to restart SignalR connection.");
    setTimeout(function () {
        connection.start({ pingInterval: 6000 });
    }, 5000);
    return console.error(err.toString());
});

connection.on("Error", function (errorMessage) {
    confirm(errorMessage);
    location.reload(true);
});

// Datatable
$(document).ready(function () {
    $.fn.dataTable.moment('DD.MM.YYYY');
    $.fn.dataTable.moment("DD.MM.YYYY HH:mm:ss");
    $.fn.dataTable.moment("HH:mm:ss");
    table = $('#table').DataTable({
        "paging": true,
        "searching": true,
        "stateSave": true,
        "aoColumnDefs": [
            { 'bSortable': false, 'aTargets': [gateCol, licensePlateCol] }
        ],
        //ohneSortierung "order": [[1, "desc"]],
        "language": {
            "lengthMenu": localizations['DataTables_LengthMenu'], //"Zeige _MENU_ Einträge pro Seite",
            "zeroRecords": localizations['DataTables_ZeroRecords'], //"Keine Einträge gefunden.",
            "info": localizations['DataTables_Info'],//"Zeige Seite _PAGE_ von _PAGES_",
            "infoEmpty": localizations['DataTables_InfoEmpty'], // "Keine Einträge verfügbar",
            "paginate": {
                "previous": localizations['DataTables_Previous'],//"Vorherige Seite",
                "next": localizations['DataTables_Next']// "Nächste Seite"
            },
            "search": localizations['Search'] //"Suche:"
        },
        "lengthMenu": [[20, 25, 50, -1], [20, 25, 50, localizations['All']]],
        "dom": '<"top"f>rt<"bottom"lp><"clear">'
    });


    ExceededWaitTimeColorCode = document.getElementById("ExceededWaitTimeColorCode").value;
    ExitColorCode = document.getElementById("ExitColorCode").value;
    HoverColorCode = document.getElementById("HoverColorCode").value;
    NewEntryColorCode = document.getElementById("NewEntryColorCode").value;
    RecentChangeColorCode = document.getElementById("RecentChangeColorCode").value;


    var prevColor = "";
    $("tr").not(':first').hover(
        function () {
            prevColor = $(this).css("background-color");
            $(this).css("background-color", HoverColorCode);
        },
        function () {
            $(this).css("background-color", prevColor);
        }
    );

    setupInfoPopOver();
    // Increase the time since registration by 1 every minute. Not 100% accurate with actual time but good enough. will be accurate at every reload of the page
    setInterval(updateTimeSinceRegistration, 60000);
});

// Menubar
function setupInfoPopOver() {
    $('[data-toggle="popover"]').popover();

    var data = "<ul>";
    data += "<li style='background-color:" + RecentChangeColorCode + "'>" + localizations['ProcessingInfo_RecentlyChanged'] + "</li>";
    data += "<li style='background-color:" + ExitColorCode + "'>" + localizations['ProcessingInfo_Exit'] + "</li>";
    data += "<li style='background-color:" + NewEntryColorCode + "'>" + localizations['ProcessingInfo_Registration'] + "</li>";
    data += "<li style='background-color:" + ExceededWaitTimeColorCode + "'>" + localizations['ProcessingInfo_WaitTimeExceeded'] + "</li>";
    data += "<hr />";
    data += "<li style='background-color:" + HoverColorCode + "'>" + localizations['ProcessingInfo_CurrentRow'] + "</li>";
    data += "<hr />";
    data += "<li>" + localizations['ProcessingInfo_Ordering'] + "</li>";
    data += "<hr />";
    data += "<li>" + localizations['EditingProcessingPage'] + "</li>";
    data += "</ul>";
    var popover = $('#InfoPopover');
    popover.attr('data-content', data);


}

function toggleDriverName(sender, license, licenseDriver) {   // license is only the license text and licenseDriver is license and the driver name
    if (sender === null || sender === undefined)
        return;
    if (sender.innerText === license)
        sender.innerText = licenseDriver;
    else
        sender.innerText = license;
}


function toggleFullDate(sender, longStr, shortStr) {
    if (sender === null || sender === undefined)
        return;
    if (sender.innerText === longStr)
        sender.innerText = shortStr;
    else
        sender.innerText = longStr;
}

function getLongDate(time) {
    if (time === null || time === undefined)
        return "";
    var date = time.getDate() < 10 ? "0" + time.getDate() : "" + time.getDate();
    var month = time.getMonth() + 1 < 10 ? "0" + (time.getMonth() + 1) : "" + (time.getMonth() + 1);
    var year = time.getFullYear();
    return "" + date + "." + month + "." + year + " " + getShortDate(time);
}

function getShortDate(time) {
    if (time === null || time === undefined)
        return "";
    var hours = time.getHours() < 10 ? "0" + time.getHours() : "" + time.getHours();
    var mins = time.getMinutes() < 10 ? "0" + time.getMinutes() : "" + time.getMinutes();
    return "" + hours + ":" + mins;
}

function highlightRow(id, color = "yellow") {
    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    var prevBackColor = row.style.backgroundColor;
    var maxWaitingTime = document.getElementById("maxWaitingTime").value;

    table.rows().every(function (rowIdx, tableLoop, rowLoop) {
        var data = this.data();
        if (data !== undefined && data !== null) {
            if (data.DT_RowId === "TR " + id) {
                var timeSinceRegistration = data[timeSinceRegistrationCol];
                var callSet = data[timeOfCallCol];

                if (timeSinceRegistration >= maxWaitingTime && !callSet.includes("toggleFullDate"))
                    prevBackColor = ExceededWaitTimeColorCode;
            }
        }
        this.data(data);
    });

    row.style.backgroundColor = color;
    setTimeout(function () {
        row.style.backgroundColor = prevBackColor;
        table.draw();
    }, 3000);
}

function highlightExceededWaitingTime() {
    var maxWaitingTime = document.getElementById("maxWaitingTime").value;
    var ExceededWaitTimeColorCode = document.getElementById("ExceededWaitTimeColorCode").value;

    table.rows().every(function (rowIdx, tableLoop, rowLoop) {
        var data = this.data();

        this.data(data);
    });
    table.draw();
}

function updateTimeSinceRegistration() {

    var maxWaitingTime = document.getElementById("maxWaitingTime").value;

    table.rows().every(function (rowIdx, tableLoop, rowLoop) {
        var data = this.data();

        if (data !== undefined && data !== null) {
            // Increase the time since registration by 1. Not 100% accurate but good enough. will be exact when reloaded from the server.
            data[timeSinceRegistrationCol] = data[timeSinceRegistrationCol];
            highlightExceededWaitingTime();
        }
        this.data(data);
    });
    table.draw();
}

function redrawTable() {
    table.rows().every(function (rowIdx, tableLoop, rowLoop) {
        var data = this.data();
        this.data(data);
    });
    table.draw();
}

function diff_minutes(dt2, dt1) {

    var diff = (dt2.getTime() - dt1.getTime()) / 1000;
    diff /= 60;
    return Math.abs(Math.round(diff));

}

// AddRegistration
connection.on("AddRegistration", function (model) {



    if (table === null || table === undefined)
        return;

    // vars
    var regist = model.registration;
    var loadingStations = model.gates;
    var gates = model.gates;
    var columns = new Array(17);
    var registTime = new Date(regist.timeOfRegistration);
    const now = new Date();

    const timeLapsed = diff_minutes(now, registTime);

    let optionSelectedCount = regist.loadCustomerPickup + regist.goodsReceiptCustomerEmpties + regist.goodsReceiptdelivery;
    if (processListFilter == "" || processListFilter == null) {
        return;
    }

    if (processListFilter == "versand" && regist.loadCustomerPickup == false && regist.loadEmptiesCollection == false) {
        return;
    }


    if (processListFilter == "anlieferung" && regist.goodsReceiptdelivery == false) {
        return;
    }

    if (processListFilter == "leergut" && regist.goodsReceiptCustomerEmpties == false) {
        return;
    }


    var id = regist.id;
    var colorCode = regist.colorCode;
    if (colorCode === null || colorCode === "" || colorCode === "#000000") {
        colorCode = "#f2f2f2";
    }

    var canModifyProcessingListElement = document.getElementById("CanModifyProcessingList");

    if (canModifyProcessingListElement !== null && canModifyProcessingListElement !== undefined) {
        canModifyProcessingList = true;
    }

    // time of registration
    var p1 = '<span class="unselectable" onclick="toggleFullDate(this,';
    var p2 = "\'" + getLongDate(registTime) + "\',";
    var p3 = "\'" + getShortDate(registTime) + "\')\">";
    var p4 = "" + getShortDate(registTime) + "</span>";
    columns[timeOfRegistrationCol] = p1 + p2 + p3 + p4;

    //0 forwarder forwarderCol

    columns[forwarderCol] = regist.forwarder;
    //1 customer customerCol

    columns[customerCol] = regist.customer;
    //2 Name nameCol


    columns[nameCol] = regist.firstName + "," + regist.lastname;
    //3 Mobilenummer   phonenumberCol

    columns[phonenumberCol] = regist.phonenumber;
    //4 Notes   commentCol

    columns[commentCol] = "<input id='Comment " + regist.id + "' value='" + regist.comment + "' onchange='commentChanged(" + id + ")' />";
    //5  Kennzeichen licensePlateCol

    columns[licensePlateCol] = regist.licensePlate;
    //6 Time of registration  shortTimeOfRegistCol

    columns[shortTimeOfRegistCol] = p1 + p2 + p3 + p4;
    //7 Time elapsed after registration minSinceRegistCol

    columns[minSinceRegistCol] = timeLapsed + " min";
    //8 Gate  gateCol


    columns[gateCol] = regist.gate;

    columns[gateCol] = "<input class=\"form-control\" onchange='setGate(" + id + ")'  id=\"Gate " + id + "\"  style=\"min-width:40px;\">";
    //9 Inactive since inactiveSinceCol

    columns[inactiveSinceCol] = "inactif";

    var buttonName = "";


    optionSelectedCount = regist.loadCustomerPickup + regist.goodsReceiptCustomerEmpties + regist.goodsReceiptdelivery;
    if (regist.statusCall == null) {
        buttonName = "call";
    }
    else if (regist.statusCall == 1 || regist.statusCall == 3 || regist.statusCall == 5) {
        buttonName = "confirm";
    }
    else if (regist.statusCall == 2 || regist.statusCall == 4 || regist.statusCall == 6) {
        buttonName = "close";
    }

    if (processListFilter == "leergut" && regist.goodsReceiptCustomerEmpties) {

    }
    if (processListFilter == "anlieferung" && regist.goodsReceiptdelivery) {
        if (regist.goodsReceiptCustomerEmpties) {
            buttonName = "";
        }

    }
    if (processListFilter == "versand" && regist.loadCustomerPickup) {
        if (optionSelectedCount > 1) {
            buttonName = "";
        }
    }






    switch (buttonName) {

        case "call":
            columns[commandCol] = "<button id='Call " + id + "' class='btn btn-success' onclick='setCall(" + id + "," + (regist.gate == null ? '' : regist.gate) + ")'>" + localizations['call_process'] + "</button>";
            break;

        case "confirm":
            columns[commandCol] = "<button id='Confirm " + id + "' class='btn btn-default' onclick='setConfirm(" + id + ")'>" + localizations['confirm_process'] + "</button>";
            break;

        case "close":
            columns[commandCol] = "<button id='Close " + id + "' class='btn btn-warning' onclick='setExit(" + id + ")'>" + localizations['close_prozess'] + "</button>";
            break;

        case "close4leergut":
            columns[commandCol] = "<button id='Call " + id + "' class='btn btn-success' onclick='setCloseForEmptyGoods(" + id + ")'>" + localizations['close_prozess'] + "</button>";
            break;
        default:
            columns[commandCol] = "";
            break;

    }




    // Add row to table
    var row = table.row.add(columns).node();
    row.id = 'TR ' + id;
    row.style.backgroundColor = colorCode;
    table.draw(false);

    // highlight the new row
    var prevBackColor = row.style.backgroundColor;
    row.style.backgroundColor = NewEntryColorCode;
    setTimeout(function (prevBack) {
        row.style.backgroundColor = prevBack;
    }, 3000, prevBackColor);

    // redraw to make sure the row is shown
    redrawTable();
});

// updateCustomer
function updateCustomer(id) {

    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    var customer = document.getElementById("Customer " + id).value;

    connection.invoke("UpdateCustomer", id, customer).catch(function (err) {

        alert("Fehler beim Setzen des Kunden. " + err);
        console.error(err.toString());

    });
}

connection.on("UpdateCustomer", function (id, name) {
    try {
        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {

                    var canModifyCustomerElement = document.getElementById("CanModifyCustomer");
                    var canModifyCustomer = false;
                    if (canModifyCustomerElement !== null && canModifyCustomerElement !== undefined) {
                        canModifyCustomer = true;
                    }
                    if (canModifyCustomer) {
                        data[customerCol] = "<input type='text' id='Customer " + id + "' onchange='updateCustomer(" + id + ")' value='" + name + "' />";
                    }
                    else {
                        data[customerCol] = "<input type='text' id='Customer " + id + "' onchange='updateCustomer(" + id + ")' value='" + name + "' disabled />";
                    }
                }
            }
            this.data(data);
        });
        table.draw();
        highlightRow(id, RecentChangeColorCode);
    }
    catch (err) {
        console.error(err.toString());
        alert(err.toString());
    }
});

//LoadingStation
function setLoadingStation(id) {
    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    var loadingStationSelect = document.getElementById("LoadingStation " + id);
    var selectedLoadingstation = loadingStationSelect.value;
    var gateSelect = document.getElementById("Gate " + id);
    var selectedGate = gateSelect.value;

    connection.invoke("SetLoadingStation", id, selectedLoadingstation, selectedGate).catch(function (err) {

        alert("Fehler beim Ändern der Ladestation. " + err);
        console.error(err.toString());

    });
}

connection.on("SetLoadingStation", function (id, selectedLoadingstation, selectedGate, loadingstations) {
    try {

        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    var canSetLoadingStationElement = document.getElementById("CanSetLoadingStation");
                    var canSetLoadingStation = false;
                    if (canSetLoadingStationElement !== null && canSetLoadingStationElement !== undefined) {
                        canSetLoadingStation = true;
                    }

                    if (canSetLoadingStation) {
                        var loadingstationsArray = loadingstations.split(",");

                        var loadingstationSelect = [];
                        loadingstationSelect.push("<select class=\"form-control\" id=\"LoadingStation " + id + "\" onchange=\"setLoadingStation(" + id + ")\" size=\"1\" style=\"min-width:70px;\">");
                        for (index = 0; index < loadingstationsArray.length; index++) {
                            if (loadingstationsArray[index] === selectedLoadingstation) {
                                loadingstationSelect.push("<option selected >" + loadingstationsArray[index] + "</option>")
                            }
                            else {
                                loadingstationSelect.push("<option>" + loadingstationsArray[index] + "</option>")
                            }
                        };
                        loadingstationSelect.push("</select>");
                        data[loadingStationCol] = loadingstationSelect.join("");
                    }
                    else {
                        data[loadingStationCol] = "<span id=\"LoadingStation " + id + "\" style=\"min-width:70px;\" >" + selectedLoadingstation + "</span>";
                    }

                }
            }
            this.data(data);
        });

        table.draw();

        connection.invoke("SetGateWithNewLoadingstation", id, selectedGate, selectedLoadingstation).catch(function (err) {

            alert("Fehler beim Ändern des Tores. " + err);
            console.error(err.toString());

        });

    }
    catch (err) {
        console.error(err.toString());
        alert(err.toString());
    }
});

//Gate

function setGate(id) {
    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    var gateSelect = document.getElementById("Gate " + id);

    let selectedLoadingstation = "";
    var gateVal = gateSelect.value;
    //innertext
    if (gateVal?.length > 0) {
        connection.invoke("SetGate", id, gateVal, selectedLoadingstation).catch(function (err) {

            alert("Fehler beim Ändern des Tores. " + err);


        });
    }

}

connection.on("SetGateWithNewLoadingstation", function (id, selectedGate, gates) {
    try {

        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();

            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    data[gateCol] = "<input class=\"form-control\" onchange=\"setGate(" + id + ")\"  id=\"Gate " + id + "\"  style=\"min-width:40px;\">";

                }
                this.data(data);
            }


            highlightRow(id, RecentChangeColorCode);



        });
        table.draw();
    }
    catch (err) {
        console.error(err?.toString());
        alert(err?.toString());
    }
});

connection.on("SetGate", function (id, selectedGate, gates) {
    try {

        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();

            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {

                    data[gateCol] = "<span id=\"Gate " + id + "\">" + selectedGate + "</span>";
                }
            }
            this.data(data);
        });
        table.draw();

        highlightRow(id, RecentChangeColorCode);
    }
    catch (err) {

    }
});


//CompanyName

function companyNameChanged(id) {
    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    var companyNameInput = document.getElementById("CompanyName " + id);
    var companyName = companyNameInput.value;

    connection.invoke("UpdateCompanyName", id, companyName).catch(function (err) {

        alert("Fehler beim Ändern des Firmennamens. " + err);
        console.error(err.toString());

    });
}

connection.on("UpdateCompanyName", function (id, companyName, colorCode) {
    try {

        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();

            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    var canSetCompanyElement = document.getElementById("CanModifyProcessingList");
                    var canSetCompanyName = false;
                    if (canSetCompanyElement !== null && canSetCompanyElement !== undefined) {
                        canSetCompanyName = true;
                    }
                    if (canSetCompanyName) {
                        data[companyNameCol] = "<td><input id=\"CompanyName " + id + "\" value=\"" + companyName + "\" onchange=\"companyNameChanged(" + id + ")\" /></td>";
                    }
                    else {
                        data[companyNameCol] = "<span>" + companyName + "</span>";
                    }

                    if (colorCode == null || colorCode == "" || colorCode == "#000000") {
                        colorCode = "#f2f2f2";
                    }


                    var row = document.getElementById("TR " + id);
                    if (row !== null || row !== undefined)
                        row.style.backgroundColor = colorCode;
                }
            }
            this.data(data);
        });
        table.draw();

        highlightRow(id, RecentChangeColorCode);
    }
    catch (err) {
        console.error(err.toString());
        alert(err.toString());
    }
});

//comment

function commentChanged(id) {
    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    var commentInput = document.getElementById("Comment " + id);
    var comment = commentInput.value;

    connection.invoke("UpdateComment", id, comment).catch(function (err) {

    });
}

connection.on("UpdateComment", function (id, comment) {
    try {

        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();

            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    var canSetCompanyElement = document.getElementById("CanModifyProcessingList");
                    var canSetcomment = false;
                    if (canSetCompanyElement !== null && canSetCompanyElement !== undefined) {
                        canSetcomment = true;
                    }
                    if (canSetcomment) {
                        data[commentCol] = "<td><input id=\"Comment " + id + "\" value=\"" + comment + "\" onchange=\"commentChanged(" + id + ")\" /></td>";
                    }
                    else {
                        data[commentCol] = "<span>" + comment + "</span>";
                    }
                }
            }
            this.data(data);
        });
        table.draw();

        highlightRow(id, RecentChangeColorCode);
    }
    catch (err) {
        if (err != undefined && err != null) {

        }
        if (err != undefined && err != null) {




        }
    }
});

//Target

function targetChanged(id) {
    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    var targetInput = document.getElementById("Target " + id);
    var target = targetInput.value;

    connection.invoke("UpdateLoadingReference", id, target).catch(function (err) {

        // redraw to make sure the original gate is shown again.
        if (table !== null && table !== undefined) {
            redrawTable();
        }


    });
}

connection.on("UpdateLoadingReference", function (id, target) {
    try {

        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();

            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    var canSetCompanyElement = document.getElementById("CanModifyProcessingList");
                    var canSetTarget = false;
                    if (canSetCompanyElement !== null && canSetCompanyElement !== undefined) {
                        canSetTarget = true;
                    }
                    if (canSetTarget) {
                        data[targetCol] = "<td><input id=\"Target " + id + "\" value=\"" + target + "\" onchange=\"targetChanged(" + id + ")\" /></td>";
                    }
                    else {
                        data[targetCol] = "<span>" + target + "</span>";
                    }
                }
            }
            this.data(data);
        });
        table.draw();

        highlightRow(id, RecentChangeColorCode);
    }
    catch (err) {
        console.error(err.toString());
        alert(err.toString());
    }
});

// update which gates are not occupied
connection.on("SetGateState", function (gate) {
    var selectOptions = document.getElementsByName(gate.name);
    for (var i = 0; i < selectOptions.length; i++) {
        if (!selectOptions[i].selected) {
            selectOptions[i].hidden = gate.isOccupied;
        }
    }
});

// Release
function setRelease(id) {
    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    connection.invoke("SetRelease", id).catch(function (err) {

        alert("Fehler beim Setzen der Freigabe. " + err);
        console.error(err.toString());

    });
}

connection.on("SetRelease", function (id, serverTime) {
    try {
        var time = new Date(serverTime);
        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    data[releaseCol] = "<span class='unselectable' onclick='toggleFullDate(this, \"" + getLongDate(time) + "\",\"" + getShortDate(time) + "\");'>" + getShortDate(time) + "</span>";

                    var canSetReleaseElement = document.getElementById("CanSetCall");
                    var canSetReleaseStart = false;
                    if (canSetReleaseElement !== null && canSetReleaseElement !== undefined) {
                        canSetReleaseStart = true;
                    }
                    if (canSetReleaseStart) {
                        data[timeOfCallCol] = "<button id='TimeOfStart " + id + "' class='btn btn-default' onclick='setCall(" + id + ")'>" + localizations['Call'] + "</button>";
                    }
                }
            }
            this.data(data);
        });
        table.draw();
        highlightRow(id, RecentChangeColorCode);
    }
    catch (err) {
        console.error(err.toString());
        alert(err.toString());
    }
});

// Call
function setCall(id, gate) {
    let comment = document.getElementById("Comment " + id).value;
    let gateSelect = document.getElementById("Gate " + id);
    let selectedGate = "";
    let row = document.getElementById("TR " + id);

    let error = document.getElementById("gate-error");
    error.style.display = "none";

    if (gateSelect != null) {
        selectedGate = gateSelect.value;
    } else {
        selectedGate = gate;
    }


    if (row === null || row === undefined)
        return;
    if (selectedGate != "" || (gate != "" && gate != undefined)) {
        connection.invoke("SetCall", id, selectedGate, comment).catch(function (err) {

            alert("Fehler beim Setzen der Freigabe. " + err);
            console.error(err.toString());

        });
    } else {
        error.style.display = "";
    }
}


function setCloseForEmptyGoods(id) {
    let comment = document.getElementById("Comment " + id).value;
    let selectedGate = "";
    let row = document.getElementById("TR " + id);

    let error = document.getElementById("gate-error");
    error.style.display = "none";



    if (row === null || row === undefined)
        return;

    connection.invoke("SetCloseForEmptyGoods", id, comment).catch(function (err) {

        alert("Fehler beim Setzen der Freigabe. " + err);
        console.error(err.toString());

    });

}




function setConfirm(id) {

    let comment = document.getElementById("Comment " + id).value;
    let row = document.getElementById("TR " + id);

    if (row === null || row === undefined)
        return;

    connection.invoke("SetConfirm", id, comment).catch(function (err) {

        alert("Fehler beim Setzen der Freigabe. " + err);
        console.error(err.toString());

    });
}

connection.on("SetCall", function (id, serverTime) {
    try {

        var time = new Date(serverTime);
        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    data[timeOfCallCol] = "<span class='unselectable' onclick='toggleFullDate(this, \"" + getLongDate(time) + "\",\"" + getShortDate(time) + "\");'>" + getShortDate(time) + "</span>";

                    var canSetCallElement = document.getElementById("CanSetCall");
                    var canSetCallStart = false;
                    if (canSetCallElement !== null && canSetCallElement !== undefined) {
                        canSetCallStart = true;
                    }
                    if (canSetCallStart) {
                        data[timeOfEntryCol] = "<button id='TimeOfEntry " + id + "' class='btn btn-default' onclick='setEntry(" + id + ")'>" + localizations['Entry'] + "</button>";
                    }
                }
            }
            this.data(data);
        });
        table.draw();
        highlightRow(id, RecentChangeColorCode);
    }
    catch (err) {
        console.error(err.toString());
        alert(err.toString());
    }
});


connection.on("SetCallWithModel", function (id, serverTime, regist) {
    try {
        var time = new Date(serverTime);
        var buttonName = "";
        var registTime = new Date(regist.timeOfRegistration);
        const now = new Date();

        const timeLapsed = diff_minutes(now, registTime);

        if (regist != null) {
            optionSelectedCount = regist.loadCustomerPickup + regist.goodsReceiptCustomerEmpties + regist.goodsReceiptdelivery;
            if (regist.statusCall == null) {
                buttonName = "call";
            }
            else if (regist.statusCall == 1 || regist.statusCall == 3 || regist.statusCall == 5) {
                buttonName = "confirm";
            }
            else if (regist.statusCall == 2 || regist.statusCall == 4 || regist.statusCall == 6) {
                buttonName = "close";
            }

        }




        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    data[timeOfCallCol] = "<span class='unselectable' onclick='toggleFullDate(this, \"" + getLongDate(time) + "\",\"" + getShortDate(time) + "\");'>" + getShortDate(time) + "</span>";

                    var canSetCallElement = document.getElementById("CanSetCall");
                    var canSetCallStart = false;
                    if (canSetCallElement !== null && canSetCallElement !== undefined) {
                        canSetCallStart = true;
                    }

                    if (buttonName == "") {

                        data[commandCol] = "";
                    }
                    if (buttonName == "call") {

                        data[commandCol] = "<button id='Call " + id + "' class='btn btn-success' onclick='setCall(" + id + "," + (regist.gate == null ? '' : regist.gate) + ")'>" + localizations['call_process'] + "</button>";
                    }
                    if (buttonName == "confirm") {

                        data[commandCol] = "<button id='Confirm " + id + "' class='btn btn-default' onclick='setConfirm(" + id + ")'>" + localizations['confirm_process'] + "</button>";
                    }
                    if (buttonName == "close") {

                        data[commandCol] = "<button id='Close " + id + "' class='btn btn-warning' onclick='setExit(" + id + ")'>" + localizations['close_prozess'] + "</button>";

                    }
                    if (buttonName == "close4leergut") {

                        data[commandCol] = "<button id='Call " + id + "' class='btn btn-success' onclick='setCloseForEmptyGoods(" + id + ")'>" + localizations['close_prozess'] + "</button>";
                    }


                    if (regist.gate != null) {
                        data[gateCol] = regist.gate;
                    }
                    data[minSinceRegistCol] = timeLapsed + " min";

                }
            }
            this.data(data);
        });
        table.draw();
        highlightRow(id, RecentChangeColorCode);
    }
    catch (err) {
        console.error(err.toString());
        alert(err.toString());
    }
});





// Entry
function setEntry(id) {
    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    connection.invoke("SetEntry", id).catch(function (err) {

        alert("Fehler beim Setzen der Einfahrt. " + err);
        console.error(err.toString());

    });
}

connection.on("SetEntry", function (id, serverTime) {
    try {
        var time = new Date(serverTime);
        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    data[timeOfEntryCol] = "<span class='unselectable' onclick='toggleFullDate(this, \"" + getLongDate(time) + "\",\"" + getShortDate(time) + "\");'>" + getShortDate(time) + "</span>";

                    var canSetProcessStartElement = document.getElementById("CanSetProcessStart");
                    var canSetProcessStart = false;
                    if (canSetProcessStartElement !== null && canSetProcessStartElement !== undefined) {
                        canSetProcessStart = true;
                    }
                    if (canSetProcessStart) {
                        data[processStartCol] = "<button id='TimeOfStart " + id + "' class='btn btn-default' onclick='setProcessStart(" + id + ")'>" + localizations['Start'] + "</button>";
                    }
                }
            }
            this.data(data);
        });
        table.draw();
        highlightRow(id, RecentChangeColorCode);
    }
    catch (err) {
        console.error(err.toString());
        alert(err.toString());
    }
});

// ProcessStart
function setProcessStart(id) {

    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    connection.invoke("SetProcessStart", id).catch(function (err) {

        alert("Fehler beim Setzen des Startzeitpunkts. " + err);
        console.error(err.toString());

    });
}

connection.on("SetProcessStart", function (id, serverTime) {
    try {
        var time = new Date(serverTime);
        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    data[processStartCol] = "<span class='unselectable' onclick='toggleFullDate(this, \"" + getLongDate(time) + "\",\"" + getShortDate(time) + "\");'>" + getShortDate(time) + "</span>";

                    var canSetProcessEndElement = document.getElementById("CanSetProcessEnd");
                    var canSetProcessEnd = false;
                    if (canSetProcessEndElement !== null && canSetProcessEndElement !== undefined) {
                        canSetProcessEnd = true;
                    }
                    if (canSetProcessEnd) {
                        data[processEndCol] = "<button id='TimeOfEnd " + id + "' class='btn btn-default' onclick='setProcessEnd(" + id + ")'>" + localizations['End'] + "</button>";
                    }
                }
            }
            this.data(data);
        });
        table.draw();
        highlightRow(id, RecentChangeColorCode);
    }
    catch (err) {
        console.error(err.toString());
        alert(err.toString());
    }
});

// ProcessEnd
function setProcessEnd(id) {

    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    connection.invoke("SetProcessEnd", id).catch(function (err) {

        alert("Fehler beim Setzen der Einfahrt. " + err);
        console.error(err.toString());

    });
}

connection.on("SetProcessEnd", function (id, serverTime) {
    try {
        var time = new Date(serverTime);
        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    data[processEndCol] = "<span class='unselectable' onclick='toggleFullDate(this, \"" + getLongDate(time) + "\",\"" + getShortDate(time) + "\");'>" + getShortDate(time) + "</span>";
                }
            }
            this.data(data);
        });
        table.draw();
        highlightRow(id, RecentChangeColorCode);
    }
    catch (err) {
        console.error(err.toString());
        alert(err.toString());
    }
});

//Exit
function setExit(id) {
    let comment = document.getElementById("Comment " + id).value;
    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    connection.invoke("SetExit", id, comment).catch(function (err) {

        alert("Fehler beim Setzen der Ausfahrt. " + err);
        console.error(err.toString());

    });
}

connection.on("SetExit", function (id, serverTime) {
    try {
        var time = new Date(serverTime);
        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    data[timeOfCallCol] = "<span class='unselectable' onclick='toggleFullDate(this, \"" + getLongDate(time) + "\",\"" + getShortDate(time) + "\");'>" + getShortDate(time) + "</span>";
                }
            }
            this.data(data);
        });
        table.draw();
        highlightRow(id, ExitColorCode);

        setTimeout(function () {
            table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                var data = this.data();
                if (data !== undefined && data !== null) {
                    if (data.DT_RowId === "TR " + id) {
                        this.remove();
                    }
                    else {
                        this.data(data);
                    }
                }
            });
            table.draw();
        }, 3000);
    }
    catch (err) {

    }
});





connection.on("EntryLicenseUnknown", function (licensePlate, time) {

    var canSetEntranceElement = document.getElementById("CanSetEntrance");
    var canSetEntrance = false;
    if (canSetEntranceElement !== null && canSetEntranceElement !== undefined) {
        canSetEntrance = true;
    }
    if (!canSetEntrance)
        return;

    var warningElement = document.getElementById("Warnings");
    warningElement.hidden = false;

    var popover = $('#Warnings');

    var data = "";
    data += '<div class="form-group" style="padding:5px;">';
    data += '<textarea class="form-control" rows="4" cols="50" readonly>' + localizations['Entry'] + ' ' + (new Date(time)).toLocaleTimeString() + ': ' + localizations['NoRegistrationWithLicense'] + licensePlate + ' ' + localizations['SetEntryManually'] + '</textarea > ';
    data += '<button onclick="hideWarning(this)" class="btn btn-primary" style="float:right;">' + localizations['Close'] + '</button>';
    data += '</div>';

    data += popover.attr('data-content');

    popover.attr('data-content', data);
    popover.popover('show');

});


connection.on("ExitLicenseUnknown", function (licensePlate, time) {

    var canSetExitElement = document.getElementById("CanSetExit");
    var canSetExit = false;
    if (canSetExitElement !== null && canSetExitElement !== undefined) {
        canSetExit = true;
    }
    if (!canSetExit)
        return;

    var warningElement = document.getElementById("Warnings");
    warningElement.hidden = false;

    var popover = $('#Warnings');

    var data = "";
    data += '<div class="form-group" style="padding:5px;">';
    data += '<textarea class="form-control" rows="4" cols="50" readonly>' + localizations['Exit'] + ' ' + (new Date(time)).toLocaleTimeString() + ': ' + localizations['NoRegistrationWithLicense'] + licensePlate + ' ' + localizations['SetExitManually'] + '</textarea > ';
    data += '<button onclick="hideWarning(this)" class="btn btn-primary" style="float:right;">' + localizations['Close'] + '</button > ';
    data += '</div>';

    data += popover.attr('data-content');

    popover.attr('data-content', data);
    popover.popover('show');

});

function hideWarning(button) {

    var div = button.parentElement;
    var dataContent = div.parentElement;

    dataContent.removeChild(div);

    var popover = $('#Warnings');
    var innerHtml = dataContent.innerHTML;
    popover.attr('data-content', innerHtml);

    if (dataContent.children.length <= 0) {
        var warningElement = document.getElementById("Warnings");

        warningElement.hidden = true;
        popover.popover('hide');
    }
}