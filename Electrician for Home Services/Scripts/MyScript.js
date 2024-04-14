function ShowHide() {
    let personType = document.getElementById('PersonType')
    let hid = document.getElementById('hidden-panel');
    if (personType.value == "Electrician") {

        hid.style.display = "";
    } else {
        hid.style.display = "none";
    }
}