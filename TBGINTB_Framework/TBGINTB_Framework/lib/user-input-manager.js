
var VerbList = function(idOfVerbList){
    var verbListElem = $(idOfVerbList);
    var verbListElemX = $(idOfVerbList + "X");
    var interfaceElem = verbListElem.parent("div");

    var messengerEngine = globalMessengerEngine;

    this.states = {
        closed: 0,
        updating: 1,
        open: 2
    };
    this.state = this.states.closed;
    var actionQueue = new ActionQueue(3);
    var verbTypesForParagraphs = [{
        id: -1,
        name: "Inspect",
        call: function () {
            messengerEngine.post("VerbList.paragraphClick");
        }
    }];
    var verbTypesForWords = null;
    var activeVerbTypes = null;

    var that = this;

    verbListElemX.click(function () {
        that.close();
    });

    var setActiveVerbTypesForParagraphs = function () {
        activeVerbTypes = verbTypesForParagraphs;
    };

    var setActiveVerbTypesForWords = function () {
        activeVerbTypes = verbTypesForWords;
    };

    var update = function () {
        if (!actionQueue.isEmpty()) {
            actionQueue.pop();
        }
    };

    var continueUpdating = function () {
        actionQueue.pop();
    };

    var openExec = function (clientX, clientY) {
        if (that.state === that.states.open) {
            return;
        }

        var i = 0;
        var len = activeVerbTypes.length;
        for (; i < len; ++i) {
            var avt = activeVerbTypes[i];
            verbListElem.append(createActionText(avt.id, avt.name, avt.call)).click(function () {
                that.close();
            });
        }

        // set position immediately
        verbListElem.css({
            left: clientX - interfaceElem.offset().left,
            top: clientY - interfaceElem.offset().top - 20
        });
        verbListElem.addClass("verbListShadowed");
        // animate opening
        verbListElem.animateAuto("width", 50, function () {
            verbListElem.animateAuto("height", 50, function () {
                verbListElem.animate({
                    padding: 10
                }, 50, function () {
                    verbListElemX.css({
                        display: "inline"
                    });
                    verbListElemX.animate({
                        opacity: 1
                    }, 25, function () {
                        if (actionQueue.isEmpty()) {
                            that.state = that.states.open;
                            messengerEngine.post("VerbList.openExec");
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

        // animate closing in reverse order
        verbListElemX.animate({
            opacity: 0
        }, 25, function () {
            verbListElemX.css({
                display: "none"
            });
            verbListElem.animate({
                height: 0
            }, 50, function () {
                // remove the verbs after animating down the height, and
                // before anything else, since we see weird artifacts if we
                // remove them last
                $(idOfVerbList + " .actionText").remove();
                verbListElem.removeClass("verbListShadowed");
                verbListElem.animate({
                    width: 0
                }, 50, function () {
                    verbListElem.animate({
                        padding: 0
                    }, 50, function () {
                        if (actionQueue.isEmpty()) {
                            that.state = that.states.closed;
                            messengerEngine.post("VerbList.closeExec");
                        }
                        else {
                            update();
                        }
                    });
                });
            });
        });
    };

    var open = function (clientX, clientY) {
        if (that.state === that.states.updating) {
            return;
        }
        that.state = that.states.updating;
        actionQueue.push(that, openExec, clientX, clientY);
        update();
    };

    this.openForParagraphs = function (clientX, clientY) {
        setActiveVerbTypesForParagraphs();
        open(clientX, clientY);
    };

    this.openForWords = function (clientX, clientY) {
        setActiveVerbTypesForWords();
        open(clientX, clientY);
    };

    this.close = function () {
        if (that.state === that.states.updating) {
            return;
        }
        that.state = that.states.updating;
        actionQueue.push(that, closeExec);
        update();
    };


    var closeAndReopen = function (clientX, clientY) {
        that.state = that.states.updating;
        actionQueue.push(that, closeExec);
        actionQueue.push(that, openExec, clientX, clientY);
        update();
    };

    this.closeAndReopenForParagraphs = function (clientX, clientY) {
        if (that.state === that.states.updating) {
            return;
        }
        setActiveVerbTypesForParagraphs();
        closeAndReopen(clientX, clientY);
    };

    this.closeAndReopenForWords = function (clientX, clientY) {
        if (that.state === that.states.updating) {
            return;
        }
        setActiveVerbTypesForWords();
        closeAndReopen(clientX, clientY);
    };

    loadAllVerbTypes = function (verbTypesData) {
        verbTypesForWords = [];

        var i = 0;
        var len = verbTypesData.length;
        for (; i < len; ++i) {
            var vt = verbTypesData[i];
            verbTypesForWords[i] = {
                id: vt.id,
                name: vt.name,
                call: function (vId) {
                    messengerEngine.post("VerbList.wordClick", vId);
                }
            };
        }

        messengerEngine.unregister("GameStateEngine.loadAllVerbTypes");
    };

    messengerEngine.register("GameStateEngine.loadAllVerbTypes", that, loadAllVerbTypes);
};

var UserInputManager = function (verbListId) {
    var verbList = new VerbList(verbListId);

    var activeId = null;

    var messengerEngine = globalMessengerEngine;

    var openVerbListForParagraphs = function (clientX, clientY) {
        this.verbList.openForParagraphs(clientX, clientY);
    };

    var openVerbListForWords = function (clientX, clientY) {
        this.verbList.openForWords(clientX, clientY);
    };

    var closeVerbList = function () {
        this.verbList.close();
    };

    var closeAndReopenVerbListForParagraphs = function (clientX, clientY, psId) {
        if (verbList.state === verbList.states.open) {
            verbList.closeAndReopenForParagraphs(clientX, clientY);
        }
        else {
            verbList.openForParagraphs(clientX, clientY);
        }
        activeId = psId;
    };

    var closeAndReopenVerbListForWords = function (clientX, clientY, wId) {
        if (verbList.state === verbList.states.open) {
            verbList.closeAndReopenForWords(clientX, clientY);
        }
        else {
            verbList.openForWords(clientX, clientY);
        }
        activeId = wId;
    };

    var getNounsForParagraphState = function () {
        verbList.close();
        messengerEngine.post("UserInputManager.getNounsForParagraphState", activeId);
    };

    var doAction = function (verbId) {
        messengerEngine.post("UserInputManager.doAction", activeId, verbId);
    };

    messengerEngine.register("InterfaceManager.iParagraphClick", this, closeAndReopenVerbListForParagraphs);
    messengerEngine.register("InterfaceManager.iWordClick", this, closeAndReopenVerbListForWords);
    messengerEngine.register("VerbList.paragraphClick", this, getNounsForParagraphState);
    messengerEngine.register("VerbList.wordClick", this, doAction);
};