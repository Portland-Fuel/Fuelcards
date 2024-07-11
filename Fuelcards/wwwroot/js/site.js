window.onload = function () {
    ConnectToXero();

}

async function ConnectToXero() {
    try {
        // Open a new tab with the Xero link
        const response = await $.ajax({
            url: '/Home/GetLink',
            type: 'POST',
            contentType: 'application/json;charset=utf-8',
            data: JSON.stringify({}),
        });
        if (response === "Already connected to xero") {
            redirectToControllerAction();
            return;
        }
        const xeroLink = response;

        window.location.href = xeroLink;

    } catch (error) {
        console.error("Error:", error);
    }
}

function redirectToControllerAction() {
    // Redirect to the controller action
    window.location.href = 'Home/index';

}

