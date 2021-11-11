using System.Windows;
using System.Windows.Media;

namespace Calculations
{
    partial class Controller
    {
        public class FontController
        {
            /// <summary>
            ///     Resets font family and size settings, then updates the relevant windows.
            /// </summary>
            public static void Reset()
            {
                PrivateReset();
                DisplaySettings();
            }

            private static void PrivateReset()
            {
                bool radians = Settings.Default.Radians;
                string fractionalFormat = Settings.Default.AnswerFormat;
                string historyItems = Settings.Default.HistoryItemsCanAppear;
                bool historyOpen = Settings.Default.HistoryWindowIsOpen;
                double width = Settings.Default.MainWindowWidth;
                double height = Settings.Default.MainWindowHeight;
                double historyWidth = Settings.Default.HistoryWindowWidth;
                double historyHeight = Settings.Default.HistoryWindowHeight;
                bool remember = Settings.Default.RememberHistoryForNextTime;
                Settings.Default.Reset();

                Settings.Default.Radians = radians;
                Settings.Default.AnswerFormat = fractionalFormat;
                Settings.Default.HistoryItemsCanAppear = historyItems;
                Settings.Default.HistoryWindowIsOpen = historyOpen;
                Settings.Default.MainWindowWidth = width;
                Settings.Default.MainWindowHeight = height;
                Settings.Default.HistoryWindowWidth = historyWidth;
                Settings.Default.HistoryWindowHeight = historyHeight;
                Settings.Default.RememberHistoryForNextTime = remember;
                Settings.Default.Save();
            }

            private static void DisplaySettings()
            {
                Default.CalculatorWindow.SetFontFamily(Family.MainFamily,
                    Family.GetFamilyOfNumberOperatorAndFunctionButtons());

                Default.CalculatorWindow.SetFontSizes(Size.DefaultMainCalc, Size.DefaultAnswer, Size.DefaultTextboxes,
                    Size.DefaultTabs, Size.DefaultDigitAndSymbolButtons, Size.DefaultFunctionAndEButtons,
                    Size.DefaultUIButtons, Size.DefaultUILabels);

                Default.FontWindow?.SetSizeForControls(Size.DefaultUILabels);
                Default.AboutWindow?.SetFontSize(Size.DefaultUILabels);

                if (Default.HistoryWindow is null)
                    return;

                Default.HistoryWindow.SetListboxFontSize(Size.DefaultMainCalc);
                Default.HistoryWindow.SetListboxFontFamily(Family.MainFamily);
                Default.HistoryWindow.SetButtonFontSizes(Size.DefaultUIButtons);
            }


            public class Size
            {
                public const int DefaultAnswer = 17;
                public const int DefaultMainCalc = 15;
                public const int DefaultDigitAndSymbolButtons = 15;
                public const int DefaultFunctionAndEButtons = 14;
                public const int DefaultTextboxes = 13;
                public const int DefaultUIButtons = 13;
                public const int DefaultUILabels = 13;
                public const int DefaultTabs = 12;

                /// <summary>
                ///     Sets the font sizes for the Main Window at startup (and history window, if it is opened at startup).
                /// </summary>
                public static void Startup()
                {
                    int change = Settings.Default.FontSizeRelativeToDefault;
                    Default.CalculatorWindow.SetFontSizes(DefaultMainCalc + change, DefaultAnswer + change,
                        DefaultTextboxes + change, DefaultTabs + change, DefaultDigitAndSymbolButtons + change,
                        DefaultFunctionAndEButtons + change, DefaultUIButtons + change, DefaultUILabels + change);

                    Default.HistoryWindow?.SetListboxFontSize(DefaultMainCalc + change);
                    Default.HistoryWindow?.SetListboxFontFamily(Family.MainFamily);
                    Default.HistoryWindow?.SetButtonFontSizes(DefaultUIButtons + change);
                }

