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

                    messengerEngine.post("TimeEngine.updateTime", time);

                    if (time.minutes() % 10 == 0) {
                        messengerEngine.post("TimeEngine.updateTimeAtTen", time);
                    }
                }
            };

            var start = function () {
                setInterval(updateTime, 5000);
            };

            this.setTime = function (setTo) {
                clearInterval(updateTime);
                time = setTo;
                start();
            };

            this.pause = function () {
                paused = true;
            };

            this.unpause = function () {
                paused = false;
            };

            messengerEngine.register("VerbList.openExec", this, this.pause);
            messengerEngine.register("VerbList.closeExec", this, this.unpause);

            start();
        }
    };
}(window.GinTub = window.GinTub || {}));