// Function to update background colors
function updateBackgroundColors(oldClass, newClass) {
    const elements = document.querySelectorAll(`.${oldClass}`);
    elements.forEach(element => {
        element.classList.remove(oldClass);
        element.classList.add(newClass);
    });
}

// Apply the theme based on localStorage and show content
function applyThemeAndShowContent() {
    const switchMode = document.getElementById('switch-mode');
    const theme = localStorage.getItem('theme') || 'light'; // Default to 'light' if no theme is set
    const html = document.documentElement;

    if (theme === 'dark') {
        html.classList.add('dark-theme');
        switchMode.checked = true;
        updateBackgroundColors('bg-light', 'bg-dark');
    } else {
        html.classList.remove('dark-theme');
        switchMode.checked = false;
        updateBackgroundColors('bg-dark', 'bg-light');
    }

    // Show the content after theme is applied
    document.body.classList.remove('theme-hidden');
    document.body.classList.add('theme-visible');
}

// Event listener for theme switch
document.getElementById('switch-mode').addEventListener('change', function () {
    const html = document.documentElement;

    if (this.checked) {
        html.classList.add('dark-theme');
        localStorage.setItem('theme', 'dark');
        updateBackgroundColors('bg-light', 'bg-dark');
    } else {
        html.classList.remove('dark-theme');
        localStorage.setItem('theme', 'light');
        updateBackgroundColors('bg-dark', 'bg-light');
    }
});

// Apply the theme and show content on page load
window.addEventListener('load', function () {
    applyThemeAndShowContent();
});