$(function () {
    btnBack();
});

var fyt = {
    FSuccess: function (res) {
        if (res.Status == "y") {
            dig.alertSuccess("提示", res.Msg, function () {
                if (res.BackUrl != "" && res.BackUrl!="close") {
                    window.location = res.BackUrl;
                }
                if (res.BackUrl == "close") {
                    dig.remove();
                }
            });
        } else {
            //提示信息
            dig.alertError("错误提示", res.Msg);
        }
    },
    FBegin: function () {
        $(".btn-save").attr("disabled", "disabled").find("span").html("正在提交...");
    },
    FEnd: function () {
        $(".btn-save").attr("disabled", false).find("span").html("确定提交");
    },
    asyncall: function () {
        $("#checkall").checkall({ chname: "checkbox_name", callback: function (e) { } });
        fyt.tabCbk();
    },
    tabCbk: function () {
        //单机行，选中复选框
        $(".table tr").slice(1).each(function (g) {
            var p = this;
            $(this).children().slice(1).click(function () {
                $($(p).children()[0]).children().each(function () {
                    if (this.type == "checkbox") {
                        if (!this.checked) {
                            this.checked = true;
                        } else {
                            this.checked = false;
                        }
                    }
                });
            });
        });
    },
    uEditors: function () {
        return [['FullScreen', 'Source', 'Undo', 'Redo', 'bold', 'italic', 'underline', 'test', 'forecolor', 'insertorderedlist', 'insertunorderedlist', 'fontfamily', 'fontsize',
        'link', 'unlink', 'lineheight', 'justifyleft', 'justifycenter', 'justifyright', 'indent', 'inserttable']];
    }
};

var dig = {
    addModel: function (title, url, width, height, fun) {
        top.dialog({
            title: title,
            url: url,
            width: width,
            height: height,
            onremove: function () {
                if (fun) fun();
            },
            onclose: function () {
            }
        }).showModal();
    },
    remove: function () {
        var dialog = top.dialog.get(window);
        dialog.close().remove();
        return;
    },
    close: function () {
        var dialog = top.dialog.get(window);
        dialog.close().remove();
        return;
    },
    alertSuccess: function (title, msg, fun) {
        top.dialog({
            title: title,
            content: '<p style="padding:5px 50px; margin-bottom:0px;"><img src="/assets/images/admin/success-32-32.png" style="margin-right:15px;" />' + msg + '</p>',
            okValue: ' 确定 ',
            ok: function () {
                if (fun) fun();
            }
        }).showModal();
    },
    alertError: function (title, msg, fun) {
        top.dialog({
            title: title,
            content: '<p style="padding:5px 50px; max-width:400px; margin-bottom:0px;"><img src="/assets/images/admin/error32-32.png" style="margin-right:15px;" />' + msg + '</p>',
            okValue: ' 确定 ',
            ok: function () {
                if (fun) fun();
            }
        }).showModal();
    },
    confim: function (title, msg, funcSuc, funcErr) {
        var d = top.dialog({
            title: title,
            content: '<p style="padding:5px 50px; margin-bottom:0px;"><img src="/assets/images/admin/warning32-32.png" style="margin-right:15px;" />' + msg + '</p>',
            okValue: ' 确定 ',
            ok: function () {
                if (funcSuc) funcSuc();
            },
            cancelValue: ' 取消 ',
            cancel: function () {
                if (funcErr) funcErr();
            }
        }).showModal();
    },
    msg: function (msg) {
        var dm = top.dialog({
            content: '<p style="padding:5px 30px; margin-bottom:0px;"><img src="/assets/images/admin/success-32-32.png" style="margin-right:15px;" />' + msg + '</p>',
        }).show();
        setTimeout(function () {
            dm.close().remove();
        }, 1500);
    }
};

//返回上一页
function btnBack()
{
    $("#btn-back").click(function () {
        history.go(-1);
    });
    $("#btn-dig-close").click(function () {
        dig.close();
    });
};


