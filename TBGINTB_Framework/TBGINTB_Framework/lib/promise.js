(function () {
    if (Promise === undefined) {
        Promise = function (asyncSuccessFunction) {
            var functionsOnSuccess = [];
            var that = this;

            var resolve = function (arg) {
                if (arg !== undefined) {
                    for (var i = 0, j = functionsOnSuccess.length; i < j; ++i) {
                        functionsOnSuccess[i](arg);
                    }
                }
                else {
                    for (var i = 0, j = functionsOnSuccess.length; i < j; ++i) {
                        functionsOnSuccess[i]();
                    }
                }
            };

            var reject = function (args) {
                throw -1;
            };

            this.then = function (afterFunction) {
                functionsOnSuccess.push(afterFunction);
                return that;
            };

            setTimeout(function () {
                asyncSuccessFunction(resolve, reject);
            }, 0);
        };
    }
})();