(function (namespace, undefined) {
    "use strict";

    namespace.Entities = namespace.Entities || {};
    namespace.Entities.Factories = namespace.Entities.Factories || {};
    namespace.Entities.Factories.createActionText = function (id, text, call) {
        var createClick = function (callWithThisId, toCall) {
            return function (e) {
                toCall(callWithThisId);
            };
        };

        return $("<div/>", {
            class: "actionText",
            text: text
        }).click(createClick(id, call));
    };
}(window.GinTub = window.GinTub || {}));