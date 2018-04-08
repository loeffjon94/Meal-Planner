$(document).on('ready', function (event) {
    $('.carousel').on('swiperight', function (event) {
        alert("Right");
        $(this).carousel('next');
    });

    $('.carousel').on('swipeleft', function (event) {
        alert("Left");
        $(this).carousel('prev');
    });

    $('ul.nav li.dropdown').hover(function () {
        $(this).find('.dropdown-menu').stop(true, true).delay(100).fadeIn(200);
    }, function () {
        $(this).find('.dropdown-menu').stop(true, true).delay(100).fadeOut(200);
        });

    $('.table').addClass('table-striped');
    $('.table').addClass('table-hover');
});