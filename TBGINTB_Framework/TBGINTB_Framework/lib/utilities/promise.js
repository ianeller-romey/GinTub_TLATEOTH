(function () {
    "use strict";

    if (Promise === undefined) {
        Promise = function (asyncSuccessFunction) {
            var resolution = null;
            var resolvedArg = undefined;
            var functionsOnSuccess = [];
            var functionsOnFailure = [];
            var that = this;

            var callOnResolved = function (functionOnSuccess) {
                if (resolvedArg !== undefined) {
                    functionOnSuccess(resolvedArg);
                }
                else {
                    functionOnSuccess();
                }
            };

            var resolve = function (arg) {
                resolution = true;
                resolvedArg = arg;

                while(functionsOnSuccess.length > 0) {
                    callOnResolved(functionsOnSuccess[i]);
                    functionsOnSuccess.shift();
                }
            };

            var reject = function (arg) {
                resolution = false;
                resolvedArg = arg;

                while (functionsOnFailure.length > 0) {
                    callOnResolved(functionsOnFailure[i]);
                    functionsOnFailure.shift();
                }
            };

            this.then = function (afterSuccessFunction, afterFailureFunction) {
                if (resolution === null) {
                    functionsOnSuccess.push(afterSuccessFunction);
                    if (afterFailureFunction) { // intentional truthiness
                        functionsOnFailure.push(afterFailureFunction);
                    }
                } else if (resolution === true) {
                    callOnResolved(afterSuccessFunction);
                } else if (resolution === false) {
                    if (afterFailureFunction) { // intentional truthiness
                        callOnResolved(afterFailureFunction);
                    }
                }

                return that;
            };

            setTimeout(function () {
                try {
                    asyncSuccessFunction(resolve, reject);
                } catch (ex) {
                    reject(ex);
                }
            }, 0);
        };

        Promise.resolve = function (arg) {
            return new Promise(function (res) {
                res(arg);
            });
        };

        Promise.reject = function (arg) {
            return new Promise(function (res, rej) {
                rej(arg);
            });
        };

        Promise.all = function () {
            if (arguments.length > 0) {
                var start = 0;
                var end = arguments.length;
                for (var i = 0; i < arguments.length; ++i) {
                    arguments[i].then(function () {
                        ++start;
                    });
                }
                return new Promise(function (res, rej) {
                    var checkCompletion = function () {
                        if (start === end) {
                            res();
                        } else {
                            setTimeout(checkCompletion, 1);
                        }
                    };
                    checkCompletion();
                });
            } else {
                return Promise.resolve();
            }
        };
    }
})();