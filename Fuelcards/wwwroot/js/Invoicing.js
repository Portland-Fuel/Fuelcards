let GCB; // Global check model

async function showSelectedNetworkCheckList(selectElement) {
    const selectedValue = selectElement.value;
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

function createCheckListRows(model, network) {
    const verticalRowsHeaders = ["Floating price data", "Invoice period", "Number of imports", "Errors in v+w", "Duplicates", "Product 18"];
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
        const button = document.createElement("button");
        button.textContent = "View Failed Sites";
        button.classList.add("FailedSitesButton");
        // Add event listener to handle button click and implement the functionality later
        rows[3].appendChild(button);
    } else {

        rows[3].appendChild(createCell("No failed sites"), "green");
        
    }

    for (let i = 4; i < verticalRowsHeaders.length; i++) {
        rows[i].appendChild(createCell("Not in model"));
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

function showApproveIfCheckboxesAllChecked() {
    const checkboxes = document.querySelectorAll(".CheckListCheckBox");
    const allChecked = Array.from(checkboxes).every(checkbox => checkbox.checked);
    document.getElementById("ApproveButton").hidden = !allChecked;
}
