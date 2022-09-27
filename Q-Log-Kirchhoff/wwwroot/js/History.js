var connection = new signalR.HubConnectionBuilder().withUrl("/HistoryHub").build();

connection.onclose(function () {
    setTimeout(function () {
        connection.start({ pingInterval: 6000 });
    }, 5000); // Restart connection after 5 seconds.
});

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

var HoverColorCode = "";

// RegistrationHistoryTable vars
var registrationTable = null;
var oldRegistrationData = null;

const idCol = 0;
const companyNameCol = 1;
const supplierNumberCol = 2;
const numberOfPeopleCol = 3;
const isSmallVehicleCol = 4;
const isBigVehicleCol = 5;
 
const approachTypCol = 8;
const targetCol = 9;
const loadingStationCol = 10;
 

const releaseCol = 13;
const callCol = 14;
const entryCol = 15;
const startCol = 16;
const endCol = 17;
const exitCol = 18;
const sendToERPCol = 19;


const licensePlateCol = 0;
const nameCol = 1;
const forwarderCol = 2;
const customerCol = 3;
const zielCol = 4;
const gateCol = 5;
const registrationCol = 6;
const phonenumberCol = 3;
const commentCol = 4;
 
const shortTimeOfRegistCol = 6;
const minSinceRegistCol = 7;
 
const inactiveSinceCol = 9;
const commandCol = 10;
const timeOfExitCol = 10;



$(document).ready(function () {
    $.fn.dataTable.moment('DD.MM.YYYY');
    $.fn.dataTable.moment("DD.MM.YYYY HH:mm:ss");
    $.fn.dataTable.moment("HH:mm:ss");
    registrationTable = $('#RegistrationHistoryTable').DataTable({
        "paging": true,
        "searching": true,
        "stateSave": false,
        "aoColumnDefs": [
            { 'bSortable': false, 'aTargets': [] }
        ],
        "order": [[registrationCol, "desc"]],
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
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, localizations['All']]],
        "dom": '<"top"f>rt<"bottom"lp><"clear">'
    });

    HoverColorCode = document.getElementById("HoverColorCode").value;

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
});

function setupInfoPopOver() {
    $('[data-toggle="popover"]').popover();
    var data = "<ul>";
    data += "<li>" + localizations['HistoryInfo_Timespan'] + "</li>";
    data += "<hr />";
    data += "<li style='background-color:" + HoverColorCode + "'>" + localizations['ProcessingInfo_CurrentRow'] + "</li>";
    data += "<hr />";
    data += "<li>" + localizations['HistoryInfo_ExportSelection'] + "</li>";
    data += "<li>" + localizations['HistoryInfo_ExportAll'] + "</li>";
    data += "<hr />";
    data += "<li>" + localizations['HistoryInfo_Filter'] + "</li>";
    data += "<hr />";
    data += "<li>" + localizations['HistoryInfo_HideColumns'] + "</li>";
    data += "<hr />";
    data += "<li>" + localizations['ProcessingInfo_Ordering'] + "</li>";
    data += "</ul>";
    var popover = $('#InfoPopover');
    popover.attr('data-content', data);
}

function toggleRegistrationCol(index, sender) {
    var table = document.getElementById("RegistrationHistoryTable");
    var ths = table.getElementsByTagName("th");
    var display = sender.checked === true ? "" : "none";
    ths[index].style.display = display;

    var rows = table.getElementsByTagName("tr");
    for (var i = 0; i < rows.length; i++) {
        var tds = rows[i].getElementsByTagName("td");
        if (tds.length <= index)
            continue;

        tds[index].style.display = display;
    }
}

function setColumnVisibility() {
    var cols = [localizations["Vehicle_number"],
        localizations["Name"],
        localizations["Forwarder"],
        localizations["Customer"],
        localizations["Destination"],
        localizations["Gate"],
    localizations["LicensePlate"],
        localizations["Register"],
        localizations["CallAction"],
        localizations["DisplayAction"],
        localizations["CloseAction"]];
    for (var i = 0; i < cols.length; i++) {
        var cb = document.getElementById("ColumnVisibility " + i);
        toggleRegistrationCol(i, cb);
    }
}

