window.onload = async function () {
    if(await CheckIfXeroConnected()){
     return;   
    }
    else{
        ConnectToXero();

    }
    

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

function redirectToControllerAction() {
    // Redirect to the controller action
    window.location.href = 'Home/index';

}

async function CheckIfXeroConnected() {
    const response = await $.ajax({
        url: '/Home/CheckXeroConnection',
        type: 'POST',
        contentType: 'application/json;charset=utf-8',
    });
    if (response === "Already connected to xero") {
        redirectToControllerAction();
        return true;
    }
    else {
        console.log("Not connected to Xero");
        return false;
    }
}
