let GCB; // Global check model
let selectedNetwork;
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
            return model.KeyfuelImports;
        case "ukfuels":
            return model.UkfuelImports;
        case "texaco":
            return model.TexacoImports;
        default:
            return "Error";
    }
}
function getFailedSiteList(network) {
    switch (network.toLowerCase()) {
        case "keyfuels":
            return model.FailedKeyfuelsSites;
        case "ukfuels":
            return model.FailedUkfuelSites;
        case "texaco":
            return model.FailedTexacoSites;
        default:
            return "Error";
    }

}
function getDuplicatesSiteList(network) {
    switch (network.toLowerCase()) {
        case "keyfuels":
            return model.KeyfuelsDuplicates;
        case "ukfuels":
            return model.UkFuelDuplicates;
        case "texaco":
            return model.TexacoDuplicates;
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
    const verticalRowsHeaders = ["Floating price data", "Invoice period", "Number of imports", "Errors in v+w", "Duplicates", "Product 18","Customer List"];
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

    rows[0].appendChild(createCell(model.BasePrice));

    const invoiceDate = new Date(model.invoiceDate);
    const dateFrom = new Date(invoiceDate);
    dateFrom.setDate(invoiceDate.getDate() - 6);
    const formatDate = date => date.toISOString().split('T')[0].replace(/-/g, '/');
    rows[1].appendChild(createCell(`${formatDate(dateFrom)} - ${formatDate(invoiceDate)}`));

    rows[2].appendChild(createCell(getImportsList(network) || "Error"));

    const failedSiteList = getFailedSiteList(network);
    if (failedSiteList.length > 0) {
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

    rows[5].appendChild(createCell("NEed to do this!"));

    rows[6].appendChild(createSelectCustomerList(network));

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
async function StartInvoicing(strtinvoicingbtn)
{
    strtinvoicingbtn.disabled = true;
    var CustList = getCustomerListFromNetwork(selectedNetwork);
    for (const customer of CustList) {
        await SortPageText(customer);
    }

    async function SortPageText(customer) {
        var StatusText = "Status:";
        var CustText = customer.name;
        var Addon = customer.addon;
        var custnameH1 = document.createElement("CustNameInvoicing");
        custnameH1.innerHTML = CustText;
        var statusH3 = document.createElement("StatusInvoicing");
    }
}

