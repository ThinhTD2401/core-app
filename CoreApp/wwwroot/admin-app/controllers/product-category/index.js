var productCategoryController = function () {

    this.initialize = function () {
        loadData();
        registerEvents();
        appcore.stopLoading();

    };

    function registerEvents() {

        $('#frmMaintainance').validate({
            errorClass: MasterClass.errorValidate,
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true },
                txtOrderM: { number: true },
                txtHomeOrderM: { number: true }
            }
        });

        $('#btnCreate').off('click').on('click', function () {
            initTreeDropDownCategory();
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '#btnEdit', function (e) {
            e.preventDefault();
            const id = $('#hidIdM').val();
            const urlGetById = "/Admin/ProductCategory/GetById";
            //appcore.startLoading();
            service.getData({ url: urlGetById, dataType: DataType.json, data: { id: id }})
                .done((response) => {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    initTreeDropDownCategory(data.ParentId);
                    renderProductImage(data.Image);
                    $('#txtDescM').val(data.Description);
                    $('#txtImage').val(data.Image);
                    $('#txtSeoKeywordM').val(data.SeoKeywords);
                    $('#txtSeoDescriptionM').val(data.SeoDescription);
                    $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                    $('#txtSeoAliasM').val(data.SeoAlias);
                    $('#ckStatusM').prop('checked', data.Status === 1);
                    $('#ckShowHomeM').prop('checked', data.HomeFlag);
                    $('#txtOrderM').val(data.SortOrder);
                    $('#txtHomeOrderM').val(data.HomeOrder);
                    $('#modal-add-edit').modal('show');
                    //appcore.stopLoading();
                })
                .fail((status) => {
                    appcore.notify(Message.loginFail, Notify.danger);
                    //appcore.stopLoading();
                });
        });

        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            let id = $('#hidIdM').val();
            appcore.confirm('Are you sure to delete?', () => {
                //appcore.startLoading();
                let urlDelete = '/Admin/ProductCategory/Delete';
                service.postData({
                    url: urlDelete,
                    dataType: DataType.json,
                    data: { id: id }
                })
                .done((response) => {
                    appcore.notify('Deleted success', 'success');
                    //appcore.stopLoading();
                    loadData();
                })
                .fail((status) => {
                    appcore.notify('Has an error in deleting progress', 'error');
                    //appcore.stopLoading();
                });
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                const id = parseInt($('#hidIdM').val());
                const name = $('#txtNameM').val();
                const parentId = $('#ddlCategoryIdM').combotree('getValue');
                const description = $('#txtDescM').val();
                const image = $('#txtImage').val();
                const order = parseInt($('#txtOrderM').val());
                const homeOrder = $('#txtHomeOrderM').val();
                const seoKeyword = $('#txtSeoKeywordM').val();
                const seoMetaDescription = $('#txtSeoDescriptionM').val();
                const seoPageTitle = $('#txtSeoPageTitleM').val();
                const seoAlias = $('#txtSeoAliasM').val();
                const status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
                const showHome = $('#ckShowHomeM').prop('checked');
                const urlSaveEntity = '/Admin/ProductCategory/SaveEntity';
                //appcore.startLoading();
                service.postData({
                    url: urlSaveEntity,
                    dataType: DataType.json,
                    data: {
                        Id: id,
                        Name: name,
                        Description: description,
                        ParentId: parentId,
                        HomeOrder: homeOrder,
                        SortOrder: order,
                        HomeFlag: showHome,
                        Image: image,
                        Status: status,
                        SeoPageTitle: seoPageTitle,
                        SeoAlias: seoAlias,
                        SeoKeywords: seoKeyword,
                        SeoDescription: seoMetaDescription
                    }
                })
                    .done(response => {
                        appcore.notify('Update success', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();
                       // appcore.stopLoading();
                        loadData(true);
                    })
                    .fail((status) => {
                        appcore.notify('Has an error in update progress', 'error');
                        //appcore.stopLoading();
                    });
            }
            return false;

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
            this.value = null;
            return false;
        });


        $('body').on('click', '.icon-image-close', function () {
            const pathImage = $(this).data('path');
            $('#txtImage').val('');
            $('#img-prouduct-content').html('');
        });

    }

    function loadData() {
        const urlGetAll = '/Admin/ProductCategory/GetAll';
        service.getData({ url: urlGetAll, dataType: DataType.json })
            .done((response) => {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });

                });
                var treeArr = appcore.unflattern(data);
                loadDataTree(treeArr);
            })
            .fail(x => Notify(Message.ajaxFail));
    }

    function loadDataTree(treeArr) {
        $('#treeProductCategory').tree({
            data: treeArr,
            dnd: true,
            onContextMenu: function (e, node) {
                e.preventDefault();
                $('#hidIdM').val(node.id);
                $('#contextMenu').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
            },
            onDrop: function (target, source, point) {
                const targetNode = $(this).tree('getNode', target);
                if (point === 'append') {
                    if (targetNode.parentId === source.parentId && targetNode.parentId !== null) {
                        reOrderNode(source, targetNode);
                    } else {
                        const targetId = source.parentId === targetNode.id ? null : targetNode.id;
                        let children = getChildrenOfNode(targetNode);
                        updateNode(source.id, targetId, children);
                    }
                }
                else if (point === 'top' || point === 'bottom') {
                    reOrderNode(source, targetNode);
                }
            }
        });

        function getChildrenOfNode(node) {
            var children = [];
            if (node.children !== undefined) {
                $.each(node.children, function (i, item) {
                    children.push({
                        key: item.id,
                        value: i
                    });
                });
            }
            return children;
        }

        function updateNode(sourceId, targetId, children) {
            const urlUpdateParentId = '/Admin/ProductCategory/UpdateParentId';
            service.postData({
                url: urlUpdateParentId,
                dataType: DataType.json,
                data: {
                    sourceId: sourceId,
                    targetId: targetId,
                    items: children
                }
            })
                .done(x => loadData())
                .fail(x => Notify.danger(Message.ajaxFail));
        }

        function reOrderNode(source, targetNode) {
            const urlReorder = '/Admin/ProductCategory/ReOrder';
            service.postData({
                url: urlReorder,
                dataType: DataType.json,
                data: {
                    sourceId: source.id,
                    targetId: targetNode.id
                }
            })
                .done(x => {
                    loadData();
                })
                .fail(x => {
                    console.log('fail');
                });
        }
    }

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');

        $('#txtDescM').val('');
        $('#txtOrderM').val('');
        $('#txtHomeOrderM').val('');
        $('#txtImage').val('');
        $('#img-prouduct-content').html('');

        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');

        $('#ckStatusM').prop('checked', true);
        $('#ckShowHomeM').prop('checked', false);
    }

    function initTreeDropDownCategory(selectedId) {
        let urlGetAll = "/Admin/ProductCategory/GetAll";
        service.getData({ url: urlGetAll, dataType: DataType.json })
            .done((response) => {
                let data = [];
                let cbtParentId = $('#ddlCategoryIdM');
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = appcore.unflattern(data);
                cbtParentId.combotree({ data: arr });
                if (selectedId !== undefined) {
                    cbtParentId.combotree('setValue', selectedId);
                }
            });
    }

    function renderProductImage(src) {
        let tmplImageProduct = $('#img-product-template').html();
        let srcImage = (src === '' || src === null) ? Image.productDefault : src;
        let render = Mustache.render(tmplImageProduct, { srcImage: srcImage });
        $('#img-prouduct-content').html(render);
    }
};

var productCategory = new productCategoryController();
productCategory.initialize();