function exportRegistrationSelection() {
    if (registrationTable === null || registrationTable === undefined)
        return;
    var data = registrationTable.rows().data();

    var csvContent = "data: text/csv;charset=utf-8,";
    //\"Name\";
    var keys = "\"ID\";\"Firmenname\";\"Lieferantennummer\";\"Anzahl Personen\";\"x < 7.5t\";\"x > 7.5t\";\"Kennzeichen\";\"Kommentar\";\"Gruppe\";\"Lieferscheinnummer/Ziel/AP\";\"Ladestelle\";\"Tor\";\"Anmeldung\";\"Freigabe\";\"Aufruf\";\"Einfahrt\";\"Start\";\"Ende\";\"Ausfahrt\"";

    csvContent += keys;

    for (var i = 0; i < data.length; i++) {
        var row = data[i];
        var rowCsv = "\n";

        for (var j = 0; j <= row.length - 1; j++) {
            if (j === isSmallVehicleCol || j === isBigVehicleCol) {
                var val = row[j].includes("glyphicon-ok") ? "1" : "0";
                rowCsv += "\"" + val + "\";";
            }
            else if (j === approachTypCol) {
                if (row[j].includes("Spedition") || row[j].includes("Forwarder")){
                    rowCsv += "\"" + "Spedition" + "\";";
                } else if (row[j].includes("Lieferant") || row[j].includes("Supplier")){
                    rowCsv += "\"" + "Lieferant" + "\";";
                } else if (row[j].includes("Paketdienst") || row[j].includes("Parcelservice")){
                    rowCsv += "\"" + "Paketdienst" + "\";";
                } else {
                    rowCsv += "\"" + "Monteur" + "\";";
                }

            }
            else if (j === sendToERPCol) {
                continue;
            }
            else {
                rowCsv += "\"" + row[j] + "\";";
            }

        }
        //if (row.length - 1 >= 0) {
        //    rowCsv += "\"" + row[j] + "\"";
        //}

        csvContent += rowCsv;
    }

    var encodedUri = encodeURI(csvContent);
    var link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", "AuswahlAnmelde-HistorieExport " + new Date().toLocaleString() + ".csv");
    document.body.appendChild(link); // Required for FormFile

    link.click(); // This will download the data file named ""AuswahlAnmelde-HistorieExport_.csv".
}

