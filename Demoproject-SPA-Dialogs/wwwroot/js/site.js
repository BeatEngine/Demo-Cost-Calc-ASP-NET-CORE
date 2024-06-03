// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function httpRequest(httpMethod, url, body, onloadCallback, size = 0, contenttype = "") {
    var xhr = new XMLHttpRequest();

    xhr.open(httpMethod, "/" + url, true);
    if (contenttype.length > 0) {
        xhr.overrideMimeType(contenttype);
        xhr.setRequestHeader("Content-Type", contenttype);
    }

    if (size > 0) {
        xhr.setRequestHeader("Content-Length", size.toString());
    }

    xhr.onload = onloadCallback;
    xhr.send(body);
}

function sendAndHandleEasyRestEvent(url, restRequestBody, reloadPage = false) {
    httpRequest("POST", url, restRequestBody, function onLoad() {
        try {
            const responseObj = JSON.parse(this.responseText);
            if (responseObj["success"].toString().toLowerCase() != 'true') {
                alert("Something went wrong while synchronizing with backend: " + responseObj["message"]);
            }
        } catch (err) {
            console.error(err);
            alert("Something went wrong while synchronizing with backend: " + err);
        }
        if (reloadPage) {
            window.location.reload();
        }

    }, restRequestBody.length);
}


