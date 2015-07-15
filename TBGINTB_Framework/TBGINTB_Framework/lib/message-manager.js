﻿
var MessageManager = function (messageTopId, messageBottomId, messageTextId, messageChoicesId) {
    var messageTopElem = $(messageTopId);
    var messageBottomElem = $(messageBottomId);
    var messageTextElem = $(messageTextId);
    var messageChoicesElem = $(messageChoicesId);

    var messengerEngine = globalMessengerEngine;

    var noMessageChoices = {
        Id: -1,
        Text: "OK"
    };

    var unloadMessage = function () {
        messageTopElem.width("0px");
        messageBottomElem.width("0px");

        messageTextElem.text("");
        $(messageChoicesId + " .actionText").remove();
    }

    var loadMessage = function (messageData) {
        messageTopElem.width("100%");
        messageBottomElem.width("100%");

        messageTextElem.text(messageData.text);

        var messageChoices = messageData.messageChoices;
        if (messageChoices == null) {
            messageChoicesElem.append(createActionText(noMessageChoices.Id, noMessageChoices.Text, unloadMessage));
        }
        else {
            var i = 0,
                len = messageChoices.length;
            for (; i < len; ++i) {
                var mc = messageChoices[i];
                messageChoicesElem.append(createActionText(mc.Id, mc.Text, null));
            }
        }
    };

    messengerEngine.register("GameStateData.setMessage", this, loadMessage);
};