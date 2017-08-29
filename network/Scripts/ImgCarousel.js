$(document).ready(function () {

    var owl = $("#owl-onpage");
    owl.owlCarousel({
        itemsCustom: [
            [0, 2],
            [979, 4]
        ],
        navigation: true,
        pagination: false,
        itemsScaleUp: true,
        addClassActive: true,
        navigationText: [
            "<i class='fa fa-chevron-left'></i>",
            "<i class='fa fa-chevron-right'></i>"
        ],
    });

    $('#owl-onpage .item a').on('click', function () {
        var theSrc = $(this).find('img').attr('src');
        var owlModal = $('#owl-modal');
        owlModal.empty();
        var item = $('<div>', { 'class': 'item' }).appendTo(owlModal);
        $('<img>', { 'src': theSrc }).appendTo(item);

        // Add others images
        $('#owl-onpage .item a').each(function (i, e) {
            var otherSrc = $(e).find('img').attr('src');
            var item = $('<div>', { 'class': 'item' }).appendTo(owlModal);
            $('<img>', { 'src': otherSrc }).appendTo(item);
        });

        // Call the carousel after clicked on 'a'
        owlModal.owlCarousel({
            singleItem: true,
            navigation: true,
            pagination: false,
            navigationText: [
                "<i class='fa fa-chevron-left'></i>",
                "<i class='fa fa-chevron-right'></i>"
            ],
        });

        $('#GListModalGallery').unbind().on('hidden.bs.modal', function () {
            owlModal.data('owlCarousel').destroy();
        });
    });
});