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
        var rst;
        try {
            rst = JSON.parse(data);
        } catch (e) {

        }
        deferred.resolve(rst);
    }).fail(function (error) {
        var rst;
        try {
            rst = JSON.parse(data);
        } catch (e) {

        }
        deferred.reject(rst);
    });
    return deferred.promise();
}

var IsSuportLocalStorage = function () {
    if (window.Storage && window.localStorage && window.localStorage instanceof Storage) {
        return true;
    }
    else return false;
};

String.prototype.format = function () {
    var resultStr = this.toString();
    // 参数为对象
    if (typeof arguments[0] === "object") {
        for (var i in arguments[0]) {
            resultStr = resultStr.replace("{" + i + "}", arguments[0][i]);
        }
    }
        // 多个参数
    else {
        for (var i = 0; i < arguments.length; i++) {
            resultStr = resultStr.replace("{" + i + "}", arguments[i]);
        }
    }
    return resultStr;
};

//var options = {
//    "method": "GET",
//    "params": {},
//    "usetoken": true
//}