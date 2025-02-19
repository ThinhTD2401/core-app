﻿var CartController = function () {
    var cachedObj = {
        colors: [],
        sizes: []
    };

    this.initialize = function () {
        $.when(loadColors(),
            loadSizes())
            .then(function () {
                loadData();
            });

        registerEvents();
    };

    function registerEvents() {

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productId: id
                },
                success: function () {
                    appcore.notify(Message.removeCartsSuccess, Notify.success);
                    loadHeaderCart();
                    loadData();
                }
            });
        });

        $('body').on('keyup', '.txtQuantity', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var q = $(this).val();
            if (q > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q
                    },
                    success: function () {
                        appcore.notify(Message.updateQuantitySuccess, Notify.success);
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                appcore.notify(Message.updateQuantityFail, Notify.danger);
            }

        });

        $('body').on('change', '.ddlColorId', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var colorId = $(this).val();
            var q = $(this).closest('tr').find('.txtQuantity').first().val();
            var sizeId = $(this).closest('tr').find('.ddlSizeId').first().val();

            if (colorId > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q,
                        color: colorId,
                        size: sizeId
                    },
                    success: function () {
                        appcore.notify(Message.updateColorSuccess, Notify.success);
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                appcore.notify(Message.updateColorFail, Notify.danger);
            }

        });

        $('body').on('change', '.ddlSizeId', function (e) {
            e.preventDefault();
            var id = parseInt($(this).closest('tr').data('id'));
            var sizeId = $(this).val();
            var q = parseInt($(this).closest('tr').find('.txtQuantity').first().val());
            var colorId = parseInt($(this).closest('tr').find('.ddlColorId').first().val());
            if (sizeId > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q,
                        color: colorId,
                        size: sizeId
                    },
                    success: function () {
                        appcore.notify(Message.updateSizeSuccess, Notify.success);
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                appcore.notify(Message.updateColorFail, Notify.danger);
            }

        });

        $('#btnClearAll').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Cart/ClearCart',
                type: 'post',
                success: function () {
                    appcore.notify(Message.removeCartsSuccess, Notify.success);
                    loadHeaderCart();
                    loadData();
                }
            });
        });
    }
    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetColors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                appcore.notify(Message.ajaxFail, Notify.danger);
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetSizes",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                appcore.notify(Message.ajaxFail, Notify.danger);
            }
        });
    }

    function getColorOptions(selectedId) {
        var colors = `<select class='form-control ddlColorId'>`;
        $.each(cachedObj.colors, function (i, color) {
            if (selectedId === color.Id)
                colors += '<option value="' + color.Id + '" selected="select">' + color.Name + '</option>';
            else
                colors += '<option value="' + color.Id + '">' + color.Name + '</option>';
        });
        colors += "</select>";
        return colors;
    }

    function getSizeOptions(selectedId) {
        var sizes = `<select class='form-control ddlSizeId'>`;
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === size.Id)
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Name + '</option>';
            else
                sizes += '<option value="' + size.Id + '">' + size.Name + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }

    function loadData() {
        $.ajax({
            url: '/Cart/GetCart',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var template = $('#template-cart').html();
                var render = "";
                var totalAmount = 0;
                $.each(response, function (i, item) {
                    render += Mustache.render(template,
                        {
                            ProductId: item.Product.Id,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: appcore.formatNumber(item.Price, 0),
                            Quantity: item.Quantity,
                            Colors: getColorOptions(item.Color === null ? 0 : item.Color.Id),
                            Sizes: getSizeOptions(item.Size === null ? "" : item.Size.Id),
                            Amount: appcore.formatNumber(item.Price * item.Quantity, 0),
                            Url: '/' + item.Product.SeoAlias + "-p." + item.Product.Id + ".html"
                        });
                    totalAmount += item.Price * item.Quantity;
                });
                $('#lblTotalAmount').text(appcore.formatNumber(totalAmount, 0));
                if (render !== "")
                    $('#table-cart-content').html(render);
                else
                    $('#table-cart-content').html(
                       `< tr >
                        <td colspan="6">
                            <p class="text-center">${Message.cartNoItem}</p>
                        </td>
                        </tr >`
                    );
            }
        });
        return false;
    }
};

var cartController = new CartController();
cartController.initialize();