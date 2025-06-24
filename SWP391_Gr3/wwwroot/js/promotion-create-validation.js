document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('promotionForm');
    const codeInput = document.getElementById('Promotion_Code');
    const promotionTypeSelect = document.getElementById('Promotion_PromotionTypeId');
    const valueInput = document.getElementById('Promotion_Value');
    const stockInput = document.getElementById('Promotion_Stock');
    const startDateInput = document.getElementById('Promotion_StartDate');
    const endDateInput = document.getElementById('Promotion_EndDate');
    const imageInput = document.getElementById('ImageFile');
    const descriptionInput = document.getElementById('Promotion_Description');

    const codeError = document.querySelector('span[data-valmsg-for="Promotion.Code"]');
    const promotionTypeError = document.querySelector('span[data-valmsg-for="Promotion.PromotionTypeId"]');
    const valueError = document.querySelector('span[data-valmsg-for="Promotion.Value"]');
    const stockError = document.querySelector('span[data-valmsg-for="Promotion.Stock"]');
    const startDateError = document.querySelector('span[data-valmsg-for="Promotion.StartDate"]');
    const endDateError = document.querySelector('span[data-valmsg-for="Promotion.EndDate"]');
    const imageError = document.querySelector('span[data-valmsg-for="ImageFile"]');
    const descriptionError = document.querySelector('span[data-valmsg-for="Promotion.Description"]');

    // Regular expressions
    const emojiRegex = /[\u{1F000}-\u{1FFFF}\u{2600}-\u{26FF}\u{2700}-\u{27BF}\u{2B50}\u{2B55}\u{2B06}\u{2B07}\u{2B05}\u{25A0}-\u{25FF}\u{2190}-\u{21FF}]/u;
    const descriptionRegex = /^[a-zA-Z0-9\s]*$/;

    // Validation function for Code
    function validateCode() {
        let value = codeInput.value.trim();
        value = value.replace(emojiRegex, '');
        codeInput.value = value;

        if (!value) {
            codeError.textContent = 'Mã khuyến mãi không được để trống';
            codeInput.setCustomValidity('Required');
            return false;
        } else if (value.length > 10) {
            codeError.textContent = 'Mã khuyến mãi không được dài quá 10 ký tự';
            codeInput.setCustomValidity('Too long');
            return false;
        } else if (emojiRegex.test(value)) {
            codeError.textContent = 'Mã khuyến mãi không được chứa biểu tượng hoặc emoji';
            codeInput.setCustomValidity('No emojis');
            return false;
        } else {
            codeError.textContent = '';
            codeInput.setCustomValidity('');
            return true;
        }
    }

    // Validation function for PromotionTypeId
    function validatePromotionType() {
        const value = promotionTypeSelect.value;
        if (!value) {
            promotionTypeError.textContent = 'Vui lòng chọn loại khuyến mãi';
            promotionTypeSelect.setCustomValidity('Required');
            return false;
        } else {
            promotionTypeError.textContent = '';
            promotionTypeSelect.setCustomValidity('');
            return true;
        }
    }

    // Validation function for Value
    function validateValue() {
        const value = valueInput.value.trim();
        const numValue = parseFloat(value);

        if (!value) {
            valueError.textContent = 'Giá trị khuyến mãi không được để trống';
            valueInput.setCustomValidity('Required');
            return false;
        } else if (isNaN(numValue) || !/^\d*\.?\d+$/.test(value)) {
            valueError.textContent = 'Giá trị khuyến mãi chỉ được chứa số';
            valueInput.setCustomValidity('Invalid number');
            return false;
        } else if (numValue <= 0) {
            valueError.textContent = 'Giá trị khuyến mãi phải là số dương';
            valueInput.setCustomValidity('Positive number');
            return false;
        } else {
            valueError.textContent = '';
            valueInput.setCustomValidity('');
            return true;
        }
    }

    // Validation function for Stock
    function validateStock() {
        const value = stockInput.value.trim();
        const numValue = parseInt(value);

        if (!value) {
            stockError.textContent = 'Số lượng không được để trống';
            stockInput.setCustomValidity('Required');
            return false;
        } else if (isNaN(numValue) || !/^\d+$/.test(value)) {
            stockError.textContent = 'Số lượng chỉ được chứa số nguyên';
            stockInput.setCustomValidity('Invalid integer');
            return false;
        } else if (numValue <= 0 || numValue > 99) {
            stockError.textContent = 'Số lượng phải là số dương và không quá 99';
            stockInput.setCustomValidity('Invalid range');
            return false;
        } else {
            stockError.textContent = '';
            stockInput.setCustomValidity('');
            return true;
        }
    }

    // Validation function for StartDate
    function validateStartDate() {
        const value = startDateInput.value;
        if (!value) {
            startDateError.textContent = 'Ngày bắt đầu không được để trống';
            startDateInput.setCustomValidity('Required');
            return false;
        } else {
            startDateError.textContent = '';
            startDateInput.setCustomValidity('');
            return true;
        }
    }

    // Validation function for EndDate
    function validateEndDate() {
        const value = endDateInput.value;
        const startDate = startDateInput.value;

        if (!value) {
            endDateError.textContent = 'Ngày kết thúc không được để trống';
            endDateInput.setCustomValidity('Required');
            return false;
        } else if (startDate && new Date(value) <= new Date(startDate)) {
            endDateError.textContent = 'Ngày kết thúc phải sau ngày bắt đầu';
            endDateInput.setCustomValidity('Invalid date');
            return false;
        } else {
            endDateError.textContent = '';
            endDateInput.setCustomValidity('');
            return true;
        }
    }

    // Validation function for ImageFile
    function validateImage() {
        const file = imageInput.files[0];
        const allowedExtensions = ['.jpg', '.jpeg', '.png', '.gif'];
        if (file) {
            const extension = file.name.toLowerCase().substring(file.name.lastIndexOf('.'));
            if (!allowedExtensions.includes(extension)) {
                imageError.textContent = 'Chỉ chấp nhận các định dạng hình ảnh: jpg, jpeg, png, gif';
                imageInput.value = '';
                imageInput.setCustomValidity('Invalid file');
                return false;
            } else {
                imageError.textContent = '';
                imageInput.setCustomValidity('');
                return true;
            }
        }
        return true; // Image is optional
    }

    // Validation function for Description
    function validateDescription() {
        let value = descriptionInput.value.trim();
        value = value.replace(emojiRegex, '');
        descriptionInput.value = value;

        if (value && value.length > 30) {
            descriptionError.textContent = 'Mô tả không được dài quá 30 ký tự';
            descriptionInput.setCustomValidity('Too long');
            return false;
        } else if (value && !descriptionRegex.test(value)) {
            descriptionError.textContent = 'Mô tả không được chứa biểu tượng hoặc ký tự đặc biệt';
            descriptionInput.setCustomValidity('Invalid characters');
            return false;
        } else {
            descriptionError.textContent = '';
            descriptionInput.setCustomValidity('');
            return true;
        }
    }

    // Attach input event listeners
    codeInput.addEventListener('input', validateCode);
    promotionTypeSelect.addEventListener('change', validatePromotionType);
    valueInput.addEventListener('input', validateValue);
    stockInput.addEventListener('input', validateStock);
    startDateInput.addEventListener('change', validateStartDate);
    endDateInput.addEventListener('change', validateEndDate);
    imageInput.addEventListener('change', validateImage);
    descriptionInput.addEventListener('input', validateDescription);

    // Prevent form submission if validation fails
    form.addEventListener('submit', function (e) {
        const isCodeValid = validateCode();
        const isPromotionTypeValid = validatePromotionType();
        const isValueValid = validateValue();
        const isStockValid = validateStock();
        const isStartDateValid = validateStartDate();
        const isEndDateValid = validateEndDate();
        const isImageValid = validateImage();
        const isDescriptionValid = validateDescription();

        if (!isCodeValid || !isPromotionTypeValid || !isValueValid || !isStockValid || !isStartDateValid || !isEndDateValid || !isImageValid || !isDescriptionValid) {
            e.preventDefault();
        }
    });
});