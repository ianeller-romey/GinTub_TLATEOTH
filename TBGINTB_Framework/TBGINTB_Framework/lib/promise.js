(function () {
    if (Promise === undefined) {
        Promise = function (asyncSuccessFunction) {
            var resolved = false;
            var resolvedArg = undefined;
            var functionsOnSuccess = [];
            var that = this;

            var callOnSuccess = function (functionOnSuccess) {
                if (resolvedArg !== undefined) {
                    functionOnSuccess(resolvedArg);
                }
                else {
                    functionOnSuccess();
                }
            };

            var resolve = function (arg) {
                resolved = true;
                resolvedArg = arg;

                while(functionsOnSuccess.length > 0) {
                    callOnSuccess(functionsOnSuccess[i]);
                    functionsOnSuccess.shift();
                }
            };

            var reject = function (args) {
                throw -1;
            };

            this.then = function (afterFunction) {
                if (!resolved) {
                    functionsOnSuccess.push(afterFunction);
                }
                else {
                    callOnSuccess(afterFunction);
                }

                return that;
            };

            setTimeout(function () {
                asyncSuccessFunction(resolve, reject);
            }, 0);
        };
    }
})();