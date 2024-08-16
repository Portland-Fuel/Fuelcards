document.addEventListener("DOMContentLoaded", function () {
    var DateSelect = document.getElementById("DateSelect");

    var availableDates = m.dates.map(function(date) {
        return date; // Ensure the date is in a format recognized by Flatpickr (e.g., "YYYY-MM-DD")
    });

    flatpickr(DateSelect, {
        dateFormat: "Y-m-d",
        enable: availableDates,
        onChange: async function(selectedDates, dateStr, instance) {
            console.log("Selected date:", dateStr);
            await GetSpecificInvoiceReport(dateStr);
        }
    });


    async function GetSpecificInvoiceReport(date) {
        try {
            let data = await $.ajax({
                url: '/InvoiceReport/getIT',
                data: JSON.stringify(date), // Ensure the date is sent as an object
                contentType: 'application/json',
                type: 'POST'
            });
            
            console.log('Invoice report:', data);
            await populateInvoiceRportTable(data);
             
        } catch (error) {
            console.error('Error fetching invoice report:', error);
            showErrorModal('Failed to fetch invoice report', error);
        }
    }
    
    

    function populateInvoiceRportTable(data) {
        var Table = document.getElementById("InvoiceReportTable");
        var thead = Table.querySelector("thead");
        var tbody = Table.querySelector("tbody");
        thead.innerHTML = "";
        tbody.innerHTML = "";
        var Headers = Object.keys(data[0]);
        var HeaderRow = document.createElement("tr");
        Headers.forEach(header => {
            var th = document.createElement("th");
            th.innerText = header;
            HeaderRow.appendChild(th);
        });
        thead.appendChild(HeaderRow);
        data.forEach(row => {
            var tr = document.createElement("tr");
            Headers.forEach(header => {
                var td = document.createElement("td");
                td.innerText = row[header];
                tr.appendChild(td);
            });
            tbody.appendChild(tr);
        });
    }

    function showErrorModal(errorMessage) {
        const modal = document.createElement('div');
        modal.classList.add('modal');
        modal.innerHTML = `
            <div class="modal-content">
                <span class="close">&times;</span>
                <p>${errorMessage}</p>
            </div>
        `;
        document.body.appendChild(modal);

        const closeButton = modal.querySelector('.close');
        closeButton.addEventListener('click', function () {
            modal.remove();
        });
    }
});

