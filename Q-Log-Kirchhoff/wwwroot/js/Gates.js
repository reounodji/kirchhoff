// SignalR
var connection = new signalR.HubConnectionBuilder().withUrl("/GatesHub").build();

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