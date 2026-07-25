



function showToast(message, type = "success") {
    const toastElement = document.getElementById("appToast");
    const toastMessage = document.getElementById("toastMessage");

    toastMessage.innerText = message;

    toastElement.classList.remove(
        "text-bg-success",
        "text-bg-danger",
        "text-bg-warning",
        "text-bg-info"
    );

    toastElement.classList.add("text-bg-" + type);

    const toast = new bootstrap.Toast(toastElement);

    toast.show();
}