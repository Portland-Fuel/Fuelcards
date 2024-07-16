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



function ShowNetworkAccountNumberInput(element){
    const Parent = element.parentElement.parentElement;
    const ElementToAppend = Parent.querySelector(".NetworkOptions");
    
    if (!element.checked) {
        const AllDivs = ElementToAppend.querySelectorAll("div");
        AllDivs.forEach(div => {
            div.remove();
        });

        const AllLabels = ElementToAppend.querySelectorAll("label");
        AllLabels.forEach(label => {
            label.remove();
        });

        const AllInputs = ElementToAppend.querySelectorAll("input");
        AllInputs.forEach(input => {
            input.remove();
        });
    } else {

        const FirstAddonInList = model[0];
        

        const AccountLabel = document.createElement("label");
        AccountLabel.textContent = "Account Number";
        AccountLabel.className = "AnimateNetworkOptions";
        ElementToAppend.appendChild(AccountLabel);

        const InputElement = document.createElement("input");
        InputElement.type = "text";
        InputElement.required = true;
        var placeholderTxt = element.value + " Account Number";
        InputElement.placeholder = placeholderTxt;
        InputElement.className = "AnimateNetworkOptions";
        InputElement.value = generateRandomValue(); // Set random value
        ElementToAppend.appendChild(InputElement);

        CreateAddonElementAndLabel(ElementToAppend,element, FirstAddonInList);

        CreateDateElementAndLabel(ElementToAppend,element, FirstAddonInList);

        if (document.getElementById('CustomerSearch').value !== "") {
            const divElement = document.createElement("div");
            divElement.className = "form-row-centered";
            ElementToAppend.appendChild(divElement);

            const loadButton = document.createElement("button");
            loadButton.className = "LoadHistoricButton";
            loadButton.textContent = "Load Historic";
            loadButton.type = "button";
            loadButton.className = "AnimateNetworkOptions LoadHistoricButton";
            loadButton.onclick = function () {
                LoadHistoricData(element);
            }
            divElement.appendChild(loadButton);
        }
    }
}
function CreateDateElementAndLabel(ElementToAppend,element,FirstAddonInList){
    const DateLabel = document.createElement("label");
    DateLabel.textContent = "Effective From";
    DateLabel.className = "AnimateNetworkOptions";
    ElementToAppend.appendChild(DateLabel);

    const DateElement = document.createElement("input");
    DateElement.type = "date";
    DateElement.required = true;
    var datePlaceholderTxt = element.value + " Date";
    DateElement.className = "AnimateNetworkOptions";
    DateElement.placeholder = datePlaceholderTxt;
    DateElement.value = generateRandomDate(); // Set random value
    ElementToAppend.appendChild(DateElement);
}
function CreateAddonElementAndLabel(ElementToAppend,element,FirstAddonInList){
    const AddonLabel = document.createElement("label");
    AddonLabel.textContent = "Addon";
    AddonLabel.className = "AnimateNetworkOptions";
    ElementToAppend.appendChild(AddonLabel);

    const AddonElement = document.createElement("input");
    AddonElement.type = "text";
    AddonElement.required = true;
    var addonPlaceholderTxt = element.value + " Addon";
    AddonElement.placeholder = addonPlaceholderTxt;
    AddonElement.className = "AnimateNetworkOptions";
    AddonElement.value = generateRandomValue(); // Set random value
    ElementToAppend.appendChild(AddonElement);
}




function LoadHistoricData(element){
    const NetworkUserSelected = element.value;
    const AccountNumInput = document.getElementById(NetworkUserSelected + "AccountNumInput").value;
    const AddonInput = document.getElementById(NetworkUserSelected + "AddonInput").value;
    const DateInput = document.getElementById(NetworkUserSelected + "DateInput").value;

    console.log("NetworkUserSelected: " + NetworkUserSelected);
    console.log("AccountNumInput: " + AccountNumInput);
    console.log("AddonInput: " + AddonInput);
    console.log("DateInput: " + DateInput);

    var HistoricData = GetHistoricData(AccountNumInput, AddonInput, DateInput);
    console.log("HistoricData: " + HistoricData);
    PopulateHistoricData(HistoricData);

}
function PopulateHistoricData(HistoricData){
    HistoricData.forEach(element => {
        console.log(element);
    });
}

function GetHistoricData(AccountNumInput, AddonInput, DateInput){
    return "Historic Data";
}
function generateRandomValue(){
    return Math.floor(Math.random() * 1000000000000);
}
function generateRandomDate(){
    var date = new Date();
    date.setDate(date.getDate() + Math.floor(Math.random() * 100));
    return date.toISOString().split('T')[0];

}

function CloseNewFixOverlay(){
    RemoveSelectElementFromNewFixForm();


    var overlayElement = document.getElementById("overlay");
    overlayElement.classList = "";
    overlayElement.classList.add("animate__zoomOutRight");
    overlayElement.classList.add("animate__animated");
    overlayElement.classList.add("overlay");

    overlayElement.hidden = true;
}

