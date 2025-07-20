
function restrictInput(element) {
    const validPattern = /^[a-zA-Z\u00C0-\u1EF9\s]*$/;
    const errorElement = document.getElementById(element.id + "Error");


    if (!validPattern.test(element.value)) {
        element.value = element.value.replace(/[^\w\s\u00C0-\u1EF9._-]/g, '');
        errorElement.classList.remove("d-none");
    } else {
        errorElement.classList.add("d-none");
    }


    if (element.value.startsWith(" ")) {
        element.value = element.value.trimStart();
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


    if (element.value.length > 10) {
        element.value = element.value.substring(0, 11);
        errorElement.innerText = "Số điện thoại không được vượt quá 10 số!";
        errorElement.classList.remove("d-none");
    }
}


function validateForm() {
    let password = document.getElementById("HashPass").value;
    let confirmPassword = document.getElementById("confirmPasswordHash").value;
    let fullName = document.getElementById("RegisterUser_FullName").value.trim();
    let address = document.getElementById("RegisterUser_address").value.trim();
    let phone = document.getElementById("RegisterUser_PhoneNumber").value.trim();

    let passwordError = document.getElementById("passwordError");
    let fullNameError = document.getElementById("fullNameError");
    let addressError = document.getElementById("addressError");
    let phoneError = document.getElementById("phoneError");

    const textPattern = /^[a-zA-Z\u00C0-\u1EF9\s]+$/;
    const mustContainLetter = /[a-zA-Z\u00C0-\u1EF9]/;
    const phonePattern = /^[0-9]+$/;


    if (password.length < 6 || password.length > 10) {
        passwordError.innerText = "Mật khẩu phải từ 6 đến 10 ký tự!";
        passwordError.classList.remove("d-none");
        return false;
    } else {
        passwordError.classList.add("d-none");
    }


    if (password !== confirmPassword) {
        passwordError.innerText = "Mật khẩu xác nhận không khớp!";
        passwordError.classList.remove("d-none");
        return false;
    } else {
        passwordError.classList.add("d-none");
    }


    if (fullName === "" || !textPattern.test(fullName) || !mustContainLetter.test(fullName)) {
        fullNameError.innerText = "Họ và tên phải chứa ít nhất một chữ cái và không được để trống!";
        fullNameError.classList.remove("d-none");
        return false;
    } else {
        fullNameError.classList.add("d-none");
    }


    if (address === "" || !textPattern.test(address) || !mustContainLetter.test(address)) {
        addressError.innerText = "Địa chỉ không được để trống và phải có ít nhất một chữ cái!";
        addressError.classList.remove("d-none");
        return false;
    } else {
        addressError.classList.add("d-none");
    }


    if (!phonePattern.test(phone) || phone.length === 0 || phone.length > 10) {
        phoneError.innerText = "Số điện thoại không được để trống và tối đa 10 số!";
        phoneError.classList.remove("d-none");
        return false;
    } else {
        phoneError.classList.add("d-none");
    }

    return true;
}

document.getElementById("RegisterUser_FullName").addEventListener("blur", function () {
    this.value = this.value.trim();
});
document.getElementById("RegisterUser_address").addEventListener("blur", function () {
    this.value = this.value.trim();
});
