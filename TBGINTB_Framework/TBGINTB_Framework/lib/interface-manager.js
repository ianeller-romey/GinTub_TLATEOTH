
var InterfaceManager = function (locationId, paragraphsId, timeId) {
    var locationElem = $(locationId);
    var paragraphsElem = $(paragraphsId);
    var timeElem = $(timeId);

    var paragraphSpans = [];

    var messengerEngine = globalMessengerEngine;

    var that = this;

    var swapLocationImage = function (location) {
        locationElem.attr("src", location);
    };

    var createParagraphSpan = function (paragraphState) {
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

        var paragraphSpan = $("<span/>", {
            class: "iParagraph"
        }).click(createParagraphSpanClick(paragraphState.id)).mouseenter(function (e) {
            $(this).addClass("iHover");
        }).mouseleave(function (e) {
            $(this).removeClass("iHover");
        });

        var words = paragraphState.words;
        for (var j = 0, len2 = words.length; j < len2; ++j) {
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

        return paragraphSpan;
    };

    var updateRemovedParagraphSpans = function (removedParagraphSpans) {
        for (var i = 0, len = removedParagraphSpans.length; i < len; ++i) {
            removedParagraphSpans[i].span.remove();
        }
    };

    var updateAddedParagraphStates = function (addedParagraphStates) {
        for (var i = 0, len = addedParagraphStates.length; i < len; ++i) {
            var addedParagraphState = addedParagraphStates[i];
            var insertBefore = paragraphSpans.firstOrNull(function (paragraphSpan) {
                return paragraphSpan.order > addedParagraphState.order;
            });
            var paragraphSpan = createParagraphSpan(addedParagraphState);

            if (insertBefore != null) {
                insertBefore.span.before(paragraphSpan);
            }
            else {
                paragraphsElem.append(paragraphSpan);
            }
            paragraphSpan.after(" ");

            paragraphSpans.push({
                id: addedParagraphState.id,
                order: addedParagraphState.order,
                span: paragraphSpan
            });
        }
    };

    var loadRoomState = function (roomStateData, paragraphStateData) {

        swapLocationImage(roomStateData.location);

        var removedParagraphSpans = paragraphSpans.where(function (activeParagraph) {
            return !paragraphStateData.any(function (newParagraph) {
                return newParagraph.id == activeParagraph.id;
            });
        });
        updateRemovedParagraphSpans(removedParagraphSpans);

        var addedParagraphStates = paragraphStateData.where(function (newParagraph) {
            return !paragraphSpans.any(function (activeParagraph) {
                return activeParagraph.id == newParagraph.id;
            });
        });
        updateAddedParagraphStates(addedParagraphStates);
    };

    var updateTime = function (time) {
        var timeString = ("0" + time.hours()).slice(-2) + ":" + ("0" + time.minutes()).slice(-2);
        timeElem.text(timeString);
    };
    
    messengerEngine.register("GameStateEngine.setActiveRoomState", this, loadRoomState);
    messengerEngine.register("TimeEngine.updateTime", this, updateTime);
};
