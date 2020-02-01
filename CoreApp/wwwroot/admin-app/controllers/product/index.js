var productController = function () {

    let quantityManagement = new QuantityManagement();
    let imageManagement = new ImageManagement();
    let wholePriceManagement = new WholePriceManagement();

    this.initialize = function () {
        loadCategories();
        loadData();
        registerEvents();
        registerControls();
        quantityManagement.initialize();
        imageManagement.initialize();
        wholePriceManagement.initialize();
        initialTags([]);
    };

    function registerEvents() {

        $('#ddlShowPage').on('change', function () {
            appcore.configs.pageSize = $(this).val();
            appcore.configs.pageIndex = 1;
            loadData(true);
        });

        $('#btnSearch').on('click', function () {
            loadData(true);
        });

        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadData(true);
            }
        });

        //Init validation
        $('#frmMaintainance').validate({
            errorClass: MasterClass.errorValidate,
            ignore: [],
            rules: {
                txtNameM: { required: true },
                ddlCategoryIdM: { required: true },
                txtPriceM: {
                    required: true,
                    number: true
                }
            }
        });

        $("#btnCreate").on('click', function () {
            setTimeout(function () {
                resetFormMaintainance();
                initTreeDropDownCategory();
                $('#modal-add-edit').modal('show');
            }, 1000);
           
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            appcore.startLoading();
            const urlGetById = '/Admin/Product/GetById';
            service.getData({
                url: urlGetById,
                dataType: DataType.json,
                data: {
                    id: id
                }
            })
                .done(response => {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    initTreeDropDownCategory(data.CategoryId);
                    $('#txtDescM').val(data.Description);
                    $('#txtUnitM').val(data.Unit);

                    $('#txtPriceM').val(data.Price);
                    $('#txtOriginalPriceM').val(data.OriginalPrice);
                    $('#txtPromotionPriceM').val(data.PromotionPrice);
                    $('#txtImage').val(data.Image);
                    renderProductImage(data.Image);

                    $('#txtTagM').val(data.Tags);
                    $('#txtMetakeywordM').val(data.SeoKeywords);
                    $('#txtMetaDescriptionM').val(data.SeoDescription);
                    $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                    $('#txtSeoAliasM').val(data.SeoAlias);

                    CKEDITOR.instances.txtContent.setData(data.Content);
                    $('#ckStatusM').prop('checked', data.Status === 1);
                    $('#ckHotM').prop('checked', data.HotFlag);
                    $('#ckShowHomeM').prop('checked', data.HomeFlag);
                    initialTags([]);
                    if (data.Tags !== null) {
                        const srcTags = data.Tags.split(',');
                        initialTags(srcTags);
                    }
                    $('#txtTagM').val(data.Tags);
                    $('#modal-add-edit').modal('show');
                    appcore.stopLoading();
                })
                .fail(status => {
                    appcore.Notify('Error not get data when editing', Notify.danger);
                    appcore.stopLoading();
                });
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            appcore.confirm('Are you sure to delete?', function () {
                const urlDelete = '/Admin/Product/Delete';
                service.postData({
                    url: urlDelete,
                    dataType: DataType.json,
                    data: { id: id },
                    beforeSendCallback: () => {
                        appcore.startLoading();
                    }
                })
                    .done(() => {
                        appcore.notify('Delete successful', 'success');
                        appcore.stopLoading();
                        loadData();
                    })
                    .fail(() => {
                        appcore.notify('Has an error in delete progress', 'error');
                        appcore.stopLoading();
                    });
            });
        });

        $('body').on('click', '.btn-close-pop', function (e) {
            e.preventDefault();
            $('#frmMaintainance').validate().resetForm();
            $('#modal-add-edit').modal('hide');
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                const id = $('#hidIdM').val();
                const name = $('#txtNameM').val();
                const categoryId = $('#ddlCategoryIdM').combotree('getValue');
                const description = $('#txtDescM').val();

                const unit = '1';
                const price = 1;
                const originalPrice = 1;
                const promotionPrice = 1;

                const image = $('#txtImage').val();
                const srctags = $('#txtTagM').tagEditor('getTags')[0].tags;
                const tags = srctags.join();
                const seoKeyword = $('#txtMetakeywordM').val();
                const seoMetaDescription = $('#txtMetaDescriptionM').val();
                const seoPageTitle = $('#txtSeoPageTitleM').val();
                const seoAlias = $('#txtSeoAliasM').val();
                const content = CKEDITOR.instances.txtContent.getData();
                const status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
                const hot = $('#ckHotM').prop('checked');
                const showHome = $('#ckShowHomeM').prop('checked');
                const urlSaveEntity = '/Admin/Product/SaveEntity';
                service.postData({
                    url: urlSaveEntity,
                    dataType: DataType.json,
                    data: {
                        Id: id,
                        Name: name,
                        CategoryId: categoryId,
                        Image: image,
                        Price: price,
                        OriginalPrice: originalPrice,
                        PromotionPrice: promotionPrice,
                        Description: description,
                        Content: content,
                        HomeFlag: showHome,
                        HotFlag: hot,
                        Tags: tags,
                        Unit: unit,
                        Status: status,
                        SeoPageTitle: seoPageTitle,
                        SeoAlias: seoAlias,
                        SeoKeywords: seoKeyword,
                        SeoDescription: seoMetaDescription
                    },
                    beforeSendCallback: () => {
                        appcore.startLoading();
                    }
                })
                    .done(response => {
                        appcore.notify('Update product successful', Notify.success);
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();
                        appcore.stopLoading();
                        loadData(true);
                    })
                    .fail(() => {
                        appcore.notify('Has an error in save product progress', Notify.danger);
                        appcore.stopLoading();
                    });
                return false;
            }

        });

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $('#fileInputImage').on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            const urlUploadImage = '/Admin/Upload/UploadImage';
            service.uploadFile({
                url: urlUploadImage,
                dataType: 'html',
                data: data,
                contentType: false,
                processData: false
            })
            .done(path => {
                $('#txtImage').val(path);
                renderProductImage(path);
            })
            .fail(() => {
                appcore.notify('There was error uploading files!', 'error');
            });

        });

        $('body').on('click','.icon-image-close', function () {
            const pathImage = $(this).data('path');
            $('#txtImage').val('');
            $('#img-prouduct-content').html('');
        });

        $('#btn-import').on('click', function () {
            initTreeDropDownCategory();
            $('#modal-import-excel').modal('show');
        });

        $('#btnImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            // Adding one more key to FormData object  
            fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));
            const urlImportExcel = '/Admin/Product/ImportExcel';
            service.uploadFile({
                url: urlImportExcel,
                dataType: 'html',
                data: fileData,
                contentType: false,
                processData: false
            })
                .done(path => {
                    $('#modal-import-excel').modal('hide');
                    loadData();
                });
            return false;
        });

        $('#btn-export').on('click', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Product/ExportExcel",
                beforeSend: function () {
                    appcore.startLoading();
                },
                success: function (response) {
                    window.location.href = response;
                    appcore.stopLoading();
                },
                error: function () {
                    appcore.notify('Has an error in progress', 'error');
                    appcore.stopLoading();
                }
            });
        });
    }

    function loadCategories() {
        appcore.startLoading();
        let urlGetCategories = '/admin/product/GetAllCategories';
        service.getData({ url: urlGetCategories, dataType: DataType.json })
            .done(response => {
                let render = "<option value=''>--Select category--</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
                $('#ddlCategorySearch').html(render);
            })
            .fail(status => {
                appcore.notify('Cannot loading product category data', Notify.error);
            });
    }

    function loadData(isPageChanged) {
        let template = $('#table-template').html();
        let render = "";
        const urlGetAllPaging = '/admin/product/GetAllPaging';
        service.getData({
            url: urlGetAllPaging,
            dataType: DataType.json,
            data: {
                categoryId: $('#ddlCategorySearch').val(),
                keyword: $('#txtKeyword').val(),
                page: appcore.configs.pageIndex,
                pageSize: appcore.configs.pageSize
            }
        })
            .done(response => {
                $.each(response.Results,
                    function (i, item) {
                        render += Mustache.render(template,
                            {
                                Id: item.Id,
                                Name: item.Name,
                                Image: item.Image === null
                                    ? '<img src="' + ImageAppCore.productDefault + '"width=25'
                                    : '<img src="' + item.Image + '" width=25 />',
                                CategoryName: item.ProductCategory.Name,
                                Price: appcore.formatNumber(item.Price, 0),
                                CreatedDate: appcore.dateTimeFormatJson(item.DateCreated),
                                Status: appcore.getStatus(item.Status)
                            });


                    });
                $('#tbl-content').html(render);
                $('#lblTotalRecords').text(response.RowCount);
                wrapPaging(response.RowCount, function () {
                    loadData();
                }, isPageChanged);
                appcore.stopLoading();
            })
            .fail(status => {
                appcore.notify(Message.ajaxFail, Notify.danger);
                appcore.stopLoading();
            });
    }

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalSize = Math.ceil(recordCount / appcore.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        if (totalSize !== 0) {
            //Bind Pagination Event
            $('#paginationUL').twbsPagination({
                totalPages: totalSize,
                visiblePages: 7,
                first: 'First',
                prev: 'Previous',
                next: 'Next',
                last: 'Last',
                onPageClick: function (event, p) {
                    appcore.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            });
        }
    }

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');

        $('#txtDescM').val('');
        $('#txtUnitM').val('');

        $('#txtPriceM').val('0');
        $('#txtOriginalPriceM').val('');
        $('#txtPromotionPriceM').val('');

        $('#txtImage').val('');
        $('#img-prouduct-content').html('');

        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');

        CKEDITOR.instances.txtContent.setData('');
        $('#ckStatusM').prop('checked', true);
        $('#ckHotM').prop('checked', false);
        $('#ckShowHomeM').prop('checked', false);
        initialTags([]);

    }

    function initTreeDropDownCategory(selectedId) {
        const urlGetAllCategories = '/Admin/ProductCategory/GetAll';
        service.getData({ url: urlGetAllCategories, dataType: DataType.json })
            .done(response => {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = appcore.unflattern(data);
                $('#ddlCategoryIdM').combotree({
                    data: arr
                });
                $('#ddlCategoryIdImportExcel').combotree({
                    data: arr
                });
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            })
            .fail(status => { });
    }

    function registerControls() {
        CKEDITOR.replace('txtContent', {});
        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };

    }

    function renderProductImage(src) {
        let tmplImageProduct = $('#img-product-template').html();
        let isNullOrEmpty = src === '' || src === null;
        let srcImage = isNullOrEmpty ? Image.productDefault : src;
        let render = Mustache.render(tmplImageProduct, { srcImage: srcImage });
        $('#img-prouduct-content').html(render);
    }

    function initialTags(src) {
        $('#txtTagM').tagEditor('destroy');
        $('#txtTagM').tagEditor({
            initialTags: src,
            delimiter: ',', /* space and comma */
            placeholder: 'Nhập các tags ...'
        });
    }

};

let product = new productController();
product.initialize();