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
        var audioEngine = namespace.Engines.AudioEngine;
        audioEngine.init("#audio", "#volumeButton", "#volumeRange", messengerEngine);

        if (namespace.EX && namespace.EX.Cheats) { // intentional truthiness
            var cheats = namespace.EX.Cheats;
            cheats.init("#master-container", messengerEngine);
        }

        var userInputManager = namespace.Managers.UserInputManager;
        userInputManager.init(new namespace.Entities.Classes.PopUpListConstructorObject("#verbList", "#interfaceBottom", "VerbList"),
            new namespace.Entities.Classes.PopUpListConstructorObject("#withList", "#interfaceBottom", "WithList"),
            new namespace.Entities.Classes.PopUpListConstructorObject("#clockList", "#interfaceTop", "ClockList"),
            messengerEngine);
        var interfaceManager = namespace.Managers.InterfaceManager;
        interfaceManager.init("#locations", "#paragraphs", "#clock", "#master-container", messengerEngine);
        var menuManager = namespace.Managers.MenuManager;
        menuManager.init("#menu", "#menuButton", "#menuEntries", "#menuFrame", "#displayFrame", "#descriptionFrame", messengerEngine);
        var messageManager = namespace.Managers.MessageManager;
        messageManager.init("#messageTop", "#messageBottom", "#messageText", "#messageChoices", "#master-container", messengerEngine);
        servicesEngine.getAllVerbTypes();
        servicesEngine.loadGame(sessionStorage.playerId);

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