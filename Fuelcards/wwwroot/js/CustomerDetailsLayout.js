function ToggleSection(SectionElement, buttonElement) {
    RemoveColorFromAllOtherbuttonsAndCloseAllSections();
    const elements = document.querySelectorAll(".section");
    elements.forEach(element => {
        element.style.display = "none";
    });

    buttonElement.style.backgroundColor = "#464949";
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