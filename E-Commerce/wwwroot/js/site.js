


function ChangeColor(element) {
    ChangeColorForAllElement();
    element.style.color = "black";
}

function ChangeColorForAllElement() {
    let AllElement = document.querySelectorAll(".Nav li");
    AllElement.forEach(function (element) {
        element.style.color = "white";
    })
}


