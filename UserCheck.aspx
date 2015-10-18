<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserCheck.aspx.cs" Inherits="Suite.UserCheck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

   <script type="text/javascript" src="js/zepto.min.js"></script>
    <script type="text/javascript" src="js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="http://g.alicdn.com/ilw/ding/0.3.8/scripts/dingtalk.js"></script>
    <script type="text/javascript" src="js/logger.js"></script>
</head>
<body>
    <script type="text/javascript" >
        dd.config({
            appId: <%= appId%>,
            corpId: '<%= corpId%>',
            timeStamp: <%= timestamp%>,
            nonceStr: '<%= nonceStr%>',
            signature: '<%= signature%>',
            jsApiList: ['runtime.info',
        'biz.contact.choose',
        'device.notification.confirm',
        'device.notification.alert',
        'device.notification.prompt',
        'biz.ding.post']
        });
        dd.ready(function () {
            //dd.runtime.info({
            //    onSuccess: function (info) {
            //        alert('runtime info: ' + JSON.stringify(info));
            //    },
            //    onFail: function (err) {
            //        alert('fail: ' + JSON.stringify(err));
            //    }
            //});

            dd.runtime.permission.requestAuthCode({
                corpId:  '<%= corpId%>',
                onSuccess: function (info) {
                    alert('authcode: ' + info.code);
                    $.ajax({
                        url: '/Ajax/DingHandler.ashx?method=getuserinfo&code=' + info.code,
                        type: 'GET',
                        success: function (data, status, xhr) {
                                alert(data);
                        },
                        error: function (xhr, errorType, error) {
                            alert(errorType + ', ' + error);
                        }
                    });
                },
                onFail: function (err) {
                    alert('fail: ' + JSON.stringify(err));
                }
            });
        });

        dd.error(function (err) {
            alert('dd error: ' + JSON.stringify(err));
        });
    </script>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
