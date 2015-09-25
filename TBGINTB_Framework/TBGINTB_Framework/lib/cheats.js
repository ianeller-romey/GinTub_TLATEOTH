(function (namespace, undefined) {
    "use strict";
    
    namespace.EX = namespace.EX || {};
    namespace.EX.Cheats = {
        init: function (masterContainerId, messengerEngine) {
            var acceptInput = false;
            var cheatsElemHeight = "150px";
            var cheatsElem = $("<div/>", {
                class: "transition150 center",
                css: {
                    position: "relative",
                    top: 0,
                    height: "0px",
                    width: "640px",
                    background: "black",
                    color: "white",
                    "font-family": "Courier New",
                    "overflow-y": "auto"
                }
            });
            $(masterContainerId).after(cheatsElem);

            var ConsoleInputManager = function () {
                var MESSAGE = messengerEngine.post;

                var PREDEFINED_CHEATS = {
                    PDF_RESET_PLAYER: 'MESSAGE("CHEAT", sessionStorage.playerId, "ResetPlayer")'
                };

                var previous = [];
                var previousIndex = -1;

                var that = this;

                this.resetConsoleInput = function () {
                    return this.addCharToConsoleInput("", "");
                };

                this.displayPreviousConsoleInput = function () {
                    return (previous.length) ? previous[previousIndex--] : "";
                };

                this.addCharToConsoleInput = function (add, str) {
                    return str.substring(0, str.length - 1) + add + "_";
                };

                this.deleteCharFromConsoleInput = function (str) {
                    return str.substring(0, str.length - 2) + "_";
                };

                this.execute = function (str) {
                    try {
                        if (PREDEFINED_CHEATS[str]) { // intentional truthiness
                            eval(PREDEFINED_CHEATS[str]);
                        } else {
                            eval(str);
                        }
                        previousIndex = previous.push(str) - 1;
                    } catch (e) {
                        return false;
                    }
                    return true;
                };
            };

            var tildeKey = 96;
            var deleteKey = 8;
            var enterKey = 13;
            var upArrowKey = 38;

            var consoleInputManager = new ConsoleInputManager();
            cheatsElem.text(consoleInputManager.resetConsoleInput());

            function updateConsoleInput(event) {
                if (event.keyCode === deleteKey) { // delete characters if the delete key is pressed
                    cheatsElem.text(consoleInputManager.deleteCharFromConsoleInput(cheatsElem.text()));
                } else if (event.keyCode === enterKey) { // parse characters if the enter key is pressed
                    var str = cheatsElem.text().substring(0, cheatsElem.text().length - 1);
                    if (consoleInputManager.execute(str) === true) {
                        cheatsElem.text(consoleInputManager.resetConsoleInput());
                    }
                    cheatsElem.text(consoleInputManager.resetConsoleInput());
                } else if (event.keyCode === upArrowKey) {
                    cheatsElem.text(consoleInputManager.displayPreviousConsoleInput());
                } else { // otherwise, add characters
                    cheatsElem.text(consoleInputManager.addCharToConsoleInput(String.fromCharCode(event.keyCode), cheatsElem.text()));
                }
            };

            function suppressBackspace(event) { // we don't want the delete key to act like the browser back button
                event = event || window.event;
                var target = event.target || event.srcElement;
                if (event.keyCode === deleteKey && !/input|textarea/i.test(target.nodeName)) {
                    updateConsoleInput(event);
                    return false;
                } else if (event.keyCode === upArrowKey) {
                    updateConsoleInput(event);
                    return false;
                }
            };
            document.onkeydown = suppressBackspace;
            document.onkeypress = suppressBackspace;

            window.addEventListener("keypress", function (event) {
                if (event.keyCode === tildeKey) {
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
                        updateConsoleInput(event);
                    }
                }
            }, true);
        }
    };
}(window.GinTub = window.GinTub || {}));
