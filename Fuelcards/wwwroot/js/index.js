function navigateToPage(actionUrl) {
    const form = document.createElement('form');
    form.method = 'POST';
    form.action = actionUrl;

    document.body.appendChild(form);
    form.submit();
}

// Event handlers for each image
function NavigateToCustomerPage() {
    navigateToPage('/Home/CustomerDetails');
}

function NavigateToEdiPage() {
    navigateToPage('/Home/Edi');
}

function NavigateToInvoicingPage() {
    navigateToPage('/Home/Invoicing');
}