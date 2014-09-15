/// <reference path="typings/jquery/jquery.d.ts" />

var Main = (function () {
    function Main() {
        this.$captchaContainer = $('#sample-captcha');
        this.$form = $("#form-sample");
        this.$statusContainer = $("#status");
        this.$statusIcon = $("#status-icon");
        this.$statusText = $("#status-text");
        this.$statusMessage = $("#status-message");
        this.$checkIsFilled = $("#check-is-filled");
        this.initializeCaptcha();
        this.bindHandlers();
    }
    Main.prototype.initializeCaptcha = function () {
        this.captcha = this.$captchaContainer.visualCaptcha({
            imgPath: '../img/',
            captcha: {
                numberOfImages: 5,
                routes: {
                    start: "/Home/Start",
                    audio: "/Home/Audio",
                    image: "/Home/Image"
                }
            }
        }).data("captcha");
    };

    Main.prototype.bindHandlers = function () {
        var _this = this;
        // Bind form submission behavior
        this.$form.submit(function () {
            if (_this.captcha.getCaptchaData().valid) {
                _this.attemptTry();
            } else {
                _this.setStatus({
                    success: false,
                    message: "Please select an option."
                });
            }
        });

        // Bind click event to "Check if visualCaptcha is filled" button
        this.$checkIsFilled.click(function () {
            _this.showVisualCaptchaFilled();
        });
    };

    Main.prototype.attemptTry = function () {
        var _this = this;
        $.ajax({
            type: "POST",
            url: "Home/Try",
            data: {
                "value": this.captcha.getCaptchaData().value
            }
        }).done(function (result) {
            _this.setStatus(result);
        }).fail(function () {
            _this.setStatus({
                success: false,
                message: "There was a problem attempting to verify the captcha; please try again."
            });
        }).always(function () {
            // Regardless of whether the request itself is a success, we need to load up a new captcha set
            _this.captcha.refresh();
        });
    };

    Main.prototype.setStatus = function (result) {
        if (result.success) {
            this.$statusContainer.addClass("valid");
        } else {
            this.$statusContainer.removeClass("valid");
        }

        this.$statusIcon.removeClass().addClass(result.success ? "icon-yes" : "icon-no");
        this.$statusText.text(result.message);
        this.$statusMessage.show();
    };

    Main.prototype.showVisualCaptchaFilled = function () {
        if (this.captcha.getCaptchaData().valid) {
            window.alert('visualCaptcha is filled!');
        } else {
            window.alert('visualCaptcha is NOT filled!');
        }
    };
    return Main;
})();

new Main();
//# sourceMappingURL=main.js.map
