function ShowLoader() {
    var Body = document.getElementById("Body");
    Body.hidden = true;
    var Loader = document.getElementById("Loader");
    Loader.hidden = false;
}

function HideLoader() {
    var Body = document.getElementById("Body");
    Body.hidden = false;
    var Loader = document.getElementById("Loader");
    Loader.hidden = true;
}

async function ConnectToXero() {
    try {
        // Open a new tab with the Xero link
        const response = await $.ajax({
            url: '/Home/GetLink',
            type: 'POST',
            contentType: 'application/json;charset=utf-8',
        });
        const xeroLink = response;

        window.location.href = xeroLink;

    } catch (error) {
        console.error("Error:", error);
    }
}
