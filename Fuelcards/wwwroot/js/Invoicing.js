window.onload = function () {
    Toast.fire({
        icon: 'success',
        title: 'Select a network to start invoicing'
    })
}

function GoTOEmail(){
    document.getElementById("EmailOutSection").hidden = false;
    document.getElementById("InvoiceSection").hidden = true;
    document.getElementById("CheckListContainer").hidden = true;
    document.getElementById("NetworkToInvoice").hidden = true;
}
function GoToInvoiceReport(){
    document.getElementById("EmailOutSection").hidden = true;
    document.getElementById("InvoiceReportSection").hidden = false;
    document.getElementById("CheckListContainer").hidden = true;
    document.getElementById("NetworkToInvoice").hidden = true;
}
document.addEventListener("DOMContentLoaded", function () {

    var modelKey = 'invoicePreCheckModel';
    var storedModel = localStorage.getItem(modelKey);

    if (!storedModel) {
        // Model not in local storage, fetch from server
        $.ajax({
            url: '/Invoicing/GetInvoicePreCheckModel',
            dataType: 'json',
            success: function (data) {
                localStorage.setItem(modelKey, JSON.stringify(data));
                model = data; // No need to parse since data is already an object
                console.log('Model fetched from server and stored in local storage:', model);
                initializePage(model);
            },
            error: async function (error) {
                document.getElementById("InitialPageLoad").hidden = true;
                document.getElementById("ModelError").hidden = false;

                var ParsedError = JSON.stringify(error.responseText);
                Swal.fire({
                    backdrop: false,
                    icon: "error",
                    title: "Sorry there has been a big error",
                    text: 'error loading model from server',
                    footer: '<a href="https://192.168.0.17:666/" target="_blank">Report it here!</a>'
                });
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
let isPaused = false;

let Invoicing = false;
function ContinueAfterInvoiceReport(){
    document.getElementById("InvoiceReportSection").hidden = true;
    document.getElementById("EmailOutSection").hidden = false;
}
async function LoadInvoiceReport(){
    startInvoicingLoader();
    var InvoiceReportTable = document.getElementById("InvoiceReportTable");
    var tbody = InvoiceReportTable.querySelector("tbody");
    var thead = InvoiceReportTable.querySelector("thead");
    tbody.innerHTML = "";
    thead.innerHTML = "";

    var InvoiceReport = await getInvoiceReport();
    if(InvoiceReport == null || InvoiceReport.length == 0){
        showErrorBox("No Invoice Report Data Found");
        stopInvoicingLoader();
        return;
    }

    var headers = Object.keys(InvoiceReport[0]);
    createInvoiceReportHeaders(thead, headers);
    createInvoiceReportRows(tbody, InvoiceReport, headers);

    stopInvoicingLoader();
}

function createInvoiceReportHeaders(thead, headers) {
    headers.forEach(headerText => {
        const th = document.createElement("th");
        th.innerHTML = headerText;
        thead.appendChild(th);
    });
}

function createInvoiceReportRows(tbody, InvoiceReport, headers) {
    InvoiceReport.forEach(item => {
        const tr = document.createElement("tr");
        headers.forEach(header => {
            const td = document.createElement("td");
            td.innerHTML = item[header];
            tr.appendChild(td);
        });
        tbody.appendChild(tr);
    });
}
    
async function getInvoiceReport(){
    try {
        let response = await $.ajax({
            url: '/Invoicing/GetInvoiceReport',
            type: 'GET',
            success: function (data) {
                return data;
            },
            error: function (xhr) {
                HandleInvoicingError(xhr);
            }
        });
        return response;
    }
    catch (xhr) {
        HandleInvoicingError(xhr);
    }
}
async function RefreshPage() {
    window.location.reload(true);
    localStorage.removeItem('invoicePreCheckModel');
}
function goBackFromNetworkCheck(buttonEle) {
    document.getElementById('ApproveButton').hidden = true;
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
async function getHeaderList(network) {
    switch (network.toLowerCase()) {
        case "keyfuels":
            return ["Floating price data", "Invoice period", "Number of imports", "Errors in v+w", "Duplicates", "Customer List"];
        case "ukfuels":
            return ["Floating price data", "Invoice period", "Number of imports", "Errors in v+w", "Duplicates", "Product 18", "Customer List"];
        case "texaco":
            return ["Floating price data", "Invoice period", "Number of imports", "Errors in v+w", "Duplicates", "Texaco Volumes", "Customer List"];
        case "fuelgenie":
            return ["Floating price data", "Invoice period", "Number of imports", "Errors in v+w", "Duplicates", "Customer List"];
        default:
            return "Error";
    }
}

async function createCheckListRows(model, network) {
    let verticalRowsHeaders = [];

    verticalRowsHeaders = await getHeaderList(network);
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
    if (failedSiteList.length > 0 || failedSiteList === undefined) {
        const TDButton = createButton("FailedSitesButton", "View Failed Sites");
        TDButton.onclick = function () {
            ShowSiteErrorForm(failedSiteList);
        };
        rows[3].appendChild(TDButton);
    } else {
        rows[3].appendChild(createCell("No failed sites"), "green");
    }

    const DuplicateSiteList = getDuplicatesSiteList(network);
    if (DuplicateSiteList.length > 0) {
        const TDButton = createButton("FailedSitesButton", "View Failed Duplicates");
        rows[4].appendChild(TDButton);
    } else {
        rows[4].appendChild(createCell("No duplicates"), "green");
    }
    if (network.toLowerCase() === "texaco") {
        rows[5].appendChild(createVolumesList(network));
        rows[6].appendChild(createSelectCustomerList(network));
    }
    else if (network.toLowerCase() === "ukfuels" || network.toLowerCase() === "ukfuel") {
        rows[5].appendChild(createCell("NEed to do this!"));
        rows[6].appendChild(createSelectCustomerList(network));
    }
    else if (network.toLowerCase() === "fuelgenie") {
        rows[5].appendChild(createSelectCustomerList(network));

    }
    else if (network.toLowerCase() === "keyfuels") {
        rows[5].appendChild(createSelectCustomerList(network));

    }
    else {
        rows[5].appendChild(createCell("Error"));
    }


    rows.forEach((row, index) => {
        const checkboxTd = document.createElement("td");
        checkboxTd.innerHTML = `<input onclick='showApproveIfCheckboxesAllChecked()' type='checkbox' class='CheckListCheckBox' id='CheckList${index}' name='CheckList${index}'>`;
        row.appendChild(checkboxTd);
    });
}




function createCell(content, color) {
    if (color === undefined) color = "white";
    const td = document.createElement("td");
    td.style.color = color;
    td.innerHTML = content;
    return td;
}
function createButton(className, textContent) {
    const button = document.createElement("button");
    button.classList.add(className);
    button.textContent = textContent;
    const td = document.createElement("td");
    td.appendChild(button);
    return td;
}
function createVolumesList(network) {
    // Assume model.texacoVolume is an object with properties as volume names and their values as the actual volume
    const Volumes = model.texacoVolume || {};

    const select = document.createElement("select");
    select.id = "TexacoVolumes";
    select.name = "TexacoVolumes";
    select.classList.add("CustomerListSelect");

    // Check if Volumes is empty (i.e., no properties) or is "Error"
    if (Object.keys(Volumes).length === 0 || Volumes === "Error") {
        const option = document.createElement("option");
        option.value = "No Volumes";
        option.text = "No Volumes";
        select.appendChild(option);
    } else {
        // Iterate over the object's properties
        for (const [volumeName, volumeValue] of Object.entries(Volumes)) {
            const option = document.createElement("option");
            option.value = volumeValue;
            option.text = `${volumeName} - ${volumeValue}`;
            select.appendChild(option);
        }
    }

    const td = document.createElement("td");
    td.appendChild(select);

    return td;
}
function createSelectCustomerList(network) {
    const CustList = getCustomerListFromNetwork(network);
    const select = document.createElement("select");
    select.id = "CustomerList";
    select.name = "CustomerList";
    select.classList.add("CustomerListSelect");
    if (CustList.length == 0 || CustList == "Error") {
        const option = document.createElement("option");
        option.value = "No Customers";
        option.text = "No Customers";
        select.appendChild(option);
    } else {
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


async function selectAllCheckBoxes() {
    var Checkboxs = document.querySelectorAll(".CheckListCheckBox");
    Checkboxs.forEach(checkbox => {
        checkbox.checked = true;
    });
    showApproveIfCheckboxesAllChecked();



}

//InvoiceSectionFunctions

async function ApproveButtonClick() {
    document.getElementById("ApproveButton").hidden = true;
    document.getElementById("InvoiceSection").hidden = false;
    document.getElementById("CheckListContainer").hidden = true;
    document.getElementById("NetworkToInvoice").hidden = true;
    document.getElementById("InvoiceSection").hidden = false;
    var CustList = getCustomerListFromNetwork(selectedNetwork);
    SetCustCountToBeInvoiced(CustList);
}
async function startInvoicingLoader(Pause = false) {
    document.getElementById("InvoiceingLoader").hidden = false;
    if(Pause){
        document.getElementById("PauseBTN").hidden = false;
    }
}
async function stopInvoicingLoader() {
    document.getElementById("InvoiceingLoader").hidden = true;
  
}
async function StartInvoicing(btn) {
    startInvoicingLoader(true);
    document.getElementById("PauseInvoicingBTN").hidden = false;
    document.getElementById("StartInvoicingBTN").hidden = true;
    document.getElementById("ResumeInvoicingBTN").hidden = true;
    Invoicing = true;
    var CustList = await getCustomerListFromNetwork(selectedNetwork);

    await InvoiceCustomers(CustList);
}
let currentCustomerIndex = 0;
async function InvoiceCustomers(CustList, startIndex = 0) {
    for (; currentCustomerIndex < CustList.length;) {
        const customer = CustList[currentCustomerIndex];
        console.log("Current Customer Index: ", currentCustomerIndex);
        console.log("Customer: ", customer);
        clearTransactionTable();
        await DisplayIntialPageText(customer);
        console.log("UpdatedContent on page for customer: ", customer);
        var updatedTransactionsWithData = await StartLoopThroughTransactions(customer);
        console.log("Updated Transactions with data: ", updatedTransactionsWithData);
        customer.customerTransactions = updatedTransactionsWithData;
        currentCustomerIndex++;
        console.log("Invoicing Customer...")
        var result = await InvoiceCustomer(customer);

        
        if (!result) {
            currentCustomerIndex = currentCustomerIndex - 1;
            return;
        }
        else {
            Toast.fire({
                icon: 'success',
                title: customer.name + ": Invoiced Successfully"
            });
            await MinusCustCountToBeinvoiced();
            continue
        }

    }

    await ConfirmInvoicing();
    await InvoicingCompletion();

    document.getElementById("PauseInvoicingBTN").hidden = true;
    document.getElementById("ResumeInvoicingBTN").hidden = true;
    document.getElementById("StartInvoicingBTN").hidden = false;
}
async function ResumeInvoicing(btn) {
    Invoicing = true;
    startInvoicingLoader();
    document.getElementById("PauseInvoicingBTN").hidden = false;
    document.getElementById("ResumeInvoicingBTN").hidden = true;

    var CustList = await getCustomerListFromNetwork(selectedNetwork);

    await InvoiceCustomers(CustList, currentCustomerIndex - 1);
}
async function InvoiceCustomer(Customer) {
    try {
        let response = await new Promise((resolve, reject) => {
            $.ajax({
                url: '/Invoicing/CompleteInvoicing',
                type: 'POST',
                data: JSON.stringify(Customer),
                contentType: 'application/json',
                success: function (data) {
                   
                    resolve(data);
                },
                error: function (xhr) {
                    HandleInvoicingError(xhr);
                    reject(new Error('Error invoicing customer.'));
                }
            });
        });

        return true;
    } catch (error) {
        console.error("Error Invoicing Customer:", error);
        return false;
    }
}

async function ConfirmInvoicing(network) {
    
    try {
        const result = await Swal.fire({
            title: 'Confirm Invoicing',
            text: 'Do you want to confirm invoicing for this network?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        });

        if (result.isConfirmed) {
            let response = await $.ajax({
                url: '/Invoicing/ConfirmInvoicing',
                data: JSON.stringify(selectedNetwork),
                contentType: 'application/json',
                type: 'POST',
                success: function (data) {
                    Toast.fire({
                        icon: 'success',
                        title: 'Invoicing Complete'
                    })
                },
                error: function (xhr) {
                    HandleConfirmInvoicingError(xhr.responseText);
                }
            });
        }
    }
    catch {
        console.error("Error Confirming Invoicing");
    }
}
async function PauseLoader(btn) {
    stopInvoicingLoader();
    const elements = document.querySelectorAll('.s, .bigcon, .big');
    const button = document.querySelector('.loader--control-button');

    if (isPaused) {
        elements.forEach(el => el.style.animationPlayState = 'running');
        button.textContent = 'Pause';
    } else {
        elements.forEach(el => el.style.animationPlayState = 'paused');
        button.textContent = 'Play';
    }

    isPaused = !isPaused;
}

async function StartLoopThroughTransactions(Customer) {
    const updatedTransactions = [];

    for (const transaction of Customer.customerTransactions) {
        await DisplayTransactionOnPage(transaction);

        const DataFromTReturn = await SendTransactionToControllerToBeProcessed(transaction, Customer);
        const Data = JSON.parse(DataFromTReturn);

        transaction.siteName = Data.siteName;
        transaction.invoicePrice = Data.invoicePrice;
        transaction.unitPrice = Data.unitPrice;
        transaction.product = Data.product;

        await PopulateAddtionalTransactionData(transaction);

        // Add the updated transaction to the array
        updatedTransactions.push(transaction);
    }

    // Return the array of updated transactions
    return updatedTransactions;
}
async function clearTransactionTable() {
    const table = document.getElementById("InvoiceSectionTransactionTable");
    const tbody = table.querySelector("tbody");
    tbody.innerHTML = "";
}

async function PopulateAddtionalTransactionData(Transaction) {
    const TransactionTable = document.getElementById("InvoiceSectionTransactionTable");
    const tbody = TransactionTable.querySelector("tbody");
    const rows = tbody.querySelectorAll("tr");

    var RecentRow = rows[rows.length - 1];
    if (RecentRow == undefined) {
        showErrorBox("Error: Transaction not added to table");
    }
    else {
        const cells = RecentRow.querySelectorAll("td");
        cells[cells.length - 4].innerHTML = Transaction.siteName;
        cells[cells.length - 3].innerHTML = Transaction.invoicePrice;
        cells[cells.length - 2].innerHTML = Transaction.unitPrice;
        cells[cells.length - 1].innerHTML = Transaction.product;
        console.log("Transaction Added to Table");

    }
}
async function DisplayTransactionOnPage(Transaction) {
    const {
        transactionNumber,
        transactionDate,
        transactionTime,
        siteCode,
        cardNumber,
        productCode,
        quantity,
        cost,
        siteName,
        invoicePrice,
        unitPrice,
        product
    } = Transaction;

    const TransactionTable = document.getElementById("InvoiceSectionTransactionTable");
    const tbody = TransactionTable.querySelector("tbody");

    // Create a document fragment to hold the new row
    const fragment = document.createDocumentFragment();

    const row = document.createElement("tr");

    const createCell = (content) => {
        const td = document.createElement("td");
        td.textContent = content;
        return td;
    };

    row.appendChild(createCell(transactionNumber));
    row.appendChild(createCell(transactionDate));
    row.appendChild(createCell(transactionTime));
    row.appendChild(createCell(siteCode));
    row.appendChild(createCell(cardNumber));
    row.appendChild(createCell(productCode));
    row.appendChild(createCell(quantity));
    row.appendChild(createCell(cost));
    row.appendChild(createCell(siteName));
    row.appendChild(createCell(invoicePrice));
    row.appendChild(createCell(unitPrice));
    row.appendChild(createCell(product));

    fragment.appendChild(row);
    tbody.appendChild(fragment);
    TransactionTable.hidden = false;
}


async function SendTransactionToControllerToBeProcessed(Transaction, customer) {
    var TransactionDataFromView = {
        invoiceDate: customer.invoiceDate,
        name: customer.name,
        addon: customer.addon,
        account: customer.account,
        customerType: customer.customerType,
        IfuelsCustomer: customer.ifuelsCustomer,
        fixedInformation: customer.fixedInformation,
        transaction: Transaction,
    }



    try {
        let response = await $.ajax({
            url: '/Invoicing/ProcessTransactionFromPage',
            type: 'POST',
            data: JSON.stringify(TransactionDataFromView),
            contentType: 'application/json',
            success: function (data) {
                var stringifyed = JSON.stringify(data)
                var ParsedData = JSON.parse(stringifyed);
                var stringifyedParse = JSON.stringify(ParsedData);
                console.log("ParsedData" + stringifyedParse);
            },
        })

        return JSON.stringify(response);
    }
    catch (xhr) {
        HandleInvoicingError(xhr);
    }

}

async function InvoicingCompletion() {
    document.getElementById("InvoiceSection").hidden = true;
    document.getElementById('InvoiceReportSection').hidden = false;
    stopInvoicingLoader();
}

async function MinusCustCountToBeinvoiced() {
    var CustCountH3 = document.getElementById("CountOfCustomersToBeInvoiced");
    var CustCount = CustCountH3.textContent;
    CustCount = CustCount.replace("Number of Customers to be Invoiced: ", "");
    CustCount = parseInt(CustCount);
    CustCount = CustCount - 1;
    if (CustCount == 0) {
        CustCountH3.hidden = false;
    } else {
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

async function DisplayIntialPageText(customer) {
    var CustText = customer.name;
    var Addon = customer.addon;
    var custnameH1 = document.getElementById("CustNameInvoicing");
    custnameH1.textContent = CustText;
}
async function StopInvoicingbtn() {
    Invoicing = false;
    stopInvoicingLoader();
    document.getElementById("ResumeInvoicingBTN").hidden = false;
    document.getElementById("PauseInvoicingBTN").hidden = true;
}