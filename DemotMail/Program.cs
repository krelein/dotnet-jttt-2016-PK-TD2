using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Text;
using System.Net.Mime;
using System.IO;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
namespace DemotMail
{


    static class Program
    {
 
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
        
            LogFile.StartLog();
          
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
         
      

       //     LogFile.AddLog("Koniec działania programu");
        }
    }

    [Serializable]
    class Zadanie
    {
        public Zadanie(string a, string b, string c, string d){
            nazwatekstu = a;
            url = b;
            adres = c;
            tekst = d;
            
        }
       public readonly string nazwatekstu;
       public readonly string url;
       public readonly string adres;
       public readonly string tekst;

        public override string ToString()
       {
           return "Nazwa zadania = " + nazwatekstu + "; Warunek =  " + tekst + "; Na stronie =  " + url + "; Wykonaj = " + adres;
       }
    }

    
    static class ListaZadan
    {
        
        static public BindingList<Zadanie> lista=new BindingList<Zadanie>();
        static public void Serialize()
        {
            
            FileStream fs = new FileStream("Datafile.dat", FileMode.Create);
            
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, lista);
                LogFile.AddLog("wykonano serializacje");
            }
            catch (SerializationException e)
            {
                LogFile.AddLog("serializacja nie powiodla sie" + e.Message);
            }
            finally
            {
                fs.Close();
            }     
        }
        static public void Deserialize()
        {
          
            FileStream fs = new FileStream("Datafile.dat", FileMode.Open);
            try 
            {
                BinaryFormatter formatter = new BinaryFormatter();
                lista = (BindingList<Zadanie>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                LogFile.AddLog("deserializacja nie powiodla sie" + e.Message);
            }
            finally
            {
                fs.Close();
            }

        }

      

    }


}
