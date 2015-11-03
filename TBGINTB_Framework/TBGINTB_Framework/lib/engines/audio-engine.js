(function (namespace, undefined) {
    "use strict";

    namespace.Engines = namespace.Engines || {};
    namespace.Engines.AudioEngine = {
        init: function (audioElemId, messengerEngine) {
            audioElemId = (audioElemId.charAt(0) === "#") ? audioElemId.slice(1) : audioElemId;
            var audioElem = document.getElementById(audioElemId);

            var audioUseDefinitions = {};

            var that = this;

            var buildAudioUseDefinitions = function (definitions) {
                audioUseDefinitions = definitions;
            };

            var playAreaAudio = function (area) {
                $(audioElemId).promiseToAnimate({ volume: 0 }, "slow").then(function () {
                    if (area.audio) {
                        audioElem.src = audioUseDefinitions[area.audio];
                        $(audioElemId).promiseToAnimate({ volume: volume }, "slow");
                    } else {
                        audioElem.src = "";
                    }
                });
            };

            var setVolume = function (v) {
                audioElem.volume = v;
            };

            var setMute = function (isMuted) {
                audioElem.muted = isMuted;  
            };

            var loadEngine = function () {
                messengerEngine.register("GameStateEngine.setArea", this, playAreaAudio);
                messengerEngine.register("VolumeManager.setVolume", this, setVolume);
                messengerEngine.register("VolumeManager.setMute", this, setMute);
                messengerEngine.register("AudioDataEngine.buildAudioUseDefinitions", this, buildAudioUseDefinitions);
            };

            loadEngine();
        }
    };
}(window.GinTub = window.GinTub || {}));