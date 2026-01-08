// DiagnosticCenter - Site-wide JavaScript

// Auto-dismiss alerts after 5 seconds
document.addEventListener('DOMContentLoaded', function () {
    const alerts = document.querySelectorAll('.alert:not(.alert-permanent)');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
});

// Confirm delete actions
document.addEventListener('DOMContentLoaded', function () {
    const deleteForms = document.querySelectorAll('form[action*="Delete"]');
    deleteForms.forEach(function (form) {
        form.addEventListener('submit', function (e) {
            if (!confirm('Are you sure you want to delete this item? This action cannot be undone.')) {
                e.preventDefault();
                return false;
            }
        });
    });
});

// Format currency inputs
function formatCurrency(input) {
    let value = input.value.replace(/[^0-9.]/g, '');
    if (value) {
        input.value = parseFloat(value).toFixed(2);
    }
}

// Add currency formatting to all currency inputs
document.addEventListener('DOMContentLoaded', function () {
    const currencyInputs = document.querySelectorAll('input[type="number"][step="0.01"]');
    currencyInputs.forEach(function (input) {
        input.addEventListener('blur', function () {
            formatCurrency(this);
        });
    });
});

// Validate mobile number format
function validateMobileNumber(input) {
    const pattern = /^[0-9+\-() ]+$/;
    if (!pattern.test(input.value)) {
        input.setCustomValidity('Mobile number can only contain digits, +, -, (), and spaces.');
    } else {
        input.setCustomValidity('');
    }
}

// Add mobile number validation
document.addEventListener('DOMContentLoaded', function () {
    const mobileInputs = document.querySelectorAll('input[name*="MobileNo"]');
    mobileInputs.forEach(function (input) {
        input.addEventListener('input', function () {
            validateMobileNumber(this);
        });
    });
});

// Table row click to details
document.addEventListener('DOMContentLoaded', function () {
    const tableRows = document.querySelectorAll('tbody tr[data-href]');
    tableRows.forEach(function (row) {
        row.style.cursor = 'pointer';
        row.addEventListener('click', function (e) {
            if (!e.target.closest('a, button')) {
                window.location.href = this.dataset.href;
            }
        });
    });
});

// Print functionality
function printPage() {
    window.print();
}

// Export to CSV (basic implementation)
function exportTableToCSV(tableId, filename) {
    const table = document.getElementById(tableId);
    if (!table) return;

    let csv = [];
    const rows = table.querySelectorAll('tr');

    rows.forEach(function (row) {
        const cols = row.querySelectorAll('td, th');
        const csvRow = [];
        cols.forEach(function (col) {
            csvRow.push('"' + col.textContent.trim().replace(/"/g, '""') + '"');
        });
        csv.push(csvRow.join(','));
    });

    downloadCSV(csv.join('\n'), filename);
}

function downloadCSV(csv, filename) {
    const csvFile = new Blob([csv], { type: 'text/csv' });
    const downloadLink = document.createElement('a');
    downloadLink.download = filename;
    downloadLink.href = window.URL.createObjectURL(csvFile);
    downloadLink.style.display = 'none';
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
}

// Initialize tooltips
document.addEventListener('DOMContentLoaded', function () {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});

// Initialize popovers
document.addEventListener('DOMContentLoaded', function () {
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
});

// Search input auto-focus
document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.querySelector('input[name="searchTerm"]');
    if (searchInput && !searchInput.value) {
        searchInput.focus();
    }
});

// Form dirty checking
let formDirty = false;

document.addEventListener('DOMContentLoaded', function () {
    const forms = document.querySelectorAll('form[method="post"]');
    forms.forEach(function (form) {
        const inputs = form.querySelectorAll('input, textarea, select');
        inputs.forEach(function (input) {
            input.addEventListener('change', function () {
                formDirty = true;
            });
        });

        form.addEventListener('submit', function () {
            formDirty = false;
        });
    });

    window.addEventListener('beforeunload', function (e) {
        if (formDirty) {
            e.preventDefault();
            e.returnValue = '';
            return '';
        }
    });
});

// Number input spinner controls
document.addEventListener('DOMContentLoaded', function () {
    const numberInputs = document.querySelectorAll('input[type="number"]');
    numberInputs.forEach(function (input) {
        input.addEventListener('wheel', function (e) {
            e.preventDefault();
        });
    });
});
