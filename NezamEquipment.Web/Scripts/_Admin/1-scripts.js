    setTimeout(function () { $('#content-wrapper > .row').css({ opacity: 1 }); }, 200);
    $('#sidebar-nav .dropdown-toggle').on('click', function (e) {
        e.preventDefault();
        var $item = $(this).parent();
        if ($item.hasClass('active')) {
            return;
        }
        if (!$item.hasClass('open')) {
            $item.parent().find('.open .submenu').slideUp('fast');
            $item.parent().find('.open').toggleClass('open');
        }
        $item.toggleClass('open');
        if ($item.hasClass('open')) {
            $item.children('.submenu').slideDown('fast');
        } else {
            $item.children('.submenu').slideUp('fast');
        }
    });
    $('body').on('mouseenter', '#page-wrapper.nav-small #sidebar-nav .dropdown-toggle', function (e) {
        var $sidebar = $(this).parents('#sidebar-nav');
        if ($(document).width() >= 1820) {
            var $item = $(this).parent();
            $item.addClass('open');
            $item.children('.submenu').slideDown('fast');
        }
    });
    $('body').on('mouseleave', '#page-wrapper.nav-small #sidebar-nav > .nav-pills > li', function (e) {
        var $sidebar = $(this).parents('#sidebar-nav');
        if ($(document).width() >= 1820) {
            var $item = $(this);
            if ($item.hasClass('open')) {
                $item.find('.open .submenu').slideUp('fast');
                $item.find('.open').removeClass('open');
                $item.children('.submenu').slideUp('fast');
            }
            $item.removeClass('open');
        }
    });
