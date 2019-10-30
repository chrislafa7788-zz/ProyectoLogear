using System;
using ExamenBelatrix;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProyectoBelatrix
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LogearErrorConsola()
        {
            // JobLogger( logToFile,  logToConsole,  logToDatabase,  logMessage,  logWarning,  logError)
            JobLogger instruccion = new JobLogger(false, true, false, false, false, true);

            Assert.AreEqual(JobLogger.LogMessage("error ! soy un mensaje de error", false, false, true), true);

        }


        [TestMethod]
        public void LogearWarningConsola()
        {

            JobLogger instruccion = new JobLogger(false, true, false, false, true, false);

            Assert.AreEqual(JobLogger.LogMessage("Warning ! soy un mensaje de Warning", false, true, false), true);

        }


        [TestMethod]
       
        public void ReciboMensajeYquieroErrorPorArchivo()
        {
            // JobLogger( logToFile,  logToConsole,  logToDatabase,  logMessage,  logWarning,  logError)
            JobLogger instruccion = new JobLogger(true, false, false, false, true, false);

            //Devuelve True porque yo TENGO un mensaje pero quiero solo guardar por archivo los ERRORES. 
            //Entonces no se llega a ejecutar nunca el Save File. ( Lo evalua y no lo hace, e imprime true)

            Assert.AreEqual(JobLogger.LogMessage("Mensaje ! soy un mensaje ", true, false, false), true);

        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExcepcionTodosNuull()
        {

            JobLogger instruccion = new JobLogger(false, false, false, false, false, false);


            JobLogger.LogMessage("error ! soy un mensaje de error", true, false, false);



        }
        



    }
}
