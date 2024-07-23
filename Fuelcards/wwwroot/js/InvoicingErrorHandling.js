function HandleInvoicingError(xhr){
    alert(xhr.responseText);
}


async function ShowSiteErrorForm(FailedSites){
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