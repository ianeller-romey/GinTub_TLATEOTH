﻿(function (namespace, undefined) {
    "use strict";

    namespace.Managers = namespace.Managers || {};
    namespace.Managers.VolumeManager = {
        init: function (volumeButtonId, volumeRangeId, messengerEngine) {
            var volumeButtonElem = $(volumeButtonId);
            var volumeRangeElem = $(volumeRangeId);

            var volume;
            var isMuted = false;
            var mutedBeforePause = false;

            var that = this;
            
            var setVolume = function (v) {
                volume = v;
                messengerEngine.post("VolumeManager.setVolume", volume);
            };

            var toggleMute = function () {
                isMuted = !isMuted;
                messengerEngine.post("VolumeManager.setMute", isMuted);
            };

            var pause = function () {
                mutedBeforePause = isMuted;
                if (!mutedBeforePause) {
                    toggleMute();
                }
            };

            var unpause = function () {
                if (!mutedBeforePause && isMuted) {
                    toggleMute();
                }
            };

            volumeButtonElem.click(function () {
                toggleMute();
                $(this).css("text-decoration", (isMuted) ? "line-through" : "none");
            });

            var volumeSwitch = function () {
                var v = volumeRangeElem.val();
                v = v * v; // non-linear
                setVolume(v);
            };
            volumeSwitch();
            volumeRangeElem.on("input change", function () {
                volumeSwitch();
            })

            messengerEngine.register("ClockList.pauseClick", this, pause);
            messengerEngine.register("PauseFader.unpauseClick", this, unpause);
        }
    };
}(window.GinTub = window.GinTub || {}));