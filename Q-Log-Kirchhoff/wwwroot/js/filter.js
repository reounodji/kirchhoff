
function resetFilter(searchInputID) {
    document.getElementById(searchInputID).value = "";
    filter(searchInputID);
}

function filter(searchInputID, tableBodyID) {
    var searchString = document.getElementById(searchInputID).value;
    searchString = searchString.toUpperCase();
    var tbody = document.getElementById(tableBodyID);
    var trList = tbody.getElementsByTagName("tr");

    for (var i = 0; i < trList.length; i++)
    {
        checkRow(trList[i], searchString);
    }
}

function checkRow(tr, searchString) {
    var hasSubstring = false;
    if (searchString === "") {
        hasSubstring = true;
    }
    else {
        var tdList = tr.getElementsByTagName("td");
        for (var i = 0; i < tdList.length; i++) {
            if (checkTd(tdList[i], searchString))
                hasSubstring = true;
        }
    }
    if (hasSubstring)
        tr.style = "";
    else
        tr.style = "display:none;";
}

function checkTd(td, searchString) {
    if (td.textContent.toUpperCase().includes(searchString))
        return true;
    return false;
}