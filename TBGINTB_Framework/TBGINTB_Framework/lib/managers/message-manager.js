(function (namespace, undefined) {
    "use strict";

    namespace.Managers = namespace.Managers || {};
    namespace.Managers.MessageManager = {
        init: function (messageTopId, messageBottomId, messageTextId, messageChoicesId, masterContainerId, messengerEngine) {
            var messageTopElem = $(messageTopId);
            var messageBottomElem = $(messageBottomId);
            var messageTextElem = $(messageTextId);
            var messageChoicesElem = $(messageChoicesId);

            var list = [];

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

                messageTextElem.empty();
                messageChoicesElem.empty();
                messengerEngine.post("MessageManager.unloadMessage");
            }

            var loadMessage = function (messageData) {
                messageTopElem.width("100%");
                messageBottomElem.width("100%");

                var messageChoices = messageData.messageChoices;

                var removeTextPromise = Promise.resolve();
                // if there is leftover text from a previous message, animate it away
                if (messageTextElem.text()) { // intentional truthiness
                    updateInterval = updateIntervalRemoval;
                    removeTextPromise = messageTextElem.animateTextRemove(that.getUpdateInterval);
                };

                removeTextPromise.then(function () {
                    list = [];
                    updateInterval = updateIntervalAdding;
                    messageTextElem.animateTextAdd(messageData.text, that.getUpdateInterval).then(function () {
                        if (!messageChoices || messageChoices.length === 0) { // intentional truthiness
                            var text = namespace.Entities.Factories.createActionText(noMessageChoices.id, noMessageChoices.text, unloadMessage);
                            text.addClass("boldText");

                            list.push(text);
                            messageChoicesElem.append(text);
                        }
                        else {
                            var i = 0, len = messageChoices.length;
                            for (; i < len; ++i) {
                                var mc = messageChoices[i];
                                var text = namespace.Entities.Factories.createActionText(mc.id, mc.text, function (mId) {
                                    messengerEngine.post("MessageChoice.click", mId);
                                });
                                text.addClass("boldText");

                                list.push(text);
                                messageChoicesElem.append(text);
                            }
                        }
                    });
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

            var messageChoiceClick = function (mId) {
                // deactivate all message choices
                for (var i = 0, j = list.length; i < j; ++i) {
                    list[i].setActive(false);
                }
                messageChoicesElem.children(".actionText").promiseToFade("fast", 0.0).then(function () {
                    messageChoicesElem.empty();
                    messengerEngine.post("MessageManager.messageChoiceClick", mId);
                });
            };

            $(masterContainerId).mousedown(function (e) {
                that.changeUpdateInterval();
            });

            messengerEngine.register("GameStateData.setMessage", this, loadMessage);
            messengerEngine.register("MessageChoice.click", this, messageChoiceClick);
        }
    };
}(window.GinTub = window.GinTub || {}));
