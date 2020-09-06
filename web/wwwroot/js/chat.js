"use strict";

var connection = new signalR.HubConnectionBuilder()
                    .withUrl("/chatHub")
                    .withAutomaticReconnect()
                    .build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    var itm = document.getElementById("templates").querySelector('.receivedMsg');
    var cln = itm.cloneNode(true);
    cln.querySelector('.badge').textContent = user;
    cln.querySelector('.card').textContent = msg;
    document.getElementById("messagesList").appendChild(cln);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").textContent;
    var group = document.getElementById("groupInput").value;
    var message = document.getElementById("messageInput").value;
    
    document.getElementById("messageInput").value = '';
    

    addSentMessageToChat(user, message);
    connection.invoke("SendMessage", group, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("addGroup").addEventListener("click", function (event) {
    var group = document.getElementById("groupInput").value;

    connection.invoke("Group", group).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

// add sent message to chat
function addSentMessageToChat(user, message) {
    
    var itm = document.getElementById("templates").querySelector('.sentMsg');
    var cln = itm.cloneNode(true);
    cln.querySelector('.badge').textContent = user;
    cln.querySelector('.card').textContent = message;
    document.getElementById("messagesList").appendChild(cln);
}