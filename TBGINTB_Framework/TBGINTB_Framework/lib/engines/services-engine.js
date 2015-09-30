(function (namespace, undefined) {
    "use strict";

    namespace.Engines = namespace.Engines || {};
    namespace.Engines.ServicesEngine = {
        init: function (messengerEngine) {
            var postAjaxError = function (status, error) {
                messengerEngine.post("ServicesEngine.ajaxError", "Ajax Error: " + status + " - " + error);
            };

            this.getAllVerbTypes = function () {
                var that = this;

                $.ajax({
                    url: "gintubservices/GinTubService.svc/GetAllVerbTypes",
                    type: 'get',
                    dataType: 'text',
                    contentType: 'application/json',
                    success: function (data, status) {
                        var verbUseData = JSON.parse(data);
                        messengerEngine.post("ServicesEngine.getAllVerbTypes", verbUseData);
                    },
                    error: function (request, status, error) {
                        postAjaxError(status, error);
                    }
                });
            };

            this.getNounsForParagraphState = function (paragraphStateId) {
                var that = this;

                $.ajax({
                    url: "gintubservices/GinTubService.svc/GetNounsForParagraphState/" + paragraphStateId,
                    type: 'get',
                    dataType: 'text',
                    contentType: 'application/json',
                    success: function (data, status) {
                        var nounData = JSON.parse(data);
                        messengerEngine.post("ServicesEngine.getNounsForParagraphState", nounData);
                    },
                    error: function (request, status, error) {
                        postAjaxError(status, error);
                    }
                });
            };

            this.loadGame = function (playerId) {
                var that = this;

                $.ajax({
                    url: "gintubservices/GinTubService.svc/LoadGame",
                    type: 'post',
                    dataType: 'text',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        playerId: playerId
                    }),
                    success: function (data, status) {
                        var playData = JSON.parse(data);
                        messengerEngine.post("ServicesEngine.loadGame", playData);
                    },
                    error: function (request, status, error) {
                        postAjaxError(status, error);
                    }
                });
            };

            this.doAction = function (playerId, nounId, verbTypeId) {
                var that = this;

                $.ajax({
                    url: "gintubservices/GinTubService.svc/DoAction",
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
                        messengerEngine.post("ServicesEngine.doAction", playData);
                    },
                    error: function (request, status, error) {
                        postAjaxError(status, error);
                    }
                });
            }

            this.mapRequest = function (areaId, playerId) {
                var that = this;

                $.ajax({
                    url: "gintubservices/GinTubService.svc/GetMapForPlayer/" + areaId + "/" + playerId,
                    type: 'get',
                    dataType: 'text',
                    contentType: 'application/json',
                    success: function (data, status) {
                        var mapData = JSON.parse(data);
                        messengerEngine.post("ServicesEngine.mapRequest", mapData);
                    },
                    error: function (request, status, error) {
                        postAjaxError(status, error);
                    }
                });
            }

            this.inventoryRequest = function (playerId) {
                var that = this;

                $.ajax({
                    url: "gintubservices/GinTubService.svc/GetInventoryForPlayer/" + playerId,
                    type: 'get',
                    dataType: 'text',
                    contentType: 'application/json',
                    success: function (data, status) {
                        var inventoryData = JSON.parse(data);
                        messengerEngine.post("ServicesEngine.inventoryRequest", inventoryData);
                    },
                    error: function (request, status, error) {
                        postAjaxError(status, error);
                    }
                });
            }

            this.historyRequest = function (playerId) {
                var that = this;

                $.ajax({
                    url: "gintubservices/GinTubService.svc/GetHistoryForPlayer/" + playerId,
                    type: 'get',
                    dataType: 'text',
                    contentType: 'application/json',
                    success: function (data, status) {
                        var historyData = JSON.parse(data);
                        messengerEngine.post("ServicesEngine.historyRequest", historyData);
                    },
                    error: function (request, status, error) {
                        postAjaxError(status, error);
                    }
                });
            }

            this.partyRequest = function (playerId) {
                var that = this;

                $.ajax({
                    url: "gintubservices/GinTubService.svc/GetPartyForPlayer/" + playerId,
                    type: 'get',
                    dataType: 'text',
                    contentType: 'application/json',
                    success: function (data, status) {
                        var partyData = JSON.parse(data);
                        messengerEngine.post("ServicesEngine.partyRequest", partyData);
                    },
                    error: function (request, status, error) {
                        postAjaxError(status, error);
                    }
                });
            }

            messengerEngine.register("UserInputManager.getNounsForParagraphState", this, this.getNounsForParagraphState);
            messengerEngine.register("GameStateEngine.doAction", this, this.doAction);
            messengerEngine.register("GameStateEngine.mapRequest", this, this.mapRequest);
            messengerEngine.register("GameStateEngine.inventoryRequest", this, this.inventoryRequest);
            messengerEngine.register("GameStateEngine.historyRequest", this, this.historyRequest);
            messengerEngine.register("GameStateEngine.partyRequest", this, this.partyRequest);

            if (namespace.EX && namespace.EX.Cheats) { // intentional truthiness
                this.doCheat = function (playerId, cheat, jsonObject) {
                    var that = this;

                    if (!jsonObject) { // intentional truthiness
                        jsonObject = {};
                    }

                    $.ajax({
                        url: "gintubservices/GinTubService.svc/DoCheat",
                        type: 'post',
                        dataType: 'text',
                        contentType: 'application/json',
                        data: JSON.stringify({
                            playerId: playerId,
                            cheat: cheat,
                            jsonString: JSON.stringify(jsonObject)
                        }),
                        success: function (data, status) {
                            var playData = JSON.parse(data);
                            messengerEngine.post("ServicesEngine.loadGame", playData);
                        },
                        error: function (request, status, error) {
                            postAjaxError(status, error);
                        }
                    });
                };
                messengerEngine.register("CHEAT", this, this.doCheat);
            }
        }
    };
}(window.GinTub = window.GinTub || {}));