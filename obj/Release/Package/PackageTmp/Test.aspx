<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Suite.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    SuiteTicket:<asp:TextBox ID="txtSuiteTicket" runat="server" Width="516px"></asp:TextBox><br/>
        <asp:Button ID="btnGetSuiteToken" runat="server" Text="获取套件访问Token" OnClick="btnGetSuiteToken_Click" />
        <br />
        临时授权码:<asp:TextBox ID="txtTempAuthCode" runat="server" Width="516px"></asp:TextBox>
        <br />
        <asp:Button ID="btnGetPermanentCode" runat="server" Text="获取企业的永久授权码" OnClick="btnGetPermanentCode_Click" />
        <br />
        企业CorpId:<asp:TextBox ID="txtCorpId" runat="server" Width="567px"></asp:TextBox>
        <br />
        永久授权码:<asp:TextBox ID="txtPermanentCode" runat="server" Width="561px"></asp:TextBox>
        <br />
        <asp:Button ID="btnGetCorpToken" runat="server" Text="获取企业授权的access_token" OnClick="btnGetCorpToken_Click" />
        <br />
        企业授权的access_token：<asp:TextBox ID="txtCorpAccessToken" runat="server" Width="484px"></asp:TextBox>
        <br/>
        <asp:Button ID="btnGetAuthInfo" runat="server" Text="获取企业授权的授权数据" OnClick="btnGetAuthInfo_Click"/>
        
        <br />
        <asp:Button ID="btnGetAgent" runat="server" Text="获取企业的应用信息" />
        
        <br />
         <asp:Button ID="btnActivateSuite" runat="server" Text="激活授权套件" OnClick="btnActivateSuite_Click" />
       
 </div>
    </form>
</body>
</html>