function filterRegistrationTable() {
    if (registrationTable === null || registrationTable === undefined)
        return;
    if (oldRegistrationData === null || oldRegistrationData === undefined || oldRegistrationData.length === 0)
        oldRegistrationData = registrationTable.rows().data();

    var newData = new Array();

    //var id = document.getElementById("IDInput").value;
    //var companyName = document.getElementById("CompanyNameInput").value;
    //var supplierNumber = document.getElementById("SupplierNumberInput").value;
    //var numberOfPeople = document.getElementById("NumberOfPeopleInput").value;
    //var licensePlate = document.getElementById("LicensePlateInput").value;
    ////var phone = document.getElementById("PhoneInput").value;
    //var isSmallVehicle = document.getElementById("IsSmallVehicleInput").value;
    //var approachTyp = document.getElementById("ApproachTypSelect").value;
    //var loadingStation = document.getElementById("LoadingStationInput").value;
    //var gate = document.getElementById("GateInput").value;
    //var registMin = document.getElementById("RegistrationMin").value;
    //var registMax = document.getElementById("RegistrationMax").value;
    //var releaseMin = document.getElementById("ReleaseMin").value;
    //var releaseMax = document.getElementById("ReleaseMax").value;
    //var callMin = document.getElementById("CallMin").value;
    //var callMax = document.getElementById("CallMax").value;
    //var entryMin = document.getElementById("EntryMin").value;
    //var entryMax = document.getElementById("EntryMax").value;
    //var startMin = document.getElementById("StartMin").value;
    //var startMax = document.getElementById("StartMax").value;
    //var endMin = document.getElementById("EndMin").value;
    //var endMax = document.getElementById("EndMax").value;
    //var exitMin = document.getElementById("ExitMin").value;
    //var exitMax = document.getElementById("ExitMax").value;
    //var sendToERP = document.getElementById("SendToERPInput").value;


    var vehiculeNo = document.getElementById("LicensePlateInput").value;
    var lastname = document.getElementById("LastnameInput").value;
    var firstname = document.getElementById("FirstNameInput").value;
    var pohnenumber = document.getElementById("Phone_numberInput").value;
    var forwarder = document.getElementById("ForwarderInput").value;
    var customer = document.getElementById("CustomerInputs").value;
 


    for (var i = 0; i < oldRegistrationData.length; i++) {
        if (oldRegistrationData[i] !== null && oldRegistrationData[i] !== undefined) {
            var addToNewData = true;
            var row = oldRegistrationData[i];

          

            if (vehiculeNo !== null && vehiculeNo !== "") {
                if (!row[licensePlateCol].toUpperCase().includes(vehiculeNo.toUpperCase())) {
                    addToNewData = false;
                }
            }

            if (lastname !== null && lastname !== "") {
                if (!row[nameCol].toUpperCase().includes(lastname.toUpperCase())) {
                    addToNewData = false;
                }
            }


            if (firstname !== null && firstname !== "") {
                if (!row[nameCol].toUpperCase().includes(firstname.toUpperCase())) {
                    addToNewData = false;
                }
            }

 

            if (forwarder !== null && forwarder !== "") {
                if (!row[forwarderCol].toUpperCase().includes(forwarder.toUpperCase())) {
                    addToNewData = false;
                }
            }


            if (customer !== null && customer !== "") {
                if (!row[customerCol].toUpperCase().includes(customer.toUpperCase())) {
                    addToNewData = false;
                }
            }

            //if (pohnenumber !== null && pohnenumber !== "") {
            //    if (!row[phonenumberCol].toUpperCase().includes(pohnenumber.toUpperCase())) {
            //        addToNewData = false;
            //    }
            //}
                        

            if (addToNewData) {
                newData.push(oldRegistrationData[i]);
            }
        }
    }
    registrationTable.clear();
    registrationTable.rows.add(newData);
    registrationTable.draw(true);
    setColumnVisibility();
}

function resetRegistrationTable() {
    if (oldRegistrationData === null || oldRegistrationData === undefined || oldRegistrationData.length === 0 || registrationTable === null || registrationTable === undefined) {
        location.reload(true);
    }
    registrationTable.clear();
    registrationTable.rows.add(oldRegistrationData);
    registrationTable.draw(true);
    setColumnVisibility();
}

function getShortDate(time) {
    if (time === null || time === undefined)
        return "";
    var hours = time.getHours() < 10 ? "0" + time.getHours() : "" + time.getHours();
    var mins = time.getMinutes() < 10 ? "0" + time.getMinutes() : "" + time.getMinutes();
    var secs = time.getSeconds() < 10 ? "0" + time.getSeconds() : "" + time.getSeconds();
    return "" + hours + ":" + mins + ":" + secs;
}

function getLongDate(time) {
    if (time === null || time === undefined)
        return "";
    return "" + time.getDate() + "." + (time.getMonth() + 1) + "." + time.getFullYear() + " " + getShortDate(time);
}

function sendToERP(id) {
    var row = document.getElementById("TR " + id);
    if (row === null || row === undefined)
        return;

    connection.invoke("ResendToERP", id).catch(function (err) {
        if (err.toString().includes("unauthorized")) {
            alert(localizations['Unauthorized']);
        }
        else {
            alert("Fehler beim erneuten senden zum ERP. " + err);
            console.error(err.toString());
        }
    });
}

connection.on("UpdateSendToERP", function (id) {
    try {

        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();

            if (data !== undefined && data !== null) {
                if (data.DT_RowId === "TR " + id) {
                    data[sendToERPCol] = "<button disabled class=\"btn btn-success\">@localizations[\"SendedToERP\"]</button>";
                                            
                }
            }
            this.data(data);
        });
        table.draw();
    }
    catch (err) {
        console.error(err.toString());
        alert(err.toString());
    }
});
