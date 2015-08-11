
var InterfaceManager = function (locationsId, paragraphsId, timeId) {
    var locationsElem = $(locationsId);
    var location1Elem = locationsElem.children().first();
    var location2Elem = locationsElem.children().last();
    var paragraphsElem = $(paragraphsId);
    var timeElem = $(timeId);
    var selectedElem;

    var paragraphSpans = [];
    var removedParagraphSpans = [];
    var addedParagraphStates = [];
    var updateIntervalFast = 2;
    var updateIntervalRemoval = 10;
    var updateIntervalAdding = 50;
    var updateInterval = 0;
    var allowClicks = true;

    var messengerEngine = globalMessengerEngine;

    var that = this;

    var swapAndFadeLocationImages = function (src, locationImageFadeIn, locationImageFadeOut) {
        locationImageFadeIn.attr("src", src);
        locationImageFadeIn.fadeTo("slow", 1.0);
        locationImageFadeOut.fadeTo("slow", 0.0, function () {
            locationImageFadeOut.attr("src", "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=");
        });
    };

    var swapLocationImage = function (location) {
        if (location1Elem.css("opacity") == 1.0) {
            swapAndFadeLocationImages(location, location2Elem, location1Elem);
        }
        else {
            swapAndFadeLocationImages(location, location1Elem, location2Elem);
        }
    };

    var removeISelected = function () {
        $(".iSelected").removeClass("iSelected");
    };

    var addISelected = function () {
        selectedElem.removeClass("iHover").addClass("iSelected");
    };

    var disableInterface = function () {
        allowClicks = false;
        $(".iParagraph").off("mouseenter").off("mouseleave");
        $(".iWord").off("mouseenter").off("mouseleave");
    };

    var enableInterface = function () {
        allowClicks = true;
        $(".iParagraph").mouseenter(function (e) {
            $(this).addClass("iHover");
        }).mouseleave(function (e) {
            $(this).removeClass("iHover");
        });
        $(".iWord").mousemove(function (e) {
            $(this).addClass("iHover").parent().removeClass("iHover");
        }).mouseleave(function (e) {
            $(this).removeClass("iHover").parent().addClass("iHover");
        });
    };

    var updateRemovedParagraphSpans = function () {
        var createWordPromise = function (paragraphSpan) {
            return new Promise(function (resolve, reject) {
                var updateWordSpans = function () {
                    var text = paragraphSpan.text();
                    if (text.length != 0 && !text.isNullOrWhitespace()) {
                        paragraphSpan.children().last().animateTextRemove(that.getUpdateInterval).then(function () {
                            updateWordSpans();
                        });
                    }
                    else {
                        resolve();
                    }
                }
                updateWordSpans();
            });
        };

        updateInterval = updateIntervalRemoval;
        removedParagraphSpans.sort(function (a, b) {
            return a.order - b.order;
        });
        return new Promise(function (resolve, reject) {
            var removeParagraphSpan = function (pIdx) {
                if (pIdx < removedParagraphSpans.length) {
                    var paragraphSpan = removedParagraphSpans[pIdx].span;
                    createWordPromise(paragraphSpan).then(function () {
                        paragraphSpan.remove();
                        paragraphSpans.splice(paragraphSpans.indexOf(removedParagraphSpans[pIdx]), 1);

                        removeParagraphSpan(pIdx + 1);
                    });
                }
                else {
                    removedParagraphSpans = [];
                    resolve();
                }
            };

            disableInterface();
            removeParagraphSpan(0);
        });
    };

    var updateAddedParagraphStates = function () {
        var createParagraphSpanClick = function (psId) {
            return function (e) {
                if (allowClicks) {
                    selectedElem = $(this);
                    messengerEngine.post("InterfaceManager.iParagraphClick", e.pageX, e.pageY, psId);
                }
            };
        };
        var createWordSpanClick = function (wId) {
            return function (e) {
                e.stopPropagation();
                if (allowClicks) {
                    selectedElem = $(this);
                    var position = this.getBoundingClientRect();
                    messengerEngine.post("InterfaceManager.iWordClick", position.right + 5, position.top + 10, wId);
                }
            };
        };
        var createWordPromise = function (paragraphSpan, words) {
            return new Promise(function (resolve, reject) {
                var updateWordSpan = function (wIdx) {
                    if (wIdx < words.length) {
                        var wordSpan = $("<span/>", {
                            class: "iWord"
                        });
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
            })
        };

        updateInterval = updateIntervalAdding;
        addedParagraphStates.sort(function (a, b) {
            return a.order - b.order;
        });
        return new Promise(function (resolve, reject) {
            var updateParagraphSpan = function (pIdx) {
                if (pIdx < addedParagraphStates.length) {
                    var addedParagraphState = addedParagraphStates[pIdx];
                    var insertBefore = paragraphSpans.firstOrNull(function (paragraphSpan) {
                        return paragraphSpan.order > addedParagraphState.order;
                    });
                    var paragraphSpan = $("<span/>", {
                        class: "iParagraph"
                    });
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

                    createWordPromise(paragraphSpan, addedParagraphState.words).then(function () {
                        updateParagraphSpan(pIdx + 1);
                    });
                }
                else {
                    enableInterface();
                    addedParagraphStates = [];
                    resolve();
                }
            };

            updateParagraphSpan(0);
        });
    };

    var loadRoomState = function (roomStateData, paragraphStateData, paragraphStateUpdatePromise) {

        swapLocationImage(roomStateData.location);

        paragraphStateUpdatePromise.then(function () {
            removedParagraphSpans = paragraphSpans.where(function (activeParagraph) {
                return !paragraphStateData.any(function (newParagraph) {
                    return newParagraph.id == activeParagraph.id;
                });
            });
            updateRemovedParagraphSpans().then(function () {
                addedParagraphStates = paragraphStateData.where(function (newParagraph) {
                    return !paragraphSpans.any(function (activeParagraph) {
                        return activeParagraph.id == newParagraph.id;
                    });
                });
                updateAddedParagraphStates();
            });
        });
    };

    var updateTime = function (time) {
        var timeString = ("0" + time.hours()).slice(-2) + ":" + ("0" + time.minutes()).slice(-2);
        timeElem.text(timeString);
    };

    this.getUpdateInterval = function () {
        return updateInterval;
    };

    this.changeUpdateInterval = function () {
        if (updateInterval != updateIntervalFast) {
            updateInterval = updateIntervalFast;
        }
    };
    
    messengerEngine.register("GameStateEngine.setActiveRoomState", this, loadRoomState);
    messengerEngine.register("VerbList.openExec", this, addISelected);
    messengerEngine.register("VerbList.closeExec", this, removeISelected);
    messengerEngine.register("TimeEngine.updateTime", this, updateTime);
};
