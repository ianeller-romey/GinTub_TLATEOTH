
var createActionText = function (id, text, call) {

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