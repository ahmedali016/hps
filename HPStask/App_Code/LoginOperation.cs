using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using HPStask.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace HPStask.App_Code
{
    internal class LoginOperation
    {
        HPSDBEntities hp = new HPSDBEntities();
        internal string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        internal int login(string user, string pass)
        {
            MD5 md5 = MD5.Create();
            var hashed = GetMd5Hash(md5, pass);
            var result = hp.Patients.FirstOrDefault(c => c.pass == hashed && c.userName == user);
            if (result != null)
            {
                return result.id;
            }
            else
            {
                return 0;
            }
        }

        internal bool checkUser(int id)
        {
            var user = hp.Patients.FirstOrDefault(c => c.id == id);
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal bool resetPass(string mail)
        {
            var user = hp.Patients.FirstOrDefault(c => c.mail == mail);
            if (user != null)
            {
                Random r = new Random();
                var rn = r.Next(1000, 20000).ToString();
                var hash = GetMd5Hash(MD5.Create(), rn);
                user.pass = hash;
                hp.SaveChanges();
                sendMail(rn, mail);

            }

            return false;
        }
        public string sendMail(string message, string mail)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("mail@gmail.com");
            msg.To.Add(mail);
            msg.Subject = "reset! " + DateTime.Now.ToString();
            msg.Body = "hi to you your new password is " + message;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("mail@gmail.com", "xxxxxx");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                return "Mail has been successfully sent!";
            }
            catch (Exception ex)
            {
                return "Fail Has error" + ex.Message;
            }
            finally
            {
                msg.Dispose();
            }
        }
    }
}