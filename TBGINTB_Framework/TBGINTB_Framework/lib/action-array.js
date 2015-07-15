
var ActionArray = function (maxSize) {
    this.array = [];
    this.maxSize = maxSize;
};

ActionArray.prototype.push = function (caller, called, params) {
    if (this.maxSize !== null && this.array.length === this.maxSize) {
        return "Maximum size reached.";
    }
    var parameters = [];
    var i = 2; // start at 2, since we handle "caller" and "called" individually
    for (; i < arguments.length; ++i) {
        parameters.push(arguments[i]);
    }
    this.array.push({
        calledBy: caller,
        toCall: called,
        calledWith: parameters
    });
};

ActionArray.prototype.applyAll = function () {
    if (this.array.length === 0) {
        return;
    }
    this.array.forEach(function (element) {
        element.toCall(element.calledBy, element.calledWith);
    });
};

ActionArray.prototype.applyAllWithParams = function () {
    if (this.array.length === 0) {
        return;
    }
    var params = arguments;
    this.array.forEach(function (element) {
        element.toCall.apply(element.calledBy, params);
    });
};

ActionArray.prototype.isEmpty = function () {
    return this.array.length === 0;
};