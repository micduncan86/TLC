using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace TLC.Data
{
    public class Email
    {
        private const string smtpHost = "smtp.gmail.com";
        private const int smtpPort = 465;
        


        public static string Send(List<string> toAddresses, string subject, string bodyContent)
        {
            var jSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string,object> creds = (Dictionary<string,object>)jSerializer.DeserializeObject(System.Text.Encoding.Default.GetString(Convert.FromBase64String(System.Configuration.ConfigurationManager.AppSettings.Get("smtpCredentials"))));
            using(var smtpClient = new System.Net.Mail.SmtpClient())
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Host = smtpHost;
                smtpClient.Port = smtpPort;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new System.Net.NetworkCredential(creds["UserName"].ToString(), creds["Password"].ToString());                
                smtpClient.Timeout = 20000;
                var Message = new System.Net.Mail.MailMessage();
                try
                {
                    
                    Message.From = new System.Net.Mail.MailAddress("teamleadercentral.app@gmail.com");
                    foreach(string to in toAddresses)
                    {
                        Message.To.Add(to);
                    }
                    Message.Subject = subject;
                    Message.Body = bodyContent;

                    smtpClient.Send(Message);
                    return "Message has been sent.";
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return ex.Message + ex.InnerException.Message;
                    }
                    
                    return  ex.Message;
                }
                finally
                {
                    Message.Dispose();
                }

            }
        }
    }
}
