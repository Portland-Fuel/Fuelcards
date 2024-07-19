function ShowSelectedNetworkCheckList(SelectElement){
    var selectedValue = SelectElement.value;
    var url = "/Invoicing/GetNetworkCheckList?selectedValue=" + selectedValue;
    $.get(url, function(data){
        $("#NetworkCheckList").html(data);
    });

}