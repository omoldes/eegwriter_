/*  ===================
 *  | EEG Writer v1.0 |
 *  ===================
 *  by Óscar Moldes and PMA
 *  
 *  Todo comentario del estilo
 *      // * Comentario
 *  quiere decir que es puramente
 *  estético y prescindible
 *  
*/ 



//importamos las librerías a usar
using System;
using Emotiv;
using System.IO;
using System.Threading;

namespace eegwriter
{
    class Program
    {
        //definimos el ID del usuario. Deafult: -1. El primer usuario será el 0
        // * también definimos un booleano para saber cuando se ha conectado la diadema por primera vez
        static int userID = -1;
        static bool hasBeenConectedMsg = false;
        //definimos el nombre del archivo a crear, proporcionando una extensión (csv, txt...)
        static string filename = DateTime.Now.ToString("yy.MM.dd HH.mm.ss") + ".csv";
        //creamos el objeto que escriba en el archivo (el StreamWriter lleva un booleano según sobreescriba o no)
        static TextWriter file = new StreamWriter(filename, false);
        //definimos una subrutina según la cual recibamos la ID de la diadema
        // * también mostramos un mesaje de éxito
        static void engine_UserAdded_Event(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("Emotiv has been connected successfully!");
            userID = (int)e.userId;
        }
        //definimos una subrutina mediante la cual recibamos los datos de la diadema
        static void engine_EmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            //creamos un objeto para la diadema
            EmoState es = e.emoState;
            //obtenemos los datos de la primera diadema conectada
            if (e.userId != 0) return;
            //definimos las variables: Tiempo, Señal y Batería
            float timeFromStart = es.GetTimeFromStart();
            EdkDll.IEE_SignalStrength_t signalStrength = es.GetWirelessSignalStatus();

            Int32 chargeLevel = 0;
            Int32 maxChargeLevel = 0;
            es.GetBatteryChargeLevel(out chargeLevel, out maxChargeLevel);
            //escribimos dichos datos en el archivo 
            // * y también por consola
            Console.WriteLine(" Time: " + timeFromStart);
            file.Write(timeFromStart + ",");

            Console.WriteLine(" Signal Strength: " + signalStrength); 
            file.Write(signalStrength + ",");

            Console.WriteLine(" Battery level: " + chargeLevel);
            file.Write(chargeLevel + ",");
            //leemos los datos de cada uno de los sensores y los escribimos en el archivo

            /*** Emotiv Insight's Sensors ***/
            /*
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_AF3) + ",");
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_T7) + ",");
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_O1) + ","); 	// Pz
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_T8) + ",");
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_AF4) + ",");
            */

            /*** Emotiv Epoc+'s Sensors ***/
          
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_CMS) + ","); //0 ¿?
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_DRL) + ","); //1 ¿?
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_FP1) + ","); //2
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_AF3) + ","); //3
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_F7) + ",");  //4
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_F3) + ",");  //5
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_FC5) + ","); //6
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_T7) + ",");  //7
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_P7) + ",");  //8
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_O1) + ",");  //9
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_O2) + ",");  //10
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_P8) + ",");  //11
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_T8) + ",");  //12
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_FC6) + ","); //13
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_F4) + ",");  //14
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_F8) + ",");  //15
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_AF4) + ","); //16
            file.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_FP2) + ","); //17
           
            //acabamos la línea en la que estamos escribiendo. En el caso de un csv, pasamos a una nueva fila
            file.WriteLine("");
        }
        //escribimos el main()
        static void Main(string[] args)
        {
            // * cambiamos el tamaño de la consola para que no estorbe demasiado a la hora de grabar los datos
            Console.SetWindowSize(48,27);
            // * mostramos un mensaje de bienvenida
            Console.WriteLine("\t     ===================");
            Console.WriteLine("\t     | EEG Writer v1.0 |");
            Console.WriteLine("\t     ===================");
            Console.WriteLine("      To stop the program, press any key.\n");

            //creamos una instancia para poder acceder a todas las funciones de la diadema
            EmoEngine engine = EmoEngine.Instance;
            //añadimos ambas subrutinas creadas anteriormente como eventos de la diadema
            engine.UserAdded += new EmoEngine.UserAddedEventHandler(engine_UserAdded_Event);
            engine.EmoStateUpdated += new EmoEngine.EmoStateUpdatedEventHandler(engine_EmoStateUpdated);
            //encendemos la conexion con la diadema
            engine.Connect();
            //creamos un objeto de nuestra clase
            Program p = new Program();
            //creamos un header para el archivo (en el caso del csv, se escribe en cada columna poniendo comas (,) )

            /*** Emotiv Insight's Header ***/

            //string header = "Seconds,Decimals,Wireless Strength,Battery Level,AF3,T7,Pz,T8,AF4";

            /*** Emotiv Epoc+'s Header ***/

            string header = "Seconds,Decimals,Wireless Strength,Battery Level,CMS,DRL,FP1,AF3,F7,F3,FC5,T7,P7,O1,O2,P8,T8,FC6,F4,F8,AF4,FP2";
            
            //escribimos la cabecera en el archivo e y pasamos a la siguiente línea
            file.WriteLine(header);
            //bucle infinito para la ejecución prolongada del programa
            while (true)
            {
                //para el programa cuando se pulse una tecla
                if (Console.KeyAvailable)
                    break;
                //ejecutar subrutinas creadas anteriormente
                engine.ProcessEvents();
                //si no hay ningún usuario conectado, no realizará medición alguna
                if ((int)userID == -1)
                {
                    //mostramos un mensaje para cuando no haya ninguna diadema conectada
                    if (!hasBeenConectedMsg)
                    {
                        Console.WriteLine(" Please, connect an Emotiv headset!");
                        hasBeenConectedMsg = true;
                    }

                    continue;
                }
                //deelay de lectura (Default: 10 ms)
                Thread.Sleep(10);
            }
            //cerramos el archivo en el que estemos escribiendo
            file.Close();
            //cerramos la conexión con la diadema
            engine.Disconnect();
        }
    }
}

