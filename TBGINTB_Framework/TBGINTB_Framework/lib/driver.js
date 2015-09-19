(function (namespace, undefined) {
    "use strict";

    $(document).ready(function init() {
        var now = new Date();
    
        var messengerEngine = namespace.Engines.MessengerEngine;
        messengerEngine.init();
        var servicesEngine = namespace.Engines.ServicesEngine;
        servicesEngine.init(messengerEngine);
        var timeEngine = namespace.Engines.TimeEngine;
        timeEngine.init(messengerEngine);
        var gameStateEngine = namespace.Engines.GameStateEngine;
        gameStateEngine.init(messengerEngine);
        var userInputManager = namespace.Managers.UserInputManager;
        userInputManager.init("#verbList", messengerEngine);
        var interfaceManager = namespace.Managers.InterfaceManager;
        interfaceManager.init("#locations", "#paragraphs", "#time", messengerEngine);
        var messageManager = namespace.Managers.MessageManager;
        messageManager.init("#messageTop", "#messageBottom", "#messageText", "#messageChoices", messengerEngine);
        servicesEngine.getAllVerbTypes();
        servicesEngine.loadGame(sessionStorage.playerId);

        $("#master-container").mousedown(function (e) {
            interfaceManager.changeUpdateInterval();
        });

        /*
        function suppressBackspace(event) { // we don't want the delete key to act like the browser back button
            event = event || window.event;
            var target = event.target || event.srcElement;
            if (event.keyCode == 8 && !/input|textarea/i.test(target.nodeName)) {
                updateUserInput(event);
                return false;
            }
        };
        document.onkeydown = suppressBackspace;
        document.onkeypress = suppressBackspace;
        */ 
    });
}(window.GinTub = window.GinTub || {}));