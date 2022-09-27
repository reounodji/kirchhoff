
window.onload += startTimer();

var maxTime;


function backToIndex() {
    document.getElementById('BackToIndex').click();
}

function resetTime() {
    var idleTimeElement = document.getElementById("IdleTimer");
    if (idleTimeElement === null || idleTimeElement === undefined)
        return;
    idleTimeElement.innerText = maxTime.toString();
    document.body.style.backgroundColor = "white";
}

function startTimer() {
    document.body.addEventListener('click', resetTime, true);
    document.body.addEventListener('keydown', resetTime, true);

    maxTime = document.getElementById("IdleTimer").value;
    if (maxTime === null || maxTime === undefined)
        return;
    var idleTimeElement = document.getElementById("IdleTimer");
    if (idleTimeElement === null || idleTimeElement === undefined)
        return;
    idleTimeElement.innerText = maxTime.toString();

    setInterval(function () {

        var curTime = parseInt(parseInt(idleTimeElement.innerText) - 1);
        idleTimeElement.innerText = curTime.toString();
        if (curTime <= 0) {
            idleTimeElement.innerText = "100";
            backToIndex();
        }
        else if (curTime <= 10) {
            if (curTime % 2 === 1)
                document.body.style.backgroundColor = "red";
            else
                document.body.style.backgroundColor = "white";
        }
    }, 1000);
}
