document.addEventListener("DOMContentLoaded", function() {
  
    var modelKey = 'invoicePreCheckModel';
    var storedModel = localStorage.getItem(modelKey);

    if (!storedModel) {
        // Model not in local storage, fetch from server
        $.ajax({
            url: '/Invoicing/GetInvoicePreCheckModel',
            dataType: 'json',
            success: function(data) {
                localStorage.setItem(modelKey, JSON.stringify(data));
                model = data; // No need to parse since data is already an object
                console.log('Model fetched from server and stored in local storage:', model);
                initializePage(model);
            },
            error: function(error) {
                console.error('Error fetching model from server:', error);
            }
        });
    } else {
        model = JSON.parse(storedModel);
        console.log('Model retrieved from local storage:', model);
        initializePage(model);
    }

    function initializePage(model) {
        document.getElementById("InitialPageLoad").hidden = true;
        document.getElementById("NetworkToInvoice").hidden = false;
        console.log(model);
    }
});
var model = null;
let GCB; // Global check model
let selectedNetwork;

let Invoicing = false;  
function goBackFromNetworkCheck(buttonEle){
    document.getElementById("NetworkToInvoice").hidden = false;
    document.getElementById("CheckListContainer").hidden = true;

}
async function showSelectedNetworkCheckList(selectElement) {
    const selectedValue = selectElement.value;
    selectedNetwork = selectedValue;
    setModelAsGlobalJson();
    showInvoicingCheckList(GCB, selectedValue);
}
function setModelAsGlobalJson() {
    const stringifyData = JSON.stringify(model, null, 2);
    GCB = JSON.parse(stringifyData);
    console.log("GCB", GCB);
}
function showInvoicingCheckList(model, selectedNetwork) {
    const container = document.getElementById("CheckListContainer");
    const table = document.getElementById("CheckListTable");

    createCheckListHeaders(table, selectedNetwork);
    createCheckListRows(model, selectedNetwork);

    document.getElementById("NetworkToInvoice").hidden = true;
    container.hidden = false;
}
function createCheckListHeaders(table, network) {
    const thead = table.querySelector("thead");
    thead.innerHTML = ""; // Clear existing headers

    const headers = ["Check List", network];
    headers.forEach(headerText => {
        const th = document.createElement("th");
        th.innerHTML = headerText;
        thead.appendChild(th);
    });
}
function getImportsList(network) {
    switch (network.toLowerCase()) {
        case "keyfuels":
            return model.keyfuelImports;
        case "ukfuels":
            return model.ukfuelImports;
        case "texaco":
            return model.texacoImports;
        default:
            return "Error";
    }
}
function getFailedSiteList(network) {
    switch (network.toLowerCase()) {
        case "keyfuels":
            return model.failedKeyfuelsSites;
        case "ukfuels":
            return model.failedUkfuelSites;
        case "texaco":
            return model.failedTexacoSites;
        default:
            return "Error";
    }

}
function getDuplicatesSiteList(network) {
    switch (network.toLowerCase()) {
        case "keyfuels":
            return model.keyfuelsDuplicates;
        case "ukfuels":
            return model.ukFuelDuplicates;
        case "texaco":
            return model.texacoDuplicates;
        default:
            return "Error";
    }

}
function getCustomerListFromNetwork(network) {
    switch (network.toLowerCase()) {
        case "keyfuels":
            return model.keyfuelsInvoiceList;
        case "ukfuels":
            return model.ukFuelInvoiceList;
        case "texaco":
            return model.texacoInvoiceList;
        default:
            return "Error";
    }

}
function createCheckListRows(model, network) {
    let verticalRowsHeaders = [];
    if(network.toLowerCase() === "ukfuels" || network.toLowerCase() === "ukfuel"){
        verticalRowsHeaders = ["Floating price data", "Invoice period", "Number of imports", "Errors in v+w", "Duplicates", "Product 18","Customer List"];
    }
    else{
        verticalRowsHeaders = ["Floating price data", "Invoice period", "Number of imports", "Errors in v+w", "Duplicates", "Customer List"];

    }
    const table = document.getElementById("CheckListTable");
    const tbody = table.querySelector("tbody");
    tbody.innerHTML = ""; // Clear existing rows

    verticalRowsHeaders.forEach(header => {
        const tr = document.createElement("tr");
        const th = document.createElement("th");
        th.innerHTML = header;
        tr.appendChild(th);
        tbody.appendChild(tr);
    });

    const rows = tbody.querySelectorAll("tr");

    rows[0].appendChild(createCell(model.basePrice));

    const invoiceDate = new Date(model.invoiceDate);
    const dateFrom = new Date(invoiceDate);
    dateFrom.setDate(invoiceDate.getDate() - 6);
    const formatDate = date => date.toISOString().split('T')[0].replace(/-/g, '/');
    rows[1].appendChild(createCell(`${formatDate(dateFrom)} - ${formatDate(invoiceDate)}`));

    rows[2].appendChild(createCell(getImportsList(network) || "Error"));

    const failedSiteList = getFailedSiteList(network);
    if (failedSiteList.length > 0 || failedSiteList ===  undefined) {
        const TDButton = createButton("FailedSitesButton","View Failed Sites");
        rows[3].appendChild(TDButton);
    } else {

        rows[3].appendChild(createCell("No failed sites"), "green");
        
    }


    const DuplicateSiteList = getDuplicatesSiteList(network);
    if (DuplicateSiteList.length > 0) {
        const TDButton = createButton("FailedSitesButton","View Failed Duplicates");
        rows[4].appendChild(TDButton);
    } else {

        rows[4].appendChild(createCell("No duplicates"), "green");
        
    }

    if(network.toLowerCase() === "ukfuels" || network.toLowerCase() === "ukfuel"){
        rows[5].appendChild(createCell("NEed to do this!"));
        rows[6].appendChild(createSelectCustomerList(network));

    }
    else{
        rows[5].appendChild(createSelectCustomerList(network));

    }



    rows.forEach((row, index) => {
        const checkboxTd = document.createElement("td");
        checkboxTd.innerHTML = `<input onclick='showApproveIfCheckboxesAllChecked()' type='checkbox' class='CheckListCheckBox' id='CheckList${index}' name='CheckList${index}'>`;
        row.appendChild(checkboxTd);
    });
}
function createCell(content,color) {
    if(color === undefined) color = "white";
    const td = document.createElement("td");
    td.style.color = color;
    td.innerHTML = content;
    return td;
}
function createButton(className,textContent) {
    const button = document.createElement("button");
    button.classList.add(className);
    button.textContent = textContent;
    const td = document.createElement("td");
    td.appendChild(button);
    return td;
}

