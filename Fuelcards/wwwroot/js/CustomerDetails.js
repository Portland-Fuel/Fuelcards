function ToggleSection(SectionElement, buttonElement) {
    RemoveColorFromAllOtherbuttonsAndCloseAllSections();
    const elements = document.querySelectorAll(".section");
    elements.forEach(element => {
        element.style.display = "none";
    });

    buttonElement.style.backgroundColor = "lightblue";
    const element = document.getElementById(SectionElement);
    if (element.style.display === "none") {
        element.style.display = "block";
    } else if(element.style.display === "block") {
        element.style.display = "none";
    }
    else{
        element.style.display = "block";
    }
}
function RemoveColorFromAllOtherbuttonsAndCloseAllSections() {
    const elements = document.querySelectorAll("#sidenav a");
    elements.forEach(element => {
        element.style.backgroundColor = "";
    });

    const sections = document.querySelectorAll(".Section");
    sections.forEach(section => {
        section.style.display = "none";
    });
}



function ShowNetworkAccountNumberInput(element){
    const ParentElement = element.parentElement;
    const ParentOfParent = ParentElement.parentElement;


    const id = element.value + "AccountNumInput";
    
    if(!element.checked){
        document.querySelectorAll("#" + id).forEach(element => {
            element.remove();
        });
    }
    else{
        const InputElement = document.createElement("input");
        InputElement.type = "text";
        InputElement.id = element.value + "AccountNumInput";
        InputElement.required = true;
        InputElement.placeholder = "Enter Account Number";
        ParentOfParent.appendChild(InputElement);    
    }
}
