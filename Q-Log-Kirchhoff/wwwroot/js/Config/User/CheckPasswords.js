function checkPasswords() {
    try
    {
        var pw1 = document.getElementById("pw1").value;
        var pw2 = document.getElementById("pw2").value;
        if (pw1 !== pw2) {
            event.preventDefault();
            var ul = document.getElementById("ErrorUL");
            var errorLi = document.getElementById("notSamePWError");
            if (errorLi !== null && errorLi !== undefined)
                return;
            var li = document.createElement("li");
            li.id = "notSamePWError";
            li.innerText = "Die eingetragenen Passwörter stimmen nicht überein!";
            ul.appendChild(li);
        }  
    }
    catch (err) {
        alert("Fehler beim überprüfen der Passwörter. Fehler: " + err);
    }
}