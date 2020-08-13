function LoadTimer() {
    if (document.getElementById('stateDebug').innerText === '') {
        document.getElementById('divRemainingTime').innerText = "This quiz is timed. You will have 10 seconds per question."
    } else {
        var timeInterval = '10000';
        var remainingSeconds = (timeInterval / 1000);
        var timeInterval = setInterval(function () {
            document.getElementById('divRemainingTime').innerText = --remainingSeconds;

            if (remainingSeconds <= 0) {
                window.location.href = "Default.aspx";
                clearInterval(timeInterval);
            }
        }, 1000);
    }
}
