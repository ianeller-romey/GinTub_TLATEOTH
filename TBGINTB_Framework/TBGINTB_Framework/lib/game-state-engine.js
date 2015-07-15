
var GameStateEngine = function () {
    var messengerEngine = globalMessengerEngine;

    var playerId = sessionStorage.playerId;
    var gameTime = new Date(1988, 7, 13, 0, 0, 0, 0);
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
            len = verbUseData.VerbTypes.length;
        for (; i < len; ++i) {
            var v = verbUseData.VerbTypes[i];
            verbTypes.push({
                id: v.Id,
                name: v.Name
            });
        }
        messengerEngine.post("GameStateEngine.loadAllVerbTypes", verbTypes);
    };

    var setArea = function (areaData) {
        area = {
            id: areaData.Id,
            name: areaData.Name
        };
        messengerEngine.post("GameStateEngine.setArea", area);
    };

    var setRoom = function (roomData) {
        room = {
            id: roomData.Id,
            name: roomData.Name,
            x: roomData.X,
            y: roomData.Y,
            z: roomData.Z
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
                id: rsd.Id,
                state: rsd.State,
                time: new Date().fromMSJSON(rsd.Time),
                location: rsd.Location
            }
        }
        messengerEngine.post("GameStateEngine.setRoomStates", roomStates);
    };

    var setParagraphStates = function (paragraphStateData) {
        paragraphStates = [];
        var i = 0,
            len = paragraphStateData.length;
        for (; i < len; ++i) {
            psd = paragraphStateData[i];
            paragraphStates[i] = {
                id: psd.Id,
                order: psd.Order,
                roomState: psd.RoomState,
                words: psd.Words
            };
        }
        messengerEngine.post("GameStateEngine.setParagraphStates", paragraphStates);
    };

    var setMessage = function (messageData) {
        if (messageData != null) {
            message = {
                id: messageData.Id,
                text: messageData.Text,
                messageChoices: messageData.MessageChoices
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
        setArea(playData.Area);
        setRoom(playData.Room);
        setRoomStates(playData.RoomStates);
        setParagraphStates(playData.ParagraphStates);

        setActiveRoomState();
    };

    var loadMessage = function (playData) {
        setMessage(playData.Message);
    };

    var updateTime = function (time) {
        gameTime = time;

        setActiveRoomState();
    };

    var doAction = function (nounId, verbTypeId) {
        messengerEngine.post("GameStateEngine.doAction", playerId, nounId, verbTypeId);
    };
    
    messengerEngine.register("ServicesEngine.loadAllVerbTypes", this, loadAllVerbTypes);
    messengerEngine.register("ServicesEngine.loadGame", this, loadGame);
    messengerEngine.register("ServicesEngine.getNounsForParagraphState", this, loadMessage);
    messengerEngine.register("ServicesEngine.doAction", this, loadMessage);
    messengerEngine.register("TimeEngine.updateTimeAtTen", this, updateTime);
    messengerEngine.register("UserInputManager.doAction", this, doAction);
};