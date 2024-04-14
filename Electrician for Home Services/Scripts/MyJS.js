
// Get the image and insert it inside the modal - use its "alt" text as a caption
var img = document.getElementsByClassName("myImages");
var modalImg = document.getElementById("Mega");
var captionText = document.getElementById("caption");

var showModal = function () {
    modalImg.src = this.src;
}

for (var i = 0; i < img.length; i++) {
    img[i].addEventListener("click", showModal);
}

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
}