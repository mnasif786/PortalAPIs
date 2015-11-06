namespace PBS.Portal.API.Data.EmailTemplate
{
    public static class EmailHtmlTemplate
    {
        // must do find replace "" with " before editing this in any HTML editor
        // this should really be a .html file but had issues in the current working directory remaining consistent in the test environment, and LocalIIS
        public static string EmailContent = @"
<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta name = ""generator"" content=""HTML Tidy for Linux/x86 (vers 25 March 2009), see www.w3.org"" />
    <meta http-equiv=""Content-Type"" content=""text/html; charset=us-ascii"" />
    <meta charset = ""UTF-8"" />
    <meta content=""width=device-width, initial-scale=1"" name=""viewport"" />
    <meta content = ""Peninsula Business services"" name=""description"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />

    <title>Peninsula Business services</title>
</head>

<body style = ""background-color: #f2f1f1; height: 100%;"" bgcolor=""#F2F1F1"">
    <font style = ""display:none; visibility:hidden; font-size:1px; color:#f2f1f1;"">
        Peninsula
        Business Services now offers a new way to access your documents
    </font>

    <table style = ""width: 100%; border-collapse: collapse; height: 100%; background-color: #fff; max-width: 600px; margin-top: 5px; margin-left:auto; margin-right:auto; box-shadow:0px 0px 3px rgba(0,0,0,0.1);""
           bgcolor=""#fff"">
        <tr style = ""height: 100px;"">
            <td style=""text-align: left; padding-left: 5px;"" align=""left"">
                <img src = ""http://peninsulabusinesservices.co.uk/images/peninsula_logonew.png"" width=""177px""
                     height=""103px"" style=""width: 177px; height: 103px; text-align: left;"" title=""peninsula business services"" />
            </td>

            
        </tr>

        <tr>
            <td colspan = ""2"">
                <table style=""width:100%;"">
                    <tr>
                        <td>
                            <p style = ""font-family:arial; font-size:14px; padding:10px; line-height:20px;"">
                            %EMailContent%
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr>
            <td colspan=""2"">
                <table style = ""background-color:#222222; width:100%; color:#fff; padding:0;"">
                    <tr>
                        <td style=""padding:0; vertical-align:top;"">
                            <p style = ""color: #fff; font-size: 16px; font-family: 'arial'; font-weight: normal; text-transform:uppercase; padding-left:10px;"">
                                Contact Us<br />
                                <span style = ""color: #fff; font-size: 27px;"">
                                    <a href=""tel:0844%892%2775""
                                       style=""color: #fff; font-size: 15px; text-decoration:none;"">
                                        0844 892 2775
                                    </a><br />
                                    <a href = ""mailto:client.satisfaction@peninsula-uk.com"" style=""color: #fff; font-size: 15px; text-decoration:none; text-transform:lowercase;"">
                                        client.satisfaction@peninsula-uk.com
                                     </a>
                                </span>
                            </p>
                        </td>

                        <td></td>

                        <td style = ""text-align:right; padding:0; vertical-align:top;"">
                            <p style=""color: #fff; font-size: 16px; font-family: 'arial'; font-weight: normal; text-transform:uppercase; padding-left:10px; margin-top:0px; padding-right:10px;"">
                                CONNECT WITH US
                            </p>
                            <a style = ""text-decoration:none"" target=""_blank"" href=""https://twitter.com/pbstweets"">
                                <img src = ""http://www.peninsulagrouplimited.com/wp-content/themes/island/dist/images/twitter_circle_color.png""
                                     style=""width:35px; max-width:35px !important; height:35px;""
                                     height=""35"" 
                                     width=""35"" 
                                     border=""0"" />
                            </a>

                            <a style = ""text-decoration:none"" target=""_blank"" href=""https://www.linkedin.com/company/peninsula-business-services"">
                                <img src = ""http://www.peninsulagrouplimited.com/wp-content/themes/island/dist/images/linkedin_circle_color.png""
                                     style=""width:35px; max-width:35px !important; height:35px;""
                                     height=""35"" 
                                     width=""35"" 
                                     border=""0"" />
                            </a> 
                            
                            <a style = ""text-decoration:none"" target=""_blank"" href=""https://www.youtube.com/user/pbspressoffice"">
                                <img src = ""http://www.peninsulagrouplimited.com/wp-content/themes/island/dist/images/youtube_circle_color.png""
                                     style=""width:35px; max-width:35px !important; height:35px;""
                                     height=""35"" 
                                     width=""35"" 
                                     border=""0"" />
                            </a>
                            
                            <a style = ""text-decoration:none"" target=""_blank"" href=""https://plus.google.com/113238170522849146848/posts"">
                                <img src = ""http://www.peninsulagrouplimited.com/wp-content/themes/island/dist/images/google_circle_color.png""
                                     style=""width:35px; max-width:35px !important; height:35px; margin-right:10px;"" 
                                     height=""35"" 
                                     width=""35"" 
                                     border=""0"" />
                            </a>
                        </td>
                    </tr>

                    <tr>
                        <td colspan = ""3"">
                            <p style=""font-family:arial; font-size:12px; color:#fff; padding-left:10px; padding-right:10px; text-align:center;"">
                                &#169; 2015 Peninsula Business Services Limited. Registered Office: The
                                Peninsula, Victoria Place, Manchester, M4 4FB.Registered in England and
                                Wales No: <span style = ""color:#fff;""> 1702759.</span>
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
";

    }
}