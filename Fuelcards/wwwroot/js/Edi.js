function showSection(sectionId) {
    const sections = document.querySelectorAll('.section');
    const currentSection = document.querySelector('.section.active');

    // Remove active class from current section and add slide-out animation
    currentSection.classList.add('slide-out');
    currentSection.classList.remove('active');

    // Wait for the slide-out animation to finish before showing the new section
    setTimeout(() => {
        sections.forEach(section => {
            section.classList.remove('active', 'slide-out', 'slide-in');
        });

        // Show the clicked section with slide-in animation
        const newSection = document.getElementById(sectionId);
        newSection.classList.add('active', 'slide-in');
    }, 300); // Duration matches the CSS animation time
}
document.addEventListener('DOMContentLoaded', function () {
    ShowBackButton();
    const networkCells = document.querySelectorAll('td[data-network]');

    networkCells.forEach(cell => {
        const networkId = parseInt(cell.getAttribute('data-network'), 10);
        let networkName = '';

        switch (networkId) {
            case 0:
                networkName = 'Keyfuels';
                break;
            case 1:
                networkName = 'UK Fuels';
                break;
            case 2:
                networkName = 'Texaco';
                break;
            case 3:
                networkName = 'Fuelgenie';
                break;
            default:
                networkName = 'Unknown'; // Fallback for unexpected values
        }

        cell.textContent = networkName;
    });
});

function startEdiLoader() {
    document.getElementById('EdiLoader').hidden = false;
}
function stopEdiLoader() {
    document.getElementById('EdiLoader').hidden = true;
}

async function ProcessEdis(event) {
    startEdiLoader();
    event.preventDefault();

    var files = document.getElementById('ediFiles').files;

    var formData = new FormData();
    for (var i = 0; i < files.length; i++) {
        if (files[i]) {  // Check if file exists before adding it
            formData.append('files', files[i]);
        }
    }

    try {
        await $.ajax({
            url: '/EdiController/MoveFilesToCorrectFolder',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                Toast.fire({
                    icon: "success",
                    title: "Files have been processed",
                    text: "All the files should be processed successfully and will be archived in the path C:\\Portland\\Fuel Trading Company\\Fuelcards - Fuelcards\\EDIFilesArchive."
                });
            },
            error: function (jqXHR) {
                var errorMessage = jqXHR.responseText || 'An unknown error occurred.';
                Toast.fire({
                    icon: "error",
                    text: errorMessage,
                });
            }
        });
    } catch (error) {
        console.log(error);
        Toast.fire({
            icon: "error",
            title: "An error occurred",
            text: error.message || "An unknown error occurred."
        });
    } finally {
        stopEdiLoader();
    }
}


function HighlightRow(RowElement) {
    if (RowElement.style.backgroundColor == 'rgb(158, 52, 52)') {
        RowElement.style.backgroundColor = '';
    }
    else {
        RowElement.style.backgroundColor = '#9e3434';
    }
}

async function CheckSites() {
    ShowEdiLoader();
    var ListOfRecentControlIDs = await GetListOfRecentControlIDs();
    $.ajax({
        url: "/EdiController/FindAnyFailedSites",
        type: "POST",
        dataType: 'json',
        data: JSON.stringify(ListOfRecentControlIDs),
        contentType: "application/json;charset=utf-8",
        success: function (SiteResponse) {
            HideEdiLoader();
            console.log("Success:", SiteResponse);
            if (SiteResponse.length == 0) {
                Toast.fire({
                    icon: "success",
                    title: "No failed sites!",
                    text: "All sites are working correctly."
                });
            }
            else {
                Toast.fire({
                    icon: "error",
                    title: "Failed sites found!",
                    text: "Failed sites have been found. Please check the table below."
                });
                ShowFailedSites(SiteResponse)

            }

            // Handle success response
        },
        error: function (error) {
            HideEdiLoader();
            console.error("Error:", error);
            // Handle error response
        }
    });
}
async function MaskedCardCheck() {
    await $.ajax({
        url: "/EdiController/MaskedCards",
        type: "POST",
        dataType: 'json',
        /*data: JSON.stringify(ListOfRecentControlIDs),*/
        contentType: "application/json;charset=utf-8",
        success: function (MaskedCardResponse) {
            Toast.fire({
                icon: "success",
                title: "No masked cards found!",
            });
        },
        error: function (error) {
            alert("Error");
        }
    });
}