                /// <summary>
                ///     Increases the font size for Main Window (and History and About windows, if open) by one point.
                /// </summary>
                public static void Increase()
                {
                    if (Settings.Default.FontSizeRelativeToDefault == 2)
                        return;

                    Settings.Default.FontSizeRelativeToDefault++;
                    Settings.Default.Save();
                    Default.CalculatorWindow.ChangeFontSizesOfMainControls(1);
                    Default.CalculatorWindow.ChangeFontSizesOfUIElements(1);

                    if (Default.HistoryWindow is not null)
                    {
                        Default.HistoryWindow.SetListboxFontSize(DefaultMainCalc +
                                                                 Settings.Default.FontSizeRelativeToDefault);
                        Default.HistoryWindow.ChangeButtonFontSizes(1);
                    }

                    Default.AboutWindow?.ChangeFontSize(1);
                }

                /// <summary>
                ///     Decreases the font size for Main Window (and History and About windows, if open) by one point.
                /// </summary>
                public static void Decrease()
                {
                    if (Settings.Default.FontSizeRelativeToDefault == -2)
                        return;

                    Settings.Default.FontSizeRelativeToDefault--;
                    Settings.Default.Save();
                    Default.CalculatorWindow.ChangeFontSizesOfMainControls(-1);
                    Default.CalculatorWindow.ChangeFontSizesOfUIElements(-1);

                    if (Default.HistoryWindow is not null)
                    {
                        Default.HistoryWindow.SetListboxFontSize(DefaultMainCalc +
                                                                 Settings.Default.FontSizeRelativeToDefault);
                        Default.HistoryWindow.ChangeButtonFontSizes(-1);
                    }

                    Default.AboutWindow?.ChangeFontSize(-1);
                }
            }

            public class Family
            {
                /// <summary>
                ///     Gets the name of the family, based on the setting.
                /// </summary>
                public static string MainFamilyAsString => Settings.Default.FontFamily;

                /// <summary>
                ///     Gets the family, based on the setting.
                /// </summary>
                public static FontFamily MainFamily =>
                    Settings.Default.FontFamily == "Hack"
                        ? (FontFamily)Application.Current.Resources["Hack"]
                        : new FontFamily(Settings.Default.FontFamily);

                /// <summary>
                ///     Gets the family applicable, based on the setting.
                /// </summary>
                /// <returns></returns>
                public static FontFamily GetFamilyOfNumberOperatorAndFunctionButtons() =>
                    Settings.Default.FontFamilyIsAlsoForNumberOperatorAndFunctionButtons
                        ? MainFamily
                        : new FontFamily("SegoeUI");

                /// <summary>
                ///     Updates the setting and updates the Main Window.
                /// </summary>
                /// <param name="value"></param>
                public static void SetMainFamilyIsAlsoForNumberOperatorAndFunctionButtons(bool value)
                {
                    Settings.Default.FontFamilyIsAlsoForNumberOperatorAndFunctionButtons = value;
                    Settings.Default.Save();
                    Default.CalculatorWindow.SetFontFamily(MainFamily, GetFamilyOfNumberOperatorAndFunctionButtons());
                }

                /// <summary>
                ///     If provided fonts cannot be found, resets the setting. Sets the font families for the History listbox and Main
                ///     Window.
                /// </summary>
                public static void Startup()
                {
                    if ((FontFamily)Application.Current.Resources["Hack"] is null &&
                        Settings.Default.FontFamily == "Hack")
                    {
                        Settings.Default.FontFamily = "SegoeUI";
                        Settings.Default.Save();
                    }

                    Default.HistoryWindow?.SetListboxFontFamily(MainFamily);
                    Default.CalculatorWindow.SetFontFamily(MainFamily, GetFamilyOfNumberOperatorAndFunctionButtons());
                }

                /// <summary>
                ///     Changes the setting and sets the font families for the History listbox and Main Window.
                /// </summary>
                /// <param name="familyKey"></param>
                public static void ChangeTo(string familyKey)
                {
                    Settings.Default.FontFamily = familyKey;
                    Settings.Default.Save();
                    Default.CalculatorWindow.SetFontFamily(MainFamily, GetFamilyOfNumberOperatorAndFunctionButtons());
                    Default.HistoryWindow?.SetListboxFontFamily(MainFamily);
                }
            }
        }
    }
}