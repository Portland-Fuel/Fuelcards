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