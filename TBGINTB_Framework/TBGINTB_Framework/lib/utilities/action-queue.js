
var ActionQueue = function (maxSize) {
    this.queue = [];
    this.maxSize = maxSize;
};

ActionQueue.prototype.push = function (caller, called, params) {
    if (this.maxSize !== null && this.queue.length === this.maxSize) {
        return "Maximum size reached.";
    }
    var parameters = [];
    var i = 2; // start at 2, since we handle "caller" and "called" individually
    for (; i < arguments.length; ++i) {
        parameters.push(arguments[i]);
    }
    this.queue.push({
        calledBy: caller,
        toCall: called,
        calledWith: parameters
    });
};

ActionQueue.prototype.pop = function () {
    if (this.queue.length === 0) {
        return;
    }
    var action = this.queue[0];
    action.toCall.apply(action.calledBy, action.calledWith);
    this.queue = this.queue.slice(1, this.queue.length);
};

ActionQueue.prototype.isEmpty = function () {
    return this.queue.length === 0;
};