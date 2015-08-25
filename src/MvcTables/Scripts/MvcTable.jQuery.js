(function ($, undefined) {

    var settingsKey = '_mvc_table_';
    var defaults = {};


    function copyAttributes(from, to) {
        var attributes = $(from).prop("attributes");

        // loop through <select> attributes and apply them on <div>
        $.each(attributes, function () {
            $(to).attr(this.name, this.value);
        });

    }

    function findTable() {
        var $that = $(this);
        var targetId = $that.attr('data-target');
        if (targetId) {
            return $('table.mvctable[data-table-id="' + targetId + '"]');
        }
        var tables = [];
        $('table.mvctable').each(function () {
            var filterClass = $(this).attr('data-filter');
            if ($that.hasClass(filterClass)) {
                tables.push($(this));
            }
        });

        return tables;
    }

    function escapeText(txt) {
        return $('<div/>').text(txt).html();
    }


    function initQueryString(params) {
        return params ? '?' + Querystring.serialize(params).toQueryString() : '';
    }

    function getMyState() {
        return new Querystring($(this).attr('data-source').split('?')[1]).deserialize();
    }

    function attachHandlers() {


        $('body').on('click.mvctable', 'a', function (e) {
            var $that = $(this);
            var tables = findTable.apply($that);
            $.each(tables, function (_, table) {
                e.preventDefault();
                var existingParams = getMyState.apply(table);
                var newVals = new Querystring($that.attr('href').split('?')[1]).deserialize();
                newVals = $.extend({}, existingParams, newVals);
                methods.refresh.apply(table, [newVals]);
            });
        });


        $('body').on('change.mvctable', 'input, select, textarea', function (e) {
            var $that = $(this);
            var tables = findTable.apply($that);
            $.each(tables, function (_, table) {
                e.preventDefault();
                var params = getMyState.apply(table);
                params[$that.attr('name')] = $that.val();
                params['PageNumber'] = 1;
                methods.refresh.apply(table, [params]);
            });
        });

        $('body').on('submit.mvctable', 'form', function (e) {
            var $that = $(this);
            var tables = findTable.apply($that);
            $.each(tables, function (_, table) {
                e.preventDefault();
                var existingParams = getMyState.apply(table);
                var params = new Querystring($that.serialize()).deserialize();
                params = $.extend({}, existingParams, params);
                methods.refresh.apply(table, [params]);

            });
        });
    }

    var methods = {
        init: function (settings) {
            return $(this).each(function (_, v) {
                var $that = $(this);
                var setts = $that.data(settingsKey);
                if (setts) return $that;
                settings = settings || {};
                setts = $.extend({}, defaults, settings);
                $that.data(settingsKey, setts);
            });
        },
        goToPage: function (page) {
            return $(this).each(function (_, v) {
                methods.refresh.apply(this, [{ PageNumber: page }]);
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
            $(this).first().find('tbody tr').each(function (i, r) {
                $(r).find('input,select,textarea').each(function (_, ip) {
                    var name = $(ip).attr('name');
                    if (name) {
                        result += ((name + '=' + escapeText($(ip).val())) + '&');
                    }
                });
            });
            return result;
        },
        validate: function () {
            if (typeof $.fn.valid === 'function') {
                var form = $(this).parents('form');
                if (form.length) {
                    return $(this).parents('form').valid();
                }
            }
        },
        save: function (url, ok, err) {
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
                    var form = $that.parents('form');
                    if (form.length) {
                        form.data('validator').resetForm();
                    }
                    var savedEvent = $.Event("saved.mvctable");
                    $that.trigger(savedEvent);
                    if (savedEvent.isDefaultPrevented())
                        return;
                    ok.apply($that, arguments);
                }
            });
        },
        refreshTable: function (params) {
            params = params || {};
            params.RenderTable = true;
            params.RenderPager = false;
        },
        refreshPaginator: function (params) {
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
            params.RenderPageSize = params.RenderPageSize || true;
            var qs = initQueryString(params);
            var url = $that.data('source').split('?')[0] + qs;
            var id = $that.data('table-id');
            $.get(url, function (data) {

                var $newHtml = $(data);
                if ($newHtml.length == 0)
                    return;


                var paginator = $newHtml.find('.mvctable-paginator');
                if (paginator.length) {
                    $('.mvctable-paginator').filter('[data-target="' + id + '"]').replaceWith(paginator);
                }

                var pageSize = $newHtml.find('.mvc-table-page-size');
                if (pageSize.length) {
                    $('.mvc-table-page-size').filter('[data-target="' + id + '"]').replaceWith(pageSize);
                }

                var tableContainer = $newHtml.find('.mvctable-container');
                if (tableContainer.length) {
                    var $table = tableContainer.find('table');
                    $($that).html($table.html());
                    copyAttributes($table, $that);
                    var refreshedEvent = $.Event("refreshed.mvctable");
                    $that.trigger(refreshedEvent);
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

    $(function () {
        attachHandlers();
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

    Querystring.serialize = function (obj) {
        var qs = '';
        for (var p in obj) {
            qs += (p + '=' + obj[p] + '&');
        }
        return new Querystring(qs);
    };

    Querystring.prototype.get = function (key, default_) {
        var value = this.params[key];
        return (value != null) ? value : default_;
    };

    Querystring.prototype.contains = function (key) {
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

    Querystring.prototype.toQueryString = function () {
        var retval = "";
        $.each(this.params, function (i, v) {
            if (i && v)
                retval += i + "=" + v + "&";
        });
        return retval;
    };

    Querystring.prototype.deserialize = function () {
        return this.params;
    };


})(jQuery);