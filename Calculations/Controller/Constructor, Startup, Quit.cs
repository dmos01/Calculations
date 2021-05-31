using System;
using System.Globalization;
using System.IO;
using System.Windows;
using EquationElements;
using NumberFormats;

namespace Calculations
{
    partial class Controller
    {
        /// <summary>
        ///     Loads settings stored as strings. Creates Constant and History classes, and list of functions.
        /// </summary>
        private Controller()
        {
            CurrentAnswerFormat = NumberFormat.GetFromTypeAsString(Settings.Default.AnswerFormat);
            if (CurrentAnswerFormat.GetType() == typeof(DecimalNumberFormat))
            {
                Settings.Default.AnswerFormat = new DecimalNumberFormat().TypeAsString;
                Settings.Default.Save();
            }

            switch (Settings.Default.HistoryItemsCanAppear)
            {
                case "Once":
                    historyItemsSetting = HistoryItemsCanAppear.Once;
                    break;
                case "ManyTimes":
                    historyItemsSetting = HistoryItemsCanAppear.ManyTimes;
                    break;
                case "OnceButMoveToTopIfAddedAgain":
                    historyItemsSetting = HistoryItemsCanAppear.OnceButMoveToTopIfAddedAgain;
                    break;
                default:
                    historyItemsSetting = HistoryItemsCanAppear.Once;
                    Settings.Default.HistoryItemsCanAppear = "MoveToTop";
                    Settings.Default.Save();
                    break;
            }

            constants = new ConstantsController(AppDomain.CurrentDomain.BaseDirectory + "/Calculations Constants.xml");
            history = new HistoryController(AppDomain.CurrentDomain.BaseDirectory + "/Calculations History.txt");
        }

        /// <summary>
        ///     Restores history, if Remember History. Restores history window if necessary. Loads Constants and adds Pi and E.
        ///     Loads Main Window width and height. Call font startups.
        /// </summary>
        /// <param name="thisCalculatorWindow"></param>
        public static void Startup(MainWindow thisCalculatorWindow)
        {
            Default.CalculatorWindow = thisCalculatorWindow;

            Constants.ImportConstants(null, false);
            Constants.AddOrUpdate(ElementsResources.PiWord, Math.PI.ToString(CultureInfo.CurrentCulture), "",
                CalculationsResources.PiDescription, false);
            Constants.AddOrUpdate(ElementsResources.EulersSymbolUpperCase, Math.E.ToString(CultureInfo.CurrentCulture),
                "",
                CalculationsResources.EulersDescription);

            if (Settings.Default.RememberHistoryForNextTime && File.Exists(History.HistoryPath))
                History.ImportHistory();

            Default.CalculatorWindow.Width = Settings.Default.MainWindowWidth;
            Default.CalculatorWindow.Height = Settings.Default.MainWindowHeight;
            Default.CalculatorWindow.Left =
                (SystemParameters.PrimaryScreenWidth / 2) - (Default.CalculatorWindow.Width / 2);
            Default.CalculatorWindow.Top =
                (SystemParameters.PrimaryScreenHeight / 2) - (Default.CalculatorWindow.Height / 2) - 20;

            FontController.Size.Startup();
            FontController.Family.Startup();

            if (Settings.Default.HistoryWindowIsOpen)
                ShowHistoryWindow();
        }

        /// <summary>
        ///     Saves history, if Remember History, or deletes it. Saves width and height of main and history windows.
        /// </summary>
        public static void Quit()
        {
            if (Settings.Default.RememberHistoryForNextTime)
                History.ExportHistory();
            else if (File.Exists(History.HistoryPath))
                File.Delete(History.HistoryPath);

            Settings.Default.MainWindowWidth = Default.CalculatorWindow.Width;
            Settings.Default.MainWindowHeight = Default.CalculatorWindow.Height;
            if (Default.HistoryWindow != null)
            {
                Settings.Default.HistoryWindowWidth = Default.HistoryWindow.Width;
                Settings.Default.HistoryWindowHeight = Default.HistoryWindow.Height;
            }

            Settings.Default.Save();
        }
    }
}