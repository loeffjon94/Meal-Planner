$(document).ready(function () {
    // Smooth scrolling using jQuery easing
    $('a.js-scroll-trigger[href*="#"]:not([href="#"])').click(function () {
        if (location.pathname.replace(/^\//, '') === this.pathname.replace(/^\//, '') && location.hostname === this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html, body').animate({
                    scrollTop: (target.offset().top)
                }, 1000, "easeInOutExpo");
                return false;
            }
        }
    });

    // Scroll reveal calls
    window.sr = ScrollReveal();
    sr.reveal('.sr-icons', {
        duration: 600,
        scale: 0.3,
        distance: '0px',
        reset: true,
        delay: 200
    }, 200);
    sr.reveal('.sr-button', {
        duration: 1000,
        delay: 200,
        reset: true
    });
    sr.reveal('.sr-contact', {
        duration: 600,
        scale: 0.3,
        distance: '0px',
        reset: true
    }, 300);

    $('body').css('height', 'unset');
    $('#menu').show();

    //slider
    var slideout = new Slideout({
        'panel': document.getElementById('panel'),
        'menu': document.getElementById('menu'),
        'padding': 220,
        'tolerance': 70
    });
    var fixed = document.querySelector('.fixed-header');

    document.querySelector('.hamburger').addEventListener('click', function () {
        slideout.toggle();
        document.querySelector(".hamburger").classList.toggle("is-active");
    });

    slideout.on('open', function () {
        fixed.style.transition = '';
        document.querySelector(".hamburger").classList.add("is-active");
    });

    slideout.on('close', function () {
        fixed.style.transition = '';
        document.querySelector(".hamburger").classList.remove("is-active");
    });

    slideout.on('translate', function (translated) {
        fixed.style.transform = 'translateX(' + translated + 'px)';
    });

    slideout.on('beforeopen', function () {
        fixed.style.transition = 'transform 300ms ease';
        fixed.style.transform = 'translateX(220px)';
    });

    slideout.on('beforeclose', function () {
        fixed.style.transition = 'transform 300ms ease';
        fixed.style.transform = 'translateX(0px)';
    });

    $('.js-scroll-trigger').on('click', function () {
        slideout.close();
    });

    //Carousel
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