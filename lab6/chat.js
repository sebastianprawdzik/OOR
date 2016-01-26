window.onload = function () {

    var textView = document.getElementById("text-view");
    var buttonSend = document.getElementById("send-button");
    var buttonStop = document.getElementById("stop-button");
    var label = document.getElementById("status-label");

    // Połaczenie do  WebSocket server!
    var socket = new WebSocket("ws://echo.websocket.org");

    socket.onopen = function (event) {
        label.innerHTML = "Start połączenia";
    }


    // WebSocket funkcja onmessage.

    socket.onmessage = function (event) {
        if (typeof event.data === "string") {
            // Wiadomosc na ekranie.
            label.innerHTML = label.innerHTML + "<br />Odp.Serwera: <strong>" + event.data + "</strong>";
        }
    }


    // WebSocket onclose event.

    socket.onclose = function (event) {
        var code = event.code;
        var reason = event.reason;
        var wasClean = event.wasClean;

        if (wasClean) {
            label.innerHTML = "Poloczenie zakończone.";
        }
        else {
            label.innerHTML = "Polaczenie zakończone z wiadomoscia: " + reason + " (Code: " + code + ")";
        }
    }


    // WebSocket funkcja onerror funkcja.

    socket.onerror = function (event) {
        label.innerHTML = "Error: " + event;
    }


    // Rozłaczenie i zamykanie połaczenia.

    buttonStop.onclick = function (event) {
        if (socket.readyState == WebSocket.OPEN) {
            socket.close();
        }
    }


    // Wysyłanie pustej wiadomosci.

    buttonSend.onclick = function (event) {
        sendMessage();
    }


    // Wysyłanie pustej wiadomosci.

    textView.onkeypress = function (event) {
        if (event.keyCode == 13) {
            sendMessage();
        }
    }

    function sendMessage() {
        if (socket.readyState == WebSocket.OPEN) {
            socket.send(textView.value);

            label.innerHTML = label.innerHTML + "<br />Wysłane do serwera: <strong>" + textView.value + "</strong>";
            textView.value = "";
        }
    }
}
