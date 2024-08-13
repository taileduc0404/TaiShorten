/*!
* Start Bootstrap - Landing Page v6.0.6 (https://startbootstrap.com/theme/landing-page)
* Copyright 2013-2023 Start Bootstrap
* Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-landing-page/blob/master/LICENSE)
*/
// This file is intentionally blank
// Use this file to add JavaScript to your project

document.getElementById('originalUrl').addEventListener('input', function () {
    const urlInput = this;
    const submitButton = document.getElementById('submitButton');
    
    if (urlInput.validity.valid) {
        submitButton.disabled = false;
    } else {
        submitButton.disabled = true;
    }
});


// Copy url
function copyToClipboard() {
    // Lấy đối tượng chứa đường dẫn ngắn
    var linkContainer = document.getElementById("shortenedLink");

    // Tạo một thẻ input ẩn để chứa đường dẫn ngắn
    var input = document.createElement("input");
    input.value = linkContainer.href;
    document.body.appendChild(input);

    // Chọn toàn bộ nội dung của thẻ input
    input.select();

    // Sao chép nội dung đã chọn vào clipboard
    document.execCommand("copy");

    // Xóa thẻ input sau khi sao chép
    document.body.removeChild(input);

    // Thông báo sao chép thành công (có thể thay đổi theo ý bạn)
    alert("Đã sao chép đường dẫn!");
}
