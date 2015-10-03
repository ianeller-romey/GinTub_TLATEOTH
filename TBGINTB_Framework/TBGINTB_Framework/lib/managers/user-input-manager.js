(function (namespace, undefined) {
    "use strict";

    namespace.Managers = namespace.Managers || {};
    namespace.Managers.UserInputManager = {
        init: function (verbListId, withListId, interfaceId, messengerEngine) {
            var PopUpList = function (idOfPopupList, idOfParent, className) {
                var popupListElem = $(idOfPopupList);
                var popupListElemX = $(idOfPopupList + "X");
                var parentElem = $(idOfParent);

                this.states = {
                    closed: 0,
                    updating: 1,
                    open: 2
                };
                this.state = this.states.closed;
                var actionQueue = new ActionQueue(3);   
                var activeVerbTypes = null;

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

                var openExec = function (clientX, clientY) {
                    if (that.state === that.states.open) {
                        return;
                    }

                    var i = 0;
                    var len = activeVerbTypes.length;
                    for (; i < len; ++i) {
                        var avt = activeVerbTypes[i];
                        popupListElem.append(namespace.Entities.Factories.createActionText(avt.id, avt.name, avt.call));
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
                            $(idOfPopupList + " .actionText").remove();
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

                this.open = function (clientX, clientY) {
                    if (that.state === that.states.updating) {
                        return;
                    }
                    that.state = that.states.updating;
                    actionQueue.push(that, openExec, clientX, clientY);
                    update();
                };

                this.close = function () {
                    if (that.state === that.states.updating) {
                        return;
                    }
                    that.state = that.states.updating;
                    actionQueue.push(that, closeExec);
                    update();
                };


                this.closeAndReopen = function (clientX, clientY) {
                    that.state = that.states.updating;
                    actionQueue.push(that, closeExec);
                    actionQueue.push(that, openExec, clientX, clientY);
                    update();
                };

                messengerEngine.register("VerbList.paragraphClick", this, this.close);
                messengerEngine.register("VerbList.wordClick", this, this.close);
            };
            
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
                                var el = $(verbListId);
                                var pos = el.offset();
                                withList.openForWith(pos.left + (el.outerWidth() / 2), pos.top + (el.outerHeight() / 2));
                            }
                        };
                    }

                    messengerEngine.unregister("GameStateEngine.loadVerbTypes");
                };

                messengerEngine.register("GameStateEngine.loadVerbTypes", that, loadVerbTypes);
            };

            VerbList.prototype = new PopUpList(verbListId, interfaceId, "VerbList");

            var WithList = function () {
                var verbTypesForWith = [];
                var inventoryEntriesAsVerbs = [];
                var needsToReload = true;

                var that = this;

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

                                inventory.inventoriesEntries.where(function (x) {
                                    return x.acquired === true;
                                }).forEach(function (x) {
                                    var inventoryVerbString = "with " + x.name.toLowerCase();
                                    var inventoryVerb = verbTypesForWith.firstOrNull(function (inv) {
                                        return inv.name === inventoryVerbString;
                                    });
                                    // if the inventory entry has a matching verb pair ...
                                    if(inventoryVerb !== null) {
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

                messengerEngine.register("GameStateEngine.loadWithVerbTypes", that, loadWithVerbTypes);
            };

            WithList.prototype = new PopUpList(withListId, interfaceId, "WithList");

            var activeId = null;
            var withList = new WithList();
            var verbList = new VerbList(verbListId, withList);

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
        }
    };
}(window.GinTub = window.GinTub || {}));
