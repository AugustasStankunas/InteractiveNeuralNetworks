using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using Python.Included;
using System.IO;
using System.Windows;

namespace InteractiveNeuralNetworks
{
	public class PythonTest
	{
		/// <summary>
		/// Grąžina python funkcijos return'ą (šiuo atvėju stringą)
		/// kuris išspaudinamas ExecuteClickMeButton Messagebox'e
		/// </summary>
		/// <returns>Pythono funkcijos return'ą. </returns>
		public static string RunPythonHelloWorld()
		{
			try
			{
				//Bazinė direktorija iš kurios reikės grįžti į aukštesnę direktoriją kurioje yra norimas .py failas
				//Base directory: \Source\Repos\InteractiveNeuralNetworks\bin\Debug\net.8.0-windows7.0\ (jeigu reikėtų vėliau)
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				string pythonFolderPath = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\Python"));

				Installer.SetupPython(); // Pythong.Included NuGet package, kad nereikėtų ranka tvarkytis python versijų.
				PythonEngine.Initialize();

				using (Py.GIL())
				{
					dynamic sys = Py.Import("sys");
					sys.path.append(pythonFolderPath);
					dynamic helloWorld = Py.Import("HelloWorld");
					dynamic result = helloWorld.Hello_World();

					return result;
				}

			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public static void ShutdownEngine()
		{
			if (PythonEngine.IsInitialized)
			{
				PythonEngine.Shutdown();
			}
			else return;

		}
	}
}
