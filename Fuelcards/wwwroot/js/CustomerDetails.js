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
async function AddNewAddon(element) {
    const table = document.getElementById('UniqueNetworkTable');
    const tableBody = table.querySelector('tbody');
    const rows = tableBody.querySelectorAll('tr');
    const row = rows[0];

    if (!row) {
        LoadUniqueNetworkOverlayHeaders();
        const result = await promptForInputs('Fill out all fields', getInitialInputsHtml());
        if (result.isConfirmed) {
            const newRow = createTableRow(result.value);
            tableBody.appendChild(newRow);
        }
    } else {
        const account = row.cells[0].textContent;
        const result = await promptForInputs('Enter Addon and Effective From', getAddonAndEffectiveFromInputsHtml());
        if (result.isConfirmed) {
            const newRow = createTableRow({
                account: account,
                addon: result.value.addon,
                effectiveFrom: result.value.effectiveFrom,
                toEmail: row.cells[3].textContent,
                ccEmail: row.cells[4].textContent,
                bccEmail: row.cells[5].textContent
            },element);
            tableBody.appendChild(newRow);
        }
    }
    sortByHeader();
}

function promptForInputs(title, html) {
    return Swal.fire({
        title: title,
        html: html,
        focusConfirm: false,
        preConfirm: () => {
            const accountInput = document.getElementById('accountInput') ? document.getElementById('accountInput').value : '';
            const addonInput = document.getElementById('addonInput').value;
            const effectiveFromInput = document.getElementById('effectiveFromInput').value;
            const toEmailInput = document.getElementById('toEmailInput') ? document.getElementById('toEmailInput').value : '';
            const ccEmailInput = document.getElementById('ccEmailInput') ? document.getElementById('ccEmailInput').value : '';
            const bccEmailInput = document.getElementById('bccEmailInput') ? document.getElementById('bccEmailInput').value : '';
            return {
                account: accountInput,
                addon: addonInput,
                effectiveFrom: effectiveFromInput,
                toEmail: toEmailInput,
                ccEmail: ccEmailInput,
                bccEmail: bccEmailInput
            };
        }
    });
}

function getInitialInputsHtml() {
    return `
        <input id="accountInput" class="swal2-input" placeholder="Account">
        <input id="addonInput" class="swal2-input" placeholder="Addon">
        <input type="date" id="effectiveFromInput" class="swal2-input" placeholder="Effective From">
        <input id="toEmailInput" class="swal2-input" placeholder="To Email">
        <input id="ccEmailInput" class="swal2-input" placeholder="CC Email">
        <input id="bccEmailInput" class="swal2-input" placeholder="BCC Email">
    `;
}

function getAddonAndEffectiveFromInputsHtml() {
    return `
        <input id="addonInput" class="swal2-input" placeholder="Addon">
        <input id="effectiveFromInput" class="swal2-input" placeholder="Effective From" type="date">
    `;
}
function sortByHeader() {
    const table = document.getElementById('UniqueNetworkTable');
    const tableBody = table.querySelector('tbody');
    const rows = Array.from(tableBody.querySelectorAll('tr'));

    rows.sort((a, b) => {
        const dateA = new Date(a.cells[2].textContent);
        const dateB = new Date(b.cells[2].textContent);
        return dateB - dateA; // Sort in descending order
    });

    tableBody.innerHTML = '';
    rows.forEach(row => {
        tableBody.appendChild(row);
    });
}
function createElement(type, attributes = {}, textContent = '') {
    const element = document.createElement(type);
    for (const attr in attributes) {
        if (attributes.hasOwnProperty(attr)) {
            if (attr === 'class') {
                element.className = attributes[attr];
            } else {
                element[attr] = attributes[attr];
            }
        }
    }
    if (textContent) {
        element.textContent = textContent;
    }
    return element;
}