function ShowNewFixForm(){
    RemoveSelectElementFromNewFixForm();
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
        overlayElement.classList = "";
        overlayElement.classList.add("animate__zoomInRight");
        overlayElement.classList.add("animate__animated");
        overlayElement.classList.add("overlay");

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
    if(selectElement === null){
        return;
    }
    else{
        selectElement.remove();

    }
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
        
        var overlayElement = document.getElementById("overlay");
        overlayElement.hidden = true;
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
if(element.value === ""){
    return;
}
    await $.ajax({
        url: '/Home/CustomerDetails', 
        type: 'POST',
        data: JSON.stringify(element.value), 
        contentType: 'application/json;charset=utf-8',
        success: async function (response) {
           SortDataAndPopulateFromSearch(response);
        },
        error: async function (xhr, error) {
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

}


function GetModelToSubmitToController(){ 
    var form = document.getElementById("AddOrEditCustomerForm");
    var formData = new FormData(form);
    var values = Object.fromEntries(formData.entries());

    console.log("Values:");
    console.log(JSON.stringify(values, null, 2));
    var AddEditCustomerFormData = {
        customerName: values["customerName"],
        emailTo: values["emailTo"],
        emailCc: values["emailCc"],
        emailBcc: values["emailBcc"],
        keyFuelsInfo: {
            NewFixesForCustomer: KeyFuelsFixs,
            accountNumber: GetValueFromId("KeyFuelsAccountNumInput"),
            addon: GetValueFromId("KeyFuelsAddonInput"),
            date: GetValueFromId("KeyFuelsDateInput"), 
        },
        texacoInfo: {
            NewFixesForCustomer: TexacoFixs,
            accountNumber: GetValueFromId("TexacoAccountNumInput"),
            addon: GetValueFromId("TexacoAddonInput"),
            date: GetValueFromId("TexacoDateInput"), 
        },
        uKFuelsInfo: {
            NewFixesForCustomer: UkFuelsFixs,
            accountNumber: GetValueFromId("UkFuelsAccountNumInput"),
            addon: GetValueFromId("UkFuelsAddonInput"),
            date: GetValueFromId("UkFuelsDateInput"), 
        },
        fuelGenieInfo: {
            NewFixesForCustomer: FuelGenieFixs,
            accountNumber: GetValueFromId("FuelgenieAccountNumInput"),
            addon: GetValueFromId("FuelgenieAddonInput"),
            date: GetValueFromId("FuelgenieDateInput"), 
        },
        invoiceOrderType: values["invoiceOrderType"],
        paymentTerm: values["paymentTerm"]
    };
    
    return AddEditCustomerFormData;
}


function GetValueFromId(id){
    var ElementToGetValue =  document.getElementById(id);
    if(ElementToGetValue === null || ElementToGetValue === undefined){
        return null;
    }
    else{
        return ElementToGetValue.value;
    }
}

document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('AddOrEditCustomerForm').addEventListener('submit', async function(event) {
        event.preventDefault();

        var form = document.getElementById("AddOrEditCustomerForm");
        var submitButton = document.getElementById("SubmitButton");

        // Prevent multiple submissions
        if(submitButton.classList.contains('animate')) {
            return;
        }

        animateButton(submitButton);

        var formData = new FormData(form);
        var values = Object.fromEntries(formData.entries());
        var parsedValues = JSON.stringify(values);

        var ModelFormToSubmitToController = GetModelToSubmitToController();
        console.log("ModelToSubmit:");
        console.log(JSON.stringify(ModelFormToSubmitToController, null, 2));

        try {
            let response = await $.ajax({
                url: '/CustomerDetails/SubmitAddOrEdit', 
                type: 'POST',
                data: JSON.stringify(ModelFormToSubmitToController), 
                contentType: 'application/json;charset=utf-8'
            });
            SortDataAndPopulateFromSearch(response);
            handleSuccess(submitButton);
            Toast.fire({
                icon: 'success',
                title: 'Form submitted successfully'
            });
        } catch (xhr) {
            console.error('XHR Response:', xhr.responseText || 'No response from server');
            console.error('Error:', xhr.statusText || 'No error message');
            handleError(submitButton, xhr);
            Toast.fire({
                icon: 'error',
                title: 'Error submitting form',
                text: xhr.statusText + ': ' + xhr.responseText,
                footer: "Contact IT if you have issues <a href='https://192.168.0.17:666/Home/ReportIssue'>Report Issue</a>"

            });
        }
    });

    function animateButton(button) {
        button.classList.add('animate');
        button.innerHTML = 'Submitting...<div class="loader"></div>';
        button.disabled = true;
    }

    function handleSuccess(button) {
        button.disabled = true;
        button.classList.remove('animate');
        button.classList.add('success');
        button.innerHTML = 'Submitted<div class="success-icon"><i class="fas fa-check"></i></div>';
        setTimeout(function() {
            button.classList.remove('success');
            button.innerHTML = 'Submit';
            button.disabled = false;
        }, 3500);
    }

    function handleError(button, xhr) {
        button.disabled = true;
        button.classList.remove('animate');
        button.classList.add('error');
        button.innerHTML = 'Error<div class="error-icon"><i class="fas fa-exclamation"></i></div>';
        setTimeout(function() {
            button.classList.remove('error');
            button.innerHTML = 'Submit';
            button.disabled = false;
        }, 3500);
    }
});

const Toast = Swal.mixin({
    toast: true,
    position: "top-end",
    showConfirmButton: false,
    timer: 3500,
    timerProgressBar: true,
    didOpen: (toast) => {
      toast.onmouseenter = Swal.stopTimer;
      toast.onmouseleave = Swal.resumeTimer;
    }
  });
 