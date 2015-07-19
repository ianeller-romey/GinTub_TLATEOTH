
var InterfaceManager = function (locationId, paragraphsId, timeId) {
    var locationElem = $(locationId);
    var paragraphsElem = $(paragraphsId);
    var timeElem = $(timeId);

    var messengerEngine = globalMessengerEngine;

    var that = this;

    var loadRoomState = function (roomStateData, paragraphStateData) {

        locationElem.attr("src", roomStateData.location);

        var createParagraphSpanClick = function (psId) {
            return function (e) {
                messengerEngine.post("InterfaceManager.iParagraphClick", e.pageX, e.pageY, psId);
            };
        };
        var createWordSpanClick = function (wId) {
            return function (e) {
                e.stopPropagation();
                messengerEngine.post("InterfaceManager.iWordClick", e.pageX, e.pageY, wId);
            };
        };

        var i = 0;
        var len = paragraphStateData.length;
        for (; i < len; ++i) {
            var ps = paragraphStateData[i];
            if (ps.words.length == 0) {
                continue;
            }

            var paragraphSpan = $("<span/>", {
                class: "iParagraph"
            }).click(createParagraphSpanClick(ps.id)).mouseenter(function (e) {
                $(this).addClass("iHover");
            }).mouseleave(function (e) {
                $(this).removeClass("iHover");
            });

            var words = ps.words;
            var j = 0;
            var len2 = words.length;
            for (; j < len2; ++j) {
                var w = words[j];
                paragraphSpan.append($("<span/>", {
                    class: "iWord",
                    text: w.text
                }).click(createWordSpanClick(w.nounId)).mouseenter(function (e) {
                    $(this).addClass("iHover").parent().removeClass("iHover");
                }).mouseleave(function (e) {
                    $(this).removeClass("iHover").parent().addClass("iHover");
                }));

                if (j != len2 - 1) {
                    w = words[j + 1];
                    wt = w.text;
                    if (wt != "." &&
                        wt != "," &&
                        wt != ";" &&
                        wt != ":" &&
                        wt != "?" &&
                        wt != "!" &&
                        wt != "\"") {
                        paragraphSpan.append(" ");
                    }
                }
            }
            paragraphsElem.append(paragraphSpan);
            paragraphsElem.append(" ");
        }
    };

    var updateTime = function (time) {
        var timeString = ("0" + time.hours()).slice(-2) + ":" + ("0" + time.minutes()).slice(-2);
        timeElem.text(timeString);
    };
    
    messengerEngine.register("GameStateEngine.setActiveRoomState", this, loadRoomState);
    messengerEngine.register("TimeEngine.updateTime", this, updateTime);
};
