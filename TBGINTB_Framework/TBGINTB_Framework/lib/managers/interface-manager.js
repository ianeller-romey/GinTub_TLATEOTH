(function (namespace, undefined) {
    "use strict";

    namespace.Managers = namespace.Managers || {};
    namespace.Managers.InterfaceManager = {
        init: function (locationsId, paragraphsId, timeId, messengerEngine) {
            var imageFader = new namespace.Entities.Classes.ImageFader(locationsId);
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

            var that = this;

            var swapLocationImage = function (location) {
                imageFader.swap(location);
            };

            var addISelected = function () {
                selectedElem.removeClass("iHover").addClass("iSelected");
            };

            var disableInterface = function () {
                paragraphSpans.forEach(function (p) {
                    p.span.disableInterfaceInput();
                    p.span.wordSpans.forEach(function (w) {
                        w.disableInterfaceInput();
                    });
                });
            };

            var enableInterface = function () {
                paragraphSpans.forEach(function (p) {
                    p.span.enableInterfaceInput();
                    p.span.wordSpans.forEach(function (w) {
                        w.enableInterfaceInput();
                    });
                });
            };

            var updateRemovedParagraphSpans = function () {
                var removeWordPromise = function (paragraphSpan) {
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
                            removeWordPromise(paragraphSpan).then(function () {
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
                var createWordPromise = function (paragraphSpan, words) {
                    return new Promise(function (resolve, reject) {
                        var updateWordSpan = function (wIdx) {
                            if (wIdx < words.length) {
                                var wordSpan = namespace.Entities.Factories.createHoverClickText(namespace.Entities.Factories.createHoverClickText.classTypes.word, words[wIdx].nounId, messengerEngine);

                                paragraphSpan.append(wordSpan);
                                paragraphSpan.wordSpans.push(wordSpan);

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
                            var paragraphSpan = namespace.Entities.Factories.createHoverClickText(namespace.Entities.Factories.createHoverClickText.classTypes.paragraph, addedParagraphState.id, messengerEngine);
                            paragraphSpan.wordSpans = [];

                            if (insertBefore != null) {
                                insertBefore.span.before(paragraphSpan);
                            } else {
                                paragraphsElem.append(paragraphSpan);
                            }
                            paragraphSpan.after(" ");

                            paragraphSpans.push({
                                id: addedParagraphState.id,
                                order: addedParagraphState.order,
                                span: paragraphSpan
                            });

                            createWordPromise(paragraphSpan, addedParagraphState.words).then(function () {
                                updateParagraphSpan(pIdx + 1);
                            });
                        } else {
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
                            return newParagraph.id === activeParagraph.id;
                        });
                    });
                    updateRemovedParagraphSpans().then(function () {
                        addedParagraphStates = paragraphStateData.where(function (newParagraph) {
                            return !paragraphSpans.any(function (activeParagraph) {
                                return activeParagraph.id === newParagraph.id;
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
            messengerEngine.register("TimeEngine.updateTime", this, updateTime);
        }
    };
}(window.GinTub = window.GinTub || {}));
