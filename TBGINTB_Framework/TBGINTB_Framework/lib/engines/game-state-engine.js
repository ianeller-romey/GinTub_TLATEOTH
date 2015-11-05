(function (namespace, undefined) {
    "use strict";

    namespace.Engines = namespace.Engines || {};
    namespace.Engines.GameStateEngine = {
        init: function (messengerEngine) {
            var playerId = sessionStorage.playerId;
            var gameTime = new moment.duration.fromIsoduration("PT0S");
            var area = {
                id: null,
                name: null
            };
            var room = {
                id: null,
                name: null,
                x: null,
                y: null,
                z: null
            };
            var verbTypes = [];
            var roomStates = [];
            var paragraphStates = [];
            var message = {
                id: null,
                text: null,
                messageChoices: []
            };
            var activeRoomState = null;

            var playerInventory = {
            };
            var playerHistory = {
            };
            var playerParty = {
            };

            var gameLoaded = false;

            var that = this;

            var loadAllVerbTypes = function (verbUseData) {
                messengerEngine.post("GameStateEngine.loadVerbTypes", verbUseData.verbTypes);
                messengerEngine.post("GameStateEngine.loadWithVerbTypes", verbUseData.withVerbTypes);
            };

            var setTime = function (time) {
                gameTime = time;
            };

            var setTimeAtTen = function (time) {
                gameTime = time;

                setActiveRoomState(createImmediateParagraphStateUpdatePromise());
            };
            
            var setGameState = function (gameStateData) {
                if (gameStateData) { // intentional truthiness
                    if(gameStateData.lastTime !== null) {
                        setTime(gameStateData.lastTime);
                    }
                    if(gameStateData.stopTime !== null) {
                        messengerEngine.post("GameStateEngine.stopTime", gameStateData.stopTime);
                    }
                }
            };

            var setArea = function (areaData) {
                if (areaData != null) {
                    area = areaData;

                    messengerEngine.post("GameStateEngine.setArea", area);
                }
            };

            var setRoom = function (roomData) {
                if (roomData != null) {
                    room = roomData;

                    messengerEngine.post("GameStateEngine.setRoom", room);
                }
            };

            var setRoomStates = function (roomStateData) {
                if (roomStateData != null) {
                    roomStates = roomStateData.select(function (roomState) {
                        return {
                            id: roomState.id,
                            state: roomState.state,
                            time: moment.duration.fromIsoduration(roomState.time),
                            location: roomState.location
                        };
                    });

                    messengerEngine.post("GameStateEngine.setRoomStates", roomStates);
                }
            };

            var setParagraphStates = function (paragraphStateData) {
                if (paragraphStateData != null) {
                    paragraphStates = paragraphStateData;

                    messengerEngine.post("GameStateEngine.setParagraphStates", paragraphStates);
                }
            };

            var setMessage = function (messageData) {
                if (messageData != null) {
                    message = {
                        id: messageData.id,
                        text: messageData.text,
                        messageChoices: messageData.messageChoices
                    };

                    messengerEngine.post("GameStateData.setMessage", message);
                }
            };

            var getParagraphStatesForRoomState = function (roomStateId) {
                var paragraphStatesForRoomState = [];
                var i = 0,
                    len = paragraphStates.length;
                for (; i < len; ++i) {
                    var ps = paragraphStates[i];
                    if (ps.roomState === null || ps.roomState === roomStateId) {
                        paragraphStatesForRoomState.push(ps);
                    }
                }
                paragraphStatesForRoomState.sort(function (a, b) {
                    return a.order - b.order;
                });
                return paragraphStatesForRoomState;
            };

            var setActiveRoomState = function (paragraphStateUpdatePromise) {
                var i = roomStates.length - 1;
                // our roomStates are ordered by .Time, so
                // we can start at the beginning and move forward until we
                // find the first roomState where .Time <= var time
                var rs;
                for (; i >= 0; --i) {
                    rs = roomStates[i];
                    if (gameTime >= rs.time) {
                        // break, because we still need to update the paragraph states if we're in the same room
                        if (activeRoomState != null && activeRoomState.id === rs.id) {
                            break;
                        }
                        activeRoomState = rs;
                        break;
                    }
                }
                var pss = getParagraphStatesForRoomState(rs.id);
                messengerEngine.post("GameStateEngine.setActiveRoomState", room.id, rs, pss, paragraphStateUpdatePromise);
            };

            var createImmediateParagraphStateUpdatePromise = function () {
                return new Promise(function (resolve, reject) {
                    resolve();
                });
            };

            var createOnCloseMessageParagraphStateUpdatePromise = function () {
                return new Promise(function (resolve, reject) {
                    var resolveOnMessage;
                    var onCloseMessageParagraphStateUpdate = function () {
                        return function () {
                            resolve();
                            messengerEngine.unregister("MessageManager.closeMessageManager", resolveOnMessage);
                        };
                    };
                    resolveOnMessage = onCloseMessageParagraphStateUpdate();

                    messengerEngine.register("MessageManager.closeMessageManager", this, resolveOnMessage);
                });
            };

            var registerAfterGameHasLoaded = function () {
                messengerEngine.register("ServicesEngine.getNounsForParagraphState", that, loadMessage);
                messengerEngine.register("ServicesEngine.doAction", that, loadActionResults);
                messengerEngine.register("ServicesEngine.doMessageChoice", that, loadActionResults);

                messengerEngine.register("TimeEngine.updateTime", that, setTime);
                messengerEngine.register("TimeEngine.updateTimeAtTen", that, setTimeAtTen);

                messengerEngine.register("UserInputManager.doAction", that, doAction);

                messengerEngine.register("MessageManager.messageChoiceClick", that, doMessageChoice);

                messengerEngine.register("MenuEntry.mapRequest", that, mapRequest);
                messengerEngine.register("MenuEntry.inventoryRequest", that, inventoryRequest);
                messengerEngine.register("MenuEntry.historyRequest", that, historyRequest);
                messengerEngine.register("MenuEntry.partyRequest", that, partyRequest);

                messengerEngine.register("WithList.inventoryRequest", that, inventoryRequest);

                gameLoaded = true;
            };

            var loadGame = function (playData) {
                if (!gameLoaded) {
                    registerAfterGameHasLoaded();
                }
                setGameState(playData.gameState);
                setArea(playData.area);
                setRoom(playData.room);
                setRoomStates(playData.roomStates);
                setParagraphStates(playData.paragraphStates);

                setActiveRoomState(createImmediateParagraphStateUpdatePromise());
            };

            var loadActionResults = function (playData) {
                setGameState(playData.gameState);
                setArea(playData.area);
                setRoom(playData.room);
                setRoomStates(playData.roomStates);
                setParagraphStates(playData.paragraphStates);
                setMessage(playData.message);

                if (playData.message == null) {
                    setActiveRoomState(createImmediateParagraphStateUpdatePromise());
                }
                else {
                    setActiveRoomState(createOnCloseMessageParagraphStateUpdatePromise());
                };
            };

            var loadMessage = function (playData) {
                setMessage(playData.message);
            };

            var doAction = function (nounId, verbTypeId) {
                messengerEngine.post("GameStateEngine.doAction", playerId, nounId, verbTypeId, gameTime);
            };

            var doMessageChoice = function (messageChoiceId) {
                messengerEngine.post("GameStateEngine.doMessageChoice", playerId, messageChoiceId);
            };

            var mapRequest = function () {
                messengerEngine.post("GameStateEngine.mapRequest", area.id, playerId);
            };

            var inventoryRequest = function () {
                messengerEngine.post("GameStateEngine.inventoryRequest", playerId);
            };

            var historyRequest = function () {
                messengerEngine.post("GameStateEngine.historyRequest", playerId);
            };

            var partyRequest = function () {
                messengerEngine.post("GameStateEngine.partyRequest", playerId);
            };

            messengerEngine.register("ServicesEngine.getAllVerbTypes", this, loadAllVerbTypes);
            messengerEngine.register("ServicesEngine.loadGame", this, loadGame);
        }
    };
}(window.GinTub = window.GinTub || {}));