function createSelectCustomerList(network) {
    const CustList = getCustomerListFromNetwork(network);
    const select = document.createElement("select");
    select.id = "CustomerList";
    select.name = "CustomerList";
    select.classList.add("CustomerListSelect");
    if(CustList.length == 0|| CustList == "Error"){
        const option = document.createElement("option");
        option.value = "No Customers";
        option.text = "No Customers";
        select.appendChild(option);
    }else{
        CustList.forEach(customer => {
            const option = document.createElement("option");
            const Val = customer.name + " - " + customer.addon;
            option.value = Val;
            option.text = Val;
            select.appendChild(option);
        });
    }
   

    const td = document.createElement("td");
    td.appendChild(select);

    return td;
}
function showApproveIfCheckboxesAllChecked() {
    const checkboxes = document.querySelectorAll(".CheckListCheckBox");
    const allChecked = Array.from(checkboxes).every(checkbox => checkbox.checked);
    document.getElementById("ApproveButton").hidden = !allChecked;
}




//InvoiceSectionFunctions

async function ApproveButtonClick() {
    document.getElementById("ApproveButton").hidden = true;
    document.getElementById("InvoiceSection").hidden = false;
    document.getElementById("CheckListContainer").hidden = true;
    document.getElementById("NetworkToInvoice").hidden = true;
    document.getElementById("InvoiceSection").hidden = false;
    document.getElementById("InvoiceSection").scrollIntoView();
}

