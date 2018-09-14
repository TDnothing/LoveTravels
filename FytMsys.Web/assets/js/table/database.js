$(function () {
    tb.initTable(1);
    //检索/刷新
    $("#btnKey,#tb-refresh").click(function () {
        tb.initTable(1);
    });
});

var url = "/FytAdmin/DataBase/", collSum = 6, tbody = $("#tables>tbody"), tdata = $("#tlist"), tb = {
    initTable: function (pageIndex) {
        var pageSize = $("#sel_page").val(), maxi = 0;
        $.ajax({
            type: "post",
            url: url + "GetListFile",
            beforeSend: function () { tbody.html(tb.ajaxLoading()); },
            data: {
                pageSize: pageSize, pageIndex: pageIndex, key: escape($("#txtKey").val())
            },
            success: function (res) {
                if (res.Status == "y") {
                    if (res.Data == "" || res.Data == null) {
                        tbody.empty().html(tb.ajaxNull());
                    } else {
                        tbody.empty();
                        tdata.tmpl(res.Data).appendTo('#trows');
                        fyt.asyncall();
                        //使用col插件实现表格头宽度拖拽
                        $(".table").colResizable();
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
            $.post(url + "DeleteFile", { id: vals }, function (res) {
                if (res.Status == "y") tb.initTable(1);
                else dig.alertError("消息", res.Msg);
                $(":checkbox").attr("checked", false);
            }, "json");
        });
    },
    fileDown: function (p) {
        $.post(url + "FileDown", { p: p }, function (res) {
        }, "json");
    }
};
function fileSizes(s) {
    s = s / 1024;
    if (s > 1024) {
        s = eval(s / 1024);
        s = s.toFixed(2) + "MB";
    } else {
        s = s.toFixed(0) + "KB";
    }
    return s;
}