(function (namespace, undefined) {
    "use strict";

    namespace.Entities = namespace.Entities || {};
    namespace.Entities.Classes = namespace.Entities.Classes || {};
    namespace.Entities.Classes.LoadingFader = function (parentElemId, promise, messengerEngine) {
        var faderElem = $("<div/>", {
            class: "coverlay",
            css: {
                "background-color": "rgba(0, 0, 0, .25)",
                "text-align": "center",
                "vertical-align": "middle"
            }
        }).appendTo(parentElemId);
        $("<img/>", {
            css: {
                "vertical-align": "middle"
            }
        }).attr("src", "images/loading.gif").width(250).height(250).appendTo($("<span/>", {
            css: {
                display: "inline-block",
                height: "100%",
                "vertical-align": "middle"
            }
        }).appendTo(faderElem));

        promise.then(function () {
                faderElem.remove();
        });
    };
}(window.GinTub = window.GinTub || {}));