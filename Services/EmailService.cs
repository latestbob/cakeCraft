using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;
    
    public EmailService()
    {
        // Configure SMTP settings (Replace with your real email settings)
        _smtpServer = "sandbox.smtp.mailtrap.io"; // SMTP Server (e.g., smtp.sendgrid.net)
        _smtpPort = 2525; // SMTP Port (Use 587 for TLS, 465 for SSL)
        _smtpUser = "09cd77aaed8a44"; // SMTP Username (Your Email)
        _smtpPass = "e98f99efa1b422"; // SMTP Password (Use environment variables for security!)
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using (var client = new SmtpClient(_smtpServer, _smtpPort))
        {
            client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
            client.EnableSsl = true; // Enable SSL/TLS

            var mailMessage = new MailMessage
            {
                From = new MailAddress("hello@cakecraftng.ng"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true // If sending HTML content
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
