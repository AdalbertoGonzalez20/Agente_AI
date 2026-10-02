// Define el espacio de nombres principal de la aplicación.
namespace Agente_AI
{
    /// <summary>
    /// Contiene el punto de entrada de la aplicación Windows Forms.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Inicia la aplicación y muestra el formulario principal.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configura los valores predeterminados de Windows Forms, como DPI y fuentes.
            ApplicationConfiguration.Initialize();

            // Abre el formulario del agente de inteligencia artificial.
            Application.Run(new FrmAgenteIA());
        }
    }
}
