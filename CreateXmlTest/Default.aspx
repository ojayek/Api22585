<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CreateXmlTest.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ece     moshanir </title>
     <style type="text/css">

         img {
                display: block;
                margin-left: auto;
                margin-right: auto;
                width: 40%;
            }
      

        .App-logo {
             animation: App-logo-spin infinite 20s linear;
             margin: 0px auto;
             width: 20%;
             text-align: center;
             height: 20px;
         }
        .App-title {
        position:static;            
        text-align:center;
        }



        body {
          direction:rtl;
          margin: 10px;
          padding: 10px;
          font-family:B Nazanin;
          font-size: 20pt;
            }


        .App-header {
          background-color: cornflowerblue;
          height: 350px;
         padding: 20px;
         color: white;
                }

        header {
           display: block;
            }

        .App {
            text-align: center;
            }




           </style>
    <link href="//db.onlinewebfonts.com/c/3671adca6f650c92b83f906e49656986?family=B+Nazanin" rel="stylesheet" type="text/css" />






</head>

    



<body>

      <header class="App-header">
       
        <h1 class="App-title">مکاتبات خارج شرکت مشانیر</h1>


       <div class="App-logo">
      <img src="image/moshanir.png" alt="Games Logo" />
          
       </div>
    </header>









 





    <form id="form1" runat="server">
        <fieldset>
            <legend>فرم اطلاعات ارسال مکاتبات  خارج شرکت  </legend>
        <div>
            <label for="ReceiverCode">کد دریافت کننده:</label>
            
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        </div>
        <div>
            <label for="DepartReceiver"> دپارتمان دریافت کننده:</label>

            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
               
        </div>
        <div>
            <label for="Organization">نام ارگان دریافت کننده:</label>

            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
               
        </div>


        <div>
            <label for="Position"> سمت گیرنده:</label>

            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
               
        </div>


         <div>
            <label for="LetterNo"> شماره نامه:</label>

            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
               
        </div>
    <input id="File1" type="file" name="Attachments" multiple ="multiple" runat="server" />

        <div>
            <label for="Subject"> موضوع نامه:</label>

            <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
               
        </div>
















          <div>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="MAKE XML" />
          </div>
            
       
        </fieldset>

    </form>
</body>

    <script type="text/javascript">



        var fileVal = document.getElementById("File1");
        var yourfile = (fileVal.value);


     </script>








</html>
