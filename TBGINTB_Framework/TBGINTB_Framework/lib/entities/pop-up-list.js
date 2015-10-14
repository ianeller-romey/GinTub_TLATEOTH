(function (namespace, undefined) {
    "use strict";

    namespace.Entities = namespace.Entities || {};
    namespace.Entities.Classes = namespace.Entities.Classes || {};
    namespace.Entities.Classes.PopUpList = function (constructorObject, messengerEngine) {
        var popupListElem = $(constructorObject.idOfPopUpList);
        var popupListElemX = $(constructorObject.idOfPopUpList + "X");
        var parentElem = $(constructorObject.idOfParent);
        var idOfPopUpList = constructorObject.idOfPopUpList;
        var className = constructorObject.className;

        this.onOpenExec = function () {
        };
        this.onCloseExec = function () {
        };

        this.states = {
            closed: 0,
            updating: 1,
            open: 2
        };
        this.state = this.states.closed;
        var actionQueue = new ActionQueue(3);
        var activeVerbTypes = null;
        this.list = [];

        var that = this;

        popupListElemX.click(function () {
            that.close();
        });

        this.setActiveVerbTypes = function (verbTypes) {
            activeVerbTypes = verbTypes;
        };

        var update = function () {
            if (!actionQueue.isEmpty()) {
                actionQueue.pop();
            }
        };

        var continueUpdating = function () {
            actionQueue.pop();
        };

        this.setOnOpenExec = function (f) {
            if (f && typeof f === "function") { // intentional truthiness
                that.onOpenExec = f;
            }
        };

        this.setOnCloseExec = function (f) {
            if (f && typeof f === "function") { // intentional truthiness
                that.onCloseExec = f;
            }
        };

        var openExec = function (clientX, clientY) {
            if (that.state === that.states.open) {
                return;
            }

            var i = 0;
            var len = activeVerbTypes.length;
            for (; i < len; ++i) {
                var avt = activeVerbTypes[i];
                var actionText = namespace.Entities.Factories.createActionText(avt.id, avt.name, avt.call);
                that.list.push(actionText);
                popupListElem.append(actionText);
            }

            if (that.onOpenExec && typeof that.onOpenExec === "function") {
                that.onOpenExec();
            }

            // set position immediately
            popupListElem.css({
                left: clientX - parentElem.offset().left,
                top: clientY - parentElem.offset().top - 20
            });
            popupListElem.addClass("popupListShadowed");
            // animate opening
            popupListElem.animateAuto("width", 50).then(function () {
                popupListElem.animateAuto("height", 50).then(function () {
                    popupListElem.promiseToAnimate({
                        padding: 10
                    }, 50).then(function () {
                        popupListElemX.css({
                            display: "inline"
                        });
                        popupListElemX.promiseToAnimate({
                            opacity: 1
                        }, 25).then(function () {
                            messengerEngine.post(className + ".openExec");
                            if (actionQueue.isEmpty()) {
                                that.state = that.states.open;
                            }
                            else {
                                update();
                            }
                        });
                    });
                });
            });
        };

        var closeExec = function () {
            if (that.state === that.states.closed) {
                return;
            }

            if (that.onCloseExec && typeof that.onCloseExec === "function") {
                that.onCloseExec();
            }

            // animate closing in reverse order
            popupListElemX.promiseToAnimate({
                opacity: 0
            }, 25).then(function () {
                popupListElemX.css({
                    display: "none"
                });
                popupListElem.promiseToAnimate({
                    height: 0
                }, 50).then(function () {
                    // remove the verbs after animating down the height, and
                    // before anything else, since we see weird artifacts if we
                    // remove them last
                    that.list = [];
                    $(idOfPopUpList + " .actionText").remove();
                    popupListElem.removeClass("popupListShadowed");
                    popupListElem.promiseToAnimate({
                        width: 0
                    }, 50).then(function () {
                        popupListElem.promiseToAnimate({
                            padding: 0
                        }, 50).then(function () {
                            messengerEngine.post(className + ".closeExec");
                            if (actionQueue.isEmpty()) {
                                that.state = that.states.closed;
                            }
                            else {
                                update();
                            }
                        });
                    });
                });
            });
        };

        this.open = function (clientX, clientY, appendAnyway) {
            var updating = (that.state === that.states.updating);
            if (!appendAnyway && updating) { // intentional truthiness
                return;
            }
            that.state = that.states.updating;
            actionQueue.push(that, openExec, clientX, clientY);
            if (!updating) {
                update();
            }
        };

        this.close = function (appendAnyway) {
            var updating = (that.state === that.states.updating);
            if (!appendAnyway && updating) { // intentional truthiness
                return;
            }
            that.state = that.states.updating;
            actionQueue.push(that, closeExec);
            if (!updating) {
                update();
            }
        };

        this.closeAndReopen = function (clientX, clientY) {
            that.state = that.states.updating;
            actionQueue.push(that, closeExec);
            actionQueue.push(that, openExec, clientX, clientY);
            update();
        };
    };

    namespace.Entities.Classes.PopUpListConstructorObject = function (idOfPopUpList, idOfParent, className) {
        this.idOfPopUpList = idOfPopUpList;
        this.idOfParent = idOfParent;
        this.className = className;
    };
}(window.GinTub = window.GinTub || {}));