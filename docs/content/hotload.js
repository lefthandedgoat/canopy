var refreshSocket = new WebSocket('ws://' + window.location.host)
    .onmessage = () => {
        location.reload();
    }
