var BaseController = function () {

    this.initialize = function () {
        registerEvents();
    };

    function registerEvents() {

        //$('body').on('click', '.add-to-cart', function (e) {
        //    appcore.startLoading();
        //    e.preventDefault();
        //    var id = $(this).data('id');
        //    $.ajax({
        //        url: '/Cart/AddToCart',
        //        type: 'post',
        //        data: {
        //            productId: id,
        //            quantity: 1,
        //            color: 0,
        //            size: 0
        //        },
        //        success: function (response) {
        //            appcore.notify(Message.addItemSuccess, Notify.success);
        //            loadHeaderCart();
        //        },
        //        error: function () {
        //            appcore.stopLoading();
        //        }
        //    });
        //});

        $('body').on('click', '.remove-cart', function (e) {
            appcore.startLoading();
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productId: id
                },
                success: function (response) {
                    appcore.notify(Message.removeItemSuccess, Notify.success);
                    loadHeaderCart();
                },
                error: function () {
                    appcore.stopLoading();
                }
            });
        });

        $(document).on('click', '#pager li', function () {
            appcore.startLoading();
            const pageSizeVal = $('#PageSize').val();
            const sortByVal = $('#SortBy').val();
            const sortOrderVal = $('#SortOrder').val();
            let pageSize = !pageSizeVal ? pageSizeVal : PagerDefault.pageSize;
            let sortBy = !sortByVal ? sortByVal : PagerDefault.sortBy;
            let sortOrder = !sortOrderVal ? sortOrderVal : PagerDefault.sortOrder;
            let keyword = PagerDefault.keyword;
            let pageIndex = this.firstElementChild.getAttribute("data-id");
            loadSearchComponent(keyword, pageSize, pageIndex, sortBy, sortOrder);
        });

        $(document).on('change', '.short-by select', function () {
            appcore.startLoading();
            const fieldName = $(this).attr('id');
            const valueField = this.value;
            let pageSize = PagerDefault.pageSize;
            let pageIndex = PagerDefault.pageIndex;
            let sortBy = PagerDefault.sortBy;
            let sortOrder = PagerDefault.sortOrder;
            let keyword = PagerDefault.keyword;
            switch (fieldName) {
                case 'PageSize':
                    pageSize = valueField;
                    break;
                case 'SortOrder':
                    sortOrder = valueField;
                    break;
                default:
                    sortBy = valueField;
                    break;
            }
            loadSearchComponent(keyword, pageSize, pageIndex, sortBy, sortOrder);
        });
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart", function (responseTxt, statusTxt, xhr) {
            appcore.stopLoading();
        });
    }

    function loadSearchComponent(keyword, pageSize, pageIndex, sortBy, sortOrder) {
        $("#search-component").load("/AjaxContent/SearchComponent",
            { keyword: keyword, pageSize: pageSize, sortBy: sortBy, sortOrder: sortOrder, pageIndex: pageIndex },
            function (responseTxt, statusTxt, xhr) {
                $('#PageSize').val(pageSize);
                $('#SortBy').val(sortBy);
                $('#SortOrder').val(sortOrder);
                appcore.stopLoading();
            });
    }
};

var baseObj = new BaseController();
baseObj.initialize();