document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.btn-next a, .btn-back a').forEach(function (link) {
        link.addEventListener('click', async function (e) {
            e.preventDefault(); 

            const url = this.getAttribute('href'); 

            try {
                const response = await fetch(url, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'text/html'
                    }
                });

                if (!response.ok) {
                    throw new Error('Network error: ' + response.statusText);
                }

                const html = await response.text();
                const nextContent = new DOMParser().parseFromString(html, 'text/html');

                // Hitta och uppdatera innehållet i #survey-section
                const surveySection = document.getElementById('survey-section');
                surveySection.innerHTML = nextContent.querySelector('#survey-section').innerHTML;

            } catch (error) {
                console.error('Error:', error);
                alert('An error occurred while loading the next question.');
            }
        });
    });
});