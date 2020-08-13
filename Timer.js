function LoadTimer() {
    if (document.getElementById('stateDebug').innerText === '') {
        document.getElementById('submit').innerHTML = "Proceed Carefully"
    } else {
        document.getElementById('TimerAlert').hidden = false;
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
