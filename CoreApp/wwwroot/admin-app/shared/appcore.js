var appcore = {
    configs: {
        pageSize: 10,
        pageIndex: 1
    },

    notify: function (message, type) {

        $.notify({
            // options
            icon: '',
            title: '',
            message: message,
            url: '',
            target: '_blank'
        },{
                // settings
                element: 'body',
                position: null,
                type: type,
                allow_dismiss: true,
                newest_on_top: false,
                showProgressbar: false,
                placement: {
                    from: "top",
                    align: "center"
                },
                offset: 20,
                spacing: 10,
                z_index: 1052,
                delay: 2000,
                timer: 1000,
                url_target: '_blank',
                mouse_over: null,
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },
                onShow: null,
                onShown: null,
                onClose: null,
                onClosed: null,
                icon_type: 'class',
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="icon"></span> ' +
                    '<span data-notify="title">{1}</span> ' +
                    '<span data-notify="message">{2}</span>' +
                    '<div class="progress" data-notify="progressbar">' +
                    '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                    '</div>' +
                    '<a href="{3}" target="{4}" data-notify="url"></a>' +
                    '</div>'
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
        if ($('.wrapper-loader').length > 0)
            $('.wrapper-loader').removeClass('hidden-loader');
    },

    stopLoading: function () {
        if ($('.wrapper-loader').length > 0)
            $('.wrapper-loader')
                .addClass('hidden-loader');
        document.body.scrollTop = 0; // For Safari
        document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
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