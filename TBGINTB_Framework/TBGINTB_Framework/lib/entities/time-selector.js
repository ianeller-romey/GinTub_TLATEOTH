(function (namespace, undefined) {
    "use strict";

    namespace.Entities = namespace.Entities || {};
    namespace.Entities.Classes = namespace.Entities.Classes || {};
    namespace.Entities.Classes.TimeSelector = function (messengerEngine) {
        this.hours = null;
        this.minutes = null;
        this.accepted = false;
        var thenFn = null;
        this.then = function (fn) {
            thenFn = fn;
        };

        var that = this;

        var close = function () {
            if (that.accepted) {
                var cover = $("<div/>", {
                    class: "overlay",
                    css: {
                        "background-color": "black",
                        opacity: 0
                    }
                }).appendTo("body");
                cover.promiseToFade("slow", 1.0).then(function () {
                    containerOuter.remove();
                    thenFn(true, that.hours, that.minutes);
                    cover.promiseToFade("slow", 0.0).then(function () {
                        cover.remove();
                    });
                });
            } else {
                containerOuter.remove();
                thenFn(false);
            }
        };

        var accept = function () {
            that.accepted = true;
            close();
        };

        var cancel = function () {
            that.accepted = false;
            close();
        };

        var containerOuter = $("<div/>", {
            class: "overlay",
            css: {
                "background-color": "rgba(0, 0, 0, 0.25)"
            }
        }).appendTo("body");
        var halfWidth = 150;
        var outerRadius = halfWidth - 32;
        var innerRadius = outerRadius - 32;
        var widthString = (2 * halfWidth) + "px";
        var container = $("<div/>", {
            class: "center",
            css: {
                position: "absolute",
                left: 0,
                right: 0,
                width: widthString,
                height: widthString,
                "background-color": "white",
                "-webkit-border-radius": "5px",
                "border-radius": "5px",
                "-webkit-box-shadow": "inset 0 0 2px 2px rgba(1,1,1,.25)",
                "box-shadow": "inset 0 0 2px 2px rgba(1,1,1,.25)"
            }
        }).appendTo(containerOuter);

        var circleCanvas = document.createElement("canvas");
        circleCanvas.width = halfWidth * 2;
        circleCanvas.height = halfWidth * 2;
        circleCanvas.style.width = widthString;
        circleCanvas.style.height = widthString;

        var ctx = circleCanvas.getContext("2d");
        ctx.beginPath();
        ctx.arc(halfWidth, halfWidth, halfWidth - 10, 0, 2 * Math.PI);
        ctx.closePath();
        ctx.stroke();
        ctx.beginPath();
        ctx.arc(halfWidth, halfWidth, innerRadius - 20, 0, 2 * Math.PI);
        ctx.closePath();
        ctx.stroke();

        container.append(circleCanvas);

        var xMarkElem = $("<div/>", {
            text: String.fromCharCode(10007), // ✗
            css: {
                position: "absolute",
                top: halfWidth + 10,
                left: halfWidth - 22,
                cursor: "default",
                color: "red",
                "font-weight": "bold"
            }
        }).click(function () {
            cancel();
        }).appendTo(container);
        var checkMarkElem = $("<div/>", {
            text: String.fromCharCode(10003), // ✓
            css: {
                position: "absolute",
                top: halfWidth + 10,
                left: halfWidth + 10,
                cursor: "default",
                color: "gray",
                "font-weight": "bold"
            }
        }).appendTo(container);
        var chosenTimeElem = $("<div/>", {
            text: "__:__",
            css: {
                position: "absolute",
                top: halfWidth - 20,
                left: halfWidth - 25,
                "font-family": "'Courier New'"
            }
        }).appendTo(container);

        var pad = function (num, size) {
            var s = num + "";
            while (s.length < size) {
                s = "0" + s;
            }
            return s;
        }

        var updateTimeElem = function () {
            var hourText = (that.hours) /* intentional truthiness */ ? pad(that.hours, 2) : "__";
            var minuteText = (that.minutes) /* intentional truthiness */ ? pad(that.minutes, 2) : "__";
            chosenTimeElem.text(hourText + ":" + minuteText);

            if (that.hours && that.minutes) { // intentional truthiness
                checkMarkElem.css("color", "green").click(function () {
                    accept();
                });
            }
        };

        var updateHours = function (h) {
            that.hours = h;
            updateTimeElem();
        };

        var updateMinutes = function (m) {
            that.minutes = m;
            updateTimeElem();
        };

        var addTimeButton = function (radius, index, text, updateFn) {
            text = new String(text);
            var x = halfWidth + (radius * namespace.Entities.Classes.TimeSelector.sinTable[index]) - (6 * text.length);
            var y = halfWidth + (radius * namespace.Entities.Classes.TimeSelector.cosTable[index]) - 9;

            $("<div/>", {
                text: text,
                css: {
                    position: "absolute",
                    top: y,
                    left: x,
                    cursor: "default",
                    "font-size": "18px",
                    "font-family": "'Arial'"
                }
            }).mouseenter(function () {
                $(this).css("font-size", "22px");
            }).mouseleave(function () {
                $(this).css("font-size", "18px");
            }).click(function () {
                updateFn($(this).text());
            }).appendTo(container);
        };
        
        var numHours = 24;
        var numMinutes = 6;
        for (var i = 0; i < numHours; ++i) {
            addTimeButton(outerRadius, i, i, updateHours);
        }
        for (var i = 0, iIncr = numHours / numMinutes, timeIncr = 60 / numMinutes; i < numMinutes; ++i) {
            addTimeButton(innerRadius, i * iIncr, i * timeIncr, updateMinutes);
        }
    };

    namespace.Entities.Classes.TimeSelector.sinTable = {};
    namespace.Entities.Classes.TimeSelector.cosTable = {};

    var numEntries = 24;
    var angleIncr = 360 / numEntries;
    for (var i = 0, angle = 180; i < numEntries; ++i) {
        namespace.Entities.Classes.TimeSelector.sinTable[i] = Math.sin((angle / 180) * Math.PI);
        namespace.Entities.Classes.TimeSelector.cosTable[i] = Math.cos((angle / 180) * Math.PI);
        angle = angle - angleIncr;
        if (angle === -angleIncr) {
            angle = 360 - angleIncr;
        }
    }
}(window.GinTub = window.GinTub || {}));