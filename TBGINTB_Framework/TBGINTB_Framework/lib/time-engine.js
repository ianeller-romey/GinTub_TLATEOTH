
var TimeEngine = function () {
    var messengerEngine = globalMessengerEngine;

    var time = new Date(1988, 7, 13, 0, 0, 0, 0);

    var updateTime = function () {
        var hours = time.getHours();
        var minutes = time.getMinutes() + 1;
        if (minutes == 60) {
            minutes = 0;
            hours = hours + 1;
            if (hours == 24) {
                hours = 0;
            }
        }
        time.setHours(hours);
        time.setMinutes(minutes);
        messengerEngine.post("TimeEngine.updateTime", time);

        if (minutes % 10 == 0) {
            messengerEngine.post("TimeEngine.updateTimeAtTen", time);
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

    start();
};
