


function showToast(message, type = "success") {

    const toastElement = document.getElementById("appToast");

    const toastTitle = document.getElementById("toastTitle");

    const toastMessage = document.getElementById("toastMessage");

    const toastIcon = document.getElementById("toastIcon");

    toastMessage.innerText = message;

    toastElement.classList.remove(
        "bg-success",
        "bg-danger",
        "bg-warning",
        "bg-info",
        "text-white"
    );

    switch (type) {

        case "success":

            toastTitle.innerText = "Success";
            toastIcon.innerText = "✅";

            toastElement.classList.add("bg-success", "text-white");

            break;

        case "danger":

            toastTitle.innerText = "Error";
            toastIcon.innerText = "❌";

            toastElement.classList.add("bg-danger", "text-white");

            break;

        case "warning":

            toastTitle.innerText = "Warning";
            toastIcon.innerText = "⚠️";

            toastElement.classList.add("bg-warning");

            break;

        case "info":

            toastTitle.innerText = "Information";
            toastIcon.innerText = "ℹ️";

            toastElement.classList.add("bg-info", "text-white");

            break;
    }

    const progress = document.getElementById("toastProgress");

    let width = 100;

    progress.style.width = "100%";

    progress.style.transition = "width 0.05s linear";

    const timer = setInterval(() => {

        width--;

        progress.style.width = width + "%";

        if (width <= 0) {

            clearInterval(timer);

        }

    }, 50);

    const toast = new bootstrap.Toast(toastElement, {
        autohide: false
    });

    toast.show();


    setTimeout(() => {

        toast.hide();

    }, 5000);

    toast.show();
}