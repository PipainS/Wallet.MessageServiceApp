class WebSocketClient {
    constructor(url) {
        this.url = url;
        this.socket = null;
    }

    connect() {
        this.socket = new WebSocket(this.url);

        this.socket.onopen = () => {
            console.log("Connected to WebSocket server");
        };

        this.socket.onmessage = (event) => {
            this.handleMessage(event.data);
        };

        this.socket.onclose = () => {
            console.log("Disconnected from WebSocket server");
        };

        this.socket.onerror = (error) => {
            console.error("WebSocket error:", error);
        };
    }

    handleMessage(message) {
        try {
            const data = JSON.parse(message);

            const userName = data.userName ? data.userName : "Аноним";
            const text = data.text ? data.text : "";

            this.displayMessage(userName, text);
        } catch (e) {
            console.error("Failed to parse message:", e);
        }
    }

    displayMessage(userName, text) {
        const messagesDiv = document.getElementById("messages");

        const messageElement = document.createElement("p");
        messageElement.textContent = `${userName}: ${text}`;

        messagesDiv.appendChild(messageElement);
    }

    sendMessage(message) {
        if (this.socket && this.socket.readyState === WebSocket.OPEN) {
            this.socket.send(message);
        } else {
            console.error("WebSocket is not connected");
        }
    }

    disconnect() {
        if (this.socket) {
            this.socket.close();
        }
    }
}
