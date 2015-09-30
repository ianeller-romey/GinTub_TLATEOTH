(function (namespace, undefined) {
    "use strict";

    namespace.Managers = namespace.Managers || {};
    namespace.Managers.MessageManager = {
        init: function (messageTopId, messageBottomId, messageTextId, messageChoicesId, masterContainerId, messengerEngine) {
            var messageTopElem = $(messageTopId);
            var messageBottomElem = $(messageBottomId);
            var messageTextElem = $(messageTextId);
            var messageChoicesElem = $(messageChoicesId);

            var updateIntervalFast = 2;
            var updateIntervalRemoval = 10;
            var updateIntervalAdding = 50;
            var updateInterval = 0;

            var noMessageChoices = {
                id: -1,
                text: "\"...\""
            };

            var that = this;

            var unloadMessage = function () {
                messageTopElem.width("0px");
                messageBottomElem.width("0px");

                messageTextElem.text("");
                $(messageChoicesId).empty();
                messengerEngine.post("MessageManager.unloadMessage");
            }

            var loadMessage = function (messageData) {
                messageTopElem.width("100%");
                messageBottomElem.width("100%");

                var messageChoices = messageData.messageChoices;

                updateInterval = updateIntervalAdding;
                messageTextElem.animateTextAdd(messageData.text, that.getUpdateInterval).then(function () {
                    if (!messageChoices || messageChoices.length === 0) { // intentional truthiness
                        var text = namespace.Entities.Factories.createActionText(noMessageChoices.id, noMessageChoices.text, unloadMessage);
                        text.addClass("boldText");
                        messageChoicesElem.append(text);
                    }
                    else {
                        var i = 0, len = messageChoices.length;
                        for (; i < len; ++i) {
                            var mc = messageChoices[i];
                            messageChoicesElem.append(namespace.Entities.Factories.createActionText(mc.id, mc.text, null));
                        }
                    }
                });
            };

            this.getUpdateInterval = function () {
                return updateInterval;
            };

            this.changeUpdateInterval = function () {
                if (updateInterval != updateIntervalFast) {
                    updateInterval = updateIntervalFast;
                }
            };

            $(masterContainerId).mousedown(function (e) {
                that.changeUpdateInterval();
            });

            messengerEngine.register("GameStateData.setMessage", this, loadMessage);
        }
    };
}(window.GinTub = window.GinTub || {}));
