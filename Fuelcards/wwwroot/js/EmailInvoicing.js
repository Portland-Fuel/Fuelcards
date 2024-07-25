async function StartSendingEmails(){
    var NetworkUserSelected = selectedNetwork;
    var CustomerList = getCustomerListFromNetwork(NetworkUserSelected);
    
    for (const Customer of CustomerList) {
        var CustomerName = Customer.name;
    
        await fillInputs(Customer);
        ClearPDfAndHtmlLoadErrors();
        if(await getCustomerInvoicePng(Customer)){
            await showErrorBox("Error loading PDF or HTML");
        };
        if(await getEmailBody(Customer)){
            await showErrorBox("Error loading PDF or HTML");
        };

        await SendEmailToCustomer(Customer);

    
        document.getElementById('DisplayEmailItemsContainer').hidden = false;
    
        await Toast.fire({
            icon: 'success',
            title: 'Email sent to ' + CustomerName
        })
    }
}
async function SendEmailToCustomer(Customer){
    var SendEmailInformation = {
        EmailDetails: {
            htmlbody: document.getElementById('EmailHtml').innerHTML,
            emailTo: document.getElementById("EmailTo").textContent,
            emailCc: document.getElementById("EmailCc").textContent,
            emailBcc: document.getElementById("EmailBcc").textContent,
            emailSubject: document.getElementById("EmailSubject").value,
        },
        CustomerInvoice: Customer,
    }




    try {
        let response = await $.ajax({
            url: '/Invoicing/SendEmail',
            type: 'POST',
            data: JSON.stringify(SendEmailInformation),
            contentType: 'application/json',
        });

        if (response && response.success) {
            console.log('Email sent successfully:', response);
        } else {
            throw new Error('Invalid response data');
        }
    } catch (error) {
        console.error('Error in SendEmailToCustomer:', error);
        await showErrorBox("Error sending email to " + Customer.name);
    }

}

async function fillInputs(Customer){
    document.getElementById('EmailCustomerName').value = Customer.name;
    document.getElementById('EmailAccountNumber').value = Customer.account;
}

async function getEmailBody(Customer) {
    let Errored = false;

    try {
        let response = await $.ajax({
            url: '/Invoicing/GetEmailBody',
            type: 'POST',
            data: JSON.stringify(Customer),
            contentType: 'application/json'
        });

        // Ensure response.html exists and is valid before assigning it
        if (response && response.html) {
            document.getElementById('EmailHtml').innerHTML = response.html;
            document.getElementById('EmailDetails').hidden = false;
        } else {
            throw new Error('Invalid response format');
        }

    } catch (error) {
        Errored = true;
        console.error('Error in getEmailBody:', error);
        var ff = document.getElementById('HtmlCon');
        if (ff) {
            var ErrorLabel = await CreateErrorLabelForPDFOrHtml();
            ff.appendChild(ErrorLabel);
        }
    }

    return Errored;
}

async function CreateErrorLabelForPDFOrHtml(){
    var ErrorLabel = document.createElement('label');
    ErrorLabel.textContent = "Error loading PDF or HTML";
    ErrorLabel.classList.add("ErrorLabelForPDFOrHtml");

    return ErrorLabel;
}

async function ClearPDfAndHtmlLoadErrors(){
    var elements = document.querySelectorAll(".ErrorLabelForPDFOrHtml"); // Add a dot before the class name
    for (let i = 0; i < elements.length; i++) {
        const element = elements[i];
        element.remove();
    }
}

async function getCustomerInvoicePng(Customer){
    let Errored = false;
    try {
        let response = await $.ajax({
            url: '/Invoicing/GetInvoicePng',
            type: 'POST',
            data: JSON.stringify(Customer),
            contentType: 'application/json',
        });

        if (response && typeof response === 'string') {
            var Img = document.createElement('img');
            Img.src = response;
            var container = document.getElementById('PDFImgContainer');
            if (container) {
                container.appendChild(Img);
            } else {
                console.error('PDFImgContainer element not found');
            }
        } else {
            console.error('Invalid response data for image source:', response);
        }
        return response;
    } catch (error) {
        Errored = true;
        console.error('Error in getCustomerInvoicePng:', error);
        var ee = document.getElementById('PFfCon');
        if (ee) {
            var ErrorLabel = await CreateErrorLabelForPDFOrHtml();
            ee.appendChild(ErrorLabel);
        }
    }
    return Errored;

}