/* 日期格式化 */
function GetDate(jsonDate, format) {
    if (jsonDate == null) {
        return "";
    }
    var d = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
    return d.format(format);
}
/* 全选反选 */
; (function ($) {
    $.fn.checkall = function (options) {

        var defaults = { chname: "checkname[]", callback: function () { } },
			options = $.extend(defaults, options),
			$obj = $(this),
			$items = $("input[name='" + options.chname + "']"),
			checkedItem = 0;
        $items.click(function () {
            if ($items.filter(":checked").length === $items.length) {
                $obj.attr("checked", true);
            } else {
                $obj.removeAttr("checked");
            }
            checkedItem = $items.filter(":checked").length;
            if (typeof options.callback === 'function') options.callback(checkedItem);
        });
        return $obj.each(function () {
            $(this).click(function () {
                if ($(this).prop('checked')) {
                    $items.prop("checked", true);
                    $items.parent().parent().addClass("selected");
                } else {
                    $items.removeAttr("checked");
                    $items.parent().parent().removeClass("selected");
                }
                checkedItem = $items.filter(":checked").length;
                if (typeof options.callback === 'function') options.callback(checkedItem);
            });
        });
    }
})(jQuery);

/*
 * Date Format 1.2.3
 * (c) 2007-2009 Steven Levithan <stevenlevithan.com>
 * MIT license
 *
 * Includes enhancements by Scott Trenda <scott.trenda.net>
 * and Kris Kowal <cixar.com/~kris.kowal/>
 *
 * Accepts a date, a mask, or a date and a mask.
 * Returns a formatted version of the given date.
 * The date defaults to the current date/time.
 * The mask defaults to dateFormat.masks.default.
 */

var dateFormat = function () {
    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
		timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
		timezoneClip = /[^-+\dA-Z]/g,
		pad = function (val, len) {
		    val = String(val);
		    len = len || 2;
		    while (val.length < len) val = "0" + val;
		    return val;
		};

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc) {
        var dF = dateFormat;

        // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
            mask = date;
            date = undefined;
        }

        // Passing date through Date applies Date.parse, if necessary
        date = date ? new Date(date) : new Date;
        if (isNaN(date)) throw SyntaxError("invalid date");

        mask = String(dF.masks[mask] || mask || dF.masks["default"]);

        // Allow setting the utc argument via the mask
        if (mask.slice(0, 4) == "UTC:") {
            mask = mask.slice(4);
            utc = true;
        }

        var _ = utc ? "getUTC" : "get",
			d = date[_ + "Date"](),
			D = date[_ + "Day"](),
			m = date[_ + "Month"](),
			y = date[_ + "FullYear"](),
			H = date[_ + "Hours"](),
			M = date[_ + "Minutes"](),
			s = date[_ + "Seconds"](),
			L = date[_ + "Milliseconds"](),
			o = utc ? 0 : date.getTimezoneOffset(),
			flags = {
			    d: d,
			    dd: pad(d),
			    ddd: dF.i18n.dayNames[D],
			    dddd: dF.i18n.dayNames[D + 7],
			    m: m + 1,
			    mm: pad(m + 1),
			    mmm: dF.i18n.monthNames[m],
			    mmmm: dF.i18n.monthNames[m + 12],
			    yy: String(y).slice(2),
			    yyyy: y,
			    h: H % 12 || 12,
			    hh: pad(H % 12 || 12),
			    H: H,
			    HH: pad(H),
			    M: M,
			    MM: pad(M),
			    s: s,
			    ss: pad(s),
			    l: pad(L, 3),
			    L: pad(L > 99 ? Math.round(L / 10) : L),
			    t: H < 12 ? "a" : "p",
			    tt: H < 12 ? "am" : "pm",
			    T: H < 12 ? "A" : "P",
			    TT: H < 12 ? "AM" : "PM",
			    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
			    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
			    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
			};

        return mask.replace(token, function ($0) {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
}();

// Some common format strings
dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd' 'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
};