function createTableRow(data) {
    const newRow = document.createElement('tr');
    newRow.appendChild(createElement('td', {}, data.account));
    newRow.appendChild(createElement('td', {}, data.addon));
    newRow.appendChild(createElement('td', {}, data.effectiveFrom));
    newRow.appendChild(createElement('td', {}, data.toEmail));
    newRow.appendChild(createElement('td', {}, data.ccEmail));
    newRow.appendChild(createElement('td', {}, data.bccEmail));

    const cell = createElement('td');
    cell.style.border = 'none';
    const label = createElement('label', { class: 'label-new' }, 'NEW');
    cell.appendChild(label);
    newRow.appendChild(cell);
    var Network = document.getElementById('NetworkNameLabelUniqueNetwork').textContent.split(" ")[1];
    var ListToPushTo;
    ListToPushTo = GetNetworkListFromNetName(Network);
    if (!ListToPushTo.includes(data)) {
        ListToPushTo.push(data);
    }
    return newRow;
}
function GetNetworkListFromNetName(NetworkName){
    switch(NetworkName.toLowerCase()){
        case "keyfuels":
            return KeyFuelsAddonList;
        case "texaco":
            return TexacoAddonList;
        case "ukfuels":
            return UkFuelsAddonList;
        case "fuelgenie":
            return FuelGenieAddonList;
    }
}
const KeyFuelsAddonList = [];
const TexacoAddonList = [];
const UkFuelsAddonList = [];
const FuelGenieAddonList = [];



function closeUniqueNetworkOverlay(element) {
    LastOpenedNetworkCheck.checked = false;
    document.getElementById('UniqueNetworkOverlay').hidden = true;
    document.getElementById('LoadHistoricAddonsButton').style.backgroundColor = "";
    document.getElementById('LoadHistoricAddonsButton').textContent = "Load Historic Addons";


}
let LastOpenedNetworkCheck = null;

async function openUniqueNetworkOverlay(element) {
    if(document.getElementById('customerName').value === "") {
        element.checked = false;
        await Swal.fire({
            icon: 'warning',
            title: 'You have not typed in the customer name!',
            text: 'Please Type a customer first.',

        });
        return;
    }
    LastOpenedNetworkCheck = element;
    var data = CustomerSearchModelData;
    const Customernametouse = document.getElementById('customerName').value;
    var CustName = document.getElementById('CustNameLabelUniqueNetwork');
    CustName.textContent = "Customer Name:" + " " + Customernametouse;
    var NetName = document.getElementById('NetworkNameLabelUniqueNetwork')
    NetName.textContent = "Network:" + " " + element.value;
    if (document.getElementById('CustomerSearch').value !== "") {
        const NetworkData = data.networkData.find(xx => xx.networkName.toLowerCase() === element.value.toLowerCase());
        if (NetworkData) {
            LoadDataIntoNetworkOverlay(element,data);
        }
        else{
            console.log("No data in db found for network: " + element.value);
        }
    }
    document.getElementById('UniqueNetworkOverlay').hidden = false;
}


function LoadDataIntoNetworkOverlay(element,data){
    ClearUniqueNetworkOverlayTable();
    document.getElementById('LoadHistoricAddonsButton').hidden = false;
    
    LoadUniqueNetworkOverlayHeaders();
    LoadUniqueNetworkOverlayData(data, element);


}
function LoadUniqueNetworkOverlayData(data,element){
    const Table = document.getElementById('UniqueNetworkTable');
    const TableBody = Table.querySelector('tbody');
    const NetworkData = data.networkData.find(xx => xx.networkName.toLowerCase() === element.value.toLowerCase());
    NetworkData.allAddons.forEach((addon, index) => {
        const row = createElement('tr');
        const cell1 = createElement('td', {}, NetworkData.account);
        const cell2 = createElement('td', {}, addon.addon);
        const cell3 = createElement('td', {}, addon.effectiveDate);
        const cell4 = createElement('td', {}, NetworkData.email.to);
        const cell5 = createElement('td', {}, NetworkData.email.cc);
        const cell6 = createElement('td', {}, NetworkData.email.bcc);

        row.appendChild(cell1);
        row.appendChild(cell2);
        row.appendChild(cell3);
        row.appendChild(cell4);
        row.appendChild(cell5);
        row.appendChild(cell6);
        TableBody.appendChild(row);

        if (index !== 0) {
            row.hidden = true;
            row.classList.add("Historicaddons");
        }
    });

    var ListOfNewAddonsFromUser = GetNetworkListFromNetName(element.value);
    ListOfNewAddonsFromUser.forEach(addon => {
        const row = createTableRow(addon);
        TableBody.appendChild(row);
    });
}

