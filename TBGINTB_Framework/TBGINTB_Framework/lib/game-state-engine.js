
var GameStateEngine = function () {
    var messengerEngine = globalMessengerEngine;

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

    var that = this;

    var loadAllVerbTypes = function (verbUseData) {
        verbTypes = [];
        var i = 0,
            len = verbUseData.verbTypes.length;
        for (; i < len; ++i) {
            var v = verbUseData.verbTypes[i];
            verbTypes.push({
                id: v.id,
                name: v.name
            });
        }
        messengerEngine.post("GameStateEngine.loadAllVerbTypes", verbTypes);
    };

    var setArea = function (areaData) {
        area = {
            id: areaData.id,
            name: areaData.name
        };
        messengerEngine.post("GameStateEngine.setArea", area);
    };

    var setRoom = function (roomData) {
        room = {
            id: roomData.id,
            name: roomData.name,
            x: roomData.x,
            y: roomData.y,
            z: roomData.z
        };
        messengerEngine.post("GameStateEngine.setRoom", room);
    };

    var setRoomStates = function (roomStateData) {
        roomStates = [];
        var i = 0, 
            len = roomStateData.length;
        for (; i < len; ++i) {
            rsd = roomStateData[i];
            roomStates[i] = {
                id: rsd.id,
                state: rsd.state,
                time: moment.duration.fromIsoduration(rsd.time),
                location: rsd.location
            }
        }
        messengerEngine.post("GameStateEngine.setRoomStates", roomStates);
    };

    var getRemovedParagraphStates = function (paragraphStateData) {
        var removedParagraphStates = [];
        var i = 0,
            len = paragraphStates.length;
        for (; i < len; ++i) {
            var j = 0,
                len2 = paragraphStateData.length,
                contains = false;
            for (; j < len2; ++j) {
                if (paragraphStateData[j].id == paragraphStates[i].id) {
                    contains = true;
                    break;
                }
            }
            if (!contains) {
                removedParagraphStates.push(paragraphStates[i]);
            }
        }
        return removedParagraphStates;
    };

    var getAddedParagraphStates = function (paragraphStateData) {
        var addedParagraphStates = [];
        var i = 0,
            len = paragraphStateData.length;
        for (; i < len; ++i) {
            var j = 0,
                len2 = paragraphStates.length,
                contains = false;
            for (; j < len2; ++j) {
                if (paragraphStates[j].id == paragraphStateData[i]) {
                    contains = true;
                    break;
                }
            }
            if (!contains) {
                addedParagraphStates.push(paragraphStateData[i]);
            }
        }
        return addedParagraphStates;
    };

    var setParagraphStates = function (paragraphStateData) {
        var removedParagraphStates = getRemovedParagraphStates(paragraphStateData);
        var addedParagraphStates = getAddedParagraphStates(paragraphStateData);
        paragraphStates = [];
        var i = 0,
            len = paragraphStateData.length;
        for (; i < len; ++i) {
            psd = paragraphStateData[i];
            paragraphStates.push({
                id: psd.id,
                order: psd.order,
                roomState: psd.roomState,
                words: psd.words
            });
        }
        messengerEngine.post("GameStateEngine.removedParagraphStates", removedParagraphStates);
        messengerEngine.post("GameStateEngine.addedParagraphStates", addedParagraphStates);
        messengerEngine.post("GameStateEngine.setParagraphStates", paragraphStates);
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
            if (ps.roomState == null || ps.roomState == roomStateId) {
                paragraphStatesForRoomState.push(ps);
            }
        }
        paragraphStatesForRoomState.sort(function (a, b) {
            return a.order - b.order;
        });
        return paragraphStatesForRoomState;
    };

    var setActiveRoomState = function () {
        var i = roomStates.length - 1;
        // our roomStates are ordered by .Time, so
        // we can start at the beginning and move forward until we
        // find the first roomState where .Time <= var time
        for (; i >= 0; --i) {
            var rs = roomStates[i];
            if (gameTime >= rs.time) {
                // no need to update if it's the same roomstate
                if (activeRoomState == rs) {
                    break;
                }
                activeRoomState = rs;
                var pss = getParagraphStatesForRoomState(rs.id);
                messengerEngine.post("GameStateEngine.setActiveRoomState", rs, pss);
                break;
            }
        }
    };

    var loadGame = function (playData) {
        setArea(playData.area);
        setRoom(playData.room);
        setRoomStates(playData.roomStates);
        setParagraphStates(playData.paragraphStates);

        setActiveRoomState();
    };

    var loadMessage = function (playData) {
        setMessage(playData.message);
    };

    var updateTime = function (time) {
        gameTime = time;

        setActiveRoomState();
    };

    var doAction = function (nounId, verbTypeId) {
        messengerEngine.post("GameStateEngine.doAction", playerId, nounId, verbTypeId);
    };
    
    messengerEngine.register("ServicesEngine.getAllVerbTypes", this, loadAllVerbTypes);
    messengerEngine.register("ServicesEngine.loadGame", this, loadGame);
    messengerEngine.register("ServicesEngine.getNounsForParagraphState", this, loadMessage);
    messengerEngine.register("ServicesEngine.doAction", this, loadMessage);
    messengerEngine.register("TimeEngine.updateTimeAtTen", this, updateTime);
    messengerEngine.register("UserInputManager.doAction", this, doAction);
};