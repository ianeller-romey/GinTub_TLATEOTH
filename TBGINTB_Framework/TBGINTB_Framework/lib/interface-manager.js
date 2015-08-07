
var InterfaceManager = function (locationId, paragraphsId, timeId) {
    var locationElem = $(locationId);
    var paragraphsElem = $(paragraphsId);
    var timeElem = $(timeId);

    var paragraphSpans = [];
    var removedParagraphSpans = [];
    var addedParagraphSpans = [];
    var updateInterval = 50;

    var messengerEngine = globalMessengerEngine;

    var that = this;

    var swapLocationImage = function (location) {
        locationElem.attr("src", location);
    };

    var removeWord = function (paragraphSpan, wordSpan) {
        wordSpan.animateTextRemove(that.getUpdateInterval).then(function () {
            removeParagraphSpan(paragraphSpan);
        });
    };

    var removeParagraphSpan = function (paragraphSpan) {
        if (paragraphSpan.text().length == 0) {
            paragraphSpan.remove();
            removedParagraphSpans();
        }
        else {
            var word = paragraphSpan.children().last();
            removeWord(paragraphSpan, word);
        }
    };

    var removeParagraphSpans = function () {
        if (removedParagraphSpans.length != 0) {
            var paragraphSpan = removedParagraphSpans[0].span;

            removedParagraphSpans.shift();
            paragraphSpans = paragraphSpans.splice(paragraphSpans.indexOf(removedParagraphSpans[0]), 1);

            removeParagraphSpan(paragraphSpan);
        }
    };

    var updateRemovedParagraphSpans = function (removedParagraphSpans) {
        removedParagraphSpans.sort(function (a, b) {
            return a.order - b.order;
        });
        removeParagraphSpans();
    };

    /*var createParagraphSpan = function (paragraphState) {
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
    };*/

    var createWordSpan = function () {
        var wordSpan = $("<span>", {
            class: "iWord"
        });
        return wordSpan;
    };

    var createParagraphSpan = function () {
        var paragraphSpan = $("<span/>", {
            class: "iParagraph"
        });
        return paragraphSpan;
    };

    var updateAddedParagraphStates = function (addedParagraphStates) {
        var updateParagraphSpan = function (pIdx) {
            if (pIdx < addedParagraphStates.length) {
                var addedParagraphState = addedParagraphStates[pIdx];
                var insertBefore = paragraphSpans.firstOrNull(function (paragraphSpan) {
                    return paragraphSpan.order > addedParagraphState.order;
                });
                var paragraphSpan = createParagraphSpan();

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
                    paragraph: addedParagraphState.paragraph,
                    span: paragraphSpan
                });

                var promise = new Promise(function (resolve, reject) {
                    var updateWordSpan = function (wIdx) {
                        if (wIdx < words.length) {
                            var wordSpan = createWordSpan();
                            paragraphSpan.append(wordSpan);

                            wordSpan.animateTextAdd(words[wIdx].text, that.getUpdateInterval).then(function () {
                                if (wIdx != words.length - 1) {
                                    var nextWord = words[wIdx + 1];
                                    var nextWordText = nextWord.text;
                                    if (nextWordText != "." &&
                                        nextWordText != "," &&
                                        nextWordText != ";" &&
                                        nextWordText != ":" &&
                                        nextWordText != "?" &&
                                        nextWordText != "!" &&
                                        nextWordText != "\"") {
                                        paragraphSpan.append(" ");
                                    }
                                }

                                updateWordSpan(wIdx + 1);
                            });
                        }
                        else {
                            resolve();
                        }
                    };

                    updateWordSpan(0);
                }).then(function () {
                    updateParagraphSpan(pIdx + 1);
                });

            }
        };

        updateParagraphSpan(0);
    };

    var loadRoomState = function (roomStateData, paragraphStateData) {

        swapLocationImage(roomStateData.location);

        var promise = new Promise(function (resolve, reject) {
            var removedParagraphSpans = paragraphSpans.where(function (activeParagraph) {
                return !paragraphStateData.any(function (newParagraph) {
                    return newParagraph.id == activeParagraph.id;
                });
            });
            updateRemovedParagraphSpans(removedParagraphSpans);
            resolve();
        }).then(function () {
            var addedParagraphStates = paragraphStateData.where(function (newParagraph) {
                return !paragraphSpans.any(function (activeParagraph) {
                    return activeParagraph.id == newParagraph.id;
                });
            });
            updateAddedParagraphStates(addedParagraphStates);
        });
    };

    var updateTime = function (time) {
        var timeString = ("0" + time.hours()).slice(-2) + ":" + ("0" + time.minutes()).slice(-2);
        timeElem.text(timeString);
    };

    InterfaceManager.prototype.getUpdateInterval = function () {
        return updateInterval;
    };
    
    messengerEngine.register("GameStateEngine.setActiveRoomState", this, loadRoomState);
    messengerEngine.register("TimeEngine.updateTime", this, updateTime);
};
