
// dùng để bắt lỗi người dùng khi họ đăng ký tài khoản 
function restrictInput(element) {
    const validPattern = /^[a-zA-Z0-9\s\u00C0-\u1EF9._-]*$/;
    const errorElement = document.getElementById(element.id + "Error");
    if (!validPattern.test(element.value)) {
        element.value = element.value.replace(/[^\w\s\u00C0-\u1EF9._-]/g, '');
        errorElement.classList.remove("d-none");
    } else {
        errorElement.classList.add("d-none");
    }
}

function restrictPhoneInput(element) {
    const validPattern = /^[0-9]*$/;
    const errorElement = document.getElementById("phoneError");
    if (!validPattern.test(element.value)) {
        element.value = element.value.replace(/[^0-9]/g, '');
        errorElement.classList.remove("d-none");
    } else {
        errorElement.classList.add("d-none");
    }
}

function validateForm() {
    let password = document.getElementById("HashPass").value;
    let confirmPassword = document.getElementById("confirmPasswordHash").value;
    let errorText = document.getElementById("passwordError");
    if (password !== confirmPassword) {
        errorText.classList.remove("d-none");
        return false;
    } else {
        errorText.classList.add("d-none");
    }

    let fullName = document.getElementById("RegisterUser_FullName").value;
    let address = document.getElementById("RegisterUser_address").value;
    let phone = document.getElementById("RegisterUser_PhoneNumber").value;
    const textPattern = /^[a-zA-Z0-9\s\u00C0-\u1EF9._-]*$/;
    const phonePattern = /^[0-9]*$/;

    if (!textPattern.test(fullName)) {
        document.getElementById("fullNameError").classList.remove("d-none");
        return false;
    }
    if (!textPattern.test(address)) {
        document.getElementById("addressError").classList.remove("d-none");
        return false;
    }
    if (!phonePattern.test(phone)) {
        document.getElementById("phoneError").classList.remove("d-none");
        return false;
    }

    return true;
}