var loginController = function() {

    this.initialize = function() {
        registerEvents();
    };

    var registerEvents = function () {
        $('#frmLogin').validate({
            errorClass: 'text-danger',
            ignore: [],
            lang: 'en',
            rules: {
                userName: {
                    required: true
                },
                password: {
                    required: true
                }
            }
        });

        $('#btnLogin').on('click',
            function (e) {
                if ($('#frmLogin').valid()) {
                    e.preventDefault();
                    var user = $('#txtUserName').val();
                    var password = $('#txtPassword').val();
                    login(user, password);
                }
            });
    };

    var login = function(user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                UserName: user,
                Password: pass
            },
            dateType: 'json',
            url: '/admin/login/authen',
            success: function(res) {
                if (res.Success) {
                    window.location.href = "/Admin/Home/Index";
                } else {
                    appcore.notify(Message.loginFail, Notify.info);
                }
            }
        });
    };
};

var login = new loginController();
login.initialize();