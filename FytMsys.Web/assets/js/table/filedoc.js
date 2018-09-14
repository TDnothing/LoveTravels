var url = "/FytAdmin/FileMiam/";
$(function () {
    //双击行事件
    $("#tables tr").dblclick(function () {
        var ext = $(this).attr("data-ext");
        if (ext == "文件夹") return;
        var eId = $(this).find("input[type='checkbox']").val();
        dig.addModel("查看/编辑", url + "CodeIndex/?path=" + $("#path").val()+"&filename="+eId+"&ext="+ext, 900, 570, function () {
            
        });
    });
});