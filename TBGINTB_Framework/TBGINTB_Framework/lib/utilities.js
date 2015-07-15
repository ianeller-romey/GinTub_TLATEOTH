
Date.prototype.toMSJSON = function () {
    var date = '/Date(' + this.getTime() + ')/';
    return date;
};

Date.prototype.fromMSJSON = function (msJSON) {
    var date = new Date(parseInt(msJSON.substr(6)));
    return date;
};