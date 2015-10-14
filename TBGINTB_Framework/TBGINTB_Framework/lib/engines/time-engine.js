(function (namespace, undefined) {
    "use strict";

    namespace.Engines = namespace.Engines || {};
    namespace.Engines.TimeEngine = {
        init: function (messengerEngine) {
            var time = moment.duration.fromIsoduration("PT0S");
            var minuteIncr = moment.duration(1, 'm');
            var hourIncr = moment.duration(1, 'h');
            var minuteSub = moment.duration(60, 'm');
            var hourSub = moment.duration(24, 'h');
            var paused = false;

            var gameLoaded = false;

            var that = this;

            var postTimeMessages = function () {
                messengerEngine.post("TimeEngine.updateTime", time);

                if (time.minutes() % 10 == 0) {
                    messengerEngine.post("TimeEngine.updateTimeAtTen", time);
                }
            };

            var updateTime = function () {
                if (!paused) {
                    time.add(minuteIncr);
                    if (time.minutes() == minuteSub.minutes()) {
                        // set minutes back to zero
                        time.subtract(minuteSub);

                        time.add(hourIncr);
                        if (time.hours() == hourSub.hours()) {
                            // set hours back to zero
                            time.subtract(hourSub);
                        }
                    }

                    postTimeMessages();
                }
            };

            var start = function () {
                setInterval(updateTime, 5000);
            };

            var registerAfterGameHasLoaded = function () {
                messengerEngine.register("VerbList.openExec", that, pause);
                messengerEngine.register("VerbList.closeExec", that, unpause);
                messengerEngine.register("ClockList.pauseClick", that, pause);
                messengerEngine.register("PauseFader.unpauseClick", that, unpause);

                gameLoaded = true;
            };

            var loadGame = function (playData) {
                if (!gameLoaded) {
                    registerAfterGameHasLoaded();
                }
                setTime(moment.duration.fromIsoduration(playData.lastTime));
            };

            var setTime = function (setTo) {
                clearInterval(updateTime);
                time = setTo;
                postTimeMessages();
                start();
            };

            var pause = function () {
                paused = true;
            };

            var unpause = function () {
                paused = false;
            };

            messengerEngine.register("ServicesEngine.loadGame", this, loadGame);
        }
    };
}(window.GinTub = window.GinTub || {}));