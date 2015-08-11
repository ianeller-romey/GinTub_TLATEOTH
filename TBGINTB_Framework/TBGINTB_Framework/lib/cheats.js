$(document).ready(function init() { // on initialization ...
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

    var acceptInput = false;
    var cheatsElemHeight = "150px";
    var cheatsElem = $("<div/>", {
        class: "transition150",
        css: {
            position: "absolute", 
            top: 0, 
            left: 0, 
            height: "0px", 
            width: "100%",
            background: "black",
            color: "white",
        }
    });
    $("#master-container").after(cheatsElem);

    var UserInputManager = function () {
    };

    UserInputManager.prototype.resetUserInput = function () {
        return this.addCharToUserInput("", "");
    };

    UserInputManager.prototype.addCharToUserInput = function (add, str) {
        return str.substring(0, str.length - 1) + add + '_';
    };

    UserInputManager.prototype.deleteCharFromUserInput = function (str) {
        return str.substring(0, str.length - 2) + '_';
    };

    var deleteKey = 8;
    var enterKey = 13;
    var userInputManager = new UserInputManager();
    cheatsElem.text(userInputManager.resetUserInput());
    function updateUserInput(event) {
        if (event.keyCode == deleteKey) { // delete characters if the delete key is pressed
            cheatsElem.text(userInputManager.deleteCharFromUserInput(cheatsElem.text()));
        }
        else if (event.keyCode == enterKey) { // parse characters if the enter key is pressed
            var str = cheatsElem.text().substring(0, cheatsElem.text().length - 1);
            /*if (engine.parse(str) === true) {
                cheatsElem.text(userInputManager.resetUserInput());
            }*/
            // TODO
            cheatsElem.text(userInputManager.resetUserInput());
        }
        else { // otherwise, add characters
            cheatsElem.text(userInputManager.addCharToUserInput(String.fromCharCode(event.keyCode), cheatsElem.text()));
        }
    };
    window.addEventListener("keypress", function (event) {
        if (event.keyCode == 96) { // tilde
            if (cheatsElem.css("height") != cheatsElemHeight) {
                cheatsElem.css("height", cheatsElemHeight).css("padding", "8px");
                acceptInput = true;
            }
            else {
                cheatsElem.css("height", "0px").css("padding", "0px");
                acceptInput = false;
            }
        }
        else {
            if (acceptInput) {
                updateUserInput(event);
            }
        }
    }, true);
});