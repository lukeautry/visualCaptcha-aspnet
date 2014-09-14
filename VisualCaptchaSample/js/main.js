$(document).ready(function () {

	var element = $('#sample-captcha').visualCaptcha({
		imgPath: '../img/',
		captcha: {
			numberOfImages: 6
		}
	});

	var captcha = element.data('captcha');
	var statusElement = $("#status-message");

	// Show success/error messages
	//if (queryString.indexOf('status=noCaptcha') !== -1) {
	//	statusElement.innerHTML = '<div class="status"> <div class="icon-no"></div> <p>visualCaptcha was not started!</p> </div>' + statusElement.innerHTML;
	//} else if (queryString.indexOf('status=validImage') !== -1) {
	//	statusElement.innerHTML = '<div class="status valid"> <div class="icon-yes"></div> <p>Image was valid!</p> </div>' + statusElement.innerHTML;
	//} else if (queryString.indexOf('status=failedImage') !== -1) {
	//	statusElement.innerHTML = '<div class="status"> <div class="icon-no"></div> <p>Image was NOT valid!</p> </div>' + statusElement.innerHTML;
	//} else if (queryString.indexOf('status=validAudio') !== -1) {
	//	statusElement.innerHTML = '<div class="status valid"> <div class="icon-yes"></div> <p>Accessibility answer was valid!</p> </div>' + statusElement.innerHTML;
	//} else if (queryString.indexOf('status=failedAudio') !== -1) {
	//	statusElement.innerHTML = '<div class="status"> <div class="icon-no"></div> <p>Accessibility answer was NOT valid!</p> </div>' + statusElement.innerHTML;
	//} else if (queryString.indexOf('status=failedPost') !== -1) {
	//	statusElement.innerHTML = '<div class="status"> <div class="icon-no"></div> <p>No visualCaptcha answer was given!</p> </div>' + statusElement.innerHTML;
	//}

	// Show an alert saying if visualCaptcha is filled or not
	var showVisualCaptchaFilled = function () {
		if (captcha.getCaptchaData().valid) {
			window.alert('visualCaptcha is filled!');
		} else {
			window.alert('visualCaptcha is NOT filled!');
		}
	};

	// Bind click event to "Check if visualCaptcha is filled" button
	var filledElement = $("#check-is-filled");
	filledElement.click(showVisualCaptchaFilled);

});