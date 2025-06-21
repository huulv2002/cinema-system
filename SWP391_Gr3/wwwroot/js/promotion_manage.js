
document.querySelectorAll('.btn').forEach(button => {
    button.addEventListener('click', function () {
        this.style.transform = 'scale(0.95)';
        setTimeout(() => {
            this.style.transform = 'scale(1)';
        }, 100);
    });
});


document.getElementById("proIdInput").addEventListener("input", function () {
    if (this.value === "") {
        window.location.href = window.location.pathname;
    }
});