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


    const AccountNumInputID = element.value + "AccountNumInput";
    const AddonNumInputID = element.value + "AddonInput";
    const DateInputID = element.value + "DateInput";
    
    if(!element.checked){
        document.querySelectorAll("#" + AccountNumInputID).forEach(element => {
            element.remove();
        });
        document.querySelectorAll("#" + AddonNumInputID).forEach(element => {
            element.remove();
        });
        document.querySelectorAll("#" + DateInputID).forEach(element => {
            element.remove();
        });
    }
    else{
        const InputElement = document.createElement("input");
        InputElement.type = "text";
        InputElement.id = element.value + "AccountNumInput";
        InputElement.required = true;
        var placeholderTxt = element.value + " Account Number";
        InputElement.placeholder = placeholderTxt;
        ParentOfParent.appendChild(InputElement);    


        const AddonElement = document.createElement("input");
        AddonElement.type = "text";
        AddonElement.id = element.value + "AddonInput";
        AddonElement.required = true;
        var addonPlaceholderTxt = element.value + " Addon";
        AddonElement.placeholder = addonPlaceholderTxt;
        ParentOfParent.appendChild(AddonElement);   

        const DateElement = document.createElement("input");
        DateElement.type = "date";
        DateElement.id = element.value + "DateInput";
        DateElement.required = true;
        var datePlaceholderTxt = element.value + " Date";
        DateElement.placeholder = datePlaceholderTxt;
        ParentOfParent.appendChild(DateElement);  
    }
}


function CloseNewFixOverlay(){
    RemoveSelectElementFromNewFixForm();


    var overlayElement = document.getElementById("overlay");

    overlayElement.hidden = true;
}

function ShowNewFixForm(){
    const SelectEleForUsers = GetSelectOptionForUserForNewFixForm();

    if (SelectEleForUsers.options.length === 0) {
        Swal.fire({
            icon: 'warning',
            title: 'No network options available',
            text: 'Please select at least one network network.',
        });
    } else {
        const parentElement = document.getElementById("NewFixSelectParent");
        parentElement.appendChild(SelectEleForUsers);
        var overlayElement = document.getElementById("overlay");
        overlayElement.hidden = false;
    }
   
}

function GetSelectOptionForUserForNewFixForm(){
    const NetworkCheckBoxes = document.querySelectorAll(".NetworkCheck");
    var Options = [];
    NetworkCheckBoxes.forEach(element => {
        if(element.checked){
            Options.push(element.value);
        }
    });

    const selectElement = document.createElement("select");
    selectElement.id = "NewFixNetworkSelect";
    Options.forEach(option => {
        const optionElement = document.createElement("option");
        optionElement.value = option;
        optionElement.text = option;
        selectElement.appendChild(optionElement);
    });

    return selectElement;
}

function RemoveSelectElementFromNewFixForm(){
    const selectElement = document.getElementById("NewFixNetworkSelect");
    selectElement.remove();
}

function AddNewFix(event){
    event.preventDefault();

    const form = document.getElementById("NewFixForm");
    if (form) {
        const formData = new FormData(form);
        const values = Object.fromEntries(formData.entries());
        
        const selectElement = document.getElementById("NewFixNetworkSelect");
        const selectedOption = selectElement.options[selectElement.selectedIndex].value;
        values["network"] = selectedOption;
        
        const periodElement = document.getElementById("period");
        const selectedPeriod = periodElement.options[periodElement.selectedIndex].value;
        values["period"] = selectedPeriod;
        
        const gradeElement = document.getElementById("grade");
        const selectedGrade = gradeElement.options[gradeElement.selectedIndex].value;
        values["grade"] = selectedGrade;
        console.log(values);
        
        var NetworkUserSelected = values["network"];
        var FixsListToAddTo = GetFixListToAddTo(NetworkUserSelected);
        console.log("Adding the fix to the network of " + NetworkUserSelected);
        FixsListToAddTo.push(values); 
        ChangeLabelToShowPlus1Fix(NetworkUserSelected);
        console.log("KeyFuelsFixs: " + KeyFuelsFixs);
        
        
    }



       
}

function GetFixListToAddTo(NetworkUserSelected){
    switch(NetworkUserSelected){
        case "KeyFuels":
            return KeyFuelsFixs;
        case "Texaco":
            return TexacoFixs;
        case "UkFuels":
            return UkFuelsFixs;
        case "FuelGenie":
            return FuelGenieFixs;
    }
}
function ChangeLabelToShowPlus1Fix(NetworkUserSelected){
    const labelElement = document.getElementById(NetworkUserSelected + "Span");
    var SpanText = labelElement.innerText
    if(SpanText === "No Fixes"){
        labelElement.textContent = "1 Fix";
    }
    else{
        var CurrentFixNum = SpanText.split(" ")[0];
        var NewFixNum = parseInt(CurrentFixNum) + 1;
        labelElement.textContent = NewFixNum + " Fixes";
    }
}


const KeyFuelsFixs = []
const TexacoFixs = []
const UkFuelsFixs = []
const FuelGenieFixs = []





async function CustomerSearchInput(element){

    ShowLoader();
if(element.value === ""){
    return;
}
    await $.ajax({
        url: '/CustomerDetails/SearchCustomer', 
        type: 'POST',
        data: JSON.stringify(element.value), 
        contentType: 'application/json;charset=utf-8',
        success: async function (response) {
            HideLoader();
           SortDataAndPopulateFromSearch(response);
        },
        error: async function (xhr, status, error) {
            HideLoader();
            element.value = "";
            console.error('XHR Response:', xhr.responseText || 'No response from server');
            console.error('Error:', error || 'No error message');
            await Swal.fire({
                icon: "error",
                title: "Sorry there has been a big error",
                text: xhr.responseText || 'An unknown error occurred.',
                footer: '<a href="https://192.168.0.17:666/" target="_blank">Report it here!</a>'
            });
        }
    });
}


function SortDataAndPopulateFromSearch(data){

    alert("Not implemented yet");
}

function SubmitAddOrEditCustomer(event){
    event.preventDefault();
    var form = document.getElementById("AddOrEditCustomerForm");
    var formData = new FormData(form);
    var values = Object.fromEntries(formData.entries());
    var parsedValues = JSON.stringify(values);
    
    console.log("Form to submit: " + parsedValues);


}


