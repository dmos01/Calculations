using Microsoft.Win32;

namespace Calculations
{
    partial class Controller
    {
        /// <summary>
        ///     Returns the file name from an OpenFileDialog, or empty.
        /// </summary>
        /// <param name="dialogTitle">The title of the dialog window.</param>
        /// <param name="filters">Example: Image files (*.bmp, *.jpg)|*.bmp;*.jpg|All files (*.*)|*.*</param>
        /// <param name="defaultExtension">Without point or star.</param>
        /// <returns>The filename or empty.</returns>
        public static string RunImportDialogAndReturnFilePath(string dialogTitle, string filters,
            string defaultExtension)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Title = dialogTitle,
                FileName = "",
                Filter = filters,
                DefaultExt = defaultExtension,
                AddExtension = true,
                ValidateNames = true,
                CheckFileExists = true,
                CheckPathExists = true
            };
            return open.ShowDialog() == true ? open.FileName : "";
        }

        /// <summary>
        ///     Returns the file name from a SaveFileDialog, or empty.
        /// </summary>
        /// <param name="dialogTitle">The title of the dialog window.</param>
        /// <param name="filters">Example: Image files (*.bmp, *.jpg)|*.bmp;*.jpg|All files (*.*)|*.*</param>
        /// <param name="defaultExtension">Without point or star.</param>
        /// <returns>The filename or empty.</returns>
        public static string RunExportDialogAndReturnFilePath(string dialogTitle, string filters,
            string defaultExtension)
        {
            SaveFileDialog save = new SaveFileDialog
            {
                Title = dialogTitle,
                FileName = "",
                Filter = filters,
                DefaultExt = defaultExtension,
                AddExtension = true,
                ValidateNames = true
            };
            return save.ShowDialog() == true ? save.FileName : "";
        }
    }
}