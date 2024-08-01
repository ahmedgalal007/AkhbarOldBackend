// ========== TABLE OF CONTENTS =========== //
//
// 1. Page Loader 
// 2. Text fitting for headlines
// 3. Parallax
// 4. Calendar
// 5. Navbar
// 6. Sidenav
// 7. Sidebar Newsfeed-1
// 8. Sidebar Newsfeed-2
// 9. Sidebar Newsfeed-3
// 10. Sidebar Newsfeed-4
// 11. Sidebar Newsfeed-5
// 12. Sidebar Newsfeed-6
// 13. News Ticker
// 14. To Top Button
// 15. Owl Carousel - News Slider & Schedule Slider
// 16. Owl Carousel - Big Gallery Slider-1	
// 17. Owl Carousel - Big Gallery Slider-2
// 18. Owl Carousel - Big Gallery Slider-3
// 19. Owl Carousel - Small Gallery Slider
// 20. Clock
// 21. Subscribe Form
// 22. Exchange Rates
// 23. Currency Converter
// 24. Sidebar Weather
// 25. Fitvids
// 26. Sidebar Scroll
//
// ======================================= //

(function($) {
    "use strict";

    /* 1. Page Loader */
    $(".loader-item").delay(700).fadeOut();
    $("#pageloader").delay(1200).fadeOut("slow");

    /* 2. Text fitting for headlines */
    $('.extra-large-caption').fitText(1.5, { minFontSize: '26px', maxFontSize: '80px' });
    $('.large-caption').fitText(1.5, { minFontSize: '26px', maxFontSize: '60px' });
    $('.medium-caption').fitText(2, { minFontSize: '20px', maxFontSize: '30px' });
    $('.small-caption').fitText(2.4, { minFontSize: '20px', maxFontSize: '26px' });
    $('.extra-small-caption').fitText(2.4, { minFontSize: '16px', maxFontSize: '22px' });
    $('.error-msg').fitText(2, { minFontSize: '36px', maxFontSize: '90px' });

    /* 3. Parallax */
    function parallaxInit() {
        $('.img-overlay1', '#parallax-section').parallax("100%", 0.8);
    }
    parallaxInit();

    /* 4. Calendar */
    $('#calendar').datepicker();

    /* 5. Navbar */
    // headroom
    /*$("#cd-vertical-nav").headroom({
        tolerance: 5,
        offset: $('#main-section').offset().top,
        classes: {
            pinned: "headroom-pinned",
            unpinned: "headroom-unpinned"
        }
    });*/

    // affix
    $('#fixed-navbar').affix({
        offset: {
            top: $('#fixed-navbar').offset().top
        }
    });

    // $('#cd-vertical-nav').affix({
    //     offset: {
    //         top: $('#cd-vertical-nav').offset().top
    //     }
    // });

    var sections = $('#main-section .module'),
        nav = $('#cd-vertical-nav'),
        nav_height = nav.outerHeight();

    $(window).on('scroll', function() {
        var cur_pos = $(this).scrollTop();

        sections.each(function() {
            var top = $(this).offset().top,
                bottom = top + $(this).outerHeight() - nav_height;

            if (cur_pos >= top && cur_pos <= bottom) {
                nav.find('a').removeClass('active');
                sections.removeClass('active');

                $(this).addClass('active');
                nav.find('a[href="#' + $(this).attr('id') + '"]').addClass('active');
            }
        });
    });

    nav.find('a').on('click', function() {
        var $el = $(this),
            id = $el.attr('href');

        $('html, body').animate({
            scrollTop: $(id).offset().top - nav_height
        }, 500);

        return false;
    });
    /* var $root = $('html, body');

  $('a[href^="#"]').click(function() {
    var href = $.attr(this, 'href');

    var top_height= $(href).offset().top ;
    console.log(top_height);
    $root.animate({
        scrollTop: top_height
    }, 500);

    return false;
});*/






    /* 6. Sidenav */
    $('[data-sidenav]').sidenav();
    $('.navbar-toggle').attr('id', $('#sidenav-toggle').attr('id'));

    // headroom
    $("#mobile-nav").headroom({
        offset: $('#main-section').offset().top,
        classes: {
            pinned: "headroom-pinned",
            unpinned: "headroom-unpinned"
        }
    });
    // Affix
    $('#mobile-nav').affix({
        offset: {
            top: $('.top-menu').height()
        }
    });

    /* 7. Sidebar Newsfeed-1 */
    $('.newsfeed-1').easyTicker({
        direction: 'up',
        easing: 'easeOutSine',
        speed: 'slow',
        interval: 5000,
        height: 'auto',
        visible: 3,
        mousePause: 1,
    });

    /* 8. Sidebar Newsfeed-2 */
    $('.newsfeed-2').easyTicker({
        direction: 'up',
        easing: 'easeOutSine',
        speed: 'slow',
        interval: 5000,
        height: 'auto',
        visible: 4,
        mousePause: 1,
    });

    /* 9. Sidebar Newsfeed-3 */
    $('.newsfeed-3').easyTicker({
        direction: 'up',
        easing: 'easeOutSine',
        speed: 'slow',
        interval: 5000,
        height: 'auto',
        visible: 5,
        mousePause: 1,
    });

    /* 10. Sidebar Newsfeed-4 */
    $('.newsfeed-4').easyTicker({
        direction: 'up',
        easing: 'easeOutSine',
        speed: 'slow',
        interval: 3000,
        height: 'auto',
        visible: 6,
        mousePause: 1,
    });

    /* 11. Sidebar Newsfeed-5 */
    $('.newsfeed-5').easyTicker({
        direction: 'up',
        easing: 'easeOutSine',
        speed: 'slow',
        interval: 3000,
        height: 'auto',
        visible: 7,
        mousePause: 1,
    });

    /* 12. Sidebar Newsfeed-6 */
    $('.newsfeed-6').easyTicker({
        direction: 'up',
        easing: 'easeOutSine',
        speed: 'slow',
        interval: 3000,
        height: 'auto',
        visible: 8,
        mousePause: 1,
    });

    /* 13. News Ticker */
    $('.newsticker').easyTicker({
        direction: 'up',
        easing: 'easeOutSine',
        speed: 'slow',
        interval: 4000,
        height: 'auto',
        visible: 1,
        mousePause: 1,
        controls: {
            up: '.up',
            down: '.down'
        }
    });

    /* 14. To Top Button */
    $().UItoTop({
        easingType: 'easeOutQuart'
    });

    /* 15. Owl Carousel - News Slider & Schedule Slider */
    $("#news-slider,#sidebar-schedule-slider").owlCarousel({
        autoPlay: 5000,
        stopOnHover: true,
        navigation: true,
        navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"],
        paginationSpeed: 1000,
        goToFirstSpeed: 2000,
        singleItem: true,
        autoHeight: true,
        transitionStyle: "fade",
        rtl: true
    });

    /* 16. Owl Carousel - Big Gallery Slider-1 */
    $("#big-gallery-slider-1").owlCarousel({
        rtl: true,
        navigation: true,
        navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"],
        items: 3 // 3 visible items
    });

    /* 17. Owl Carousel - Big Gallery Slider-2 */
    $("#big-gallery-slider-2").owlCarousel({
        rtl: true,
        navigation: true,
        navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"],
        items: 4 // 4 visible items
    });

    /* 18. Owl Carousel - Big Gallery Slider-3 */
    $("#big-gallery-slider-3").owlCarousel({
        rtl: true,
        navigation: true,
        navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"],
        items: 9 // 5 visible items
    });

    /* 19. Owl Carousel - Small Gallery Slider */
    $("#small-gallery-slider").owlCarousel({
        navigation: true,
        navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"],
        items: 4, // 4 items above 1400px browser width
        itemsDesktop: [1400, 3], // 3 items between 1400px and 901px
        itemsDesktopSmall: [900, 2], // 2 items betweem 900px and 601px
        itemsTablet: [600, 1], // 1 items between 600 and 0
        itemsMobile: false // itemsMobile disabled - inherit from itemsTablet option
    });

    /* 20. Clock */
    //function getDate() {
    //    var date = new Date();
    //    var weekday = date.getDay();
    //    var month = date.getMonth();
    //    var day = date.getDate();
    //    var year = date.getFullYear();
    //    var hour = date.getHours();
    //    var minutes = date.getMinutes();
    //    var seconds = date.getSeconds();
    //    if (hour < 10) hour = "0" + hour;
    //    if (minutes < 10) minutes = "0" + minutes;
    //    if (seconds < 10) seconds = "0" + seconds;
    //    var monthNames = ["يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيه", "يوليو", "أغسطس",
    //		"سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر"
    //    ];
    //    var weekdayNames = ["الأحد", "الإثنين", "الثلاثاء", "الأربعاء", "الخميس",
    //		"الجمعة", "السبت"
    //    ];
    //    var ampm = " م ";
    //    if (hour < 12) ampm = " ص ";
    //    if (hour > 12) hour -= 12;
    //    var showDate = weekdayNames[weekday] + ", " + monthNames[month] + " " + day + ", " + year;
    //    var showTime = hour + ":" + minutes + ":" + seconds + ampm;
    //    document.getElementById('date').innerHTML = showDate;
    //    document.getElementById('time').innerHTML = showTime;
    //    requestAnimationFrame(getDate);
    //}
    //getDate();

    /* 21. Subscribe Form */
    $('#subscribeForm').ketchup().submit(function() {
        if ($(this).ketchup('isValid')) {
            var action = $(this).attr('action');
            $.ajax({
                url: action,
                type: 'POST',
                data: {
                    email: $('#address').val()
                },
                success: function(data) {
                    $('#result').html(data);
                },
                error: function() {
                    $('#result').html('Sorry, an error occurred.');
                }
            });
        }
        return false;
    });

  

    /* 25. FitVids plugin for readjusting video sizes */
    $(".video-container").fitVids();

    /* 26. Sidebar Scroll */
    $(".sidebar-scroll").mCustomScrollbar({
        setWidth: false,
        setHeight: 876,
        setTop: 0,
        setLeft: 0,
        axis: "y",
        scrollbarPosition: "outside",
        scrollInertia: 950,
        autoDraggerLength: true,
        autoHideScrollbar: false,
        autoExpandScrollbar: false,
        alwaysShowScrollbar: 0,
        snapAmount: null,
        snapOffset: 0,
        mouseWheel: {
            enable: true,
            scrollAmount: 200,
            axis: "y",
            preventDefault: false,
            deltaFactor: "auto",
            normalizeDelta: true,
            invert: false,
            disableOver: ["select", "option", "keygen", "datalist", "textarea"]
        },
        scrollButtons: {
            enable: false,
            scrollType: "stepless",
            scrollAmount: "auto"
        },
        keyboard: {
            enable: true,
            scrollType: "stepless",
            scrollAmount: "auto"
        },
        contentTouchScroll: 25,
        advanced: {
            autoExpandHorizontalScroll: false,
            autoScrollOnFocus: "input,textarea,select,button,datalist,keygen,a[tabindex],area,object,[contenteditable='true']",
            updateOnContentResize: true,
            updateOnImageLoad: true,
            updateOnSelectorChange: false,
            releaseDraggableSelectors: false
        },
        theme: "light",
        callbacks: {
            onInit: false,
            onScrollStart: false,
            onScroll: false,
            onTotalScroll: false,
            onTotalScrollBack: false,
            whileScrolling: false,
            onTotalScrollOffset: 0,
            onTotalScrollBackOffset: 0,
            alwaysTriggerOffsets: true,
            onOverflowY: false,
            onOverflowX: false,
            onOverflowYNone: false,
            onOverflowXNone: false
        },
        live: false,
        liveSelector: null
    });

})(jQuery);