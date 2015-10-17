(function (namespace, undefined) {
    "use strict";

    namespace.Entities = namespace.Entities || {};
    namespace.Entities.Classes = namespace.Entities.Classes || {};
    namespace.Entities.Classes.PauseFader = function (messengerEngine) {
        var faderElem = $("<div/>", {
            class: "overlay",
            css: {
                "background-color": "black",
                opacity: 0
            }
        }).appendTo("body");

        faderElem.promiseToFade("slow", .25).then(function () {
            faderElem.click(function () {
                faderElem.off("click").promiseToFade("slow", 0).then(function () {
                    faderElem.remove();
                    messengerEngine.post("PauseFader.unpauseClick", true);
                });
            });
        });
    };
}(window.GinTub = window.GinTub || {}));