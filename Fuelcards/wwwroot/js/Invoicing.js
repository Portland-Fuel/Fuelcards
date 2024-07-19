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

function ShowInvoicingCheckList(model, selectedNetwork){
    var Table = document.getElementById("CheckListTable");
    CreateChecListHeaders(Table,selectedNetwork);   
    CreateCheckListRows(model, selectedNetwork);


    document.getElementById("NetworkToInvoice").hidden = true;
    Table.hidden = false;
}   
function CreateChecListHeaders(Table, network){
    var thead = Table.getElementsByTagName("thead")[0];
    var th = document.createElement("th");
    th.innerHTML = "Check List"; // Create an empty cell
    thead.appendChild(th);
    th = document.createElement("th");
    th.innerHTML = network;
    thead.appendChild(th);
}

function CreateCheckListRows(model, network){
    var VerticalRowsHeaders = ["Floating price data","Invoice period","Number of imports","Errors in v+w", "Duplicates", "Product 18"];
    var Table = document.getElementById("CheckListTable");
    var tbody = Table.getElementsByTagName("tbody")[0];
    VerticalRowsHeaders.forEach(function(header){
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
    td.innerHTML = model.InvoicePeriod;
    tr[1].appendChild(td);

    td = document.createElement("td");
    td.innerHTML = "Not in model";
    tr[2].appendChild(td);

    td = document.createElement("td");
    td.innerHTML ="Not in model";
    tr[3].appendChild(td);

    td = document.createElement("td");
    td.innerHTML = "Not in model";
    tr[4].appendChild(td);

    td = document.createElement("td");
    td.innerHTML = "Not in model";
    tr[5].appendChild(td);

  

}
