document.addEventListener("DOMContentLoaded", function () {
    const fullNameInput = document.getElementById("RegisterUser_FullName");
    const addressInput = document.getElementById("RegisterUser_address");
    const phoneInput = document.getElementById("RegisterUser_PhoneNumber");
    const passwordInput = document.getElementById("HashPass");
    const confirmPasswordInput = document.getElementById("confirmPasswordHash");

    // sự kiện kiểm tra khi nhập
    [fullNameInput, addressInput, phoneInput, passwordInput, confirmPasswordInput].forEach(input => {
        input.addEventListener("input", toggleRegisterButton);
    });

    if (fullNameInput) {
        fullNameInput.addEventListener("blur", function () {
            this.value = this.value.trim();
        });
    }

    if (addressInput) {
        addressInput.addEventListener("blur", function () {
            this.value = this.value.trim();
        });
    }
});

function restrictInput(element) {
    const validPattern = /^[a-zA-Z\u00C0-\u1EF9\s]*$/;
    const errorElement = document.getElementById(element.id + "Error");

    if (!validPattern.test(element.value)) {
        element.value = element.value.replace(/[^a-zA-Z\u00C0-\u1EF9\s]/g, '');
        errorElement.classList.remove("d-none");
    } else {
        errorElement.classList.add("d-none");
    }

    if (element.value.startsWith(" ")) {
        element.value = element.value.trimStart();
    }
    toggleRegisterButton(); // kiểm tra nút
}

function restrictPhoneInput(element) {
    const validPattern = /^0[0-9]*$/;
    const errorElement = document.getElementById("phoneError");

    element.value = element.value.replace(/[^0-9]/g, '');
    if (element.value.length > 10) {
        element.value = element.value.substring(0, 10);
    }

    if (!validPattern.test(element.value) || element.value.length !== 10) {
        errorElement.classList.remove("d-none");
    } else {
        errorElement.classList.add("d-none");
    }
    toggleRegisterButton(); // kiểm tra nút
}

function validateForm() {
    // Kiểm tra khi submit
    return checkFormValid();
}

// toàn bộ form có hợp lệ không
function checkFormValid() {
    let password = document.getElementById("HashPass").value;
    let confirmPassword = document.getElementById("confirmPasswordHash").value;
    let fullName = document.getElementById("RegisterUser_FullName").value.trim();
    let address = document.getElementById("RegisterUser_address").value.trim();
    let phone = document.getElementById("RegisterUser_PhoneNumber").value.trim();

    const textPattern = /^[a-zA-Z\u00C0-\u1EF9\s]+$/;
    const mustContainLetter = /[a-zA-Z\u00C0-\u1EF9]/;
    const phonePattern = /^0[0-9]{9}$/;

    return (
        password.length >= 6 &&
        password.length <= 10 &&
        password === confirmPassword &&
        fullName !== "" && textPattern.test(fullName) && mustContainLetter.test(fullName) &&
        address !== "" && textPattern.test(address) && mustContainLetter.test(address) &&
        phonePattern.test(phone)
    );
}

// bật/tắt đăng ký
function toggleRegisterButton() {
    const registerBtn = document.getElementById("registerBtn");
    if (checkFormValid()) {
        registerBtn.disabled = false;
    } else {
        registerBtn.disabled = true;
    }
}
