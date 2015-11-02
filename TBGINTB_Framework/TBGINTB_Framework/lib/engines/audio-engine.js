(function (namespace, undefined) {
    "use strict";

    namespace.Engines = namespace.Engines || {};
    namespace.Engines.AudioEngine = {
        init: function (audioElemId, messengerEngine) {
            audioElemId = (audioElemId.charAt(0) === "#") ? audioElemId.slice(1) : audioElemId;
            var audioElem = document.getElementById(audioElemId);

            var audioUseDefinitions = {};

            var possibleFormats = [{
                name: "mpeg",
                ext: "mp3"
            }, {
                name: "ogg",
                ext: "ogg"
            }];
            var supportedFormat = null;

            var that = this;

            var initSupportedFormat = function () {
                if (!audioElem || !audioElem.canPlayType) { // intentional truthiness
                    supportedFormat = null;
                } else {
                    for (var i = 0; i < possibleFormats.length; ++i) {
                        if (!!(audioElem.canPlayType("audio/" + possibleFormats[i].name + ";").replace(/no/, ""))) {
                            supportedFormat = possibleFormats[i];
                            break;
                        }
                    }
                }

                return supportedFormat !== null;
            }

            var buildAudioUseDefinitions = function (data) {
                data.forEach(function (x) {
                    audioUseDefinitions[x.name] = {
                        id: x.id,
                        name: x.name,
                        audioFile: x.audioFile,
                        isLooped: x.isLooped
                    };
                    audioUseDefinitions[x.id] = audioUseDefinitions[x.name];
                    // we're storing data by name and number, so we can play it based on name or number
                });
            };

            var loadAllAudio = function (audioUseData) {
                buildAudioUseDefinitions(audioUseData.audio);
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
                if (initSupportedFormat()) {

                    messengerEngine.register("ServicesEngine.getAllAudio", this, loadAllAudio);
                    messengerEngine.register("GameStateEngine.setArea", this, playAreaAudio);
                    messengerEngine.register("VolumeManager.setVolume", this, setVolume);
                    messengerEngine.register("VolumeManager.setMute", this, setMute);

                    messengerEngine.post("AudioEngine.getAllAudio", supportedFormat.ext);
                }
            };

            loadEngine();
        }
    };
}(window.GinTub = window.GinTub || {}));