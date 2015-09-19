(function (namespace, undefined) {
    "use strict";

    namespace.Entities = namespace.Entities || {};
    namespace.Entities.Classes = namespace.Entities.Classes || {};
    namespace.Entities.Classes.ImageFader = function (locationsId) {
        var locationsElem = $(locationsId);

        this.initCanvas = function () {
            var webGL = null;
            try {
                var canvasElems = locationsElem.find("canvas");
                webGL = canvasElems[0].getContext("webgl") || canvasElems[0].getContext("experimental-webgl");
            }
            catch (e) {
            }

            if (!webGL) {
                webGL = null;
            }

            //return (webGL) ? true : false;
            return false;
        };

        this.initLegacy = function () {
            var FadeMe = function (imageId) {
                var imageElem = $(imageId);
                this.active = false;

                this.fadeIn = function (src) {
                    this.active = true;
                    imageElem.attr("src", src);
                    return imageElem.promiseToFade("slow", 1.0);
                };

                this.fadeOut = function () {
                    this.active = false;
                    var promise = imageElem.promiseToFade("slow", 0.0);
                    promise.then(function () {
                        imageElem.attr("src", "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=");
                    });
                    return promise;
                };
            };

            var locationsChildren = locationsElem.find("img");
            var image1Elem = new FadeMe("#" + locationsChildren.first()[0].id);
            var image2Elem = new FadeMe("#" + locationsChildren.last()[0].id);

            this.swap = function (src) {
                if (image1Elem.active) {
                    image1Elem.fadeOut();
                    image2Elem.fadeIn(src);
                }
                else {
                    image1Elem.fadeIn(src);
                    image2Elem.fadeOut();
                }
            }

            return true;
        };

        var fallback = false;
        var canvasElems = locationsElem.find("canvas");
        if (canvasElems.length > 0) {
            fallback = !this.initCanvas();
        } else {
            fallback = true;
        }

        if (fallback) {
            this.initLegacy();
        }
    };
}(window.GinTub = window.GinTub || {}));