window.onload = async function () {
    if(await CheckIfXeroConnected()){
     return;   
    }
    else{
        ConnectToXero();

    }
    

}


function redirectToControllerAction() {
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
