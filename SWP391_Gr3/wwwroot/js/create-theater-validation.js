document.addEventListener('DOMContentLoaded', function () {
    const nameInput = document.querySelector('input[name="TheaterViewModel.Name"]');
    const locationInput = document.querySelector('input[name="TheaterViewModel.Location"]');
    const form = document.querySelector('form');
    const nameErrorSpan = document.querySelector('span[data-valmsg-for="TheaterViewModel.Name"]');
    const locationErrorSpan = document.querySelector('span[data-valmsg-for="TheaterViewModel.Location"]');

    // Regular expression to detect emojis/icons
    const emojiRegex = /[\u{1F000}-\u{1FFFF}\u{2600}-\u{26FF}\u{2700}-\u{27BF}\u{2B50}\u{2B55}\u{2B06}\u{2B07}\u{2B05}\u{25A0}-\u{25FF}\u{2190}-\u{21FF}]/u;

    // Function to validate input fields
    function validateInput(input, errorSpan, fieldName) {
        return function (e) {
            let value = input.value.trim();
            // Remove emojis/icons
            value = value.replace(emojiRegex, '');
            input.value = value;

            if (!value || value.length === 0) {
                errorSpan.textContent = `${fieldName} không được để trống`;
                input.setCustomValidity('Required');
            } else if (/\s{2,}/.test(value)) {
                errorSpan.textContent = `${fieldName} không được chứa nhiều khoảng trắng liên tiếp`;
                input.setCustomValidity('No consecutive whitespace');
            } else if (value.length > 30) {
                errorSpan.textContent = `${fieldName} không được dài quá 30 ký tự`;
                input.setCustomValidity('Too long');
            } else if (emojiRegex.test(value)) {
                errorSpan.textContent = `${fieldName} không được chứa biểu tượng hoặc emoji`;
                input.setCustomValidity('No emojis');
            } else {
                errorSpan.textContent = '';
                input.setCustomValidity('');
            }
        };
    }

    // Attach input event listeners
    nameInput.addEventListener('input', validateInput(nameInput, nameErrorSpan, 'Tên rạp'));
    locationInput.addEventListener('input', validateInput(locationInput, locationErrorSpan, 'Địa điểm'));

    // Prevent form submission if validation fails
    form.addEventListener('submit', function (e) {
        const nameValue = nameInput.value.trim();
        const locationValue = locationInput.value.trim();

        // Validate Name
        if (!nameValue || nameValue.length === 0) {
            nameErrorSpan.textContent = 'Tên rạp không được để trống';
            nameInput.setCustomValidity('Required');
            e.preventDefault();
        } else if (/\s{2,}/.test(nameValue)) {
            nameErrorSpan.textContent = 'Tên rạp không được chứa nhiều khoảng trắng liên tiếp';
            nameInput.setCustomValidity('No consecutive whitespace');
            e.preventDefault();
        } else if (nameValue.length > 30) {
            nameErrorSpan.textContent = 'Tên rạp không được dài quá 30 ký tự';
            nameInput.setCustomValidity('Too long');
            e.preventDefault();
        } else if (emojiRegex.test(nameValue)) {
            nameErrorSpan.textContent = 'Tên rạp không được chứa biểu tượng hoặc emoji';
            nameInput.setCustomValidity('No emojis');
            e.preventDefault();
        }

        // Validate Location
        if (!locationValue || locationValue.length === 0) {
            locationErrorSpan.textContent = 'Địa điểm không được để trống';
            locationInput.setCustomValidity('Required');
            e.preventDefault();
        } else if (/\s{2,}/.test(locationValue)) {
            locationErrorSpan.textContent = 'Địa điểm không được chứa nhiều khoảng trắng liên tiếp';
            locationInput.setCustomValidity('No consecutive whitespace');
            e.preventDefault();
        } else if (locationValue.length > 30) {
            locationErrorSpan.textContent = 'Địa điểm không được dài quá 30 ký tự';
            locationInput.setCustomValidity('Too long');
            e.preventDefault();
        } else if (emojiRegex.test(locationValue)) {
            locationErrorSpan.textContent = 'Địa điểm không được chứa biểu tượng hoặc emoji';
            locationInput.setCustomValidity('No emojis');
            e.preventDefault();
        }
    });
});