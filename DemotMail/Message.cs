using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Net;

namespace DemotMail
{
    class Message
    {
        private SmtpClient client;

        /// <summary>
        /// Lista wszystkich załączników
        /// </summary>
        private List<Attachment> att;
        /// <summary>
        /// Lista adresów url do plików które mają zostać załaczone w mailu
        /// </summary>
        private List<string> files;
        /// <summary>
        /// Adres url który ma zostać przeszukany np http://demotywatory.pl 
        /// </summary>
        string url = "";
     
        /// <summary>
        /// Konstruktor wymaga podania konta na gmailu z którego mają zostac
        /// wysłane maile oraz hasła do niego.
        /// </summary>
        /// <param name="GmailAcount">adres gmail</param>
        /// <param name="pass">hasło</param>
        public Message(string GmailAcount, string pass)
        {
            att = new List<Attachment>();
            files = new List<string>();
            client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential(GmailAcount, pass);
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        /// <summary>
        /// Utawia adres url
        /// </summary>
        /// <param name="_url"></param>
        public void SetUrl(string _url)
        {
            url=_url;
        }
       
        /// <summary>
        /// Zwraca nazwę pliku z adresu url np. dla aaa/bbb/ccc.jpg zwróci ccc.jpg 
        /// </summary>
        /// <param name="path">adres url</param>
        /// <returns>Nazwa pliku</returns>
        private string GetNameFromPath(string path)
        {
            string[] Separator = new string[] { "/" };
            string[] name = (path.Split(Separator, StringSplitOptions.None));
            return name[name.Length - 1];
        }
   
        /// <summary>
         /// Dodaje załączniki do listy załączników - att
         /// </summary>
         /// <param name="phrase">fraza jaką mają zawierać opisy pod obrazkami
         /// aby zostały wysłane</param>
        private void GetAttachments(string phrase)
        {
         
            PicturesFromHtml pictures = new PicturesFromHtml(url);
            files.Clear();
            att.Clear();
            pictures.AdFileIf(files,phrase); // dodaje adresy plików do wysania pod kątem szukanej frazy

            LogFile.AddLog("Rozpoczęto dodawanie załączników");
            foreach (string file in files)
            {
                try
                {   
 
                    var name  = GetNameFromPath(file);
                    var stream = new WebClient().OpenRead(file); 
                    Attachment data = new Attachment(stream, name);
                    att.Add(data);

                    LogFile.AddLog("Prawidłowo dodano załącznik - " + name);
                }
                catch
                {
                    LogFile.AddLog("Dodawanie załącznika nie powiodło sie - " + file);
                }
            }
            LogFile.AddLog("Zakończono dodawanie załączników");
        }
 
        /// <summary>
        /// Wysyła maila na podane konto z załącznikami spełniającymi konkretne założenia
        /// </summary>
        /// <param name="to">Do kogo ma zostać wysłany mail</param>
        /// <param name="phrase">fraza jak ma wystąpić przy obrazku</param>
        public void Send(string to, string phrase)
        {
            
            MailMessage mail = new MailMessage("demotmailtest@gmail.com", to, "DemotMessage", "Your daily demot report. Selected phrase: " + phrase);
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            GetAttachments(phrase); // zbiera załączniki do listy

            LogFile.AddLog("Rozpoczęto operację wysyłania maila");

            if (att.Capacity != 0)
            {
                foreach (Attachment data in att) // dodaje załączniki do maila
                    mail.Attachments.Add(data);
                try
                {
                    client.Send(mail);
                    LogFile.AddLog("Wysłano mail");
                }
                catch
                {
                    LogFile.AddLog("Próba wysłania maila nie powiodła sie");
                }
            }
            else
            {
                LogFile.AddLog("Nie wysłano maila ponieważ żaden plik nie spełnieł wymagań");
            }
        }
        
    }
}
