// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Handle scrollTo query parameter for repository navigation
$(document).ready(function() {
    // Get the scrollTo parameter from the URL
    const urlParams = new URLSearchParams(window.location.search);
    const scrollToRepo = urlParams.get('scrollTo');
    
    if (scrollToRepo) {
        // Find the element with the matching ID (repository name)
        const targetElement = document.getElementById(scrollToRepo);
        
        if (targetElement) {
            // Scroll to the element smoothly
            targetElement.scrollIntoView({ 
                behavior: 'smooth', 
                block: 'start' 
            });
            
            // Optional: Add a visual highlight to the scrolled-to element
            const repoRow = targetElement.closest('.rowStyle');
            if (repoRow) {
                repoRow.style.backgroundColor = '#fff3cd';
                // Remove the highlight after 3 seconds
                setTimeout(function() {
                    repoRow.style.backgroundColor = '';
                }, 3000);
            }
        }
    }
});
