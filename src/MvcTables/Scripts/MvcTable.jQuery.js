(function($, undefined) {

    var settingsKey = '_mvc_table_';
    var internalFilterClass = "mvctable-trigger";
    var defaults = {};
    
    function copyEvents(source, target) {
        var evs = $._data(source[0], 'events');
        for (var ev in evs) {
            for (var i = 0; i < evs[ev].length; i++)
                $.fn[ev].apply(target, [evs[ev][i].handler]);
        }
    }

    function escapeText(txt) {
        return $('<div/>').text(txt).html();
    }

    function removeHandlers() {
        var $that = $(this);
        var id = $that.data('table-id');
        $('body').off('.' + id + '.mvctable');
    }

    function initQueryString(params) {
        return params ? '?' + Querystring.serialize(params).toQueryString() : '';
    }

    function attachHandlers() {
        var $that = $(this);
        var id = $that.data('table-id');
        var filter = $that.data('filter');

        //$('a.' + internalFilterClass).on('click.' + id + '.mvctable', function (e) {
        //    e.preventDefault();
        //    var newVals = new Querystring($(this).attr('href').split('?')[1]).deserialize();
        //    methods.refresh.apply($that, [newVals]);
        //});

        $('body').on('click.' + id + '.mvctable', 'a.' + filter, function (e) {
            e.preventDefault(); 
            var newVals = new Querystring($(this).attr('href').split('?')[1]).deserialize();
            methods.refresh.apply($that, [newVals]);
        });
        
        var changeElements = '';
        $.each(['input', 'select', 'textarea'], function (_, v) {
            changeElements += (v + '.' + filter + ',');
        });
        changeElements = changeElements.substr(0, changeElements.length - 1);
        $('body').on('change.' + id + '.mvctable', changeElements, function (e) {
            e.preventDefault();
            var params = {};
            params[$(this).attr('name')] = $(this).val();
            methods.refresh.apply($that, [params]);
        }); 
        $('body').on('submit.' + id + '.mvctable', 'form[data-target="' + id + '"].' + filter, function (e) {
            e.preventDefault();
            var params = new Querystring($(this).serialize()).deserialize();
            methods.refresh.apply($that, [params]);
        });
    }

    var methods = {
        init: function(settings) {
            return $(this).each(function (_, v) {
                var $that = $(this);
                var setts = $that.data(settingsKey);
                if (setts) return $that;
                settings = settings || {};
                setts = $.extend({}, defaults, settings);
                $that.data(settingsKey, setts);
                attachHandlers.apply($that, []);
            });
        },
        goToPage: function(page) {
            return $(this).each(function (_, v) {
                methods.refresh.apply(this, [{PageNumber: page}]);
            });
        },
        sortBy: function (column, asc) {
            return $(this).each(function (_, v) {
                asc = typeof asc === "undefined" ? true : false;
                methods.refresh.apply(this, [{ SortColumn: column, SortAscending: asc }]);
            });
        },
        serialize: function () {
            var result = '';
            $(this).first().find('tbody tr').each(function(i, r) {
                $(r).find('input,select,textarea').each(function(_, ip) {
                    var name = $(ip).attr('name');
                    if (name) {
                        result += ((name + '=' + escapeText($(ip).val())) + '&');
                    }
                });
            });
            return result;
        },
        validate: function() {
            if (typeof $.fn.valid === 'function') {
                return $(this).parents('form').valid();
            }
        },
        save: function(url, ok , err) {
            var stringToPost = methods.serialize.apply(this);
            var $that = this;
            var savingEvent = $.Event("saving.mvctable");
            $that.trigger(savingEvent);
            if (savingEvent.isDefaultPrevented())
                return;
            $.ajax({
                type: "POST",
                url: url,
                data: stringToPost,
                success: function () {
                    $that.parents('form').data('validator').resetForm();
                    var savedEvent = $.Event("saved.mvctable");
                    $that.trigger(savedEvent);
                    if (savedEvent.isDefaultPrevented())
                        return;
                    ok.apply($that, arguments);
                }
            });
        },
        refreshTable: function(params) {
            params = params || {};
            params.RenderTable = true;
            params.RenderPager = false;
        },
        refreshPaginator: function(params) {
            params = params || {};
            params.RenderTable = false;
            params.RenderPager = true;
        },
        refresh: function (params) {
            var $that = $(this);
            var refreshEvent = $.Event("refresh.mvctable");
            $that.trigger(refreshEvent);
            if (refreshEvent.isDefaultPrevented())
                return;
            params = params || {};
            params.RenderTable = params.RenderTable || true;
            params.RenderPager = params.RenderPager || true;
            var qs = initQueryString(params);
            var url = $that.data('source') + qs;
            var id = $that.data('table-id');
            $.get(url, function(data) {
                removeHandlers.apply($that);
                var $newHtml = $(data);
                if ($newHtml.length == 0)
                    return;
                var paginator = $($newHtml[0]).is('.mvctable-paginator') ? $($newHtml[0]) : $($newHtml[1]);
                if (paginator.length) {
                    $('.mvctable-paginator').filter('[data-target="' + id + '"]').replaceWith(paginator);
                    if ($newHtml.length == 1)
                        return;
                }
                if ($($newHtml[0]).is('.mvctable-container')) {
                    var $table = $($newHtml[0]).find('table');
                    copyEvents($that, $table);
                    $that.replaceWith($table);
                    var refreshedEvent = $.Event("refreshed.mvctable");
                    $table.trigger(refreshedEvent);
                    methods.init.apply($table, [{}]);
                }
            });
        }
    };
    
    $.fn.mvctable = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.mvctable');
        }

    };

    $(function() {
        $('.mvctable').mvctable();
    });


    /* Client-side access to querystring name=value pairs
            Version 1.3
            28 May 2008
            
            License (Simplified BSD):
            http://adamv.com/dev/javascript/qslicense.txt
            See more at http://adamv.com/dev/javascript/querystring
    */
    function Querystring(qs) { // optionally pass a querystring to parse
        this.params = {};

        if (qs == null) qs = location.search.substring(1, location.search.length);
        if (qs.length == 0) return;

        // Turn <plus> back to <space>
        // See: http://www.w3.org/TR/REC-html40/interact/forms.html#h-17.13.4.1
        qs = qs.replace(/\+/g, ' ');
        var args = qs.split('&'); // parse out name/value pairs separated via &

        // split out each name=value pair
        for (var i = 0; i < args.length; i++) {
            var pair = args[i].split('=');
            var name = decodeURIComponent(pair[0]);

            var value = (pair.length == 2)
                    ? decodeURIComponent(pair[1])
                    : name;

            this.params[name] = value;
        }
    }

    Querystring.serialize = function(obj) {
        var qs = '';
        for (var p in obj) {
            qs += (p + '=' + obj[p] + '&');
        }
        return new Querystring(qs);
    };

    Querystring.prototype.get = function(key, default_) {
        var value = this.params[key];
        return (value != null) ? value : default_;
    };

    Querystring.prototype.contains = function(key) {
        var value = this.params[key];
        return (value != null);
    };
    
    Querystring.prototype.addOrReplace = function (key, value) {
        var me = this;
        var obj = {};
        obj = $.isPlainObject(key) ? key : (function () { obj[key] = value; return obj; })();
        $.each(obj, function (k, v) {
            me.params[k] = obj[k];
        });
    };
    
    Querystring.prototype.merge = function (otherQs) {
        var qs = new Querystring(this.toQueryString());
        qs.addOrReplace(otherQs.params);
        return qs;
    };
    
    Querystring.prototype.toQueryString = function() {
        var retval = "";
        $.each(this.params, function (i, v) {
            if(i && v)
                retval += i + "=" + v + "&";
        });
        return retval;
    };
    
    Querystring.prototype.deserialize = function () {
        return this.params;
    };


})(jQuery);

