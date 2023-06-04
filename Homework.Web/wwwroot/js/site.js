var pauseTime;
function search() {
    document.getElementById("spinner").style.display = "";
    clearTimeout(pauseTime);
    pauseTime = setTimeout(function () {
        performSearch();
    }, 200);
}

function performSearch() {
    var searchElement = document.getElementById("search");
    var searchString = searchElement.value ? searchElement.value.toUpperCase() : "";
    console.log(searchString);

    productBoxElements = document.getElementsByClassName("product-box");

    // Loop through all list items, and hide those who don't match the search query
    for (i = 0; i < productBoxElements.length; i++) {
        var cardTitleElements = productBoxElements[i].getElementsByClassName("card-title");
        var cardTitle = cardTitleElements[0].textContent;

        if (cardTitle && cardTitle.toUpperCase().indexOf(searchString) > -1) {
            productBoxElements[i].style.display = "";
        } else {
            productBoxElements[i].style.display = "none";
        }
    }

    document.getElementById("spinner").style.display = "none";

}
