/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
interface JQuery {
	visualCaptcha: any;
}

class Main {
	$captchaContainer: JQuery = $('#sample-captcha');
	$form: JQuery = $("#form-sample");
	$statusContainer: JQuery = $("#status");
	$statusIcon: JQuery = $("#status-icon");
	$statusText: JQuery = $("#status-text");
	$statusMessage: JQuery = $("#status-message");
	$checkIsFilled: JQuery = $("#check-is-filled");

	captcha: any;

	constructor() {
		this.initializeCaptcha();
		this.bindHandlers();
	}

	private initializeCaptcha(): void {
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
	}

	private bindHandlers(): void {

		// Bind form submission behavior
		this.$form.submit(() => {
			this.attemptTry();
		});

		// Bind click event to "Check if visualCaptcha is filled" button
		this.$checkIsFilled.click(this.showVisualCaptchaFilled);
	}

	private attemptTry(): void {
		$.ajax({
			type: "POST",
			url: "Home/Try",
			data: {
				"value": this.captcha.getCaptchaData().value
			}
		}).done(result=> {
				this.setStatus(result);
			}).fail(() => {
				this.setStatus({
					success: false,
					message: "There was a problem attempting to verify the captcha; please try again."
				});
			}).always(this.captcha.refresh()); // Regardless of whether the request itself is a success, we need to load up a new captcha set
	}

	private setStatus(result: any): void {
		if (result.success) {
			this.$statusContainer.addClass("valid");
		} else {
			this.$statusContainer.removeClass("valid");
		}

		this.$statusIcon.removeClass().addClass(result.success ? "icon-yes" : "icon-no");
		this.$statusText.text(result.message);
		this.$statusMessage.show();
	}

	private showVisualCaptchaFilled() : void {
		if (this.captcha.getCaptchaData().valid) {
			window.alert('visualCaptcha is filled!');
		} else {
			window.alert('visualCaptcha is NOT filled!');
		}
	}
}

new Main();