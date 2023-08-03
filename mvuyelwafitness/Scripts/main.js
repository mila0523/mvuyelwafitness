var sidemenu = document.getElementById("mysidebar");

function togglenav() {
    sidemenu.style.left = "0px"     
}

function toggleclosenav() {
    sidemenu.style.left = "-302px"
}

/* When the user clicks on the button, 
toggle between hiding and showing the dropdown content */
function openDropDown() {
    document.getElementById("myDropdown").style.display = "block";
}


// Close the dropdown if the user clicks outside of it
function closeDropDown() { 
    document.getElementById("myDropdown").style.display = "none";
}


//loader
document.addEventListener('DOMContentLoaded', function () {
    var myDiv = document.getElementById('loader-bg');
    myDiv.style.opacity = 1;
    myDiv.style.zIndex = 9999;

    // Fade out the div after a delay
    setTimeout(function () {
        myDiv.style.zIndex = 0;
        myDiv.style.transition = 'opacity 2s';
        myDiv.style.opacity = 0;
        
    }, 1000); // Adjust the delay (in milliseconds) as needed
});