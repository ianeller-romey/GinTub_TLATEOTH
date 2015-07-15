
var MessengerEngine = function () {
    this.messages = {};
};

MessengerEngine.prototype.register = function (message, object, funct) {
    if (this.messages[message] === undefined || this.messages[message] === null) {
        this.messages[message] = new ActionArray();
    }

    this.messages[message].push(object, funct);
};

MessengerEngine.prototype.unregister = function (message, funct) {
    if (this.messages[message] === undefined || this.messages[message] === null) {
        return "Cannot unregister from a message that doesn't exist.";
    }

    var actionArray = this.messages[message];
    var index = actionArray.array.indexOf(funct);
    if (index > -1) {
        actionArray.array.splice(index, 1);
    }

    if (actionArray.isEmpty()) {
        this.messages[message] = null;
    }
};

MessengerEngine.prototype.post = function (message) {
    if (this.messages[message] === undefined || this.messages[message] === null) {
        return "Cannot post a message that doesn't exist.";
    }

    var params = Array.prototype.slice.call(arguments, 1);
    var actionArray = this.messages[message];
    actionArray.applyAllWithParams.apply(actionArray, params);
};

var globalMessengerEngine = new MessengerEngine();