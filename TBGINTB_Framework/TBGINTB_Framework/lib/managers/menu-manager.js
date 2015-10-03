(function (namespace, undefined) {
    "use strict";

    namespace.Managers = namespace.Managers || {};
    namespace.Managers.MenuManager = {
        init: function (menuId, menuButtonId, menuEntriesId, menuFrameId, displayFrameId, descriptionFrameId, messengerEngine) {
            var MenuEntry = function (menuEntriesId, menuFrameId, displayFrameId, descriptionFrameId) {
                this.elem = null;
                this.needToReload = true; // TODO: Update this based on ServicesEngine.doAction
                this.isActive = false;
                this.dData = null;
            };

            MenuEntry.prototype.createClickSelector = function () {
                var that = this;
                var iSelClass = "iSelected";

                return function (senderText) {
                    if (senderText === that.entryText) {
                        if (!that.elem.hasClass(iSelClass)) {
                            that.isActive = true;
                            that.elem.addClass(iSelClass);
                        }
                    } else {
                        that.isActive = false;
                        that.elem.removeClass(iSelClass);
                    }
                };
            };

            MenuEntry.prototype.createDisplayEntry = function (displayText, descriptionText) {
                var entry = $("<span />", {
                    text: displayText
                });
                entry.mouseenter(function () {
                    $(descriptionFrameId).text(descriptionText);
                }).mouseleave(function () {
                    $(descriptionFrameId).empty();
                });
                return entry;
            };

            MenuEntry.prototype.createClickFunc = function () {
                var that = this;

                var dataLoaded = function (data) {
                    messengerEngine.unregister("ServicesEngine." + that.dataName, dataLoaded);
                    that.dData = data;
                };

                return function () {
                    messengerEngine.post("MenuEntry.click", that.entryText);

                    if (that.needToReload) {
                        that.dData = null;
                        messengerEngine.register("ServicesEngine." + that.dataName, that, dataLoaded);
                        messengerEngine.post("MenuEntry." + that.dataName, true);
                    }

                    var menuFrame = $(menuFrameId);
                    menuFrame.promiseToFade("fast", 0.0).then(function () {
                        if (that.isActive) {
                            $(displayFrameId).empty();
                            $(descriptionFrameId).empty();
                            var checkDataLoaded = function () {
                                if (that.dData !== null) {
                                    that.arrangeData(that.dData);
                                    menuFrame.promiseToFade("fast", 1.0);
                                } else {
                                    setTimeout(checkDataLoaded, 1);
                                }
                            }
                            checkDataLoaded();
                        }
                    });
                }
            };

            var MapEntry = function (entryText, dataName) {
                this.entryText = entryText;
                this.dataName = dataName;

                this.arrangeData = function (mapData) {
                    // TODO: Z changing
                    var mes = mapData.mapEntries;
                    var maxX = mes.max(function (m) {
                        return m.x;
                    });
                    var maxY = mes.max(function (m) {
                        return m.y;
                    });
                    var minZ = 0;/*mes.min(function(m) {
                        return m.z;
                    });*/
                    var maxZ = 0;/*mes.max(function(m) {
                        return m.z;
                    });*/
                    for (var x = 0; x <= maxX; ++x) {
                        for (var y = 0; y <= maxY; ++y) {
                            for (var z = minZ; z <= maxZ; ++z) {
                                var entry = mes.firstOrNull(function (m) {
                                    return m.x === x && m.y === y && m.z === 0;
                                });
                                var piece = (entry /* intentional truthiness */ && entry.visited) ? this.createDisplayEntry("⊕", entry.name) : this.createDisplayEntry(" ");
                                $(displayFrameId).append(piece);
                            }
                            $(displayFrameId).append("<br />");
                        }
                    }
                };

                messengerEngine.register("MenuEntry.click", this, this.createClickSelector());

                this.elem = namespace.Entities.Factories.createActionText(0, this.entryText, this.createClickFunc());
                $(menuEntriesId).append(this.elem).append("<br />");
            };

            MapEntry.prototype = new MenuEntry(menuEntriesId, menuFrameId, displayFrameId, descriptionFrameId);
            
            var InventoriesEntry = function (entryText, dataName) {
                this.entryText = entryText;
                this.dataName = dataName;

                this.arrangeData = function (inventoryData) {
                    var that = this;
                    var createDisplayEntryElem = function (x) {
                        return that.createDisplayEntry(x.name, x.description);
                    };

                    $(displayFrameId).appendJsonTable(2, inventoryData.inventoriesEntries, createDisplayEntryElem);
                };

                messengerEngine.register("MenuEntry.click", this, this.createClickSelector());

                this.elem = namespace.Entities.Factories.createActionText(0, this.entryText, this.createClickFunc());
                $(menuEntriesId).append(this.elem).append("<br />");
            };

            InventoriesEntry.prototype = new MenuEntry(menuEntriesId, menuFrameId, displayFrameId, descriptionFrameId);

            var menuElem = $(menuId);
            var menuButtonElem = $(menuButtonId);

            var mapEntry                = new MapEntry("LOCATION", "mapRequest");
            var inventoryEntry  = new InventoriesEntry("SUPPLIES", "inventoryRequest");
            var historyEntry    = new InventoriesEntry("JOURNAL", "historyRequest");
            var partyEntry      = new InventoriesEntry("COMPANY", "partyRequest");

            var menuHeights = {};
            menuHeights[true] = 360;
            menuHeights[false] = 34;
            var isMenuOpen = false;

            var that = this;

            var toggleMenu = function () {
                isMenuOpen = !isMenuOpen;
                menuElem.css("height", menuHeights[isMenuOpen]);
            };

            menuButtonElem.click(function () {
                toggleMenu();
            });
        }
    };
}(window.GinTub = window.GinTub || {}));
