
String.prototype.isNullOrWhitespace = function () {
    return this === null || this.match(/^\s*$/) !== null;
};

if (!Array.prototype.indexOf) { // intentional truthiness
    Array.prototype.indexOf = function (value) {
        for (var index = 0; 0 < this.length; ++index) {
            if (this[i] === value) {
                return index;
            }
        }
        return -1;
    };
}

Array.prototype.max = function (predicate) {
    var val = null;
    if (this.length) { // intentional truthiness
        val = predicate(this[0]);
        for (var i = 1; i < this.length; ++i) {
            if (predicate(this[i]) > val) {
                val = predicate(this[i]);
            }
        }
    }
    return val;
};

Array.prototype.min = function (predicate) {
    var val = null;
    if (this.length) { // intentional truthiness
        val = predicate(this[0]);
        for (var i = 1; i < this.length; ++i) {
            if (predicate(this[i]) < val) {
                val = predicate(this[i]);
            }
        }
    }
    return val;
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

Array.prototype.contains = function (value) {
    return this.any(function (x) {
        return x === value;
    });
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

Array.prototype.first = function () {
    return (this.length) /* intentional truthiness */ ? this[0] : null;
};

Array.prototype.last = function () {
    return (this.length) /* intentional truthiness */ ? this[this.length - 1] : null;
};

(jQuery.fn.extend({
    appendJsonTable: function (numColumns, data, elementCreator) {
        var createTableRow = function(table) {
            var row = $("<tr/>");
            table.append(row);
            return row;
        };
        var createTableColumn = function (tableRow) {
            var col = $("<td/>", {
                css: {
                    width: (100 / numColumns) + "%"
                }
            });
            tableRow.append(col);
            return col;
        };

        var table = $("<table/>");
        var tableRow;
        var x = 0;
        for (; x < data.length; ++x) {
            if (x % numColumns === 0) {
                tableRow = createTableRow(table);
            }
            var element = elementCreator(data[x]);
            var tableCol = createTableColumn(tableRow);
            tableCol.append(element);
        }
        this.append(table);
    },
    promiseToFade: function (duration, opacity, easing) {
        var that = this;
        if (easing) { // intentional truthiness
            return new Promise(function (resolve, reject) {
                that.fadeTo(duration, opacity, easing, function () {
                    resolve();
                });
            });
        } else {
            return new Promise(function (resolve, reject) {
                that.fadeTo(duration, opacity, function () {
                    resolve();
                });
            });
        }
    },
    promiseToAnimate: function (prop, speed) {
        var that = this;
        return new Promise(function (resolve, reject) {
            that.animate(prop, speed, function () {
                resolve();
            });
        });
    },
    animateAuto: function (prop, speed) {
        var elem;
        var height;
        var width;
        var promises = [];
        this.each(function (i, el) {
            el = jQuery(el);
            elem = el.clone().css({ "height": "auto", "width": "auto" }).appendTo("body");
            height = elem.css("height"),
            width = elem.css("width"),
            elem.remove();

            var animPromise;
            if (prop === "height") {
                animPromise = new Promise(function (resolve) {
                    el.promiseToAnimate({ "height": height }, speed).then(function () {
                        el.css("height", "auto");
                        resolve();
                    });
                });
            } else if (prop === "width") {
                animPromise = new Promise(function (resolve) {
                    el.promiseToAnimate({ "width": width }, speed).then(function () {
                        el.css("width", "auto");
                        resolve();
                    });
                });
            } else if (prop === "both") {
                animPromise = new Promise(function (resolve) {
                    el.promiseToAnimate({ "width": width, "height": height }, speed).then(function () {
                        el.css("height", "auto");
                        el.css("width", "auto");
                        resolve();
                    });
                });
            } else {
                animPromise = Promise.resolve();
            }
            promises.push(animPromise);
        });
         return Promise.all(promises);
    },
    animateTextAdd: function (text, interval) {
        var that = this;

        return new Promise(function (resolve, reject) {
            var index = 0;
            var animateTextAddAtInterval = function () {
                if (index < text.length) {
                    that.append(text[index++]);

                    var updateInterval = (typeof interval === "function") ? interval() : interval;
                    setTimeout(function () { animateTextAddAtInterval(); }, updateInterval);
                }
                else {
                    resolve();
                }
            };
            animateTextAddAtInterval();
        });
    },
    animateTextRemove: function (interval, removeWhenComplete) {
        var that = this;

        return new Promise(function (resolve, reject) {
            var animateTextRemoveAtInterval = function () {
                var text = that.text();
                if (text.length == 0) {
                    if (removeWhenComplete) { // intentional truthiness
                        that.remove();
                    }
                    resolve();
                }
                else {
                    that.text(text.slice(0, -1));

                    var updateInterval = (typeof interval === "function") ? interval() : interval;
                    setTimeout(function () { animateTextRemoveAtInterval(); }, updateInterval);
                }
            };
            animateTextRemoveAtInterval();
        });
    }
}));