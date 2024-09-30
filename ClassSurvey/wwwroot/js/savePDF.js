function savePDF() {
    var element = document.querySelector('#overallAnalysisModal .modal-body');

    // Dynamiskt lägga till CSS för sidbrytning
    var style = document.createElement('style');
    style.textContent = `
        h1 {
            page-break-before: always;
        }
        h1:first-of-type {
            page-break-before: auto;
        }
        .modal-body {
            page-break-inside: avoid;
        }
    `;
    document.head.appendChild(style);

    var today = new Date();
    var options = { year: 'numeric', month: '2-digit', day: '2-digit' };
    var formattedDate = today.toLocaleDateString('sv-SE', options).replace(/\//g, '-');

    var fileName = `WIN23_Overall_Analysis(${formattedDate}).pdf`;

    html2pdf().from(element).set({
        margin: 1,
        filename: fileName,
        html2canvas: { scale: 2 },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' }
    }).save().catch(function (error) {
        console.error('Error while generating PDF:', error);
    });
}
