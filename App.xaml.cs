using System;
using System.Windows;

namespace DecisionTree
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new MainPage();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // Jeśli aplikacja działa poza debugerem, to powiadom o wyjątku używając
            // mechanizmu wyjątków przeglądarki. W programie Internet Explorer zostanie wyświetlona żółta ikona alertu 
            // na pasku stanu, a program Firefox wyświetli błąd skryptu.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // Uwaga: Pozwoli to aplikacji kontynuować działanie po zgłoszeniu wyjątku,
                // który nie został obsłużony. 
                // W aplikacjach produkcyjnych obsługa błędów powinna zostać zastąpiona tak, żeby 
                // powiadomić witrynę sieci Web o błędzie i zatrzymać aplikację.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
