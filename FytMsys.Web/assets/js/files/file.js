$(function () {
    $(".select-img").click(function () {
        var t = $(this).attr("data-text");
        f.operFile(t);
    });
    $(".sign-up").click(function () {
        $("#fileUp").unbind();
        $("#fileUp").click();
        $("#fileUp").change(function () {
            f.signUpFile("");
        });
    });

    $(".fclose").click(function () {
        dig.remove();
    });
});

var f = {
    operFile: function(inpt) {
        top.dialog({
            url: "/FytAdmin/FileMiam/Index?fields=" + inpt,
            title: '选择文件',
            width: 800,
            height: 480,
            data: $('#' + inpt).val(), // 给 iframe 的数据
            onclose: function() {
                this.returnValue && $('#' + inpt).val(this.returnValue);
                //dialog.focus();
            },
            oniframeload: function() {
                //console.log('iframe ready')
            }
        })
            .showModal();
        return false;
    },
    signUpFile: function(upInput) {
        var isImg = $('input[name="filestype"]:checked').val();
        var isThum = ($('#isthum').is(':checked') ? "1" : "0");
        var isWater = ($('#isWater').is(':checked') ? "1" : "0");
        var subUrl = "/FytAdmin/FileMiam/SignUpFile?upFiles=fileUrl&isImg=" + isImg + "&isThum=" + isThum + "&isWater=" + isWater;
        $("#forms").ajaxSubmit({
            beforeSubmit: function() {
                $(".sign-up").attr("disabled", "disabled").html(" 正在上传...");
            },
            success: function(data) {
                if (data.Status == "y") {
                    $("#fileUrl").val(data.Data);
                } else {
                    dig.alertError("消息", data.Msg);
                }
                $(".sign-up").attr("disabled", false).html(" 单文件上传");
                $("#fileUp").unbind();
            },
            error: function(e) {
                $(".sign-up").attr("disabled", false).html(" 单文件上传");
                console.log(e);
            },
            url: subUrl,
            type: "post",
            dataType: "json",
            timeout: 600000
        });
    },
    fileSizes: function(s) {
        s = s / 1024;
        if (s > 1024) {
            s = eval(s / 1024);
            s = s.toFixed(2) + "MB";
        } else {
            s = s.toFixed(0) + "KB";
        }
        return s;
    }
};



function delFolder(p, fold) {
    dig.confim("删除提示", "确定要删除选择的文件吗？", function () {
        var ajaxUrl = "/FytAdmin/FileMiam/DeleteBy";
        var isFile = (fold == "文件夹" ? 0 : 1);
        $.post(ajaxUrl, { path: $("#path").val() + p, isfile: isFile }, function (res) {
            if (res.Status == "y")
                initFiles($("#path").val());
            else {
                dig.alertError("提示", res.Msg);
            }
        }, "json");
    });
}

function OpenParentFolder() {
    var p = $("#path").val();
    if (p == spath) return;
    p = p.substring(0, p.lastIndexOf('/'));
    p = p.substring(0, p.lastIndexOf('/')) + "/";
    $("#path").val(p);
    initFiles(p);
}

function OpenFolder(p) {
    var npath = $("#path").val() + p + "/";
    $("#path").val(npath);
    initFiles(npath);
}

function SetFile(f, t) {
    $(t).addClass("selected").siblings().removeClass('selected');
    $("#fileUrl").val($("#path").val() + f);
}

function initFiles(path) {
    if (path == spath) {
        $("#fback").addClass("hidden");
        $("#lvmodel").removeClass("hidden");
    } else {
        $("#fback").removeClass("hidden");
        $("#lvmodel").addClass("hidden");
    }
    var collSum = 5, tbody = $("#tables > tbody"), tdata = $("#tlist");
    $.post("/FytAdmin/FileMiam/GetFileData", { path: path }, function (res) {
        if (res.Status == "y") {
            if (res.Data == "" || res.Data == null) {
                dig.alertError("提示", "该目录下没有文件了！");
                OpenParentFolder();
            } else {
                tbody.empty();
                tdata.tmpl(res.Data).appendTo('#trows');
                $(".table").colResizable();
                //增加图片列表
                var ulHtml = '';
                $.each(res.Data, function (index, fi) {
                    if (fi.ext == "文件夹") {
                        ulHtml += '<li onclick="OpenFolder(\'' + fi.name + '\')"><div class="file-wrapper"><i class="file-type-dir file-preview"></i><span class="file-title">' + fi.name + '</span></div><span class="icon"></span></li> ';
                    } else {
                        var imgUrl = $("#path").val() + fi.name;
                        //获得后缀名
                        var k = fi.name.substr(fi.name.indexOf(".") + 1);
                        k = k.toLowerCase();
                        if (k == "jpg" || k == "gif" || k == "png" || k == "bmp") {
                            ulHtml += '<li onclick="SetFile(\'' + fi.name + '\',this)"><img width="113" height="113" src="' + imgUrl + '" /><span class="icon"></span></li> ';
                        }
                        else {
                            ulHtml += '<li onclick="SetFile(\'' + fi.name + '\',this)"><div class="file-wrapper"><i class="file-type-'+k+' file-preview"></i><span class="file-title">' + fi.name + '</span></div><span class="icon"></span></li> ';
                        }
                        
                    }
                    $(".flist").html(ulHtml);
                });
            }
        } else {
            dig.alertError("错误提示", res.Msg);
        }
    }, "json");
}