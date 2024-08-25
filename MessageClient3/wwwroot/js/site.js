document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("getMessagesButton").addEventListener("click", async function () {
        try {
            let response = await fetch("/Messages/GetMessages");
            if (response.ok) {
                let messages = await response.json();
                let messagesContainer = document.getElementById("messagesContainer");
                messagesContainer.innerHTML = ''; // Очистите контейнер перед добавлением новых сообщений

                // Преобразуйте JSON в объекты и отобразите их
                let parsedMessages = JSON.parse(messages);
                parsedMessages.forEach(message => {
                    let messageElement = document.createElement("div");
                    messageElement.textContent = `${message.timestamp} ${message.userName}: ${message.text}`;
                    messagesContainer.appendChild(messageElement);
                });
            } else {
                console.error("Ошибка при получении сообщений");
            }
        } catch (error) {
            console.error("Ошибка:", error);
        }
    });
});
