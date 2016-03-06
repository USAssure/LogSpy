//jquery dependency
var app = (function () {
    var wireUpAppBar = function() {
        $('#app-bar').on('click', 'a.app-filter', function () {
            var item = $(this);
            $('#app-bar li').removeClass('active');
            $(item).parent().addClass('active');
            $.get($(item).data('url'), { environment: $('div#environment-selector label.active > input:radio').attr('id'), appName: $(item).data('app-name') }, function (data) {
                $('#log-table').html(data);
            });
        });
    };

    var wireUpLogTable = function() {
        $('#log-table').on('click', 'tr.log-item-row', function () {
            var item = $(this);
            $.get($(item).data('url'), { environment: $('div#environment-selector label.active > input:radio').attr('id'), id: $(item).data('id') }, function (data) {
                $('.modal-body').html(data);
                $('.modal').modal('show');
            });
        });
    };

    var wireUpSearchBar = function() {
        $('#search-text').keyup(function () {
            var length = $(this).val().length;
            var searchText = $(this);
            var app = $('#app-bar li.active > a.app-filter').data('app-name');
            if (length >= 2) {
                $.get($(searchText).data('url'), { environment: $('div#environment-selector label.active > input:radio').attr('id'), appName: $(app).data('app-name'), query: $(searchText).val() }, function (data) {
                    $('#log-table').html(data);
                });
            } else {
                $.get($('#log-table').data('url'), function (data) {
                    $('#log-table').html(data);
                });
            }
        });
    };

    var init = function () {
        $.get($('#environment-selector').data('url'), function (data) {
            $('#environment-selector').html(data);
        });

        $.get($('#log-table').data('url'), function (data) {
            $('#log-table').html(data);
        });

        $.get($('#error-count').data('url'), function (data) {
            $('#error-count').html(data);
        });

        $.get($('#app-bar').data('url'), function (data) {
            $('#app-bar').html(data);
        });

        wireUpAppBar();
        wireUpLogTable();
        wireUpSearchBar();
    };

    return {
        init: init
    }
})();

$(function () {
    app.init();
});