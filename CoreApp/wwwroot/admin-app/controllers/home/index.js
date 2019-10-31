var homeController = function () {
    this.initialize = function () {
        registerEvent();
        appcore.stopLoading();
    };

    function registerEvent() {

    }
}

var homeController = new homeController();
homeController.initialize();