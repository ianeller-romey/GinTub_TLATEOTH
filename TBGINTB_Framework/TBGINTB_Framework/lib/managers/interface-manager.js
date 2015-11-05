(function (namespace, undefined) {
    "use strict";

    namespace.Managers = namespace.Managers || {};
    namespace.Managers.InterfaceManager = {
        init: function (locationsId, paragraphsId, clockId, interfaceBottomId, messengerEngine) {
            var createHoverClickText = function (classType, idNum, messengerEngine) {
                var span = null;
                var classTypes = createHoverClickText.classTypes;
                switch (classType) {
                    case classTypes.paragraph:
                        span = $("<span/>", {
                            class: classType
                        });
                        span.idNum = idNum;

                        var spanClick = function (e) {
                            span.removeClass("iHover").addClass("iSelected");
                            var onVerbListClose = function () {
                                span.removeClass("iSelected");
                                messengerEngine.unregister("VerbList.closeExec", onVerbListClose);
                            };

                            var onVerbListOpen = function () {
                                messengerEngine.unregister("VerbList.openExec", onVerbListOpen);
                                messengerEngine.register("VerbList.closeExec", span, onVerbListClose);
                            };

                            messengerEngine.register("VerbList.openExec", span, onVerbListOpen);
                            messengerEngine.post("InterfaceManager.iParagraphClick", e.pageX, e.pageY, span.idNum);
                        }

                        span.enableInterfaceInput = function () {
                            span.mouseenter(function (e) {
                                span.addClass("iHover");
                            }).mouseleave(function (e) {
                                span.removeClass("iHover");
                            }).click(spanClick);
                        }
                        span.disableInterfaceInput = function () {
                            span.off("mouseenter").off("mouseleave").off("click");
                        };
                        break;

                    case classTypes.word:
                        span = $("<span/>", {
                            class: classType
                        });
                        span.idNum = idNum;

                        var spanClick = function (e) {
                            e.stopPropagation();

                            span.removeClass("iHover").addClass("iSelected");
                            var onVerbListClose = function () {
                                span.removeClass("iSelected");
                                messengerEngine.unregister("VerbList.closeExec", onVerbListClose);
                            };

                            var onVerbListOpen = function () {
                                messengerEngine.unregister("VerbList.openExec", onVerbListOpen);
                                messengerEngine.register("VerbList.closeExec", span, onVerbListClose);
                            };

                            messengerEngine.register("VerbList.openExec", span, onVerbListOpen);

                            var position = span[0].getBoundingClientRect();
                            messengerEngine.post("InterfaceManager.iWordClick", position.right + 5, position.top + 10, span.idNum);
                        };

                        span.enableInterfaceInput = function () {
                            span.mousemove(function (e) {
                                span.addClass("iHover").parent().removeClass("iHover");
                            }).mouseleave(function (e) {
                                span.removeClass("iHover").parent().addClass("iHover");
                            }).click(spanClick);
                        };
                        span.disableInterfaceInput = function () {
                            span.off("mousemove").off("mouseleave").off("click");
                        };
                        break;
                }

                span.unregisterAll = function () {
                    messengerEngine.unregisterAll(span);
                };

                messengerEngine.register("InterfaceManager.iParagraphClick", span, span.disableInterfaceInput);
                messengerEngine.register("InterfaceManager.iWordClick", span, span.disableInterfaceInput);
                messengerEngine.register("VerbList.openExec", span, span.enableInterfaceInput);

                return span;
            };

            createHoverClickText.classTypes = {
                paragraph: "iParagraph",
                word: "iWord"
            };

            var imageFader = new namespace.Entities.Classes.ImageFader(locationsId);
            var paragraphsElem = $(paragraphsId);
            var clockElem = $(clockId);
            var selectedElem;
            var pauseFader = null;
            var loadingFader = null;

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
                                var w = paragraphSpan.wordSpans.pop();
                                w.unregisterAll();
                                w.animateTextRemove(that.getUpdateInterval, true).then(function () {
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
                return new Promise(function (resolve, reject) {
                    var removeParagraphSpan = function (pIdx) {
                        if (pIdx < removedParagraphSpans.length) {
                            var paragraphSpan = removedParagraphSpans[pIdx].span;
                            removeWordPromise(paragraphSpan).then(function () {
                                paragraphSpan.unregisterAll();
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
                                var wordSpan = createHoverClickText(createHoverClickText.classTypes.word, words[wIdx].nounId, messengerEngine);

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
                            var paragraphSpan = createHoverClickText(createHoverClickText.classTypes.paragraph, addedParagraphState.id, messengerEngine);
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
                messengerEngine.post("InterfaceManager.loadingRoomState", true);
                swapLocationImage(roomStateData.location);

                paragraphStateUpdatePromise.then(function () {
                    removedParagraphSpans = paragraphSpans.where(function (activeParagraph) {
                        return !paragraphStateData.any(function (newParagraph) {
                            return newParagraph.id === activeParagraph.id;
                        });
                    });
                    removedParagraphSpans.sort(function (a, b) {
                        return b.order - a.order;
                    });
                    updateRemovedParagraphSpans().then(function () {
                        addedParagraphStates = paragraphStateData.where(function (newParagraph) {
                            return !paragraphSpans.any(function (activeParagraph) {
                                return activeParagraph.id === newParagraph.id;
                            });
                        });
                        updateAddedParagraphStates().then(function () {
                            messengerEngine.post("InterfaceManager.loadingRoomState", false);
                        });
                    });
                });
            };

            var loadingWhileDoAction = function () {
                var promise = new Promise(function (resolve, reject) {
                    var loadingWhileDoActionFinished = function () {
                        messengerEngine.unregister("ServicesEngine.doAction", loadingWhileDoActionFinished);
                        resolve();
                        loadingFader = null;
                    };
                    messengerEngine.register("ServicesEngine.doAction", that, loadingWhileDoActionFinished);
                });
                loadingFader = new namespace.Entities.Classes.LoadingFader(interfaceBottomId, promise, messengerEngine);
            };

            var updateTime = function (time) {
                var timeString = ("0" + time.hours()).slice(-2) + ":" + ("0" + time.minutes()).slice(-2);
                clockElem.text(timeString);
            };

            var pause = function () {
                disableInterface();
                pauseFader = new namespace.Entities.Classes.PauseFader(messengerEngine);
            };

            var unpause = function () {
                pauseFader = null;
                enableInterface();
            };

            var showOrHideTimeForArea = function (area) {
                if (area.displayTime) {
                    clockElem.css("display", "inline");
                } else {
                    clockElem.css("display", "none");
                }
            };

            this.getUpdateInterval = function () {
                return updateInterval;
            };

            this.changeUpdateInterval = function () {
                if (updateInterval != updateIntervalFast) {
                    updateInterval = updateIntervalFast;
                }
            };

            clockElem.click(function () {
                var pos = clockElem.offset();
                messengerEngine.post("InterfaceManager.clockClick", pos.left + 40, pos.top + 30);
            });

            $(interfaceBottomId).mousedown(function (e) {
                that.changeUpdateInterval();
            });

            messengerEngine.register("GameStateEngine.setArea", this, showOrHideTimeForArea);
            messengerEngine.register("GameStateEngine.setActiveRoomState", this, loadRoomState);
            messengerEngine.register("GameStateEngine.doAction", this, loadingWhileDoAction);
            messengerEngine.register("TimeEngine.updateTime", this, updateTime);
            messengerEngine.register("ClockList.pauseClick", this, pause);
            messengerEngine.register("PauseFader.unpauseClick", this, unpause);
        }
    };
}(window.GinTub = window.GinTub || {}));
