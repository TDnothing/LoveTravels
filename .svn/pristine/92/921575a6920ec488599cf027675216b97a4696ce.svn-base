var hmUrl = "/FytAdmin/Index/", hm = {
    checkSite: function(sid) {
        $.post(hmUrl + "NewSite", { sid: sid }, function(res) {
            if (res.Status == "y") {
                dig.alertSuccess("消息", res.Msg, function() {
                    window.location.reload();
                });
            } else {
                dig.alertError("提示", res.Msg);
            }
        }, "json");
    },
    exitLogin: function() {
        dig.confim("退出提示", "您确定要退出后台管理系统吗？", function () {
            $.post(hmUrl + "OutLogin", null, function (res) {
                if (res.Status == "y")
                    window.location = '/FytAdmin/login/';
                else {
                    dig.alertError("提示", res.Msg);
                }
            }, "json");
        });
    }
};

$(function () {
    $(".dl_exit").click(function () {
        hm.exitLogin();
    });
    $(".nav>li>a").click(function () {
        $(".nav>li>a").removeClass("selected");
        $(this).addClass("selected");
    });
});

function leftHide() {
    $(".dl-second-nav,.user-menu").hide();
    $(".dl-inner-tab").css({'margin-left':0});
}
function leftShow() {
    $(".dl-second-nav,.user-menu").show();
    $(".dl-inner-tab").css({ 'margin-left': 200 });
}