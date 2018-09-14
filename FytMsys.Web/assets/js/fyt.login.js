$(document).ready(function() {
    $(".form-signin").initValidform();
    log.initCode();
});

var log = {
    Begin: function () {
        $(".btn-login").attr("disabled", "disabled");
        $("#eMsg").addClass("Validform_wrong").show().html("正在登录...");
    },
    Success: function (reg) {
        if (reg.Status == "y") {
            $("#eMsg").hide();
            $('.ids').html('<div class="progress"><div class="progress-bar progress-bar-success" aria-valuetransitiongoal="100"></div></div>');
            $('.progress .progress-bar').progressbar();
            setTimeout(function() {
                window.location = reg.BackUrl;
            }, 500);
        } else {
            $("#eMsg").show().html(reg.Msg);
            $(".btn-login").attr("disabled", false);
            $("#imgVerify").click();
        }
    },
    initCode: function() {
        $.ajax({
            type: "post",
            url: "/fytadmin/login/GetVaildateCode?t=" + (new Date()).getTime(),
            data: null,
            beforeSend: function() { $("#imgVerify").attr("src", "/assets/images/login/loading.gif"); },
            success: function(data) {
                $("#imgVerify").attr("src", "/fytadmin/login/GetVaildateCode?t=" + (new Date()).getTime());
            },
            error: function(e) {
            }
        });
        $("#imgVerify").click(function() {
            this.src = "/fytadmin/login/GetVaildateCode?t=" + (new Date()).getTime();
        });
    }
};

//初始化验证表单
$.fn.initValidform = function () {
    var checkValidform = function (formObj) {
        $(formObj).Validform({
            tiptype: function (msg, o, cssctl) {
                /*msg：提示信息;
                o:{obj:*,type:*,curform:*}
                obj指向的是当前验证的表单元素（或表单对象）；
                type指示提示的状态，值为1、2、3、4， 1：正在检测/提交数据，2：通过验证，3：验证失败，4：提示ignore状态；
                curform为当前form对象;
                cssctl:内置的提示信息样式控制函数，该函数需传入两个参数：显示提示信息的对象 和 当前提示的状态（既形参o中的type）；*/
                //全部验证通过提交表单时o.obj为该表单对象;
                if (!o.obj.is("form")) {
                    //页面上不存在提示信息的标签时，自动创建;
                    if (o.obj.parent("div").find(".Validform_checktip").length == 0) {
                        o.obj.parent("div").append("<label class='Validform_checktip' />");
                        o.obj.parent("div").next().find(".Validform_checktip").remove();
                    }
                    var objtip = o.obj.parent("div").find(".Validform_checktip");
                    cssctl(objtip, o.type);
                    objtip.text(msg);
                }
            },
            showAllError: true
        });
    };
    return $(this).each(function () {
        checkValidform($(this));
    });
};