async function StartInvoicing(btn) {
    document.getElementById("StopInvoicingBTN").hidden = false;
    document.getElementById("StartInvoicingBTN").hidden = true;
    document.getElementById("StartInvoicingAgainBTN").hidden = true;
    Invoicing = true;
    var CustList = getCustomerListFromNetwork(selectedNetwork);
    SetCustCountToBeInvoiced(CustList);
    for (const customer of CustList) {
        while (!Invoicing) {
            console.log("Invoicing Stopped");
            await new Promise(resolve => setTimeout(resolve, 500)); // Check every 500ms if Invoicing is true
        }
        console.log("Invoicing Resumed");
        await DisplayPageText(customer);

        await InvoiceCustomer(customer);

        MinusCustCountToBeinvoiced();
        await new Promise(resolve => setTimeout(resolve, 2000));

    }
    InvoicingCompletion();
    
    document.getElementById("StopInvoicingBTN").hidden = true;
    document.getElementById("StartInvoicingAgainBTN").hidden = true;
    document.getElementById("StartInvoicingBTN").hidden = false;
}
async function InvoiceCustomer(customer) {
    ChangeStatusText("Invoicing Customer");
    console.log(customer);
    try {
        let response = await $.ajax({
            url: '/Invoicing/InvoiceCustomer',
            type: 'POST',
            data: JSON.stringify(customer),
            contentType: 'application/json',
            success: function(data) {
                console.log(data);
            },
        })
    }
    catch(xhr){
        HandleInvoicingError(xhr);
        console.error('Error Invoicing Customer:', error);
    }
    
    ChangeStatusText("Customer Invoiced");
}

async function InvoicingCompletion() {
    ChangeStatusText("Invoicing Completed");
    document.getElementById("InvoiceSection").hidden = true;
    document.getElementById("AfterInvoicingCompletion").hidden = false;
    document.getElementById("AfterInvoicingCompletion").scrollIntoView();
    document.getElementById("InvoicingCompletionMessage").textContent = "Invoicing Completed";
}

async function MinusCustCountToBeinvoiced(){
    var CustCountH3 = document.getElementById("CountOfCustomersToBeInvoiced");
    var CustCount = CustCountH3.textContent;
    CustCount = CustCount.replace("Number of Customers to be Invoiced: ","");
    CustCount = parseInt(CustCount);
    CustCount = CustCount - 1;
    if(CustCount == 0){
        CustCountH3.hidden = true;
    }else{
        var CustText = "Number of Customers to be Invoiced: " + CustCount;
        CustCountH3.textContent = CustText;
    }

}
async function SetCustCountToBeInvoiced(CustList) {
    var CustCount = CustList.length;
    var CustCountH3 = document.getElementById("CountOfCustomersToBeInvoiced");
    CustCountH3.hidden = false;
    var CustText = "Number of Customers to be Invoiced: " + CustCount;
    CustCountH3.textContent = CustText;
    console.log(CustCount);
    
}
async function StartInvoicingAgain(btn) {
    Invoicing = true;
    document.getElementById("StopInvoicingBTN").hidden = false;
    document.getElementById("StartInvoicingAgainBTN").hidden = true;
}

async function DisplayPageText(customer) {
    var CustText = customer.name;
    var Addon = customer.addon;
    var custnameH1 = document.getElementById("CustNameInvoicing");
    custnameH1.textContent = CustText;
    ChangeStatusText("Loading....");
}

async function ChangeStatusText(StatusText) {
    var statusH3 = document.getElementById("StatusInvoicing");
    statusH3.textContent = StatusText;
}

async function StopInvoicingbtn() {
    Invoicing = false;
    ChangeStatusText("Invoicing Stopped");
    document.getElementById("StartInvoicingAgainBTN").hidden = false;
    document.getElementById("StopInvoicingBTN").hidden = true;
}
