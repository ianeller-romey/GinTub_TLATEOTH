
var ServicesEngine = function () {
    this.messengerEngine = globalMessengerEngine;

    this.messengerEngine.register("UserInputManager.getNounsForParagraphState", this, this.getNounsForParagraphState);
    this.messengerEngine.register("GameStateEngine.doAction", this, this.doAction);
};

ServicesEngine.prototype.getAllVerbTypes = function () {
    var that = this;

    $.ajax({
        url: "http://ironandrose/gintub/lion/gintubservices/GinTubService.svc/GetAllVerbTypes",
        type: 'get',
        dataType: 'text',
        contentType: 'application/json',
        success: function (data, status) {
            var verbUseData = JSON.parse(data);
            that.messengerEngine.post("ServicesEngine.getAllVerbTypes", verbUseData);
        },
        error: function (request, status, error) {
            var iii = 0;
        }
    });
};

ServicesEngine.prototype.getNounsForParagraphState = function (paragraphStateId) {
    var that = this;

    $.ajax({
        url: "http://ironandrose/gintub/lion/gintubservices/GinTubService.svc/GetNounsForParagraphState/" + paragraphStateId,
        type: 'get',
        dataType: 'text',
        contentType: 'application/json',
        success: function (data, status) {
            var nounData = JSON.parse(data);
            that.messengerEngine.post("ServicesEngine.getNounsForParagraphState", nounData);
        },
        error: function (request, status, error) {
            var iii = 0;
        }
    });
};

ServicesEngine.prototype.loadGame = function (playerId) {
    var that = this;

    $.ajax({
        url: "http://ironandrose/gintub/lion/gintubservices/GinTubService.svc/LoadGame",
        type: 'post',
        dataType: 'text',
        contentType: 'application/json',
        data: JSON.stringify({
            playerId: playerId
        }),
        success: function (data, status) {
            var playData = JSON.parse(data);
            that.messengerEngine.post("ServicesEngine.loadGame", playData);
        },
        error: function (request, status, error) {
            var iii = 0;
        }
    });
};

ServicesEngine.prototype.doAction = function (playerId, nounId, verbTypeId) {
    var that = this;

    $.ajax({
        url: "http://ironandrose/gintub/lion/gintubservices/GinTubService.svc/DoAction",
        type: 'post',
        dataType: 'text',
        contentType: 'application/json',
        data: JSON.stringify({
            playerId: playerId,
            nounId: nounId,
            verbTypeId: verbTypeId
        }),
        success: function (data, status) {
            var playData = JSON.parse(data);
            that.messengerEngine.post("ServicesEngine.doAction", playData);
        },
        error: function (request, status, error) {
            var iii = 0;
        }
    });
};