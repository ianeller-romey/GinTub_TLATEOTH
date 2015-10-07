(function (namespace, undefined) {
    "use strict";

    namespace.Engines = namespace.Engines || {};
    namespace.Engines.AudioEngine = {
        init: function (audioElemTag, messengerEngine) {
            var audioUseDefinitions = {};
            var audioData = {};
            var playingLoopedAudio = {};

            var possibleFormats = [{
                name: "mpeg",
                ext: "mp3"
            }, {
                name: "ogg",
                ext: "ogg"
            }];
            var supportedFormat = null;

            var audioContext;
            var gainNode;

            var that = this;

            var initAudioContext = function () {
                try {
                    window.AudioContext = window.AudioContext || window.webkitAudioContext;
                    audioContext = new AudioContext();
                    gainNode = audioContext.createGain();
                }
                catch (e) {
                }

                if (!audioContext) {
                    audioContext = null;
                }

                return (audioContext) ? true : false;
            };

            var initSupportedFormat = function (tagName) {
                var audio = document.getElementsByTagName(tagName)[0];

                if (!audio || !audio.canPlayType) { // intentional truthiness
                    supportedFormat = null;
                } else {
                    for (var i = 0; i < possibleFormats.length; ++i) {
                        if (!!(audio.canPlayType("audio/" + possibleFormats[i].name + ";").replace(/no/, ""))) {
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
                    // we're storing data by name and number, so we can play it based on name or number;
                    // we'll want to play it based on name for UI sounds,
                    // and by number based on area.audio ids
                });
            };

            var getAudioData = function (name) {
                if (!audioUseDefinitions[name]) { // intentional truthiness
                    return;
                }

                if (audioData[name] === undefined) {
                    var audioPromise = new Promise(function (resolve, reject) {
                        var aN = name;
                        var aF = audioUseDefinitions[aN].audioFile;
                        var loadAudio = function (file, data) {
                            if (file === aF) {
                                messengerEngine.unregister("ServicesEngine.loadAudio", loadAudio);

                                audioContext.decodeAudioData(data, function (buffer) {
                                    audioData[aN] = {
                                        isLooped: audioUseDefinitions[aN].isLooped,
                                        buffer: buffer
                                    };
                                    resolve(audioData[aN].buffer);
                                });
                            }
                        };

                        messengerEngine.register("ServicesEngine.loadAudio", that, loadAudio);
                    });

                    messengerEngine.post("AudioEngine.loadAudio", audioUseDefinitions[name].audioFile);

                    return audioPromise;
                } else {
                    return Promise.resolve(audioData[name].buffer);
                }
            };
    
            var playAudio = function (audio) {
                var audioName = (typeof audio === "number") ? audioUseDefinitions[audio].name : audio;

                if (audioUseDefinitions[audioName]) { // intentional truthiness
                    var isLooped = audioUseDefinitions[audioName].isLooped;
                    if (isLooped && playingLoopedAudio[audioName] != null) {
                        // don't play more than one of the same looped audio
                        return;
                    }
                    getAudioData(audioName).then(function (buffer) {
                        setTimeout(function () {
                            var source = audioContext.createBufferSource();
                            source.connect(audioContext.destination);
                            source.buffer = buffer;
                            if (isLooped) {
                                source.loop = true;
                                playingLoopedAudio[audioName] = source;
                            }
                            source.start(0);
                        }, 0);
                    });
                }
            };

            var stopAudio = function (audioName) {
                if (playingLoopedAudio[audioName] != null) {
                    playingLoopedAudio[audioName].stop(0);
                    playingLoopedAudio[audioName] = null;
                }
            };

            var loadAllAudio = function (audioUseData) {
                buildAudioUseDefinitions(audioUseData.audio);
            };

            var playAreaAudio = function (area) {
                playAudio(area.audio);
            };

            var loadEngine = function () {
                if (initAudioContext() && initSupportedFormat(audioElemTag)) {
                    messengerEngine.register("playAudio", this, playAudio);
                    messengerEngine.register("stopAudio", this, stopAudio);
                    messengerEngine.register("ServicesEngine.getAllAudio", this, loadAllAudio);
                    messengerEngine.register("GameStateEngine.setArea", this, playAreaAudio);

                    messengerEngine.post("AudioEngine.getAllAudio", supportedFormat.ext);
                }
            };

            loadEngine();
        }
    };
}(window.GinTub = window.GinTub || {}));