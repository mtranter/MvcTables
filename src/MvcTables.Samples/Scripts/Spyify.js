(function($) {
    var methods = {
        init: function(opts) {
            var selector = typeof opts == "string" ? opts : opts.selector;
            var first = true;
            $(selector).each(function () {
                var id = $(this).attr('id');
                var text = $(this).find('h4').text();
                var a = $('<a/>').attr('href', '#' + id).text(text);
                var li = $('<li/>').append(a);
                if (first) {
                    li.addClass("active");
                    first = false;
                }
                $('#SpyTargets').append(li);
            });
            $('[data-spy="scroll"]').each(function () {
                $(this).scrollspy('refresh');
            });
        }
    };


    $.fn.spyify = methods.init;
})(jQuery)