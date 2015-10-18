<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Suite._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    MsgSig:<asp:TextBox ID="txtMsgSig" runat="server" Width="516px"></asp:TextBox><br/>
    TimeStamp:<asp:TextBox ID="txtTimeStamp" runat="server" Width="490px"></asp:TextBox><br/>
    Nonce:<asp:TextBox ID="txtNonce" runat="server" Width="518px"></asp:TextBox><br/>
    EncryptStr:<asp:TextBox ID="txtEncryptStr" runat="server" Height="151px" Width="669px"></asp:TextBox><br/>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        <br />
        <asp:TextBox ID="txtEchoStr" runat="server" Width="963px"></asp:TextBox>
    </div>
    </form>
</body>
</html>
