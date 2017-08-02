//$(function () {
//    $('#openModal').click(function () {
//        var url = $('#changeModal').data('url');

//        $.get(url,
//            function (data) {
//                $('#changeContainer').html(data);

//                $('#changeModal').modal('show');
//            });
//    });
//});





$("#changePhoto").click(function () {

    uploadFile();

});


function uploadFile() {
    //var formData = new FormData($('#formChangePhoto')[0]);

    var formData = new FormData();
    //formData.append("Image", file);
    formData = $('#imageFile').val();

    $.ajax({
        url: '@Url.Action("ChangePhoto", "Users", new { @imageFile = "formData"})',
        type: 'Post',
        beforeSend: function () { },
        success: function () { },
        xhr: function () { // Custom XMLHttpRequest
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) { // Check if upload property exists
                // Progress code if you want
            }
            return myXhr;
        },
        error: function () { },
        data: formData,
        cache: false,
        contentType: false,
        processData: false
    });
}