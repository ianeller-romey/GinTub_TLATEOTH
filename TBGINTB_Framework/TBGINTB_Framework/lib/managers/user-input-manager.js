(function (namespace, undefined) {
    "use strict";

    namespace.Managers = namespace.Managers || {};
    namespace.Managers.UserInputManager = {
        init: function (verbListConstructorObject, withListConstructorObject, clockListConstructorObject, messengerEngine) {
            var VerbList = function (verbListId, withList) {
                var verbTypesForParagraphs = [{
                    id: -1,
                    name: "Inspect",
                    call: function () {
                        messengerEngine.post("VerbList.paragraphClick");
                    }
                }];
                var verbTypesForWords = null;

                var that = this;

                var closeWithList = function () {
                    //if (withList.state !== that.states.closed) {
                        withList.close(true);
                    //}
                };

                this.setOnCloseExec(function () {
                    closeWithList();
                });

                this.setActiveVerbTypesForParagraphs = function () {
                    that.setActiveVerbTypes(verbTypesForParagraphs);
                };

                this.setActiveVerbTypesForWords = function () {
                    that.setActiveVerbTypes(verbTypesForWords);
                };

                this.openForParagraphs = function (clientX, clientY) {
                    that.setActiveVerbTypesForParagraphs();
                    that.open(clientX, clientY);
                };

                this.openForWords = function (clientX, clientY) {
                    that.setActiveVerbTypesForWords();
                    that.open(clientX, clientY);
                };

                this.closeAndReopenForParagraphs = function (clientX, clientY) {
                    if (that.state === that.states.updating) {
                        return;
                    }
                    that.setActiveVerbTypesForParagraphs();
                    that.closeAndReopen(clientX, clientY);
                };

                this.closeAndReopenForWords = function (clientX, clientY) {
                    if (that.state === that.states.updating) {
                        return;
                    }
                    that.setActiveVerbTypesForWords();
                    that.closeAndReopen(clientX, clientY);
                };

                var loadVerbTypes = function (verbTypesData) {
                    verbTypesForWords = [];

                    var i = 0;
                    var len = verbTypesData.length;
                    for (; i < len; ++i) {
                        var vt = verbTypesData[i];
                        verbTypesForWords[i] = {
                            id: vt.id,
                            name: vt.name,
                            call: (vt.name.toLowerCase() !== "with") ? function (vId) {
                                messengerEngine.post("VerbList.wordClick", vId);
                            } : function (vId) {
                                if (withList.state === that.states.closed) {
                                    var el = $(verbListId);
                                    var pos = el.offset();
                                    withList.openForWith(pos.left + (el.outerWidth() / 2), pos.top + (el.outerHeight() / 2));
                                }
                            }
                        };
                    }

                    messengerEngine.unregister("GameStateEngine.loadVerbTypes");
                };

                var deactivateVerbList = function () {
                    for (var i = 0, j = that.list.length; i < j; ++i) {
                        that.list[i].setActive(false);
                    }
                };

                var activateVerbList = function () {
                    for (var i = 0, j = that.list.length; i < j; ++i) {
                        that.list[i].setActive(true);
                    }
                };

                messengerEngine.register("GameStateEngine.loadVerbTypes", that, loadVerbTypes);
                messengerEngine.register("InterfaceManager.clockClick", that, that.close);
                messengerEngine.register("WithList.openExec", that, deactivateVerbList);
                messengerEngine.register("WithList.closeExec", that, activateVerbList);
                messengerEngine.register("VerbList.paragraphClick", that, that.close);
                messengerEngine.register("VerbList.wordClick", that, that.close);
            };

            VerbList.prototype = new namespace.Entities.Classes.PopUpList(verbListConstructorObject, messengerEngine);

            var WithList = function () {
                var verbTypesForWith = [];
                var inventoryEntriesAsVerbs = [];
                var needsToReload = true;
                
                var verbTypesWhenEmpty = {
                    id: -1,
                    name: "NO SUPPLIES",
                    call: function () {
                    }
                };

                var that = this;

                var openWithList = function () {
                    var noSupplies = that.list.firstOrNull(function(x){
                        return x.text() === verbTypesWhenEmpty.name;
                    });
                    if (noSupplies) { // intentional truthiness
                        noSupplies.setActive(false);
                    }
                };

                this.setOnOpenExec(function () {
                    openWithList();
                });

                this.setActiveVerbTypesForInventory = function () {
                    return new Promise(function (resolve, reject) {
                        var set = function (inventory) {
                            // if we need to reload but haven't ...
                            if (needsToReload && !inventory) { // intentional truthiness
                                inventoryEntriesAsVerbs = [];
                                messengerEngine.register("ServicesEngine.inventoryRequest", that, set);
                                messengerEngine.post("WithList.inventoryRequest", true);
                            // if we've requested to reload and received a response ...
                            } else if (needsToReload && inventory) { // intentional truthiness
                                messengerEngine.unregister("ServicesEngine.inventoryRequest", set);

                                var entries = inventory.inventoriesEntries.where(function (x) {
                                    return x.acquired === true;
                                });
                                if (entries.length > 0) {
                                    entries.forEach(function (x) {
                                        var inventoryVerbString = "with " + x.name.toLowerCase();
                                        var inventoryVerb = verbTypesForWith.firstOrNull(function (inv) {
                                            return inv.name === inventoryVerbString;
                                        });
                                        // if the inventory entry has a matching verb pair ...
                                        if (inventoryVerb !== null) {
                                            inventoryEntriesAsVerbs.push({
                                                id: inventoryVerb.id,
                                                name: x.name,
                                                call: inventoryVerb.call
                                            });
                                            // otherwise, what do we care?
                                        } else {
                                            inventoryEntriesAsVerbs.push({
                                                id: -1,
                                                name: x.name,
                                                call: function () {
                                                    // TODO: Failure message
                                                }
                                            });
                                        }
                                    });
                                } else {
                                    inventoryEntriesAsVerbs.push(verbTypesWhenEmpty);
                                }
                                that.setActiveVerbTypes(inventoryEntriesAsVerbs);
                                needsToReload = false;
                                resolve();
                            // no need to reload
                            } else {
                                that.setActiveVerbTypes(inventoryEntriesAsVerbs);
                                resolve();
                            }
                        }
                        set();
                    });
                };

                this.openForWith = function (clientX, clientY) {
                    that.setActiveVerbTypesForInventory().then(function () {
                        that.open(clientX, clientY);
                    });
                };

                this.closeAndReopenForWith = function (clientX, clientY) {
                    if (that.state === that.states.updating) {
                        return;
                    }
                    that.setActiveVerbTypesForInventory().then(function () {
                        that.closeAndReopen(clientX, clientY);
                    });
                };

                var loadWithVerbTypes = function (verbTypesData) {
                    var i = 0;
                    var len = verbTypesData.length;
                    for (; i < len; ++i) {
                        var vt = verbTypesData[i];
                        verbTypesForWith[i] = {
                            id: vt.id,
                            name: vt.name.toLowerCase(), // store as lower case because it's easier to compare against when creating a list of verbs from inventory items
                            call: function (vId) {
                                messengerEngine.post("WithList.wordClick", vId);
                            }
                        };
                    }

                    messengerEngine.unregister("GameStateEngine.loadWithVerbTypes");
                };

                var updateNeedsToReload = function () {
                    needsToReload = true;
                };

                messengerEngine.register("GameStateEngine.loadWithVerbTypes", that, loadWithVerbTypes);
                messengerEngine.register("ServicesEngine.loadGame", that, updateNeedsToReload);
                messengerEngine.register("ServicesEngine.doAction", that, updateNeedsToReload);
                messengerEngine.register("ServicesEngine.doMessageChoice", that, updateNeedsToReload);
                messengerEngine.register("InterfaceManager.clockClick", that, that.close);
                messengerEngine.register("VerbList.paragraphClick", that, that.close);
                messengerEngine.register("VerbList.wordClick", that, that.close);
            };

            WithList.prototype = new namespace.Entities.Classes.PopUpList(withListConstructorObject, messengerEngine);

            var ClockList = function () {
                var verbs = [{
                    id: -1,
                    name: "Pause",
                    call: function () {
                        messengerEngine.post("ClockList.pauseClick", true);
                    }
                }, {
                    id: -2,
                    name: "Wait",
                    call: function () {
                        messengerEngine.post("ClockList.waitClick", true);
                    }
                }];
                var timeSelector = null;

                var that = this;

                this.openForClock = function (clientX, clientY) {
                    that.setActiveVerbTypes(verbs);
                    that.open(clientX, clientY);
                };

                var closeTimeSelector = function (accepted, hours, minutes) {
                    timeSelector = null;
                    if (accepted) {
                        messengerEngine.post("ClockList.waitTime", hours, minutes);
                    }
                };

                var openTimeSelector = function () {
                    timeSelector = new namespace.Entities.Classes.TimeSelector(messengerEngine);
                    timeSelector.then(closeTimeSelector);
                };

                messengerEngine.register("InterfaceManager.iParagraphClick", that, that.close);
                messengerEngine.register("InterfaceManager.iWordClick", that, that.close);
                messengerEngine.register("ClockList.pauseClick", that, that.close);
                messengerEngine.register("ClockList.waitClick", that, that.close);
                messengerEngine.register("ClockList.waitClick", that, openTimeSelector);
            };

            ClockList.prototype = new namespace.Entities.Classes.PopUpList(clockListConstructorObject, messengerEngine);

            var activeId = null;
            var withList = new WithList();
            var verbList = new VerbList(verbListConstructorObject.idOfPopUpList, withList);
            var clockList = new ClockList();

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
                } else {
                    verbList.openForParagraphs(clientX, clientY);
                }
                activeId = psId;
            };

            var closeAndReopenVerbListForWords = function (clientX, clientY, wId) {
                if (verbList.state === verbList.states.open) {
                    verbList.closeAndReopenForWords(clientX, clientY);
                } else {
                    verbList.openForWords(clientX, clientY);
                }
                activeId = wId;
            };

            var openClockList = function (clientX, clientY) {
                if (clockList.state === clockList.states.closed) {
                    clockList.openForClock(clientX, clientY);
                }
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
            messengerEngine.register("InterfaceManager.clockClick", this, openClockList);
            messengerEngine.register("VerbList.paragraphClick", this, getNounsForParagraphState);
            messengerEngine.register("VerbList.wordClick", this, doAction);
        }
    };
}(window.GinTub = window.GinTub || {}));
