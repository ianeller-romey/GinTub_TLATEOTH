(function (namespace, undefined) {
    "use strict";

    // TODO: Condense onVerbListClose/Open declarations so we're not duplicating code

    namespace.Entities = namespace.Entities || {};
    namespace.Entities.Factories = namespace.Entities.Factories || {};
    namespace.Entities.Factories.createHoverClickText = function (classType, idNum, messengerEngine) {
        var span = null;
        var classTypes = namespace.Entities.Factories.createHoverClickText.classTypes;
        switch (classType) {
            case classTypes.paragraph:
                span = $("<span/>", {
                    class: classType
                });
                span.idNum = idNum;

                var spanClick = function (e) {
                    span.removeClass("iHover").addClass("iSelected");
                    var onVerbListClose = function () {
                        span.removeClass("iSelected");
                        messengerEngine.unregister("VerbList.closeExec", onVerbListClose);
                    };

                    var onVerbListOpen = function () {
                        messengerEngine.unregister("VerbList.openExec", onVerbListOpen);
                        messengerEngine.register("VerbList.closeExec", this, onVerbListClose);
                    };

                    messengerEngine.register("VerbList.openExec", this, onVerbListOpen);
                    messengerEngine.post("InterfaceManager.iParagraphClick", e.pageX, e.pageY, span.idNum);
                }

                span.enableInterfaceInput = function () {
                    span.mouseenter(function (e) {
                        span.addClass("iHover");
                    }).mouseleave(function (e) {
                        span.removeClass("iHover");
                    }).click(spanClick);
                }
                span.disableInterfaceInput = function () {
                    span.off("mouseenter").off("mouseleave").off("click");
                };
                break;

            case classTypes.word:
                span = $("<span/>", {
                    class: classType
                });
                span.idNum = idNum;

                var spanClick = function (e) {
                    e.stopPropagation();

                    span.removeClass("iHover").addClass("iSelected");
                    var onVerbListClose = function () {
                        span.removeClass("iSelected");
                        messengerEngine.unregister("VerbList.closeExec", onVerbListClose);
                    };

                    var onVerbListOpen = function () {
                        messengerEngine.unregister("VerbList.openExec", onVerbListOpen);
                        messengerEngine.register("VerbList.closeExec", this, onVerbListClose);
                    };

                    messengerEngine.register("VerbList.openExec", this, onVerbListOpen);

                    var position = span[0].getBoundingClientRect();
                    messengerEngine.post("InterfaceManager.iWordClick", position.right + 5, position.top + 10, span.idNum);
                };

                span.enableInterfaceInput = function () {
                    span.mousemove(function (e) {
                        span.addClass("iHover").parent().removeClass("iHover");
                    }).mouseleave(function (e) {
                        span.removeClass("iHover").parent().addClass("iHover");
                    }).click(spanClick);
                };
                span.disableInterfaceInput = function () {
                    span.off("mousemove").off("mouseleave").off("click");
                };
                break;
        }
        return span;
    };

    namespace.Entities.Factories.createHoverClickText.classTypes = {
        paragraph: "iParagraph",
        word: "iWord"
    };
}(window.GinTub = window.GinTub || {}));