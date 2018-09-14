$(function () {
    //选择一级复选框，子级选中
    $(".munu-wall .tree-one input").each(function () {
        $(this).unbind("click");
        $(this).bind("click", function () {
            if ($(this).prop('checked')) {
                $(this).parent().parent().parent(".ibox-content").find(".tree-ul input").prop("checked", true);
            } else {
                $(this).parent().parent().parent(".ibox-content").find(".tree-ul input").prop("checked", false);
            }
        });
    });
    //选择二级复选框，判断一级是否选中，如果未选中，则更改选中状态
    $(".munu-wall .tree-ul input").each(function () {
        $(this).unbind("click");
        $(this).bind("click", function () {
            if ($(this).prop('checked')) {
                //判断父级
                var o = $(this).parent().parent().parent().parent().parent(".ibox-content").find(".tree-one input");
                if (!o.prop('checked')) {
                    o.prop('checked', true);
                }
            }
        });
    });
    //赋值角色ID
    $(".rolemenu").each(function () {
        $(this).unbind("click");
        $(this).bind("click", function () {
            GetSelectMenu($(this).attr("data-id"));
        });
    });
    //角色选中状态
    $(".category-list>li>a").click(function () {
        var m = $(this).find("span").html();
        $("#phtml").html("已选择角色："+m);
    });
});
//根据角色查询权限ID列表
function GetSelectMenu(id) {
    $("#roleId").val(id);
    var url = "/FytAdmin/SysAdmin/PermissionData/";
    $(".munu-wall input").prop('checked', false);
    $.post(url, { roldId: id }, function (res) {
        $.each(res, function (m, i) {
            var t = res[m].NodeID;
            $(".munu-wall input").each(function () {
                if ($(this).val() == t) {
                    $(this).prop('checked', true);
                }
            });
        });
    }, "json");
}
function menu_saveloging() {
    if ($("#roleId").val() == "0") {
        dig.alertError("消息提示", "请选择对应角色");
        return false;
    }
    $("#shtml").html("正在提交...");
}

function menuSuccess(jsonData) {
    if (jsonData.Status == "e") {
        dig.alertError("消息提示", jsonData.Msg);
    } else {
        dig.alertSuccess("消息提示", "权限分配成功！");
    }
}