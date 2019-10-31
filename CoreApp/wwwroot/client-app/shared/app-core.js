var appcore = {
    configs: {
        pageSize: 10,
        pageIndex: 1
    },
    notify: function (message, type) {
        $.notify(message, {
            // whether to hide the notification on click
            clickToHide: true,
            // whether to auto-hide the notification
            autoHide: true,
            // if autoHide, hide after milliseconds
            autoHideDelay: 5000,
            // show the arrow pointing at the element
            arrowShow: true,
            // arrow size in pixels
            arrowSize: 5,
            // position defines the notification position though uses the defaults below
            //position: '...',
            //// default positions
            //elementPosition: 'top right',
            globalPosition: 'top right',
            // default style
            style: 'bootstrap',
            // default class (string or [string])
            className: type,
            // show animation
            showAnimation: 'slideDown',
            // show animation duration
            showDuration: 400,
            // hide animation
            hideAnimation: 'slideUp',
            // hide animation duration
            hideDuration: 200,
            // padding between element and notification
            gap: 2
        });
    },
    confirm: function (message, okCallback) {
        bootbox.confirm({
            message: message,
            buttons: {
                confirm: {
                    label: 'OK',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Cancel',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result === true) {
                    okCallback();
                }
            }
        });
    },

    dateFormatJson: function (datetime) {
        if (datetime === null || datetime === '')
            return '';
        return moment(datetime).format('DD-MM-YYYY');
    },

    dateTimeFormatJson: function (datetime) {
        if (datetime === null || datetime === '')
            return '';
        return moment(datetime).format('DD-MM-YYYY HH:MM:SS');
    },

    startLoading: function () {
        if ($('.loader').length > 0)
            $('.loader').removeClass('hide-loader');
    },

    stopLoading: function () {
        if ($('.loader').length > 0)
            $('.loader')
                .addClass('hide-loader');
    },

    getStatus: function (status) {
        if (status === 1)
            return '<span class="badge bg-green">Kích hoạt</span>';
        else
            return '<span class="badge bg-red">Khoá</span>';
    },

    formatNumber: function (number, precision) {
        if (!isFinite(number)) {
            return number.toString();
        }

        var a = number.toFixed(precision).split('.');
        a[0] = a[0].replace(/\d(?=(\d{3})+$)/g, '$&,');
        return a.join('.');
    },

    unflattern: function (arr) {
        var map = {};
        var roots = [];
        for (var i = 0; i < arr.length; i += 1) {
            var node = arr[i];
            node.children = [];
            map[node.id] = i; // use map to look-up the parents
            if (node.parentId !== null) {
                // find parent
                let parent = arr.find(x => x.id === node.parentId);

                if (parent) {
                    arr[map[node.parentId]].children.push(node);
                    arr[map[node.parentId]].children.sort(function (a, b) {
                        return a.sortOrder - b.sortOrder;
                    });
                }
            } else {
                roots.push(node);
            }
        }
        return roots;
    }
};

$(document).ajaxSend(function (e, xhr, options) {
    if (options.type.toUpperCase() === "POST" || options.type.toUpperCase() === "PUT") {
        var token = $('form').find("input[name='__RequestVerificationToken']").val();
        xhr.setRequestHeader("RequestVerificationToken", token);
    }
});