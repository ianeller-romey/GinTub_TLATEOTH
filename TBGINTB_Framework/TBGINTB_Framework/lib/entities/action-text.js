(function (namespace, undefined) {
    "use strict";

    namespace.Entities = namespace.Entities || {};
    namespace.Entities.Factories = namespace.Entities.Factories || {};
    namespace.Entities.Factories.createActionText = function (id, text, call) {
        var clickMe = function (callWithThisId, toCall) {
                toCall(callWithThisId);
        };

        var clickHandler = function () {
            clickMe(id, call);
        };

        var actionText = $("<div/>", {
            class: "actionText actionTextActive",
            text: text
        }).click(clickHandler);

        actionText.setActive = function (active) {
            if (active) {
                $(this).click(clickHandler).addClass("actionTextActive");
            } else {
                $(this).off("click").removeClass("actionTextActive");
            }
        };

        return actionText;
    };
}(window.GinTub = window.GinTub || {}));