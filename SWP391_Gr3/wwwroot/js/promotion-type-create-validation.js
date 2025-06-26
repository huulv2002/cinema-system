document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('form');
    const titleInput = document.querySelector('#PromotionType_Title');
    const submitButton = document.querySelector('button[type="submit"]');
    const errorSpan = document.querySelector('span[data-valmsg-for="PromotionType.Title"]');

    //  emoji
    const emojiRegex = /[\u{1F600}-\u{1F64F}\u{1F300}-\u{1F5FF}\u{1F680}-\u{1F6FF}\u{1F1E0}-\u{1F1FF}\u{2702}-\u{27B0}\u{24C2}-\u{1F251}]/u;

    function validateTitle() {
        const title = titleInput.value;
        let errorMessage = '';

        //   trống hoặc chỉ chứa khoảng trắng
        if (!title.trim()) {
            errorMessage = 'Tiêu đề không được để trống hoặc chỉ chứa khoảng trắng.';
        }
            //  > 100 ký tự
        else if (title.length > 100) {
            errorMessage = 'Tiêu đề không được dài quá 100 ký tự.';
        }
        // Check  emoji
        else if (emojiRegex.test(title)) {
            errorMessage = 'Tiêu đề không được chứa emoji.';
        }

        // error message 
        if (errorMessage) {
            errorSpan.textContent = errorMessage;
            errorSpan.style.display = 'block';
            submitButton.disabled = true;
        } else {
            errorSpan.textContent = '';
            errorSpan.style.display = 'none';
            submitButton.disabled = false;
        }
    }

    titleInput.addEventListener('input', validateTitle);


    form.addEventListener('submit', function (event) {
        validateTitle();
        if (errorSpan.textContent) {
            event.preventDefault();
        }
    });
});