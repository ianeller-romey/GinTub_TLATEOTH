(function (namespace, undefined) {
    "use strict";

    namespace.Entities = namespace.Entities || {};
    namespace.Entities.Classes = namespace.Entities.Classes || {};
    namespace.Entities.Classes.TimeSelector = function (messengerEngine) {
        var halfWidth = 150;
        var outerRadius = halfWidth - 32;
        var innerRadius = outerRadius - 48;
        var widthString = (2 * halfWidth) + "px";
        var container = $("<div/>", {
            css: {
                position: "absolute",
                top: 200,
                left: 200,
                width: widthString,
                height: widthString,
                "background-color": "white"
            }
        }).appendTo("body");

        var circleCanvas = document.createElement("canvas");
        circleCanvas.width = widthString;
        circleCanvas.height = widthString;
        circleCanvas.style.width = widthString;
        circleCanvas.style.height = widthString;

        var ctx = circleCanvas.getContext("2d");
        ctx.beginPath();
        ctx.arc(outerRadius, outerRadius, outerRadius, 0, 2 * Math.PI);
        ctx.stroke();

        container.append(circleCanvas);

        var addTimeButton = function (radius, index, text) {
            var x = halfWidth + (radius * namespace.Entities.Classes.TimeSelector.sinTable[index]) - 8;
            var y = halfWidth + (radius * namespace.Entities.Classes.TimeSelector.cosTable[index]) - 8;

            $("<div/>", {
                text: text,
                css: {
                    position: "absolute",
                    top: y,
                    left: x
                }
            }).appendTo(container);
        };
        
        var numHours = 24;
        var numMinutes = 12;
        for (var i = 0; i < numHours; ++i) {
            addTimeButton(outerRadius, i, i);
        }
        for (var i = 0, iIncr = numHours / numMinutes, timeIncr = 60 / numMinutes; i < numMinutes; ++i) {
            addTimeButton(innerRadius, i * iIncr, i * timeIncr);
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