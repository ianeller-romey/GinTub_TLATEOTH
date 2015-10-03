﻿(function (namespace, undefined) {
    "use strict";

    namespace.Engines = namespace.Engines || {};
    namespace.Engines.MessengerEngine = {
        init: function () {
            var messageTypes = [];
            var messageRegistration = {};
            var messages = [];

            var validMessageType = function (messageType) {
                return messageTypes.contains(messageType);
            }

            var createMessageType = function (messageType) {
                if (!validMessageType(messageType)) {
                    messageTypes.push(messageType);

                    if (messageRegistration[messageType] === undefined) {
                        messageRegistration[messageType] = [];
                    }
                }
            };

            this.register = function (messageType, object, funct) {
                if (!validMessageType(messageType)) {
                    throw "Cannot register for " + messageType + ", a messageType that does not exist.";
                }

                messageRegistration[messageType].push({
                    caller: object,
                    toCall: funct
                });
            };

            this.unregister = function (messageType, funct) {
                if (!validMessageType(messageType)) {
                    throw "Cannot unregister from " + messageType + ", a messageType that does not exist.";
                }

                var messageTypeRegistration = messageRegistration[messageType];
                var messageRegisterer = messageTypeRegistration.firstOrNull(function (x) {
                    return x.toCall === funct;
                });
                if (messageRegisterer) { // intentional truthiness
                    var index = messageTypeRegistration.indexOf(messageRegisterer);
                    if (index > -1) {
                        messageTypeRegistration.splice(index, 1);
                    }
                }
            };

            this.post = function (messageType) {
                if (!validMessageType(messageType)) {
                    throw "Cannot post " + messageType + ", a messageType that does not exist.";
                }

                var messageTypeRegistration = messageRegistration[messageType];
                var params = Array.prototype.slice.call(arguments, 1);
                messageTypeRegistration.forEach(function (x) {
                    x.toCall.apply(x.caller, params);
                });
            };

            createMessageType("ServicesEngine.ajaxError");
            createMessageType("ServicesEngine.getAllVerbTypes");
            createMessageType("ServicesEngine.getNounsForParagraphState");
            createMessageType("ServicesEngine.loadGame");
            createMessageType("ServicesEngine.doAction");
            createMessageType("ServicesEngine.mapRequest");
            createMessageType("ServicesEngine.inventoryRequest");
            createMessageType("ServicesEngine.historyRequest");
            createMessageType("ServicesEngine.partyRequest");

            createMessageType("TimeEngine.updateTime");
            createMessageType("TimeEngine.updateTimeAtTen");

            createMessageType("GameStateEngine.loadVerbTypes");
            createMessageType("GameStateEngine.loadWithVerbTypes");
            createMessageType("GameStateEngine.setArea");
            createMessageType("GameStateEngine.setRoom");
            createMessageType("GameStateEngine.setRoomStates");
            createMessageType("GameStateEngine.setParagraphStates");
            createMessageType("GameStateData.setMessage");
            createMessageType("GameStateEngine.setActiveRoomState");
            createMessageType("GameStateEngine.doAction");
            createMessageType("GameStateEngine.mapRequest");
            createMessageType("GameStateEngine.inventoryRequest");
            createMessageType("GameStateEngine.historyRequest");
            createMessageType("GameStateEngine.partyRequest");

            createMessageType("InterfaceManager.iParagraphClick");
            createMessageType("InterfaceManager.iWordClick");
            createMessageType("InterfaceManager.iParagraphClick");
            createMessageType("InterfaceManager.iWordClick");

            createMessageType("MenuEntry.mapRequest");
            createMessageType("MenuEntry.inventoryRequest");
            createMessageType("MenuEntry.historyRequest");
            createMessageType("MenuEntry.partyRequest");
            createMessageType("MenuEntry.click");

            createMessageType("MessageManager.unloadMessage");

            createMessageType("VerbList.paragraphClick");
            createMessageType("VerbList.wordClick");
            createMessageType("VerbList.openExec");
            createMessageType("VerbList.closeExec");

            createMessageType("WithList.inventoryRequest");
            createMessageType("WithList.wordClick");
            createMessageType("WithList.openExec");
            createMessageType("WithList.closeExec");

            createMessageType("UserInputManager.getNounsForParagraphState");
            createMessageType("UserInputManager.doAction");

            createMessageType("CHEAT");
        }
    };
}(window.GinTub = window.GinTub || {}));