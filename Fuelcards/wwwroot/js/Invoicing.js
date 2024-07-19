let GCB; //Gloabl check model

async function ShowSelectedNetworkCheckList(SelectElement){
    var selectedValue = SelectElement.value;
    SetModelAsGloabJson();
    ShowInvoicingCheckList(GCB, selectedValue);
    
}
function SetModelAsGloabJson(){
    const stringifyData = JSON.stringify(model, null, 2);
    GCB = JSON.parse(stringifyData);
    console.log("GCB" + GCB);
}

function ShowInvoicingCheckList(model, selectedNetwork) {
    var container = document.getElementById("CheckListContainer");
    var table = document.getElementById("CheckListTable");
    CreateCheckListHeaders(table, selectedNetwork);   
    CreateCheckListRows(model, selectedNetwork);

    document.getElementById("NetworkToInvoice").hidden = true;
    container.hidden = false;
}

function CreateCheckListHeaders(table, network) {
    var thead = table.getElementsByTagName("thead")[0];
    var th = document.createElement("th");
    th.innerHTML = "Check List"; // Create an empty cell
    thead.appendChild(th);
    th = document.createElement("th");
    th.innerHTML = network;
    thead.appendChild(th);
}
function GetImportsList(network){
    switch(network.toLowerCase()){
        case "keyfuels":
            return model.KeyfuelImports;
        case "ukfuels":
            return model.UkfuelImports;
        case "texaco":
            return model.TexacoImports;
    }
}

function CreateCheckListRows(model, network) {
    var VerticalRowsHeaders = ["Floating price data", "Invoice period", "Number of imports", "Errors in v+w", "Duplicates", "Product 18"];
    var table = document.getElementById("CheckListTable");
    var tbody = table.getElementsByTagName("tbody")[0];
    VerticalRowsHeaders.forEach(function(header) {
        var tr = document.createElement("tr");
        var th = document.createElement("th");
        th.innerHTML = header;
        tr.appendChild(th);
        tbody.appendChild(tr);
    });
    var tr = tbody.getElementsByTagName("tr");

    var td = document.createElement("td");
    td.innerHTML = model.BasePrice;
    tr[0].appendChild(td);

    td = document.createElement("td");
    td.innerHTML = model.invoiceDate;
    tr[1].appendChild(td);

    td = document.createElement("td");
    td.innerHTML = GetImportsList(network) || "Error";
    tr[2].appendChild(td);

    td = document.createElement("td");
    td.innerHTML = "Not in model";
    tr[3].appendChild(td);

    td = document.createElement("td");
    td.innerHTML = "Not in model";
    tr[4].appendChild(td);

    td = document.createElement("td");
    td.innerHTML = "Not in model";
    tr[5].appendChild(td);

    for (var i = 0; i < VerticalRowsHeaders.length; i++) {
        var td = document.createElement("td");
        td.innerHTML = "<input onclick='ShowApproveIfCheckboxesAllChecked()' type='checkbox' class='CheckListCheckBox' id='CheckList" + i + "' name='CheckList" + i + "'>";
        tr[i].appendChild(td);
    }
}

function ShowApproveIfCheckboxesAllChecked() {
    var checkboxes = document.getElementsByClassName("CheckListCheckBox");
    var allChecked = true;
    for (var i = 0; i < checkboxes.length; i++) {
        if (!checkboxes[i].checked) {
            allChecked = false;
            break;
        }
    }
    document.getElementById("ApproveButton").hidden = !allChecked;
}