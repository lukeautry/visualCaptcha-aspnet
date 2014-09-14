var element = $('#sample-captcha').visualCaptcha({
	imgPath: '../img/',
	captcha: {
		numberOfImages: 5
	},
	routes: {
		start: "../Start"
	}
});

// use next code for getting captcha object
var catcha = element.data('captcha');

var statusElement = document.getElementById('status-message');
var queryString = window.location.search;

// Show success/error messages
if (queryString.indexOf('status=noCaptcha') !== -1) {
	statusElement.innerHTML = '<div class="status"> <div class="icon-no"></div> <p>visualCaptcha was not started!</p> </div>' + statusElement.innerHTML;
} else if (queryString.indexOf('status=validImage') !== -1) {
	statusElement.innerHTML = '<div class="status valid"> <div class="icon-yes"></div> <p>Image was valid!</p> </div>' + statusElement.innerHTML;
} else if (queryString.indexOf('status=failedImage') !== -1) {
	statusElement.innerHTML = '<div class="status"> <div class="icon-no"></div> <p>Image was NOT valid!</p> </div>' + statusElement.innerHTML;
} else if (queryString.indexOf('status=validAudio') !== -1) {
	statusElement.innerHTML = '<div class="status valid"> <div class="icon-yes"></div> <p>Accessibility answer was valid!</p> </div>' + statusElement.innerHTML;
} else if (queryString.indexOf('status=failedAudio') !== -1) {
	statusElement.innerHTML = '<div class="status"> <div class="icon-no"></div> <p>Accessibility answer was NOT valid!</p> </div>' + statusElement.innerHTML;
} else if (queryString.indexOf('status=failedPost') !== -1) {
	statusElement.innerHTML = '<div class="status"> <div class="icon-no"></div> <p>No visualCaptcha answer was given!</p> </div>' + statusElement.innerHTML;
}


// Binds an element to callback on click
// @param element object like document.getElementById() (has to be a single element)
// @param callback function to run when the element is clicked
var bindClick = function (element, callback) {
	if (element.addEventListener) {
		element.addEventListener('click', callback, false);
	} else {
		element.attachEvent('onclick', callback);
	}
};

// Show an alert saying if visualCaptcha is filled or not
var sayIsVisualCaptchaFilled = function () {
	if (captcha.getCaptchaData().valid) {
		window.alert('visualCaptcha is filled!');
	} else {
		window.alert('visualCaptcha is NOT filled!');
	}
};

// Bind that function to the appropriate link
var filledElement = $("#check-is-filled")[0];
bindClick(filledElement, sayIsVisualCaptchaFilled);