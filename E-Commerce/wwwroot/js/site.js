


//function ChangeColor(element) {
//    event.preventDefault();
//    ChangeColorForAllElement();
//    element.style.color = "White";

//    window.location.href = element.href;



//}

//function ChangeColorForAllElement() {
//    let AllElement = document.querySelectorAll(".Nav a");
//    AllElement.forEach(function (element) {
//        element.style.color = "black";
//    })
//}



// Function to change color and store selection
function ChangeColor(element) {
    // Remove active class from all links
    document.querySelectorAll("nav a").forEach(a => a.classList.remove("active-link"));

    // Add active class to clicked link
    element.classList.add("active-link");

    // Save the selected item's text in localStorage
    localStorage.setItem("activeMenu", element.innerText.trim());
}

// Function to load the active menu item on page load
function LoadActiveMenu() {
    let activeMenu = localStorage.getItem("activeMenu");
    if (activeMenu) {
        document.querySelectorAll("nav a").forEach(a => {
            if (a.innerText.trim() === activeMenu) {
                a.classList.add("active-link");
            }
        });
    }
}

// Call function when page loads
LoadActiveMenu();