// Internationalization strings
dateFormat.i18n = {
    dayNames: [
		"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
		"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    monthNames: [
		"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
		"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ]
};

// For convenience...
Date.prototype.format = function (mask, utc) {
    return dateFormat(this, mask, utc);
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

//分页
/*amountPage 总页数  page 当前页 classname 页面属性 method 加载方法 pageindex每页显示数量*/
function MvcPage(amountPage, page, method, classname) {
    var html = '';
    if (amountPage == 1) {
        html = '';
    } else {
        if (page == 1) {
            html = '<li class="paginate_button first disabled"><a href="javascript:void(0);">首页</a></li>';
            html += '<li class="paginate_button previous disabled"><a href="javascript:void(0);">上一页</a></li>';
        } else {
            html = '<li class="paginate_button first"><a href="javascript:' + classname + '.' + method + '(1);">首页</a></li>';
            html += '<li class="paginate_button previous"><a href="javascript:' + classname + '.' + method + '(' + (page - 1) + ');">上一页</a></li>';
        }
        if (amountPage < 9) {
            for (var i = 1; i < amountPage + 1; i++) {
                html += '<li class="' + (i == page ? 'paginate_button active' : 'paginate_button') + '"><a href="javascript:' + classname + '.' + method + '('+i+');">' + i + '</a></li>';
            }
        } else {
            var maxfeye = parseInt(page) + 4;
            var minfeye = parseInt(page) - 3;
            if (page < 8) {
                maxfeye = 8;
                minfeye = 1;
            }
            if (maxfeye > amountPage) {
                minfeye = amountPage - 4;
                maxfeye = amountPage;
            }

            for (var f = minfeye; f < maxfeye + 1; f++) {
                html += '<li class="' + (f == page ? 'paginate_button active' : 'paginate_button') + '"><a href="javascript:' + classname + '.' + method + '(' + f + ');">' + f + '</a></li>';
            }
            if (amountPage - page > 6) {
                html += '<li class="paginate_button dot"><a href="javascript:void(0)">...</a></li>';
                html += '<li class="paginate_button"><a  href="javascript:' + classname + '.' + method + '(' + amountPage + ');">' + amountPage + '</a></li>';
            }
        }
        //if (amountPage - page > 10) {
        //    for (var i = 0; i <= 10; i++) {
        //        if (i == 0) {
        //            html += ' <li class="paginate_button active"><a href="javascript:' + classname + '.' + method + '(' + page + ');">' + page + '</a></li>';
        //        } else if (i == 9) {
        //            html += '<li class="paginate_button dot"><a href="javascript:void(0)">...</a></li>';
        //        } else if (i == 10) {
        //            html += ' <li class="paginate_button"><a href="javascript:' + classname + '.' + method + '(' + amountPage + ');">' + amountPage + '</a></li>';
        //        } else {
        //            html += ' <li class="paginate_button"><a href="javascript:' + classname + '.' + method + '(' + (page + i + 1) + ');">' + (page + i) + '</a></li>';
        //        }
        //    }
        //} else {
        //    if (amountPage > 11) {
        //        for (var m = amountPage - 10; m <= amountPage; m++) {
        //            if (m == page) {
        //                html += ' <li class="paginate_button active"><a href="javascript:' + classname + '.' + method + '(' + m + ');">' + m + '</a></li>';
        //            } else {
        //                html += ' <li class="paginate_button"><a href="javascript:' + classname + '.' + method + '(' + m + ');">' + m + '</a></li>';
        //            }
        //        }
        //    } else {
        //        for (var m = 1; m <= amountPage; m++) {
        //            if (m == page) {
        //                html += ' <li class="paginate_button active"><a href="javascript:' + classname + '.' + method + '(' + m + ');">' + m + '</a></li>';
        //            } else {
        //                html += ' <li class="paginate_button"><a href="javascript:' + classname + '.' + method + '(' + m + ');">' + m + '</a></li>';
        //            }
        //        }
        //    }
        //}
        if (page == amountPage) {
            html += ' <li class="paginate_button next disabled"><a href="javascript:void(0);">下一页</a></li>';
            html += ' <li class="paginate_button last disabled"><a href="javascript:void(0);">尾页</a></li>';
        }
        else {
            html += '<li class="paginate_button next"><a href="javascript:' + classname + '.' + method + '(' + (page + 1) + ');">下一页</a></li>';
            html += '<li class="paginate_button last"><a href="javascript:' + classname + '.' + method + '(' + amountPage + ');">尾页</a></li>';
        }
    }
    return html;
}