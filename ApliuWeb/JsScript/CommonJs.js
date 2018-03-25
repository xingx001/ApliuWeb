$(document).ready(function () {
    $.ajaxSetup({
        async: false
    });
});
var ApliuCommon = {};
ApliuCommon.HttpSend = function (apiurl, options) {
    var v;
    if (options) {
        if (options.params) {
            apiurl += $.param(options.params);
        }
        if (options.method && options.method == "GET") {
            v = $.get(apiurl);
        }
        else if (options.method && options.method == "POST") {
            v = $.post(apiurl, options.params);
        }
        else {
            v = $.get(apiurl);
        }
    }
    else {
        v = $.get(apiurl);
    }
    var deferred = $.Deferred();
    //var v = $.ajax({ url: apiurl, async: false });
    v.done(function (data) {
        var rst = JSON.parse(data);
        var result = "执行结果：" + rst.result + "\r\n返回码：" + rst.code + "\r\n返回信息：" + rst.msg;
        deferred.resolve(result);
    }).fail(function (error) {
        var rst = JSON.parse(data);
        var result = "执行结果：" + rst.result + "\r\n返回码：" + rst.code + "\r\n返回信息：" + rst.msg;
        deferred.reject(result);
    });
    return deferred.promise();
}

var IsSuportLocalStorage = function () {
    if (window.Storage && window.localStorage && window.localStorage instanceof Storage) {
        return true;
    }
    else return false;
};
//var options = {
//    "method": "GET",
//    "params": {},
//    "usetoken": true
//}