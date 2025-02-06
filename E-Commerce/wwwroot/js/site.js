


function ChangeColor(element) {
    ChangeColorForAllElement();
    element.style.color = "black";
}

function ChangeColorForAllElement() {
    let AllElement = document.querySelectorAll(".Nav a");
    AllElement.forEach(function (element) {
        element.style.color = "white";
    })
}


