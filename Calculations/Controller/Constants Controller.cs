using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EquationElements;
using EquationElements.Operators;
using static EquationElements.Utils;


namespace Calculations
{
    partial class Controller
    {
        public class ConstantsController
        {
            //SortedDictionary instead of SortedSet because the latter requires an object of the same type when using .Contains().
            private SortedDictionary<string, Constant> sortedConstants { get; }

            private string ConstantsPath { get; }

            public ConstantsController(string constantsPath)
            {
                sortedConstants = new SortedDictionary<string, Constant>(StringComparer.CurrentCultureIgnoreCase);
                ConstantsPath = constantsPath;
            }

            /// <summary>
            ///     Get copy without spaces.
            /// </summary>
            /// <returns></returns>
            public IDictionary<string, string> GetNameValuePairs() => sortedConstants.ToDictionary(pair => pair.Key,
                pair => pair.Value.ValueWithoutSpaces, StringComparer.CurrentCultureIgnoreCase);

            public bool Exists(string nameWithoutSpaces) => sortedConstants.ContainsKey(nameWithoutSpaces);

            public bool TryGetProperties(string constantName, out string value, out string unit, out string description)
            {
                constantName = RemoveSpaces(constantName);
                if (sortedConstants.TryGetValue(constantName, out Constant constant))
                {
                    value = constant.Value;
                    description = constant.Description;
                    unit = constant.Unit;
                    return true;
                }

                value = null;
                description = null;
                unit = null;
                return false;
            }

            public void Delete(string nameWithoutSpaces, bool saveAfterDelete = true)
            {
                if (nameWithoutSpaces is null || IsOperator.StringIsPiOrEulers(nameWithoutSpaces))
                    return;

                sortedConstants.Remove(nameWithoutSpaces);

                if (saveAfterDelete)
                    SaveConstants();
            }

            public void AddOrUpdate(string name, string value, string unit, string description, bool saveAfter = true)
            {
                string nameWithoutSpaces = RemoveSpaces(name);

                if (sortedConstants.ContainsKey(nameWithoutSpaces))
                    sortedConstants[nameWithoutSpaces].Overwrite(value, unit, description);
                else
                {
                    sortedConstants.Add(nameWithoutSpaces, new Constant(name, value, unit, description));
                }

                if (saveAfter)
                    SaveConstants();
            }

            public List<string> GetAllNames() => SearchAllFieldsAndReturnNames();

            public List<string> SearchAllFieldsAndReturnNames(string searchText = "")
            {
                searchText = RemoveSpaces(searchText);

                return sortedConstants
                    .Where(x => x.Key.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) ||
                                x.Value.Matches(searchText)).Select(x => x.Value.Name).ToList();
            }

            /// <summary>
            ///     Imports Constants. Skips invalid and duplicate Constants.
            /// </summary>
            /// <param name="path">The full path and file name to import from. If null, Controller default.</param>
            /// <param name="saveAfterImport">
            ///     Optionally specify if the list of Constants is saved to the application location after
            ///     import. If there are invalid or duplicate Constants in that file, they will be lost.
            /// </param>
            public void ImportConstants(string path = null, bool saveAfterImport = true)
            {
                if (IsNullEmptyOrOnlySpaces(path))
                    path = ConstantsPath;
                if (File.Exists(path) == false)
                    return;


                using (StreamReader reader = new(path))
                {
                    try
                    {
                        while (reader.ReadLine() != null) //<constant>
                        {
                            string name = ReadXmlField("name");
                            string value = ReadXmlField("value");
                            string unit = ReadXmlField("unit");
                            string description = ReadXmlField("description");

                            string nameWithoutSpaces = RemoveSpaces(name);

                            //Not checking value for validity for performance reasons.
                            if (ConstantNameIsValid(nameWithoutSpaces, out _) &&
                                !sortedConstants.ContainsKey(nameWithoutSpaces))
                            {
                                sortedConstants.Add(nameWithoutSpaces, new Constant(name, value, unit, description));
                            }

                            reader.ReadLine(); //</constant>
                        }

                        string ReadXmlField(string fieldIdentifier)
                        {
                            string output = reader.ReadLine();
                            output = output?.Remove(0, fieldIdentifier.Length + 2);
                            return output?.Remove((output.Length - 1) - (fieldIdentifier.Length + 2));
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                    finally
                    {
                        reader.Close();
                    }
                }

                if (saveAfterImport)
                    SaveConstants();
            }

            /// <summary>
            ///     Exports Constants to the application location. Will not export Pi and E. Deletes file if no Constants (except Pi
            ///     and E) exist.
            /// </summary>
            private void SaveConstants()
            {
                if (sortedConstants.Count > 2)
                    ExportConstants();
                else if (File.Exists(ConstantsPath))
                    File.Delete(ConstantsPath);
            }

            /// <summary>
            ///     Exports Constants that match the filter. Will not export Pi and E.
            /// </summary>
            /// <param name="path">The full path and file name to export to. If null, Controller default.</param>
            /// <param name="filter">Only Constants that match this filter will be exported. If null, export all Constants.</param>
            public void ExportConstants(string path = null, string filter = null)
            {
                if (IsNullEmptyOrOnlySpaces(path))
                    path = ConstantsPath;

                using StreamWriter writer = new(path);
                ICollection<Constant> toExport;

                if (IsNullEmptyOrOnlySpaces(filter)) //Export all
                {
                    toExport = sortedConstants.Where(x => !IsOperator.StringIsPiOrEulers(x.Key)).Select(x => x.Value)
                        .ToList();
                }
                else
                {
                    filter = RemoveSpaces(filter);
                    toExport = sortedConstants
                        .Where(x => !IsOperator.StringIsPiOrEulers(x.Key) && x.Value.Matches(filter))
                        .Select(x => x.Value).ToList();
                }

                foreach (Constant constant in toExport)
                {
                    writer.WriteLine("<constant>");
                    writer.WriteLine("<name>" + constant.Name + "</name>");
                    writer.WriteLine("<value>" + constant.Value + "</value>");
                    writer.WriteLine("<unit>" + constant.Unit + "</unit>");
                    writer.WriteLine("<description>" + constant.Description + "</description>");
                    writer.WriteLine("</constant>");
                }

                writer.Close();
            }

            public bool ConstantNameIsValid(string nameWithoutSpaces, out string errorMessage)
            {
                //Word Name is valid.
                try
                {
                    Word.ThrowExceptionIfNameIsInvalid(nameWithoutSpaces);
                }
                catch (Exception exception)
                {
                    errorMessage = exception.Message;
                    return false;
                }

                //Name contains invalid character.
                if (nameWithoutSpaces.Any(x => !(char.IsLetter(x) || char.IsDigit(x))))
                {
                    errorMessage = DialogResources.ConstantNameInvalidCharacterMessage;
                    return false;
                }

                //Name is Pi or Eulers
                if (IsOperator.StringIsPiOrEulers(nameWithoutSpaces))
                {
                    errorMessage = DialogResources.PiAndECannotBeModifiedMessage;
                    return false;
                }

                errorMessage = null;
                return true;
            }
        }
    }
}