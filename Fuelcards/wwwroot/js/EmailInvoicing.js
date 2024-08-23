 async function StartEmailing(Btn) {
    try {
        Btn.hidden = true;
        startInvoicingLoader();
    
        var Customers = getCustomerListFromNetwork(selectedNetwork);    
        var FirstCustomer = Customers[0];
        await loadDetailsForNextCustomer(FirstCustomer);
        var Count = document.getElementById('CountOfCustomersToBeEmailed');
        Count.textContent = Customers.length;
        stopInvoicingLoader();
    }
    catch(err){
        showErrorBox(err);
        Btn.hidden = false;
    }
    finally{
        stopInvoicingLoader();
    }
   

}


let loopedCustomerCount = 0;
async function loadDetailsForNextCustomer(customer) {
    await fillInputs(customer);
    document.getElementById('DisplayEmailItemsContainer').hidden = false;
    await getEmailBody(customer);
    await getCustomerInvoicePng(customer);
}
async function ResumeEmails(btn) {
    var element = document.getElementById('ResumeEmailing');
    element.hidden = true;
    var btn = document.getElementsByName("ResumeEmails");
    isPaused = false;
    SendAllEmails(btn,loopedCustomerCount - 1);
}
async function SendAllEmails(Btn,startIndex = 0) {
    Btn.hidden = true;
    startInvoicingLoader(true);
    var Customers = getCustomerListFromNetwork(selectedNetwork);

    for (let i = startIndex; i < Customers.length; i++) {
        if (isPaused) {
            stopInvoicingLoader();

            var element = document.getElementById('ResumeEmailing');
            element.hidden = false;
            Toast.fire({
                icon: 'info',
                title: 'Emailing paused'
            })
            return;
        }

        const Customer = Customers[i];
        await loadDetailsForNextCustomer(Customer, Customers.length);
        var SendEmailResult = await SendEmailToCustomer(Customer);
        
        if (!SendEmailResult) {
            await showErrorBox("Error sending email to " + Customer.name);
        }

        var Count = document.getElementById('CountOfCustomersToBeEmailed');
        if(Count.textContent === "0"){
            Count.textContent = Customers.length;
        }
        else{
            Count.textContent = parseInt(Count.textContent) - 1;
        }
        loopedCustomerCount++;

        if (loopedCustomerCount === Customers.length) {
            document.getElementById('DisplayEmailItemsContainer').hidden = true;
        }
    }

    stopInvoicingLoader();

    await EmailingCompletion()
}

async function EmailingCompletion(){
    var element = document.getElementById('ResumeEmailing');
    element.hidden = true;
    Toast.fire({
        icon: 'success',
        title: 'Emailing completed'
    })
    var EmailOutSeciton = document.getElementById('EmailOutSection');
    EmailOutSeciton.hidden = true;
    document.getElementById('NetworkToInvoice').hidden = false;
    var options = document.getElementById('NetworkSelect').options;
    for (let i = 0; i < options.length; i++) {
        const option = options[i];
        if (option.value === selectedNetwork) {
            option.disabled = true;
            option.selected = false;
            break;
        }
    }

}


async function SendEmailToCustomer(Customer){
    var SendEmailInformation = {
        EmailDetails: {
            htmlbody: document.getElementById('EmailHtml').innerHTML,
            emailTo: document.getElementById("EmailTo").textContent,
            emailCc: document.getElementById("EmailCC").textContent,
            emailBcc: document.getElementById("EmailBCC").textContent,
            emailSubject: document.getElementById("EmailSubject").textContent,
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
        var res1 = JSON.stringify(response);
        var res2 = JSON.parse(res1);
        if (res2 && res2.success) {
            Toast.fire({
                icon: 'success',
                title: 'Email sent successfully'
            });
            return true;
        } else {
            throw new Error('Invalid response data');
        }
    } catch (error) {
        console.error('Error in SendEmailToCustomer:', error);
        return false;
    }

}

async function fillInputs(Customer){
    document.getElementById('EmailCustomerName').textContent = Customer.name;
    document.getElementById('EmailAccountNumber').textContent = Customer.account;
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

async function getCustomerInvoicePng(Customer) {
    let Errored = false;
    try {
        let response = await $.ajax({
            url: '/Invoicing/GetInvoicePng',
            type: 'POST',
            data: JSON.stringify(Customer),
            contentType: 'application/json',
            statusCode: {
                400: async function() {
                    var errorMessage = 'Invalid input data.';
                    console.error(errorMessage);
                    await showErrorBox(errorMessage);
                },
                404: async function() {
                    var errorMessage = 'Invoice not found.';
                    console.error(errorMessage);
                    await showErrorBox(errorMessage);
                },
                500:  async function() {
                    var errorMessage = 'Server error occurred.';
                    console.error(errorMessage);
                    await showErrorBox(errorMessage);
                }
            }
        });

        if (response && typeof response === 'string' && response.startsWith("data:image/png;base64,")) {
            var Img = document.createElement('img');
            Img.src = response;
            Img.alt = 'PDF';

            var container = document.getElementById('PDFImgContainer');
            if (container) {
                var elements = container.querySelectorAll("*");
                for (let i = 0; i < elements.length; i++) {
                    const element = elements[i];
                    element.remove();
                }
                container.appendChild(Img);
            } else {
                const errorMessage = 'PDFImgContainer element not found';
                console.error(errorMessage);
                await showErrorBox(errorMessage);
            }
        } else {
            const errorMessage = 'Invalid response data for image source: ' + response;
            console.error(errorMessage);
            await showErrorBox(errorMessage);
        }
        return response;
    } catch (error) {
        Errored = true;
        const errorMessage = 'Error in getCustomerInvoicePng: ' + error;
        console.error(errorMessage);
        await showErrorBox(errorMessage);
        var container = document.getElementById('PDFImgContainer');
        if (container) {
            var elements = container.querySelectorAll("*");
            for (let i = 0; i < elements.length; i++) {
                const element = elements[i];
                element.remove();
            }
        }
        var ee = document.getElementById('PFfCon');
        if (ee) {
            var ErrorLabel = await CreateErrorLabelForPDFOrHtml();
            if (ErrorLabel) {
                ee.appendChild(ErrorLabel);
            } else {
                const errorMessage = 'ErrorLabel creation failed.';
                console.error(errorMessage);
                await showErrorBox(errorMessage);
            }
        } else {
            const errorMessage = 'PFfCon element not found';
            console.error(errorMessage);
            await showErrorBox(errorMessage);
        }
    }
    return Errored;
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
        var res1 = JSON.stringify(response);
        var res2 = JSON.parse(res1);
        // Ensure response.html exists and is valid before assigning it
        if (res2 && res2.html) {
            document.getElementById('EmailHtml').innerHTML = response.html;
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