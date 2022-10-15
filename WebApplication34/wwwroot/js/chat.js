var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.start().then(function () {
    console.log("ss");
}).catch(function (err) {
    return console.error(err);
});

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
 
    li.textContent = `${user} says ${message}`;
});

connection.on("Connected", function (conId) {
    console.log(conId+" Connectid");
})

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err);
    });
    event.preventDefault();
});

$(document).ready(function(){

    connection.on("UserJoin", function (userId,username) {
        $('li[data-id=' + userId + ']').css("color", "green");
        $('li[data-id=' + userId + ']').text(username);
    })
    connection.on("UserClose", function (userId,username,date) {
        $('li[data-id=' + userId + ']').css("color", "gray");
        $('li[data-id=' + userId + ']').text(username + "-" + date);

    })
})
