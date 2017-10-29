$(document).on('pageinit', function (event) {
    $('.carousel').on('swiperight', function (event) {
        alert("Right");
        $(this).carousel('next');
    });

    $('.carousel').on('swipeleft', function (event) {
        alert("Left");
        $(this).carousel('prev');
    });
});