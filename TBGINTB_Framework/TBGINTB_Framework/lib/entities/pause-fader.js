(function (namespace, undefined) {
    "use strict";

    namespace.Entities = namespace.Entities || {};
    namespace.Entities.Classes = namespace.Entities.Classes || {};
    namespace.Entities.Classes.PauseFader = function (messengerEngine) {
        var faderElem = $("<div/>", {
            class: "transition150 center",
            css: {
                opacity: 0,
                "background-color": "black",
                position: "fixed",
                width: "100%",
                height: "100%",
                top: 0,
                left: 0,
                "z-index": 1000
            }
        }).appendTo("body");

        faderElem.promiseToFade("slow", 0.25).then(function () {
            faderElem.click(function () {
                faderElem.off("click").promiseToFade("slow", 0.0).then(function () {
                    faderElem.remove();
                    messengerEngine.post("PauseFader.unpauseClick", true);
                });
            });
        });
    };
}(window.GinTub = window.GinTub || {}));