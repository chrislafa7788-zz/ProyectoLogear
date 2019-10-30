using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenBelatrix
{
    using System;
    using System.Linq;
    using System.Text;
    public class JobLogger
    {
        private static bool _logToFile;
        private static bool _logToConsole;
        private static bool _logMessage;
        private static bool _logWarning;
        private static bool _logError;
        private static bool _LogToDatabase;




        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError)
                {
                    _logError = logError;
                    _logMessage = logMessage;
                    _logWarning = logWarning;
                    _LogToDatabase = logToDatabase;
                    _logToFile = logToFile;
                    _logToConsole = logToConsole;
                }

       
        public static bool LogMessage(string message_str, bool message_b, bool warning, bool error)
        {

            //validaciones
            if (string.IsNullOrWhiteSpace(message_str)  ) {
                throw new Exception("Message must be specified");
            }
           
            

            if (!_logToConsole && !_logToFile && !_LogToDatabase)
                {
                    throw new Exception("Invalid configuration, please set at least one type of log");
                }



            if ((!message_b && !warning  && !error))
                {
                    throw new Exception("Error, Warning or Message must be specified");
                }



            //saco el tipo de mensaje
            Mensaje mensaje = new Mensaje { };
                  

            if (message_b == true) {
                mensaje.Tipo= 1;
            }else if (error == true)
            {
                mensaje.Tipo = 2;
            }else if (warning == true)
            {
                mensaje.Tipo = 3;
            }

            mensaje.Descripcion = message_str;


            bool[] arr = new bool[3];
            arr[0] = _logMessage;
            arr[1] = _logError;
            arr[2] = _logWarning;


            for (int i = 0; i < arr.Length; i++) {

                if ( i == mensaje.Tipo-1 && (arr[i]==true) ) { //estoy parado sobre el mensaje? es true? entonces logeo el mensaje.



                    if (_LogToDatabase) // Base de Datos
                    {
                        if (!funcionLogBD(mensaje))
                        {
                            throw new Exception("Error saving the log to DB");
                        }

                    }


                    if (_logToConsole) // Consola
                    {
                        if (!funcionLogConsola(mensaje))
                        {

                            throw new Exception("Error printing the log to console");
                        }
                    }

                    if (_logToFile) // Archivo
                    {
                        if (!funcionLogArchivo(mensaje))
                        {

                            throw new Exception("Error saving the log  to file");
                        }


                    }



                }//end if 
               
                    
                    
       }//end for


            return true;
        }


        private static bool funcionLogConsola(Mensaje mensaje)
        {


            switch (mensaje.Tipo)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.White; //mensaje
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red; //error
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Yellow; //warning
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

            }


            Console.WriteLine(DateTime.Now.ToShortDateString() + mensaje.Descripcion);

           


            return true;




        }



        private static bool funcionLogBD(Mensaje mensaje)
        {

            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
            connection.Open();
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("Insert into Log Values('" + mensaje.Descripcion + "', " + mensaje.Tipo.ToString() + ")");
            command.ExecuteNonQuery();
            connection.Close();

            return true;

        }

        private static bool funcionLogArchivo(Mensaje mensaje) {


            string l = "";

            if (!System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
            {
                l = System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
            }

            l = l + DateTime.Now.ToShortDateString() + mensaje.Descripcion;


            System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", l);


            return true;

        }



    }
}
