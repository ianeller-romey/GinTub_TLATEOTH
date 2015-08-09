
String.prototype.isNullOrWhitespace = function () {
    return this === null || this.match(/^\s*$/) !== null;
};

Array.prototype.any = function (predicate) {
    var contains = false;
    for (var i = 0, len = this.length; i < len; ++i) {
        if (predicate(this[i])) {
            contains = true;
            break;
        }
    }
    return contains;
};

Array.prototype.firstOrNull = function (predicate) {
    var firstObject = null;
    for (var i = 0, len = this.length; i < len; ++i) {
        if (predicate(this[i])) {
            firstObject = this[i];
            break;
        }
    }
    return firstObject;
};

Array.prototype.select = function (predicate) {
    var selectArray = [];
    for (var i = 0, len = this.length; i < len; ++i) {
        selectArray.push(predicate(this[i]));
    }
    return selectArray;
};

Array.prototype.where = function (predicate) {
    var whereArray = [];
    for (var i = 0, len = this.length; i < len; ++i) {
        if (predicate(this[i])) {
            whereArray.push(this[i]);
        }
    }
    return whereArray;
};

(jQuery.fn.extend({
    animateAuto: function (prop, speed, callback) {
        var elem;
        var height;
        var width;
        return this.each(function (i, el) {
            el = jQuery(el);
            elem = el.clone().css({ "height": "auto", "width": "auto" }).appendTo("body");
            height = elem.css("height"),
            width = elem.css("width"),
            elem.remove();

            if (prop === "height") {
                el.animate({ "height": height }, speed, callback);
            }
            else if (prop === "width") {
                el.animate({ "width": width }, speed, callback);
            }
            else if (prop === "both") {
                el.animate({ "width": width, "height": height }, speed, callback);
            }
        });
    },
    animateTextAdd: function (text, interval) {
        var that = this;

        var promise = new Promise(function (resolve, reject) {
            var index = 0;
            var animateTextAddAtInterval = function () {
                if (index < text.length) {
                    that.append(text[index++]);

                    var updateInterval = (typeof interval == "function") ? interval() : interval;
                    setTimeout(function () { animateTextAddAtInterval(); }, updateInterval);
                }
                else {
                    resolve();
                }
            };
            animateTextAddAtInterval();
        });
        return promise;
    },
    animateTextRemove: function (interval) {
        var that = this;

        var promise = new Promise(function (resolve, reject) {
            var animateTextRemoveAtInterval = function () {
                var text = that.text();
                if (text.length == 0) {
                    that.remove();
                    resolve();
                }
                else {
                    that.text(text.slice(0, -1));

                    var updateInterval = (typeof interval == "function") ? interval() : interval;
                    setTimeout(function () { animateTextRemoveAtInterval(); }, updateInterval);
                }
            };
            animateTextRemoveAtInterval();
        });
        return promise;

    }
}));