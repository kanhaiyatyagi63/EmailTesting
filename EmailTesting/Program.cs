// See https://aka.ms/new-console-template for more information
using CoreHtmlToImage;
using SendGrid;
using SendGrid.Helpers.Mail;

Console.WriteLine("Hello, World!");

var source = @"
                    <tr>
                        <td>
                            <table cellpadding=""8""
                                style=""border: 1px solid #b8c1ce; border-spacing: 0; background: rgba(0,0,0,0.04);table-layout: fixed;width:100%"">
                                <thead style=""background-color: #333;color: #fff;font-size: 15px; font-weight: 600;"">
                                    <tr>
                                        <td>Year</td>
                                        <td>Make</td>
                                        <td>Model</td>
                                        <td>Color</td>
                                        <td>Stock #</td>
                                    </tr>
                                </thead>
                                <tbody style=""font-size: 15px;color: #333;"">
                                    <tr>
                                        <td>2021</td>
                                        <td>Jeep</td>
                                        <td>Grand Cherokee L</td>
                                        <td>Gray</td>
                                        <td>NG160128A</td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3 style=""color:#0c1b2f;font-size:16px;font-weight:600;"">Recon Package Summary</h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellpadding=""8""
                                style=""border: 1px solid #b8c1ce; border-spacing: 0; background: rgba(0,0,0,0.04);table-layout: fixed;width:60%"">
                                <thead style=""background-color: #333;color: #fff;font-size: 15px; font-weight: 600;"">
                                    <tr>
                                        <td></td>
                                        <td>Total Cost ($)</td>
                                        <td>Cost ($)</td>
                                        <td>Net ($)</td>
                                    </tr>
                                </thead>
                                <tbody style=""font-size: 15px;color: #333;"">
                                    <tr>
                                        <td>Internal</td>
                                        <td>275.00</td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Reconditioning</td>
                                        <td>250.00</td>
                                        <td>267.50</td>
                                        <td>-17.50</td>
                                    </tr>
                                    <tr>
                                        <td>Added Cosmetics</td>
                                        <td>125.00</td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Quoted Work</td>
                                        <td>0</td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Total Cost Store</td>
                                        <td>650.00</td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>";

//<tr> <img src=""cid:GUID54321"" alt=""testing""/> </tr>

var htmlContent = @"<!doctype html>
<html>

<body>
    <table style=""width:100%;font-family:sans-serif;border-collapse:collapse;"">
        <tr>
            <td>
                <table style=""width:100%;background:#fdfdfd;padding:20px 20px;"">
                     <tr>
                        <td>
                            <h3 style=""color:#0c1b2f;font-size:16px;font-weight:600;"">Dear Ganesh</h3>
                            <p style=""color:#6c6c6c;font-size:16px;line-height:26px;""> This vehicle is awaiting your
                                review: </p>
                        </td>
                    </tr>
                    <tr> <img src=""cid:GUID54321"" alt=""Recon Package Approval""/> </tr>
					<tr>
                        <td>
                            <p style=""color:#000;font-size:16px;line-height:26px; margin: 20px 0;""> You can open <a
                                    style=""text-decoration:none;color:red;""
                                    href=""https://localhost:44307/vehicle/redirect/recon/package/29/995/19209/76?company=3&store=29""
                                    target=""_blank""> recon package approval form</a> or <a
                                    style=""text-decoration:none;color:red;""
                                    href=""https://localhost:44307/approve/recon/package/995/19209/e3705572b14942df9506bd24e9bae7b4/b145fff0-f22b-43ff-965b-eeec50a53917/76?company=3&store=29""
                                    target=""_blank"">approve</a> directly. </p>
                            <p style=""color:#6c6c6c;font-size:16px;line-height:26px;""> Note: The approval link expires
                                in 48 hours. </p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h5 style=""margin: 0;color: #0c1b2f;font-size:16px;"">Thank you,</h5>
                            <p style=""margin-top:8px;color:#929292;font-size:16px;"">Recon Partners</p>
                            <p><img style=""width: 160px;"" alt=""Recon Partners logo""
                                    src=""https://localhost:44307/assets/images/recon-partners-logo.png"" /></p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>";


var converter = new HtmlConverter();
var bytes = converter.FromHtmlString(source,quality:500);
var file = Convert.ToBase64String(bytes);

Execute(file).GetAwaiter().GetResult();

Console.ReadKey();


async Task Execute(string file)
{
    try
    {
        //removed names and email because due to data breach policy
        var fromEmail = "";
        var fromName = "";
        var ToEmail = "";
        var ToName = "";
        var sendGridKey = "";
        var client = new SendGridClient(sendGridKey);
        var messageEmail = new SendGridMessage()
        {
            From = new EmailAddress(fromEmail, fromName),
            Subject = "POC!",
            HtmlContent = htmlContent
        };
        messageEmail.AddTo(new EmailAddress(ToEmail, ToName));
        messageEmail.AddAttachment("Image.jpg", file, null, "inline", "GUID54321");
        var response = await client.SendEmailAsync(messageEmail);
    }
    catch (Exception ex)
    {
        throw ex;
    }
}