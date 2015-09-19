(function (namespace, undefined) {
    "use strict";

    namespace.Managers = namespace.Managers || {};
    namespace.Managers.MessageManager = {
        init: function (messageTopId, messageBottomId, messageTextId, messageChoicesId, messengerEngine) {
            var messageTopElem = $(messageTopId);
            var messageBottomElem = $(messageBottomId);
            var messageTextElem = $(messageTextId);
            var messageChoicesElem = $(messageChoicesId);

            var noMessageChoices = {
                id: -1,
                text: "\"...\""
            };

            var unloadMessage = function () {
                messageTopElem.width("0px");
                messageBottomElem.width("0px");

                messageTextElem.text("");
                $(messageChoicesId + " .actionText").remove();
                messengerEngine.post("MessageManager.unloadMessage");
            }

            var loadMessage = function (messageData) {
                messageTopElem.width("100%");
                messageBottomElem.width("100%");

                messageTextElem.text(messageData.text);

                var messageChoices = messageData.messageChoices;
                if (!messageChoices || messageChoices.length === 0) { // intentional truthiness
                    messageChoicesElem.append(createActionText(noMessageChoices.id, noMessageChoices.text, unloadMessage));
                }
                else {
                    var i = 0,
                        len = messageChoices.length;
                    for (; i < len; ++i) {
                        var mc = messageChoices[i];
                        messageChoicesElem.append(createActionText(mc.id, mc.text, null));
                    }
                }
            };

            messengerEngine.register("GameStateData.setMessage", this, loadMessage);
        }
    };
}(window.GinTub = window.GinTub || {}));
