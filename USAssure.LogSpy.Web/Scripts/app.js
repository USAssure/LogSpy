//jquery dependency
var app = (function () {
    var getHours = function () {
        return $('#date-filter').val();
    };

    var getAppName = function () {
        return $('#app-bar li.active > a.app-filter').data('app-name');
    };

    var getEnvironment = function () {
        return $('div#environment-selector label.active > input:radio').attr('id');
    };

    var getMachineName = function () {
        return null;
    };

    var getQuery = function () {
        return $('#search-text').val();
    }

    var retrieveLogs = function (url, hours, appName, machineName, environment, query) {
        $.get(url, { environment: environment, appName: appName, machineName: machineName, hours: hours, query: query }, function (data) {
            $('#main-content').html(data);
        });
    };

    var wireUpDateFilterDropDown = function () {
        $('#date-filter').change(function () {
            retrieveLogs($(this).data('url'), $(this).val(), getAppName(), getMachineName(), getEnvironment(), getQuery());
        });
    };

    var wireUpAppBar = function () {
        $('#main-content').on('click', '#app-bar a.app-filter', function () {
            var item = $(this);
            $('#app-bar li').removeClass('active');
            $(item).parent().addClass('active');
            $.get($(item).data('url'), { environment: getEnvironment(), appName: $(item).data('app-name'), hours: getHours(), query: getQuery() }, function (data) {
                $('#main-content').html(data);
            });
        });
    };

    var wireUpLogTable = function() {
        $('#main-content').on('click', '#log-table tr.log-item-row', function () {
            var item = $(this);
            $.get($(item).data('url'), { environment: getEnvironment(), id: $(item).data('id') }, function (data) {
                $('#log-id').html('Log: ' + $(item).data('id'));
                $('.modal-body').html(data);
                $('.modal').modal('show');
            });
        });
    };

    var wireUpSearchBar = function() {
        $('#search-text').keyup(function () {
            var length = $(this).val().length;
            var searchText = $(this);
            if (length >= 2) {
                $.get($(searchText).data('url'), { environment: getEnvironment(), appName: getAppName(), query: $(searchText).val(), hours: getHours()}, function (data) {
                    $('#main-content').html(data);
                });
            } else {
                $.get($('#main-content').data('url'), { environment: getEnvironment(), appName: getAppName(), hours: getHours() }, function (data) {
                    $('#main-content').html(data);
                });
            }
        });
    };

    var wireUpEnvironmentSelector = function () {
        $('#environment-selector').on('click', 'label.btn', function () {
            var environment = $(this).children().first().attr('id');
            retrieveLogs($('#main-content').data('url'), getHours(), null, null, environment, null);
        });
    }

    var init = function () {
        //initialize content
        $.get($('#environment-selector').data('url'), function (data) {
            $('#environment-selector').html(data);
        });

        $.get($('#main-content').data('url'), { hours: getHours() }, function (data) {
            $('#main-content').html(data);
        });

        //wire up events
        wireUpAppBar();
        wireUpLogTable();
        wireUpSearchBar();
        wireUpDateFilterDropDown();
        wireUpEnvironmentSelector();
    };

    return {
        init: init
    }
})();

$(function () {
    app.init();
    console.log('init');
});