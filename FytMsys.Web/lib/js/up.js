$(function () {
    $("#asign").click(function () {
        $("#signfile").unbind();
        $("#signfile").click();
        $("#signfile").change(function () {
            f.signUpFile();
        });
    });
    $("#ashow").click(function () {
        $("#showfile").unbind();
        $("#showfile").click();
        $("#showfile").change(function () {
            f.showUpFile();
        });
    });
    $("#bshow").click(function () {
        $("#bannerfile").unbind();
        $("#bannerfile").click();
        $("#bannerfile").change(function () {
            f.bannerUpFile();
        });
    });
});

var f = {
    signUpFile: function () {
        var subUrl = "/FileUp/SignUpFile?upFiles=signfile";
        $("#forms").ajaxSubmit({
            beforeSubmit: function () {
                $("#asign").attr("disabled", "disabled").html("正在上传...");
            },
            success: function (data) {
                if (data.Status == "y") {
                    $("#CoverImg").val(data.Data);
                    $("#sp-img").show();
                    $("#sp-img img").attr("src", data.Data);
                } else {
                    alert(data.Msg);
                }
                $("#asign").attr("disabled", false).html("Banner图");
                $("#signfile").unbind();
            },
            error: function (e) {
                $("#asign").attr("disabled", false).html("Banner图");
                console.log(e);
            },
            url: subUrl,
            type: "post",
            dataType: "json",
            timeout: 600000
        });
    },
    showUpFile: function () {
        var subUrl = "/FileUp/ShowUpFile?upFiles=showfile";
        $("#forms").ajaxSubmit({
            beforeSubmit: function () {
                $("#ashow").attr("disabled", "disabled").html("正在上传...");
            },
            success: function (data) {
                if (data.Status == "y") {
                    $("#ShowImg").val(data.Data);
                    $("#sh-img").show();
                    $("#sh-img img").attr("src", data.Data);
                } else {
                    alert(data.Msg);
                }
                $("#ashow").attr("disabled", false).html("展示图");
                $("#showfile").unbind();
            },
            error: function (e) {
                $("#ashow").attr("disabled", false).html("展示图");
                console.log(e);
            },
            url: subUrl,
            type: "post",
            dataType: "json",
            timeout: 600000
        });
    },
    bannerUpFile: function () {
        var subUrl = "/FileUp/BannerUpFile?upFiles=bannerfile";
        $("#forms").ajaxSubmit({
            beforeSubmit: function () {
                $("#bshow").attr("disabled", "disabled").html("正在上传...");
            },
            success: function (data) {
                if (data.Status == "y") {
                    $("#CoverImg").val(data.Data);
                    $("#sb-img").show();
                    $("#sb-img img").attr("src", data.Data);
                } else {
                    alert(data.Msg);
                }
                $("#bshow").attr("disabled", false).html("Banner图");
                $("#bannerfile").unbind();
            },
            error: function (e) {
                $("#bshow").attr("disabled", false).html("Banner图");
                console.log(e);
            },
            url: subUrl,
            type: "post",
            dataType: "json",
            timeout: 600000
        });
    }
}