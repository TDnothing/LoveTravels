$(function() {
    var H = $(window).height();
    $("#DeployBase, #ad_tab").css({ 'height': H - 0 });
    tb.initTable();
    $("#Insert").click(function () {
        dig.addModel("添加/编辑角色", url + "GroupModfiy/0", 850, 400, function () {
            tb.initTable(1);
            dig.remove();
        });
    });
    $("#Modfiy").click(function () {
        var did = $("#hdel").val();
        if (did == 0) {
            dig.alertError("提示", "请选择要编辑的信息！"); return;
        }
        dig.addModel("添加/编辑角色", url + "GroupModfiy/" + did, 850, 400, function () {
            tb.initTable(1);
        });
    });
});

var url = "/FytAdmin/Member/", collSum = 8, tbody = $("#tables>tbody"), tdata = $("#tlist"), tb = {
    initTable: function () {
        $.post(url+"GroupData", null, function (res) {
            if (res.Status == "y") {
                $("#aDrow").empty();
                $("#tlist").tmpl(res.Data).appendTo('#aDrow');
                $("dt.pthy").click(function () {
                    $("dt.pthy").removeClass('selected');
                    $(this).addClass("selected");
                    $("#hdel").val($(this).attr("data-id"));
                });
            } else {
                dig.alertError("消息", res.Msg);
            }
        }, "json");
    },
    ajaxLoading: function () {
        return "<tr><td colspan=\"" + collSum + "\" style='text-align:center;'><img src='/assets/images/skin/T1BHtLFbFdXXaHNz_X-16-16.gif' /> loading...</td></tr>";
    },
    ajaxNull: function () {
        return "<tr><td colspan=\"" + collSum + "\" style='text-align:center;'><img src='/assets/images/admin/face-sad.png' /> <span>　暂无数据</span></td></tr>";
    },
    deletes: function () {
        var did = $("#hdel").val();
        if (did == 0) {
            dig.alertError("提示", "请选择要删除角色的记录！");
            return;
        }
        var msg = "删除记录后不可恢复，您确定吗？";
        dig.confim("删除确认", msg, function () {
            $.post(url + "GroupDeleteBy", { id: did }, function (res) {
                if (res.Status == "y") tb.initTable();
                else dig.alertError("消息", res.Msg);
                $(":checkbox").attr("checked", false);
            }, "json");
        });
    }
};