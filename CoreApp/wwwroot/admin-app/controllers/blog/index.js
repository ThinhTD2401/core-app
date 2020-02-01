var BlogController = function () {

    this.initialize = function () {
        registerEvents();
        registerControls();
        loadData();
        initialTags([]);
    };

    function registerEvents() {
        $("#btnCreate").on('click', function () {
            setTimeout(function () {
                resetFormMaintainance();
                $('#modal-add-edit').modal('show');
            }, 1000);
        });

        $('#btnSave').on('click', function (e) {
            e.preventDefault();
            saveData();
        });

        //Init validation
        $('#frmMaintainance').validate({
            errorClass: MasterClass.errorValidate,
            ignore: [],
            rules: {
                txtNameM: { required: true }
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
            $(this).val(null);
            service.uploadFile({
                url: urlUploadImage,
                dataType: 'html',
                data: data,
                contentType: false,
                processData: false
            })
                .done(path => {
                    $('#txtImage').val(path);
                    renderBlogImage(path);
                    appcore.notify(Message.uploadFileSuccess, Notify.success);
                })
                .fail(() => {
                    appcore.notify(Message.uploadFileFail, Notify.danger);
                });

        });

        $('body').on('click', '.icon-image-close', function () {
            const pathImage = $(this).data('path');
            $('#txtImage').val('');
            $('#img-blog-content').html('');
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            appcore.startLoading();
            const urlGetById = '/Admin/Blog/GetById';
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
                    $('#txtDescM').val(data.Description);
                    $('#txtImage').val(data.Image);
                    if (data.Image === null || data.Image === '') {
                        $('#img-blog-content').html('');
                    } else {
                        renderBlogImage(data.Image);
                    }
                    $('#hidImage').val(data.Image);
                    if (data.Tags !== null) {
                        const srcTags = data.Tags.split(',');
                        initialTags(srcTags);
                    }
                    $('#txtTagM').val(data.Tags);
                    $('#txtMetakeywordM').val(data.SeoKeywords);
                    $('#txtMetaDescriptionM').val(data.SeoDescription);
                    $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                    $('#txtSeoAliasM').val(data.SeoAlias);

                    CKEDITOR.instances.txtContent.setData(data.Content);
                    $('#ckStatusM').prop('checked', data.Status === 1);
                    $('#ckHotM').prop('checked', data.HotFlag);
                    $('#ckShowHomeM').prop('checked', data.HomeFlag);

                    $('#modal-add-edit').modal('show');
                    appcore.stopLoading();
                })
                .fail(status => {
                    appcore.Notify(Message.getDataFail, Notify.danger);
                    appcore.stopLoading();
                });
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            appcore.confirm('Are you sure to delete?', function () {
                const urlDelete = '/Admin/Blog/Delete';
                service.postData({
                    url: urlDelete,
                    dataType: DataType.json,
                    data: { id: id },
                    beforeSendCallback: () => {
                        appcore.startLoading();
                    }
                })
                    .done(() => {
                        appcore.notify(Message.deleteSuccess, Notify.success);
                        appcore.stopLoading();
                        loadData();
                    })
                    .fail(() => {
                        appcore.notify(Message.deleteFail, Notify.error);
                        appcore.stopLoading();
                    });
            });
        });

        $('body').on('click', '.btn-close-pop', function (e) {
            e.preventDefault();
            $('#frmMaintainance').validate().resetForm();
            $('#modal-add-edit').modal('hide');
        });

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
    }

    function renderBlogImage(src) {
        let tmplImageBlog = $('#img-blog-template').html();
        let render = Mustache.render(tmplImageBlog, { srcImage: src });
        $('#img-blog-content').html(render);
    }

    function saveData() {
        if ($('#frmMaintainance').valid()) {
            var d = new Date,
                dformat = [d.getFullYear() + 1,
                d.getMonth(),
                d.getDate()].join('-');
            const id = $('#hidIdM').val();
            const name = $('#txtNameM').val();
            const description = $('#txtDescM').val();
            const createDate = dformat;
            const image = $('#txtImage').val();
            if (image === null || image === '') {
                deleteImage($('#hidImage').val());
            }
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
            const urlSaveEntity = '/Admin/Blog/SaveEntity';
            service.postData({
                url: urlSaveEntity,
                dataType: DataType.json,
                data: {
                    Id: id,
                    Name: name,
                    Image: image,
                    DateCreated: createDate,
                    Description: description,
                    HomeFlag: showHome,
                    HotFlag: hot,
                    Tags: tags,
                    Content: content,
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
                    appcore.notify(Message.updateSuccess, Notify.success);
                    $('#modal-add-edit').modal('hide');
                    resetFormMaintainance();
                    appcore.stopLoading();
                    loadData(true);
                })
                .fail(() => {
                    appcore.notify(Message.updateFail, Notify.danger);
                    appcore.stopLoading();
                });
            return false;
        }
    }

    function deleteImage(path) {
        service.postData({
            url: '/Admin/Upload/DeleteFile', dataType: DataType.json, data: { path: path }
        });
    }

    function loadData(isPageChanged) {
        appcore.startLoading();
        let template = $('#table-template').html();
        let render = "";
        const urlGetAllPaging = '/admin/blog/GetAllPaging';
        service.getData({
            url: urlGetAllPaging,
            dataType: DataType.json,
            data: {
                keyword: $('#txtKeyword').val(),
                pageIndex: appcore.configs.pageIndex,
                pageSize: appcore.configs.pageSize
            }
        })
            .done(response => {
                if (response.Results.length === 0) {
                    $('#tbl-content').html(`<tr><td colspan="5" style="text-align: center">Không có dữ liệu hiển thị</td><tr>`);
                } else {
                    $.each(response.Results,
                        function (i, item) {
                            render += Mustache.render(template,
                                {
                                    Id: item.Id,
                                    Name: item.Name,
                                    Image: item.Image === null
                                        ? '<img src="' + ImageAppCore.productDefault + '"width=25'
                                        : '<img src="' + item.Image + '" width=25 />',
                                    CreatedDate: appcore.dateTimeFormatJson(item.DateCreated),
                                    Status: appcore.getStatus(item.Status)
                                });
                        });
                    $('#tbl-content').html(render);
                }
               
                $('#lblTotalRecords').text(response.RowCount);
                wrapPaging(response.RowCount, function () {
                    loadData();
                }, isPageChanged);
                appcore.stopLoading();
            })
            .fail(status => {
                appcore.notify(Message.getDataFail, Notify.danger);
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
                visiblePages: 2,
                first: '<span> <i class="fa fa-angle-double-left"></i> </span>',
                prev: '<span> <i class="fa fa-angle-left"></i></i></span>',
                next: '<span>  <i class="fa fa-angle-right"></i></i></span>',
                last: '<span> <i class="fa fa-angle-double-right"></i></span>',
                onPageClick: function (event, p) {
                    appcore.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            });
        }
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

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        $('#txtDescM').val('');
        $('#txtImage').val('');
        $('#img-blog-content').html('');
        $('#txtContent').val('0');

        $('#txtMetakeywordM').val('');
        $('#txtSeoAliasM').val('');
        $('#txtMetaDescriptionM').val('');
        initialTags([]);
        CKEDITOR.instances.txtContent.setData('');

        $('#ckStatusM').prop('checked', true);
        $('#ckShowHomeM').prop('checked', false);

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

var blogController = new BlogController();
blogController.initialize();
