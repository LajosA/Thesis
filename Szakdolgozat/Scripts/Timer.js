var start;

function instance() {
    time += 1000;
    elapsed = Math.floor(time / 1000);

    document.getElementById('LTPerc').textContent = elapsed;
    document.getElementById('hf').value = elapsed;

    if (elapsed == 45) {
        document.getElementById('BtnHalfTime').style.visibility = 'visible';
    }
    else {
        var diff = (new Date().getTime() - start) - time+1000;
        window.setTimeout(instance, (1000 - diff));
    }
}



function instance2() {
    time += 1000;
    elapsed = Math.floor(time / 1000);

    document.getElementById('LTPerc').textContent = elapsed;
    document.getElementById('hf').value = elapsed;
    if (elapsed == 90) {
        document.getElementById('BtnFullTime').style.visibility = 'visible';
    }
    else {
        var diff = (new Date().getTime() - start) - time + 46000;
        window.setTimeout(instance2, (1000 - diff));
    }

}

function kezdes() {
    start = new Date().getTime(),
        time = 1000,
        elapsed = '1';
    window.setTimeout(instance, 1000);
}

function ujrakezdes() {

    start = new Date().getTime(),
        time = 46000,
        elapsed = '46';
    window.setTimeout(instance2, 1000);
}