async function GetListOfRecentControlIDs() {
    var Table = document.getElementById('EdiTable');
    var tbody = Table.getElementsByTagName('tbody')[0];
    var TableRows = tbody.getElementsByTagName('tr');
    var ListOfRecentControlIDs = [];
    for (var i = 0; i < TableRows.length; i++) {
        var Row = TableRows[i];
        var ControlID = Row.cells[0].innerText;
        ListOfRecentControlIDs.push(ControlID);
    }

    return ListOfRecentControlIDs;
}
async function ShowFailedSites(SiteResponse) {
    var Table = document.getElementById('FailedSitesTable');
    var thead = Table.getElementsByTagName('thead')[0];
    thead.innerHTML = '';
    var Headers = ["Site", "Network", "Status"];
    for (var i = 0; i < Headers.length; i++) {
        var th = document.createElement('th');
        th.innerHTML = Headers[i];
        thead.appendChild(th);
    }
    var tbody = Table.getElementsByTagName('tbody')[0];
    tbody.innerHTML = '';
    for (var i = 0; i < SiteResponse.length; i++) {
        var Row = document.createElement('tr');
        var Site = document.createElement('td');
        var Network = document.createElement('td');
        var Status = document.createElement('td');
        var siteData = SiteResponse[i];

        Site.innerHTML = siteData.code;
        Network.innerHTML = siteData.network;
        Status.innerHTML = "Failed";

        Row.appendChild(Site);
        Row.appendChild(Network);
        Row.appendChild(Status);

        (function (siteData) {
            Row.addEventListener('mouseover', function () {
                Toast.fire({
                    icon: "success",
                    title: "Click to fix site: " + siteData.code,
                });
            });
            Row.addEventListener('click', async function () {
                await Swal.fire({
                    title: 'Fix Site',
                    html: `
                        <div style="text-align: left;">
                            <label for="networkInput" style="display: block; margin-bottom: 5px;">Network:</label>
                            <input type="text" value="${siteData.network}" id="networkInput" name="networkInput" required style="width: 100%; padding: 8px; margin-bottom: 10px; box-sizing: border-box;">
    
                            <label for="SiteNameInput" style="display: block; margin-bottom: 5px;">Site Name:</label>
                            <input type="text" id="SiteNameInput" name="SiteNameInput" required style="width: 100%; padding: 8px; margin-bottom: 10px; box-sizing: border-box;">
    
                            <label for="siteCodeInput" style="display: block; margin-bottom: 5px;">Site Code:</label>
                            <input type="text" id="siteCodeInput" value="${siteData.code}" name="siteCodeInput" required style="width: 100%; padding: 8px; margin-bottom: 10px; box-sizing: border-box;">
    
                            <label for="bandInput" style="display: block; margin-bottom: 5px;">Band:</label>
                            <input type="text" id="bandInput" name="bandInput" required style="width: 100%; padding: 8px; margin-bottom: 10px; box-sizing: border-box;">

                        </div>
                    `,
                    showCancelButton: true,
                    confirmButtonText: 'Submit',
                    cancelButtonText: 'Cancel',
                    preConfirm: () => {
                        const networkInput = document.getElementById('networkInput').value;
                        const siteCodeInput = document.getElementById('siteCodeInput').value;
                        const bandInput = document.getElementById('bandInput').value;
                        const siteNameInput = document.getElementById('SiteNameInput').value;

                        if (!networkInput || !siteCodeInput || !bandInput || !siteNameInput) {
                            Swal.showValidationMessage('Please fill in all fields.');
                            return false; // Prevent the modal from closing
                        }

                        return {
                            network: networkInput,
                            code: siteCodeInput,
                            band: bandInput,
                            name: siteNameInput,

                        };
                    }
                }).then((result) => {
                    if (result.isConfirmed) {
                        console.log(result.value);

                        $.ajax({
                            url: "/EdiController/UploadNewFixedSite",
                            type: "POST",
                            dataType: 'json',
                            data: JSON.stringify(result.value),
                            contentType: "application/json;charset=utf-8",
                            success: async function (response) {
                                await RunThroughFailedSites(result.value.code);
                                Toast.fire({
                                    icon: "success",
                                    title: "Site has been fixed!",
                                    text: "Site has been fixed successfully."
                                });
                            },
                            error: function (error) {
                                console.error("Error:", error);
                                Toast.fire({
                                    icon: "error",
                                    title: "Error!",
                                    text: "An error occurred while fixing the site."
                                });
                            }
                        });
                    }
                });
            });
        })(siteData);
        tbody.appendChild(Row);
    }
}
async function RunThroughFailedSites(sitecode) {
    var Table = document.getElementById('FailedSitesTable');
    var tbody = Table.getElementsByTagName('tbody')[0];
    var TableRows = tbody.getElementsByTagName('tr');
    for (var i = 0; i < TableRows.length; i++) {
        var Row = TableRows[i];
        var Site = Row.cells[0].innerText;
        if (Site == sitecode) {
            Row.style.backgroundColor = 'darkgreen';
            Row.cells[Row.cells.length - 1].innerText = "Fixed";
        }
    }
}

async function ShowEdiLoader() {
    document.getElementById('EdiLoader').hidden = false;
}
async function HideEdiLoader() {
    document.getElementById('EdiLoader').hidden = true;
}

const Toast = Swal.mixin({
    toast: true,
    position: "top-end",
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.onmouseenter = Swal.stopTimer;
        toast.onmouseleave = Swal.resumeTimer;
    }
});