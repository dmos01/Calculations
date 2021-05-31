using NumberFormats;

namespace Calculations
{
    partial class Controller
    {
        /// <summary>
        ///     Updates the setting.
        /// </summary>
        /// <param name="radians">If false, degrees.</param>
        public static void ChangeRadiansOrDegrees(bool radians)
        {
            Settings.Default.Radians = radians;
            Settings.Default.Save();
            History.UpdateRadiansOrDegrees();
        }

        /// <summary>
        ///     Updates the setting.
        /// </summary>
        public static void ChangeAnswerFormat(NumberFormat newFormat)
        {
            CurrentAnswerFormat = newFormat;
            Settings.Default.AnswerFormat = newFormat.TypeAsString;
            Settings.Default.Save();
            History.UpdateAnswerFormats();
        }

        /// <summary>
        ///     Updates the setting that history is saved to an application file for next time to application runs.
        /// </summary>
        /// <param name="remember"></param>
        public static void ChangeRememberHistory(bool remember)
        {
            Settings.Default.RememberHistoryForNextTime = remember;
            Settings.Default.Save();
        }

        /// <summary>
        ///     Updates the setting. moveToTop will not be relevant if onlyAppearOnce is false.
        /// </summary>
        /// <param name="onlyAppearOnce"></param>
        /// <param name="moveToTop"></param>
        public static void ChangeHistoryItemsCanAppear(bool onlyAppearOnce, bool moveToTop)
        {
            if (onlyAppearOnce == false)
                Default.historyItemsSetting = HistoryItemsCanAppear.ManyTimes;
            else if (moveToTop == false)
                Default.historyItemsSetting = HistoryItemsCanAppear.Once;
            else
                Default.historyItemsSetting = HistoryItemsCanAppear.OnceButMoveToTopIfAddedAgain;

            Settings.Default.HistoryItemsCanAppear = HistoryItemsSetting.ToString();
            Settings.Default.Save();
        }
    }
}