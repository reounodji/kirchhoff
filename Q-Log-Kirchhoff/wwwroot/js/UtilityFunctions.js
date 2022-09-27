function disableButton(id) {
    var btn = document.getElementById(id);
    if (btn !== null && btn !== undefined) {
        btn.innerText = "\u27F3 ...";
        btn.disabled = true;
    }

}