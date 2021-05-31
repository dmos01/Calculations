using NumberFormats;

namespace Calculations
{
    partial class Controller
    {
        public enum HistoryItemsCanAppear
        {
            Once,
            OnceButMoveToTopIfAddedAgain,
            ManyTimes
        }

        private readonly ConstantsController constants;
        private readonly HistoryController history;
        private HistoryItemsCanAppear historyItemsSetting;

        private static Controller Default { get; } = new Controller();


        public static ConstantsController Constants => Default.constants;
        public static HistoryController History => Default.history;
        public static HistoryItemsCanAppear HistoryItemsSetting => Default.historyItemsSetting;

        public static bool Radians => Settings.Default.Radians;

        public static NumberFormat CurrentAnswerFormat { get; private set; }
    }
}