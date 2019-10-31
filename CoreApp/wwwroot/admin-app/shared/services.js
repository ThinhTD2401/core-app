function Service() {

    function getData({ url, dataType = 'json', data = [] }) {
        return $.ajax({
            url: url,
            type: 'get',
            dataType: dataType,
            data: data
        });
    }

    function postData({ url, dataType = 'json', data = [], beforeSendCallBack}) {

        return $.ajax({
            url: url,
            type: 'post',
            dataType: dataType,
            data: data,
            beforeSend: function () {
                if (beforeSendCallBack !== undefined) {
                    beforeSendCallBack();
                }
            }
        });
    }

    function uploadFile({ url, dataType = 'json', data = [], beforeSendCallBack, contentType = 'json', processData = true }) {
        return $.ajax({
            url: url,
            type: 'post',
            dataType: dataType,
            data: data,
            contentType: contentType,
            processData: processData,
            beforeSend: function () {
                if (beforeSendCallBack !== undefined) {
                    beforeSendCallBack();
                }
            }
        });
    }

    return {
        // ...
        getData,
        postData,
        uploadFile
    };
    
}
var service = new Service();

