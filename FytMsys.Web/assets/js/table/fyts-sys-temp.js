var oTable;
$(function () {

    oTable=tab.initTable();
    $("#tb-refresh").on("click", function () {
        //alert(oTable);
        oTable.fnReloadAjax(oTable.fnSettings());
    });

    $("#btnKey").click(function () {
        oTable.fnClearTable(oTable);
        tab.initTable();
    });

});


var tab = {
    initTable: function () {
        var table = $('#tables').DataTable({
            ajax: {
                type: "post",
                url: "/FytAdmin/SysBasic/TempData/",
                dataSrc: function (res) {
                    return res.Data;
                },
                data: function (d) {
                    var param = {};
                    param.iDisplayStart = d.start;
                    param.iDisplayLength = d.length;
                    param.key = $("#txtKey").val();
                    return param;
                }
            },
            columns: [
                { data: 'ID' },
                { data: "Title" },
                { data: "Url" },
                { data: "IsLock" },
                { data: "Sort" },
                { data: "AddDate" }
            ],
            "aoColumnDefs": [{ "bSortable": false, "aTargets": [0, 3] }, { "class": "tn", "targets": [0] },
                    {
                        "targets": [0],
                        "render": function (data, type, full) {
                            return '<input name="checkbox_name" type="checkbox" value="' + data + '">';
                        }
                    },
            {
                "targets": [3],
                "render": function (data, type, full) {
                    if (data == true)
                        return '<input name="cbs" type="checkbox" checked="checked" value="" disabled="disabled">';
                    else {
                        return '<input name="cbs" type="checkbox" value="" disabled="disabled">';
                    }
                }
            }, { width: '60px', targets: 0 }],
            "pagingType": "full_numbers",
            "sLoadingRecords": "正在加载数据...",
            "sZeroRecords": "暂无数据",
            stateSave: true,
            "searching": false,
            "dom": 'rt<"bottom"iflp<"clear">>',
            "language": {
                "processing": "玩命加载中...",
                "lengthMenu": "显示 _MENU_ 项结果",
                "zeroRecords": "没有匹配结果",
                "info": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
                "infoEmpty": "显示第 0 至 0 项结果，共 0 项",
                "infoFiltered": "(由 _MAX_ 项结果过滤)",
                "infoPostFix": "",
                "url": "",
                "paginate": {
                    "first": "首页",
                    "previous": "上一页",
                    "next": "下一页",
                    "last": "末页"
                }
            },
            "initComplete": function () {
                fyt.asyncall();
                //使用col插件实现表格头宽度拖拽
                $(".table").colResizable();
            }
        });
        return table;
    }
};

/*
 add this plug in
 // you can call the below function to reload the table with current state
 Datatables刷新方法
 oTable.fnReloadAjax(oTable.fnSettings());
 */
$.fn.dataTableExt.oApi.fnReloadAjax = function(oSettings) {
    //oSettings.sAjaxSource = sNewSource;
    this.fnClearTable(this);
    
    this.oApi._fnProcessingDisplay(oSettings, true);
    var that = this;
    alert(oSettings.sAjaxSource);
    $.getJSON(oSettings.sAjaxSource, null, function(json) {
        /* Got the data - add it to the table */
        for (var i = 0; i < json.aaData.length; i++) {
            that.oApi._fnAddData(oSettings, json.aaData[i]);
        }
        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
        that.fnDraw(that);
        that.oApi._fnProcessingDisplay(oSettings, false);
    });
};
