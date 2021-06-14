namespace Calculations
{
    partial class Controller
    {
        private WinAbout AboutWindow;
        private MainWindow CalculatorWindow;
        private WinFont FontWindow;
        private WinHistory HistoryWindow;

        /// <summary>
        ///     If the window is already open, focuses on that instance. Otherwise, creates one and loads settings. Sets the
        ///     history window to open on startup next time.
        /// </summary>
        public static void ShowHistoryWindow()
        {
            if (Default.HistoryWindow is null)
            {
                Default.HistoryWindow = new WinHistory
                {
                    Width = Settings.Default.HistoryWindowWidth,
                    Height = Settings.Default.HistoryWindowHeight,
                    Owner = Default.CalculatorWindow
                };
                Default.HistoryWindow.SetButtonFontSizes(FontController.Size.DefaultUIButtons +
                                                         Settings.Default.FontSizeRelativeToDefault);
                Default.HistoryWindow.SetListboxFontSize(FontController.Size.DefaultMainCalc +
                                                         Settings.Default.FontSizeRelativeToDefault);
                Default.HistoryWindow.SetListboxFontFamily(FontController.Family.MainFamily);

                History.DisplayItems();

                Default.HistoryWindow.Show();
                Settings.Default.HistoryWindowIsOpen = true;
                Settings.Default.Save();
            }
            else
            {
                Default.HistoryWindow.Focus();
            }
        }

        /// <summary>
        ///     Saves the width and height. If the program isn't quitting, sets the history window to not open on startup next
        ///     time.
        /// </summary>
        /// <param name="programIsQuitting"></param>
        public static void CloseHistoryWindow(bool programIsQuitting = false)
        {
            Settings.Default.HistoryWindowWidth = Default.HistoryWindow.Width;
            Settings.Default.HistoryWindowHeight = Default.HistoryWindow.Height;
            if (programIsQuitting == false)
                Settings.Default.HistoryWindowIsOpen = false;
            Settings.Default.Save();
            Default.HistoryWindow = null;
        }

        /// <summary>
        ///     If the window is already open, focuses on that instance. Otherwise, creates one.
        /// </summary>
        public static void ShowFontWindow()
        {
            if (Default.FontWindow is null)
            {
                Default.FontWindow = new WinFont {Owner = Default.CalculatorWindow};
                Default.FontWindow.SetSizeForControls(FontController.Size.DefaultUILabels +
                                                      Settings.Default.FontSizeRelativeToDefault);
                Default.FontWindow.Show();
            }
            else
            {
                Default.FontWindow.Focus();
            }
        }

        public static void CloseFontWindow()
        {
            Default.FontWindow = null;
        }

        /// <summary>
        ///     If the window is already open, focuses on that instance. Otherwise, creates one.
        /// </summary>
        public static void ShowAboutWindow()
        {
            if (Default.AboutWindow is null)
            {
                Default.AboutWindow = new WinAbout {Owner = Default.CalculatorWindow};
                Default.AboutWindow.SetFontSize(FontController.Size.DefaultUILabels +
                                                Settings.Default.FontSizeRelativeToDefault);
                Default.AboutWindow.Show();
            }
            else
            {
                Default.AboutWindow.Focus();
            }
        }

        public static void CloseAboutWindow()
        {
            Default.AboutWindow = null;
        }
    }
}