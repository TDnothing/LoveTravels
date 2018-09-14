$(function () {
    tb.initTable(1);
    //检索/刷新
    $("#btnKey,#tb-refresh").click(function () {
        tb.initTable(1);
    });
    //页码调整
    $("#sel_page").change(function () {
        tb.initTable(1);
    });
    $("#Insert").click(function () {
        dig.addModel("添加/编辑版位", url + "ClassModfiy/0?Flag="+flag, 850, 480, function () {
            tb.initTable(1);
            dig.remove();
        });
    });
    $("#Modfiy").click(function () {
        var vals = '', lsum = 0;
        $('input[name="checkbox_name"]:checked').each(function () {
            vals = $(this).val();
            lsum += 1;
        });
        if (!vals) { dig.alertError("提示", "请选择要编辑的信息！"); return; }
        if (lsum > 1) { dig.alertError("提示", "编辑信息只能选择一项！"); return; }
        dig.addModel("添加/编辑管理员", url + "ClassModfiy/" + vals + "?Flag="+flag, 850, 480, function () {
            tb.initTable(1);
        });
    });
});
var flag = $("#Flag").val();
var url = "/FytAdmin/AdvManage/", collSum = 9, tdata = $("#tlist"), tb = {
    initTable: function (pageIndex) {
        var pageSize = $("#sel_page").val(), maxi = 0;
        $.ajax({
            type: "post",
            url: url + "IndexData",
            beforeSend: function () {  },
            data: {
                flag: flag
            },
            success: function (res) {
                if (res.Status == "y") {
                    if (res.Data == "" || res.Data == null) {
                        $("#trows").html('');
                    } else {
                        $('#trows').empty();
                        tdata.tmpl(res.Data).appendTo('#trows');
                        fyt.asyncall();
                        $("#trows a").click(function () {
                            $("#trows a").css({ color: "#666666" });
                            $(this).css({ color: "red" });
                        });
                    }
                } else {
                    dig.alertError("错误提示", res.Msg);
                }
            },
            error: function (e) {
                console.log(e);
            }
        });
    },
    ajaxLoading: function () {
        return "<tr><td colspan=\"" + collSum + "\" style='text-align:center;'><img src='/assets/images/skin/T1BHtLFbFdXXaHNz_X-16-16.gif' /> loading...</td></tr>";
    },
    ajaxNull: function () {
        return "<tr><td colspan=\"" + collSum + "\" style='text-align:center;'><img src='/assets/images/admin/face-sad.png' /> <span>　暂无数据</span></td></tr>";
    },
    deletes: function (id) {
        var vals = '';
        if (id == 0) {
            $('input[name="checkbox_name"]:checked').each(function () {
                vals += $(this).val() + ',';
            });
            if (!vals) {
                dig.alertError("提示", "对不起，请选中您要操作的记录！");
                return;
            }
        } else {
            vals = id;
        }
        var msg = "删除记录后不可恢复，您确定吗？";
        dig.confim("删除确认", msg, function () {
            $.post(url + "DeleteBy", { id: vals }, function (res) {
                if (res.Status == "y") tb.initTable(1);
                else dig.alertError("消息", res.Msg);
                $(":checkbox").attr("checked", false);
            }, "json");
        });
    }
};