document.addEventListener("DOMContentLoaded", function () {
    const cards = document.querySelectorAll(".theater-card");
    const hiddenInput = document.getElementById("TheaterId");

    cards.forEach(card => {
        card.addEventListener("click", function () {
            // Bỏ chọn tất cả
            cards.forEach(c => c.classList.remove("selected"));
            // Chọn cái được click
            this.classList.add("selected");

            // Lấy ID rạp
            const theaterId = this.getAttribute("data-theater-id");
            hiddenInput.value = theaterId;
        });
    });
});
