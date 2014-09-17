/// <reference path="typings/jquery/jquery.d.ts" />
interface JQuery {
	visualCaptcha: any;
}

class Main {
	private $captchaContainer: JQuery = $('#sample-captcha');
	private $form: JQuery = $("#form-sample");
	private $statusContainer: JQuery = $("#status");
	private $statusIcon: JQuery = $("#status-icon");
	private $statusText: JQuery = $("#status-text");
	private $statusMessage: JQuery = $("#status-message");
	private $checkIsFilled: JQuery = $("#check-is-filled");
	private captcha: any;

	constructor() {
		this.initializeCaptcha();
		this.bindHandlers();
	}

	private initializeCaptcha(): void {
		this.captcha = this.$captchaContainer.visualCaptcha({
			imgPath: '../img/',
			captcha: {
				url: "/Home",
				numberOfImages: 5,
				routes: {
					start: "/Start",
					audio: "/Audio",
					image: "/Image"
				}
			}
		}).data("captcha");
	}

	private bindHandlers(): void {

		// Bind form submission behavior
		this.$form.submit(() => {
			if (this.captcha.getCaptchaData().valid) {
				this.attemptTry();
			} else {
				this.setStatus({
					success: false,
					message: "Please select an option."
				});
			}
		});

		// Bind click event to "Check if visualCaptcha is filled" button
		this.$checkIsFilled.click(()=> {
			this.showVisualCaptchaFilled();
		});
	}

	private attemptTry(): void {
		$.ajax({
			type: "POST",
			url: "Home/Try",
			data: {
				"value": this.captcha.getCaptchaData().value
			}
		}).done(result => {
				this.setStatus(result);
			}).fail(() => {
				this.setStatus({
					success: false,
					message: "There was a problem attempting to verify the captcha; please try again."
				});
			}).always(() => {
			// Regardless of whether the request itself is a success, we need to load up a new captcha set
			this.captcha.refresh();
		}); 
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

var vm = new Main();