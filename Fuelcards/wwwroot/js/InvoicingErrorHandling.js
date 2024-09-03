function HandleInvoicingError(xhr) {
    document.getElementById("ResumeInvoicingBTN").hidden = false;
    Invoicing = false;
    stopInvoicingLoader();
    showErrorBox(xhr.statusText + ': ' + xhr.responseText);
}
async function HandleInvoicingModelLoadErrors(data) {
    document.getElementById("InitialPageLoad").hidden = true;
    document.getElementById("ModelError").hidden = false;
    var response = JSON.parse(data);
    HideBackButton();
    var ParsedError = JSON.stringify(response.message);
    document.getElementById("ModelErrorMessage").textContent = "Error: " + ParsedError;
    switch (response.exceptionType) {
        case "SiteErrorException":
            console.log("Inventory Item Code Not Found in Database");
            showErrorBox(response.description);
        default:
            console.error("Unhandled error: ", response.message);
            showErrorBox(response.message);
            break;
    }

}

function HandleConfirmInvoicingError(responseText) {
    var response = JSON.parse(responseText);
    switch (response.exceptionType) {
        case "InventoryItemCodeNotInDb":
            console.log("Inventory Item Code Not Found in Database");
            ShowItemCodeNotFoundinDatabaseForm(response.description);
            break;
        // Handle other cases if needed
        default:
            console.error("Unhandled error: ", response.message);
    }
}
async function ShowItemCodeNotFoundinDatabaseForm(description) {
    const { value: formValues } = await Swal.fire({
        title: 'Item Code Not Found',
        html:
            '<label for="description">Description</label>' +
            `<input id="description" class="swal2-input" value="${description}" readonly>` +
            '<label for="itemCode">Item Code</label>' +
            '<input id="itemCode" class="swal2-input" placeholder="Item Code">',
        focusConfirm: false,
        showCancelButton: true,
        confirmButtonText: 'Submit',
        cancelButtonText: 'Cancel',
        preConfirm: () => {
            return {
                description: document.getElementById('description').value,
                itemCode: document.getElementById('itemCode').value
            };
        }
    });


    if (formValues) {
        const { description, itemCode } = formValues;

        // Do something with the input values
        console.log('Description:', description);
        console.log('Item Code:', itemCode);


        var ItemCodeAndProductData = {
            Description: description,
            ItemCode: itemCode
        };
        try {
            let response = await $.ajax({
                url: '/Invoicing/UploadNewItemInventoryCode',
                type: 'POST',
                data: JSON.stringify(ItemCodeAndProductData),
                contentType: 'application/json;charset=utf-8'
            });
            Toast.fire({
                icon: 'success',
                title: 'Item code uploaded successfully'
            });

        } catch (xhr) {
            console.error('XHR Response:', xhr.responseText || 'No response from server');
            console.error('Error:', xhr.statusText || 'No error message');
            Toast.fire({
                icon: 'error',
                title: 'Error uploading item code',
                text: xhr.statusText + ': ' + xhr.responseText,
                footer: "Contact IT if you have issues <a href='https://192.168.0.17:666/Home/ReportIssue'>Report Issue</a>"
            });
            return;
        }
    }

    await ConfirmInvoicing();
}

async function ShowSiteErrorForm(FailedSites) {
    for (let i = 0; i < FailedSites.length; i++) {
        const failedSite = FailedSites[i];
        const { value: formValues } = await Swal.fire({
            title: 'Site Error Form',
            html:
                `<input id="siteNum" class="swal2-input" placeholder="Site Number" value="${failedSite}" readonly>` +
                '<input id="siteName" class="swal2-input" placeholder="Site Name">' +
                '<input id="siteBand" class="swal2-input" placeholder="Site Band">',
            focusConfirm: false,
            showCancelButton: true,
            confirmButtonText: 'Submit',
            cancelButtonText: 'Cancel',
            preConfirm: () => {
                return {
                    siteNum: document.getElementById('siteNum').value,
                    siteName: document.getElementById('siteName').value,
                    siteBand: document.getElementById('siteBand').value
                };
            }
        });

        if (formValues) {
            const { siteNum, siteName, siteBand } = formValues;

            // Do something with the input values
            console.log('Site Number:', siteNum);
            console.log('Site Name:', siteName);
            console.log('Site Band:', siteBand);


            var SiteData = {
                SiteNumber: siteNum,
                SiteName: siteName,
                SiteBand: siteBand
            };

            try {
                let response = await $.ajax({
                    url: '/Invoicing/HandleFailedSite',
                    type: 'POST',
                    data: JSON.stringify(SiteData),
                    contentType: 'application/json;charset=utf-8'
                });
                Toast.fire({
                    icon: 'success',
                    title: 'Site submitted successfully'
                });
            } catch (xhr) {
                console.error('XHR Response:', xhr.responseText || 'No response from server');
                console.error('Error:', xhr.statusText || 'No error message');
                Toast.fire({
                    icon: 'error',
                    title: 'Error submitting site information',
                    text: xhr.statusText + ': ' + xhr.responseText,
                    footer: "Contact IT if you have issues <a href='https://192.168.0.17:666/Home/ReportIssue'>Report Issue</a>"

                });
            }
            document.getElementById('siteName').value = '';
            document.getElementById('siteBand').value = '';
            continue;
        }
    }
}

async function showErrorBox(Msg) {
    await Swal.fire({
        icon: 'error',
        title: 'Error',
        text: Msg,
        footer: "Contact IT if you have issues <a href='https://192.168.0.142:666/Home/ReportIssue'>Report Issue</a>"
    })
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