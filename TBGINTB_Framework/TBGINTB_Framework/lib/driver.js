
$(document).ready(function init() { // on initialization ...
    var now = new Date();
    
    var servicesEngine = new ServicesEngine();
    var timeEngine = new TimeEngine();
    var gameStateEngine = new GameStateEngine();
    var userInputManager = new UserInputManager("#verbList");
    var interfaceManager = new InterfaceManager("#location", "#paragraphs", "#time");
    var messageManager = new MessageManager("#messageTop", "#messageBottom", "#messageText", "#messageChoices");
    servicesEngine.loadAllVerbTypes();
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