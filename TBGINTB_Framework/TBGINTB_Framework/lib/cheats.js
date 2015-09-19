(function () {
    "use strict";

    $(document).ready(function init() {
        function suppressBackspace(event) { // we don't want the delete key to act like the browser back button
            event = event || window.event;
            var target = event.target || event.srcElement;
            if (event.keyCode == 8 && !/input|textarea/i.test(target.nodeName)) {
                updateConsoleInput(event);
                return false;
            }
        };
        document.onkeydown = suppressBackspace;
        document.onkeypress = suppressBackspace;

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
        $("#master-container").after(cheatsElem);

        var ConsoleInputManager = function () {
            var that = this;

            this.resetConsoleInput = function () {
                return this.addCharToConsoleInput("", "");
            };

            this.addCharToConsoleInput = function (add, str) {
                return str.substring(0, str.length - 1) + add + '_';
            };

            this.deleteCharFromConsoleInput = function (str) {
                return str.substring(0, str.length - 2) + '_';
            };
        };

        var deleteKey = 8;
        var enterKey = 13;
        var consoleInputManager = new ConsoleInputManager();
        cheatsElem.text(consoleInputManager.resetConsoleInput());
        function updateConsoleInput(event) {
            if (event.keyCode == deleteKey) { // delete characters if the delete key is pressed
                cheatsElem.text(consoleInputManager.deleteCharFromConsoleInput(cheatsElem.text()));
            }
            else if (event.keyCode == enterKey) { // parse characters if the enter key is pressed
                var str = cheatsElem.text().substring(0, cheatsElem.text().length - 1);
                /*if (engine.parse(str) === true) {
                    cheatsElem.text(consoleInputManager.resetConsoleInput());
                }*/
                // TODO
                cheatsElem.text(consoleInputManager.resetConsoleInput());
            }
            else { // otherwise, add characters
                cheatsElem.text(consoleInputManager.addCharToConsoleInput(String.fromCharCode(event.keyCode), cheatsElem.text()));
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
                    updateConsoleInput(event);
                }
            }
        }, true);
    });
}());
