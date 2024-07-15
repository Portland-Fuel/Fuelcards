$(document).ready(function() {
    $('#importButton').click(async function() {
        var files = [];
        files.push($('#ediFile1')[0].files[0]);
        files.push($('#ediFile2')[0].files[0]);
        files.push($('#ediFile3')[0].files[0]);
        files.push($('#ediFile4')[0].files[0]);

        var formData = new FormData();
        for (var i = 0; i < files.length; i++) {
            if (!files[i]) {
                alert('Please select all four files.');
                return;
            }
            formData.append('files', files[i]);
        }

        await $.ajax({
            url: '/api/EdiController/MoveFilesToCorrectFolder',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function(response) {
                Toast.fire({
                    icon: "success",
                    title: "Files have been moved!",
                    text: "Files have been moved to the correct folder. Please wait for the system to import the files."
                });
            },
            error: function() {
                alert('Error uploading files.');
            }
        });
    });
});
document.addEventListener('DOMContentLoaded', function () {
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


function HighlightRow(RowElement){
    if(RowElement.style.backgroundColor == 'rgb(158, 52, 52)'){
        RowElement.style.backgroundColor = '';
    }
    else{
        RowElement.style.backgroundColor = '#9e3434';
    }
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