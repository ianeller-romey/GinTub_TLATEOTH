﻿
var InterfaceManager = function (locationId, paragraphsId, timeId) {
    var locationElem = $(locationId);
    var paragraphsElem = $(paragraphsId);
    var timeElem = $(timeId);

    var paragraphSpans = [];
    var removedParagraphSpans = [];
    var addedParagraphSpans = [];
    var updateInterval = 50;
    var allowClicks = true;

    var messengerEngine = globalMessengerEngine;

    var that = this;

    var swapLocationImage = function (location) {
        locationElem.attr("src", location);
    };

    var disableInterface = function () {
        allowClicks = false;
        $(".iParagraph").off("mouseenter").off("mouseleave");
        $(".iWord").off("mouseenter").off("mouseleave");
    };

    var enableInterface = function () {
        allowClicks = false;
        $(".iParagraph").mouseenter(function (e) {
            $(this).addClass("iHover");
        }).mouseleave(function (e) {
            $(this).removeClass("iHover");
        });
        $(".iWord").mouseenter(function (e) {
            $(this).addClass("iHover").parent().removeClass("iHover");
        }).mouseleave(function (e) {
            $(this).removeClass("iHover").parent().addClass("iHover");
        });
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
        disableInterface();

        removedParagraphSpans.sort(function (a, b) {
            return a.order - b.order;
        });
        removeParagraphSpans();
    };

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
        var createParagraphSpanClick = function (psId) {
            return function (e) {
                if (allowClicks) {
                    messengerEngine.post("InterfaceManager.iParagraphClick", e.pageX, e.pageY, psId);
                }
            };
        };
        var createWordSpanClick = function (wId) {
            return function (e) {
                e.stopPropagation();
                if (allowClicks) {
                    messengerEngine.post("InterfaceManager.iWordClick", e.pageX, e.pageY, wId);
                }
            };
        };

        var updateParagraphSpan = function (pIdx) {
            if (pIdx < addedParagraphStates.length) {
                var addedParagraphState = addedParagraphStates[pIdx];
                var insertBefore = paragraphSpans.firstOrNull(function (paragraphSpan) {
                    return paragraphSpan.order > addedParagraphState.order;
                });
                var paragraphSpan = createParagraphSpan();
                paragraphSpan.click(createParagraphSpanClick(addedParagraphState.id));

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
                            wordSpan.click(createWordSpanClick(words[wIdx].nounId));

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
            else {
                enableInterface();
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
