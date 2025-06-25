document.addEventListener('DOMContentLoaded', function () {
    const roomCodeInput = document.querySelector('input[name="Room.Code"]');
    const form = document.querySelector('form');
    const errorSpan = document.querySelector('span[data-valmsg-for="Room.Code"]');

    roomCodeInput.addEventListener('input', function (e) {
   
        let value = e.target.value.replace(/[^0-9]/g, '').trim();

  
        e.target.value = value;

   
        if (!value || value.length === 0) {
            errorSpan.textContent = 'Mã phòng không được để trống';
            roomCodeInput.setCustomValidity('Required');
        } else if (/\s/.test(e.target.value)) {
            errorSpan.textContent = 'Mã phòng không được chứa khoảng trắng';
            roomCodeInput.setCustomValidity('No whitespace');
        } else {
            const numValue = parseInt(value);
            if (numValue < 1001 || numValue > 9999) {
                errorSpan.textContent = 'Mã phòng phải nằm trong khoảng từ 1001 đến 9999';
                roomCodeInput.setCustomValidity('Invalid range');
            } else {
                errorSpan.textContent = '';
                roomCodeInput.setCustomValidity('');
            }
        }
    });


    form.addEventListener('submit', function (e) {
        const value = roomCodeInput.value.trim();
        const numValue = parseInt(value);

        if (!value || value.length === 0) {
            errorSpan.textContent = 'Mã phòng không được để trống';
            roomCodeInput.setCustomValidity('Required');
            e.preventDefault();
        } else if (/\s/.test(value)) {
            errorSpan.textContent = 'Mã phòng không được chứa khoảng trắng';
            roomCodeInput.setCustomValidity('No whitespace');
            e.preventDefault();
        } else if (numValue < 1001 || numValue > 9999) {
            errorSpan.textContent = 'Mã phòng phải nằm trong khoảng từ 1001 đến 9999';
            roomCodeInput.setCustomValidity('Invalid range');
            e.preventDefault();
        }
    });
});