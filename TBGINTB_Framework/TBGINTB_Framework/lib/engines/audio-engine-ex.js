﻿(function (namespace, undefined) {
    "use strict";

    namespace.Engines = namespace.Engines || {};
    namespace.Engines.AudioEngineEx = {
        init: function (messengerEngine) {
            var audioUseDefinitions = {};
            var audioData = {};
            var playingLoopedAudio = {};

            var audioContext;
            var gainNode;
            var areaGainNodes = [];
            var activeAreaGainNode = 0,
                maxGainNodes = 2;
            var volume;

            var that = this;

            var initAudioContext = function () {
                try {
                    window.AudioContext = window.AudioContext || window.webkitAudioContext;
                    audioContext = new AudioContext();

                    if (!audioContext.createGain)
                        audioContext.createGain = audioContext.createGainNode;

                    gainNode = audioContext.createGain();
                    gainNode.connect(audioContext.destination);

                    for (var i = 0; i < maxGainNodes; ++i) {
                        var gainN = audioContext.createGain();
                        gainN.connect(audioContext.destination);
                        activeAreaGainNode = areaGainNodes.push(gainN) - 1;
                    }
                    areaGainNodes[activeAreaGainNode].gain.linearRampToValueAtTime(volume, audioContext.currentTime);
                }
                catch (e) {
                }

                if (!audioContext) {
                    audioContext = null;
                }

                return (audioContext) ? true : false;
            };

            var buildAudioUseDefinitions = function (definitions) {
                audioUseDefinitions = definitions;
            };

            var getAudioData = function (name) {
                if (!audioUseDefinitions[name]) { // intentional truthiness
                    return;
                }

                if (audioData[name] === undefined) {
                    // if the data isn't loaded, load it asynchronously
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

                    messengerEngine.post("AudioEngineEx.loadAudio", audioUseDefinitions[name].audioFile);

                    return audioPromise;
                } else {
                    // data's already loaded, not much to do here
                    return Promise.resolve(audioData[name].buffer);
                }
            };
    
            var playAudio = function (audio, gainN) {
                var audioName = (typeof audio === "number") ? audioUseDefinitions[audio].name : audio;
                gainN = (gainN) /* intentional truthiness */ ? gainN : gainNode;

                if (audioUseDefinitions[audioName]) { // intentional truthiness
                    var isLooped = audioUseDefinitions[audioName].isLooped;
                    if (isLooped && playingLoopedAudio[audioName] != null) {
                        // don't play more than one of the same looped audio
                        return;
                    }
                    getAudioData(audioName).then(function (buffer) {
                        var source = audioContext.createBufferSource();
                        source.connect(gainN);
                        source.buffer = buffer;
                        if (isLooped) {
                            source.loop = true;
                            playingLoopedAudio[audioName] = source;
                        }
                        source.start(0);
                    });
                }
            };

            var stopAudio = function (audioName) {
                if (playingLoopedAudio[audioName] != null) {
                    playingLoopedAudio[audioName].stop(0);
                    playingLoopedAudio[audioName] = null;
                }
            };

            var playAreaAudio = function (area) {
                var oldN = areaGainNodes[activeAreaGainNode];
                activeAreaGainNode = (activeAreaGainNode + 1) % areaGainNodes.length;
                var newN = areaGainNodes[activeAreaGainNode];

                playAudio(area.audio, newN);

                oldN.gain.linearRampToValueAtTime(0, audioContext.currentTime);
                newN.gain.linearRampToValueAtTime(volume, audioContext.currentTime);
            };

            var setVolume = function (v) {
                volume = v;
                gainNode.gain.value = volume;
                if (areaGainNodes[activeAreaGainNode]) { // intentional truthiness
                    areaGainNodes[activeAreaGainNode].gain.linearRampToValueAtTime(v, audioContext.currentTime);
                }
            };

            var setMute = function (isMuted) {
                setVolume((isMuted) ? 0.0 : volume);
            };

            var loadEngine = function () {
                if (initAudioContext()) {
                    messengerEngine.register("playAudio", this, playAudio);
                    messengerEngine.register("stopAudio", this, stopAudio);
                    messengerEngine.register("VolumeManager.setVolume", this, setVolume);
                    messengerEngine.register("VolumeManager.setMute", this, setMute);
                    messengerEngine.register("AudioDataEngine.buildAudioUseDefinitions", this, buildAudioUseDefinitions);
                }
            };

            loadEngine();
        }
    };
}(window.GinTub = window.GinTub || {}));