<%@ page language="C#" autoeventwireup="true" inherits="_Default" CodeFile="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EFPlatform.CodeGenerator Demo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<asp:Button ID="Button1" runat="server" Text="Generate Category Pages" EnableViewState="False" OnClick="Button1_Click" /><br />
		<br />
		<asp:Button ID="Button2" runat="server" Text="Generate Product Pages" EnableViewState="False" OnClick="Button2_Click" /><br />
		<br />
		<asp:HyperLink ID="HyperLink4" runat="server" EnableViewState="False" NavigateUrl="./Template/Category.html" Target="_blank">View Category Template</asp:HyperLink><br />
		<br />
		<asp:HyperLink ID="HyperLink3" runat="server" EnableViewState="False" NavigateUrl="./Template/Product.html" Target="_blank">View Product Template</asp:HyperLink><br />
		<br />
		<asp:HyperLink ID="HyperLink1" runat="server" EnableViewState="False" NavigateUrl="./Html/Category1.html" Target="_blank">Open Category Page</asp:HyperLink><br />
		<br />
		<asp:HyperLink ID="HyperLink2" runat="server" EnableViewState="False" NavigateUrl="./Html/Product1.html" Target="_blank">Open Product Page</asp:HyperLink></div>
    </form>
</body>
</html>
