const KeyFuelsFixs = []
const TexacoFixs = []
const UkFuelsFixs = []
const FuelGenieFixs = []

const NewKeyFuelsAddonList = [];
const NewTexacoAddonList = [];
const NewUkFuelsAddonList = [];
const NewFuelGenieAddonList = [];

const NewKeyFuelsAccountList = [];
const NewTexacoAccountList = [];
const NewUkFuelsAccountList = [];
const NewFuelGenieAccountList = [];




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
function SortDataAndPopulateFromSearch(data){
    clearAllInputsOnCustomerDetailsPage();
    FillCustomerName(data.customerName);
    //FillInvoiceOrderType(data.invoiceOrderType);
    //FillPaymentTerm(data.paymentTerm);
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
function FillPaymentTerm(paymentTerm) {
    const PaymentTermElement = document.getElementById("paymentTerm");
    PaymentTermElement.value = paymentTerm;
    for (let i = 0; i < PaymentTermElement.options.length; i++) {
        if (PaymentTermElement.options[i].value === paymentTerm) {
            PaymentTermElement.options[i].selected = true;
            break;
        }
    }
    PaymentTermElement.disabled = true;
}

function FillInvoiceOrderType(invoiceOrderType) {
    const InvoiceOrderTypeElement = document.getElementById("invoiceOrderType");
    InvoiceOrderTypeElement.value = invoiceOrderType;
    for (let i = 0; i < InvoiceOrderTypeElement.options.length; i++) {
        if (InvoiceOrderTypeElement.options[i].value === invoiceOrderType) {
            InvoiceOrderTypeElement.options[i].selected = true;
            break;
        }
    }
    InvoiceOrderTypeElement.disabled = true;
}
function FillCustomerName(customerName){
    const CustomerNameElement = document.getElementById("customerName");
    CustomerNameElement.value = customerName;
    CustomerNameElement.disabled = true;
}

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

    //sortByHeader();
    LastOpenedNetworkCheck = element;
    var data = CustomerSearchModelData;
    const Customernametouse = document.getElementById('customerName').value;
    var CustName = document.getElementById('CustNameLabelUniqueNetwork');
    CustName.textContent = "Customer Name:" + " " + Customernametouse;
    var NetName = document.getElementById('NetworkNameLabelUniqueNetwork')
    NetName.textContent = "Network:" + " " + element.textContent;

    var FixList =  GetFixListToAddTo(element.textContent);
    var count = FixList.length;
    
    var NewFixCount = document.getElementById('NewFixCount');
    NewFixCount.textContent = count + " Fixes";


    if (document.getElementById('CustomerSearch').value !== "") {
        const NetworkData = data.networkData.filter(xx => xx.networkName.toLowerCase() === element.textContent.toLowerCase());
        if (NetworkData.length > 0) {
            LoadDataIntoNetworkOverlay(element, NetworkData);
        } else {
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
function clearTable(table) {
    // Ensure the table is a valid element
    if (table && table.nodeName === 'TABLE') {
        // Remove all rows in the tbody
        const tableBody = table.querySelector('tbody');
        if (tableBody) {
            while (tableBody.firstChild) {
                tableBody.removeChild(tableBody.firstChild);
            }
        }

        // Remove all headers in the thead
        const tableHead = table.querySelector('thead');
        if (tableHead) {
            while (tableHead.firstChild) {
                tableHead.removeChild(tableHead.firstChild);
            }
        }
    } else {
        console.error('Invalid table element provided.');
    }
}

async function AddNewAddon(element) {
    const table = document.getElementById('HistoricAddons');
    const tablehead = table.querySelector('thead');
    const tableBody = table.querySelector('tbody');
    const HeadRow = tablehead.querySelectorAll('tr');
    const result = await promptForAddonInputs('Enter Addon and Effective From', getAddonAndEffectiveFromInputsHtml());
    if (result.isConfirmed) {
        const newRow = createAddonTableRow({
            addon: result.value.addon,
            effectiveFrom: result.value.effectiveFrom,
        }, element);

        if (HeadRow.length === 0) {
            LoadHistoricAddonsHeades(table);
        }

        tableBody.appendChild(newRow);
    }
}

function promptForInputs(title, html) {
    return Swal.fire({
        title: title,
        html: html,
        focusConfirm: false,
        preConfirm: () => {
            const accountInput = document.getElementById('accountInput') ? document.getElementById('accountInput').value : '';
            const toEmailInput = document.getElementById('toEmailInput') ? document.getElementById('toEmailInput').value : '';
            const ccEmailInput = document.getElementById('ccEmailInput') ? document.getElementById('ccEmailInput').value : '';
            const bccEmailInput = document.getElementById('bccEmailInput') ? document.getElementById('bccEmailInput').value : '';
            const paymentTerm = document.getElementById('paymentTermsInput') ? document.getElementById('paymentTermsInput').value : '';
            const invoiceFormatType = document.getElementById('invoiceFormatTypeInput') ? document.getElementById('invoiceFormatTypeInput').value : '';
            return {
                account: accountInput,
                toEmail: toEmailInput,
                ccEmail: ccEmailInput,
                bccEmail: bccEmailInput,
                paymentTerm: paymentTerm,
                invoiceFormatType: invoiceFormatType,
            };
        }
    });
}
function promptForAddonInputs(title, html) {
    return Swal.fire({
        title: title,
        html: html,
        focusConfirm: false,
        preConfirm: () => {
            const addonInput = document.getElementById('addonInput') ? document.getElementById('addonInput').value : '';
            const effectiveFromInput = document.getElementById('effectiveFromInput') ? document.getElementById('effectiveFromInput').value : '';
            return {
                addon: addonInput,
                effectiveFrom: effectiveFromInput
            };
        }
    });
}


function getInitialInputsHtml(data) {
    return `
        <input id="accountInput" class="swal2-input" value="${data.account}" placeholder="Account">
        <input id="toEmailInput" class="swal2-input" value="${data.toEmail}" placeholder="To Email">
        <input id="ccEmailInput" class="swal2-input" value="${data.ccEmail}" placeholder="CC Email">
        <input id="bccEmailInput" class="swal2-input" value="${data.bccEmail}" placeholder="BCC Email">
        <input id="paymentTermsInput" class="swal2-input" value="${data.paymentTerm}" placeholder="Payment Terms">
        <input id="invoiceFormatTypeInput" class="swal2-input" value="${data.invoiceFormatType}" placeholder="Invoice Format type">

    `;
}

async function AddNewAccount(btnelement) {
    var data = GetFirstAccountRowFromTable();
    const result = await promptForInputs('Enter Details', getInitialInputsHtml(data));
    if (result.isConfirmed) {
        const table = document.getElementById('UniqueNetworkTable');
        const newRow = createAccounTableRow(result.value);
        table.appendChild(newRow);
        var Network = document.getElementById('NetworkNameLabelUniqueNetwork').textContent.split(" ")[1];

        GetAccountListFromNetworkName(Network).push(result.value);
    }
}
function GetFirstAccountRowFromTable(){
    const table = document.getElementById('UniqueNetworkTable');
    const tableBody = table.querySelector('tbody');
    const rows = tableBody.querySelectorAll('tr');
    if (rows.length === 0) {
        return {
            account: '',
            toEmail: '',
            ccEmail: '',
            bccEmail: '',
            paymentTerm: '',
            invoiceFormatType: '',
        };
    }
    const lastRow = rows[rows.length - 1];
    const cells = lastRow.querySelectorAll('td');
    return {
        account: cells[0].textContent,
        toEmail: cells[1].textContent,
        ccEmail: cells[2].textContent,
        bccEmail: cells[3].textContent,
        paymentTerm: cells[4].textContent,
        invoiceFormatType: cells[5].textContent,
    };

}
function createAccounTableRow(data){
    const newRow = document.createElement('tr');
    newRow.appendChild(createElement('td', {}, data.account));
    newRow.appendChild(createElement('td', {}, data.toEmail));
    newRow.appendChild(createElement('td', {}, data.ccEmail));
    newRow.appendChild(createElement('td', {}, data.bccEmail));
    newRow.appendChild(createElement('td', {}, data.paymentTerm));
    newRow.appendChild(createElement('td', {}, data.invoiceFormatType));
    const label = createElement('label', { class: 'label-new' }, 'NEW');
    newRow.appendChild(label);

    return newRow;


}


function GetAccountListFromNetworkName(NetworkName){
    switch(NetworkName.toLowerCase()){
        case "keyfuels":
            return NewKeyFuelsAddonList;
        case "texaco":
            return NewTexacoAddonList;
        case "ukfuel":
            return NewUkFuelsAddonList;
        case "fuelgenie":
            return NewFuelGenieAddonList;
    }
}
function getAddonAndEffectiveFromInputsHtml() {
    return `
        <input id="addonInput" class="swal2-input" placeholder="Addon">
        <input id="effectiveFromInput" class="swal2-input" placeholder="Effective From" type="date">
    `;
}
function LoadHistoricAddons(element){   
    const HistoricAddonsTable = document.getElementById('HistoricAddons');

    if (element.textContent === "Load Historic Addons") {
        element.textContent = "Hide Historic Addons";
        element.style.backgroundColor = "indianred";
    } else {
        const HistoricAddonsTable = document.getElementById('HistoricAddons');
        clearTable(HistoricAddonsTable);
        element.style.backgroundColor = "";
        element.textContent = "Load Historic Addons";
        return;
    }
    var NetowrkCurrentlySelected = document.getElementById('NetworkNameLabelUniqueNetwork').textContent.split(" ")[1];
    var NetworkData = CustomerSearchModelData.networkData.find(xx => xx.networkName.toLowerCase() === NetowrkCurrentlySelected.toLowerCase());
    var HistoricAddons = NetworkData.allAddons;
    if(HistoricAddonsTable.querySelector('thead').querySelector('tr') === null){
        LoadHistoricAddonsHeades(HistoricAddonsTable);

    }


    const tbody = HistoricAddonsTable.querySelector('tbody');
    HistoricAddons.forEach((addon) => {
        const row = createElement('tr');
        const cell1 = createElement('td', {}, addon.addon);
        const cell2 = createElement('td', {}, addon.effectiveDate);
        row.appendChild(cell1);
        row.appendChild(cell2);
        tbody.appendChild(row);
    });

}






function LoadHistoricAddonsHeades(HistoricAddonsTable){
    const thead = HistoricAddonsTable.querySelector('thead');
    const rthead = createElement('tr');
    const addonHeader = createElement('th', {}, 'Addon');
    const effectiveDateHeader = createElement('th', {}, 'Effective Date');
    rthead.appendChild(addonHeader);
    rthead.appendChild(effectiveDateHeader);
    thead.appendChild(rthead);

}

function LoadUniqueNetworkOverlayData(data,element){
    const Table = document.getElementById('UniqueNetworkTable');
    const TableBody = Table.querySelector('tbody');

    data.forEach((networkData) => {
        const row = createElement('tr');
        const cell1 = createElement('td', {}, networkData.account);
        const cell4 = createElement('td', {}, networkData.email.to);
        const cell5 = createElement('td', {}, networkData.email.cc);
        const cell6 = createElement('td', {}, networkData.email.bcc);
        const cell7 = createElement('td', {}, networkData.paymentTerms);
        const cell8 = createElement('td', {}, networkData.invoiceFormatType);


        row.appendChild(cell1);
        row.appendChild(cell4);
        row.appendChild(cell5);
        row.appendChild(cell6);
        row.appendChild(cell7);
        row.appendChild(cell8);
        TableBody.appendChild(row);
    });



    // Modified $SELECTION_PLACEHOLDER$ code
    /*
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
   

    var ListOfNewAddonsFromUser = GetNetworkAddonListFromNetName(element.value);
    ListOfNewAddonsFromUser.forEach(addon => {
        const row = createAddonTableRow(addon);
        TableBody.appendChild(row);
    });
     */
}
function createAddonTableRow(data) {
    const newRow = document.createElement('tr');
    newRow.appendChild(createElement('td', {}, data.addon));
    newRow.appendChild(createElement('td', {}, data.effectiveFrom));

    const cell = createElement('td');
    cell.style.border = 'none';
    const label = createElement('label', { class: 'label-new' }, 'NEW');
    cell.appendChild(label);
    newRow.appendChild(cell);
    var Network = document.getElementById('NetworkNameLabelUniqueNetwork').textContent.split(" ")[1];
    var ListToPushTo;
    ListToPushTo = GetNetworkAddonListFromNetName(Network);
    if (!ListToPushTo.includes(data)) {
        ListToPushTo.push(data);
    }
    return newRow;
}
function GetNetworkAddonListFromNetName(NetworkName){
    switch(NetworkName.toLowerCase()){
        case "keyfuels":
            return NewKeyFuelsAddonList;
        case "texaco":
            return NewTexacoAddonList;
        case "ukfuel":
            return NewUkFuelsAddonList;
        case "fuelgenie":
            return NewFuelGenieAddonList;
    }
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
    if (textContent !== undefined && textContent !== null) {
        element.textContent = textContent;
    }
    return element;
}

function LoadUniqueNetworkOverlayHeaders(){
    const Table = document.getElementById('UniqueNetworkTable');
    const TableHead = Table.querySelector('thead');
    const TableheadRow = createElement('tr');
    
    const headers = ['Account Number', 'Email To', 'Email CC', 'Email BCC','Payment Terms','Invoice fomat type']; 
    
    headers.forEach(header => {
        const th = createElement('th', {}, header);
        TableheadRow.appendChild(th);
    });
    
    TableHead.appendChild(TableheadRow);
}
function ClearUniqueNetworkOverlayTable() {
    const table = document.getElementById('UniqueNetworkTable');
    const TableheadRow = table.querySelector('thead');
    TableheadRow.innerHTML = '';
    const tableBody = table.querySelector('tbody');
    tableBody.innerHTML = '';
}
function closeUniqueNetworkOverlay(element) {
    document.getElementById('UniqueNetworkOverlay').hidden = true;

    var UniqueNetworkTable =  document.getElementById('UniqueNetworkTable').hidden = true;
    clearTable(UniqueNetworkTable);
    var HistoricAddonsTable = document.getElementById('HistoricAddons');
    clearTable(HistoricAddonsTable);

}



function ShowNewFix(element){
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
function ChangeLabelToShowPlus1Fix(NetworkUserSelected){
    const labelElement = document.getElementById("NewFixCount");
    var SpanText = labelElement.innerText
    if(SpanText === "0 New Fixs"){
        labelElement.textContent = "1 Fix";
    }
    else{
        var CurrentFixNum = SpanText.split(" ")[0];
        var NewFixNum = parseInt(CurrentFixNum) + 1;
        labelElement.textContent = NewFixNum + " Fixes";
    }
}

function GetFixListToAddTo(NetworkUserSelected){
    switch(NetworkUserSelected.toLowerCase()){
        case "keyfuels":
            return KeyFuelsFixs;
        case "texaco":
            return TexacoFixs;
        case "ukfuels":
            return UkFuelsFixs;
        case "fuelgenie":
            return FuelGenieFixs;
        case "ukfuel":
            return UkFuelsFixs;
    }
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
 