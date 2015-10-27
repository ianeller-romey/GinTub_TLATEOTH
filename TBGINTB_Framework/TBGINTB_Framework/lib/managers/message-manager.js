(function (namespace, undefined) {
    "use strict";

    namespace.Managers = namespace.Managers || {};
    namespace.Managers.MessageManager = {
        init: function (messageTopId, messageBottomId, messageTextId, messageChoicesId, masterContainerId, messengerEngine) {
            var messageTopElem = $(messageTopId);
            var messageBottomElem = $(messageBottomId);
            var messageTextElem = $(messageTextId);
            var messageChoicesElem = $(messageChoicesId);

            var loadingFader = null;

            var unloadMessageTypeBeforeChoice = "BeforeChoice";
            var unloadMessageTypeBeforeClosing = "BeforeClosing";
            var fadeOutTextPromise = null;

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

            var unloadMessage = function (unloadMessageType) {
                // if there is leftover text from a previous message, animate it away
                if (messageTextElem.text()) { // intentional truthiness
                    updateInterval = updateIntervalRemoval;
                    fadeOutTextPromise = messageTextElem.promiseToFade("fast", 0.0);
                    fadeOutTextPromise.then(function () {
                        messageTextElem.text("").css("opacity", 1.0);
                        // we want to be the first to set a "then", so that we can post this message for the loadingFader immediately,
                        // so that we can deactivate it immediately if loadMessage is ready to happen (which it might be, since we asynchronously
                        // request the next response of the message choice before we begin removing text); so, even though we can't "technically"
                        // rely on unloadMessage to be done until after this promise AND the promiseToFade below are done, we want to post this message
                        // from right here
                        messengerEngine.post("MessageManager.unloadMessage" + unloadMessageType, true);
                        fadeOutTextPromise = null;
                    });
                };
                // deactivate all message choices
                for (var i = 0, j = list.length; i < j; ++i) {
                    list[i].setActive(false);
                }

                return Promise.all([messageChoicesElem.children(".actionText").promiseToFade("fast", 0.0), fadeOutTextPromise]);
            };

            var closeMessageManager = function () {
                unloadMessage(unloadMessageTypeBeforeClosing).then(function () {
                    messageTopElem.width("0px");
                    messageBottomElem.width("0px");

                    messageTextElem.empty();
                    messageChoicesElem.empty();
                    messengerEngine.post("MessageManager.closeMessageManager");
                });
            }

            var loadMessage = function (messageData) {
                messageTopElem.width("100%");
                messageBottomElem.width("100%");

                var messageChoices = messageData.messageChoices;

                if (!fadeOutTextPromise) { // intentional truthiness
                    fadeOutTextPromise = Promise.resolve();
                }
                
                fadeOutTextPromise.then(function () {
                    messengerEngine.post("MessageManager.loadMessage", true);
                    list = [];
                    updateInterval = updateIntervalAdding;
                    messageTextElem.animateTextAdd(messageData.text, that.getUpdateInterval).then(function () {
                        if (!messageChoices || messageChoices.length === 0) { // intentional truthiness
                            var text = namespace.Entities.Factories.createActionText(noMessageChoices.id, noMessageChoices.text, closeMessageManager);
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
                unloadMessage(unloadMessageTypeBeforeChoice).then(function () {
                    messageChoicesElem.empty();
                });
                messengerEngine.post("MessageManager.messageChoiceClick", mId);
            };

            var loadingWhileDoMessageChoice = function () {
                var promise = new Promise(function (resolve, reject) {
                    var loadingWhileDoMessageChoiceFinished = function () {
                        messengerEngine.unregister("MessageManager.loadMessage", loadingWhileDoMessageChoiceFinished);
                        resolve();
                        loadingFader = null;
                    };
                    messengerEngine.register("MessageManager.loadMessage", that, loadingWhileDoMessageChoiceFinished);
                });
                loadingFader = new namespace.Entities.Classes.LoadingFader(messageTopId, promise, messengerEngine);
            };

            $(masterContainerId).mousedown(function (e) {
                that.changeUpdateInterval();
            });

            messengerEngine.register("GameStateData.setMessage", this, loadMessage);
            messengerEngine.register("MessageChoice.click", this, messageChoiceClick);
            messengerEngine.register("MessageManager.unloadMessageBeforeChoice", this, loadingWhileDoMessageChoice);
        }
    };
}(window.GinTub = window.GinTub || {}));