function ClearUniqueNetworkOverlayTable() {
    const table = document.getElementById('UniqueNetworkTable');
    const TableheadRow = table.querySelector('thead');
    TableheadRow.innerHTML = '';
    const tableBody = table.querySelector('tbody');
    tableBody.innerHTML = '';
}
function LoadUniqueNetworkOverlayHeaders(){
    const Table = document.getElementById('UniqueNetworkTable');
    const TableHead = Table.querySelector('thead');
    const TableheadRow = createElement('tr');
    
    const headers = ['Account Number', 'Addon', 'Effective From', 'Email To', 'Email CC', 'Email BCC']; 
    
    headers.forEach(header => {
        const th = createElement('th', {}, header);
        TableheadRow.appendChild(th);
    });
    
    TableHead.appendChild(TableheadRow);
}

function LoadHistoricAddons(element){   
    if(element.textContent === "Load Historic Addons"){
        element.textContent = "Hide Historic Addons";
        element.style.backgroundColor = "indianred";

    }else{
        element.style.backgroundColor = "";
        element.textContent = "Load Historic Addons";
    }



    const HistoricAddons = document.querySelectorAll('.Historicaddons');
    HistoricAddons.forEach(element => {
        if (element.hidden) {
            element.hidden = false;
        } else {
            element.hidden = true;
        }
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

    const parentElement = document.getElementById("NewFixSelectParent");
    parentElement.appendChild(SelectEleForUsers);
    var overlayElement = document.getElementById("overlay");
    overlayElement.classList = "";
    overlayElement.classList.add("animate__zoomInRight");
    overlayElement.classList.add("animate__animated");
    overlayElement.classList.add("overlay");

    overlayElement.hidden = false;
   
}

function GetSelectOptionForUserForNewFixForm(){
    const Options = ['KeyFuels', 'FuelGenie', 'Texaco', 'UkFuels'];

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

let CustomerSearchModelData;

async function CustomerSearchInput(element){
if(element.value === ""){
    return;
}
    await $.ajax({
        url: '/CustomerDetails/SearchCustomer', 
        type: 'POST',
        data: JSON.stringify(element.value), 
        contentType: 'application/json;charset=utf-8',
        success: async function (response) {
            const stringifyData = JSON.stringify(response, null, 2);
            CustomerSearchModelData = JSON.parse(stringifyData);
            SortDataAndPopulateFromSearch(CustomerSearchModelData);
            Toast.fire({
                icon: 'success',
                title: 'Successfully Loaded Customer'
            });
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

function clearAllInputsOnCustomerDetailsPage() {
    // Clear all input fields in the AddOrEditSection
    const addOrEditSectionInputs = document.querySelectorAll('#AddOrEditSection input');
    addOrEditSectionInputs.forEach(input => {
        if (input.type === 'checkbox' || input.type === 'radio') {
            input.checked = false;
        } else {
            input.value = '';
        }
    });

    // Clear all select fields in the AddOrEditSection
    const addOrEditSectionSelects = document.querySelectorAll('#AddOrEditSection select');
    addOrEditSectionSelects.forEach(select => {
        select.selectedIndex = 0;
    });

    // Clear all input fields in the CustomerFixSection
    const customerFixSectionInputs = document.querySelectorAll('#CustomerFixSection input');
    customerFixSectionInputs.forEach(input => {
        if (input.type === 'checkbox' || input.type === 'radio') {
            input.checked = false;
        } else {
            input.value = '';
        }
    });

    // Clear all select fields in the CustomerFixSection
    const customerFixSectionSelects = document.querySelectorAll('#CustomerFixSection select');
    customerFixSectionSelects.forEach(select => {
        select.selectedIndex = 0;
    });
}
function SortDataAndPopulateFromSearch(data){
    clearAllInputsOnCustomerDetailsPage();
   
    FillCustomerName(data.customerName);
}

function FillCustomerName(customerName){
    const CustomerNameElement = document.getElementById("customerName");
    CustomerNameElement.value = customerName;
    CustomerNameElement.disabled = true;
}
function GetModelToSubmitToController(){ 
    var form = document.getElementById("AddOrEditCustomerForm");
    var formData = new FormData(form);
    var values = Object.fromEntries(formData.entries());

    console.log("Values:");
    console.log(JSON.stringify(values, null, 2));
    var AddEditCustomerFormData = {
        customerName: document.getElementById('customerName').value,
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
        addons:{
            keyFuels: KeyFuelsAddonList,
            texaco: TexacoAddonList,
            uKFuels: UkFuelsAddonList,
            fuelGenie: FuelGenieAddonList
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
 