﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EquationCalculator {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CalculatorExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CalculatorExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EquationCalculator.CalculatorExceptionMessages", typeof(CalculatorExceptionMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  is invalid..
        /// </summary>
        public static string LogToNegativeOrZeroAfterParameter {
            get {
                return ResourceManager.GetString("LogToNegativeOrZeroAfterParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Logs cannot use negative numbers or 0. .
        /// </summary>
        public static string LogToNegativeOrZeroBeforeParameter {
            get {
                return ResourceManager.GetString("LogToNegativeOrZeroBeforeParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Logs cannot use negative numbers or 0..
        /// </summary>
        public static string LogToNegativeOrZeroDefault {
            get {
                return ResourceManager.GetString("LogToNegativeOrZeroDefault", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No equation to calculate..
        /// </summary>
        public static string NoEquationDefault {
            get {
                return ResourceManager.GetString("NoEquationDefault", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot determine whether an &apos;E&apos; is *10^ or Euler&apos;s Number (2.718 to 3dp)..
        /// </summary>
        public static string UndeterminedUseOfEDefault {
            get {
                return ResourceManager.GetString("UndeterminedUseOfEDefault", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unknown problem happened while calculating..
        /// </summary>
        public static string UnknownErrorDefault {
            get {
                return ResourceManager.GetString("UnknownErrorDefault", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot; was found but no value for it was provided..
        /// </summary>
        public static string ValueOfVariableNotProvidedAfterParameter {
            get {
                return ResourceManager.GetString("ValueOfVariableNotProvidedAfterParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Variable &quot;.
        /// </summary>
        public static string ValueOfVariableNotProvidedBeforeParameter {
            get {
                return ResourceManager.GetString("ValueOfVariableNotProvidedBeforeParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A Variable was found but no value for it was provided..
        /// </summary>
        public static string ValueOfVariableNotProvidedDefault {
            get {
                return ResourceManager.GetString("ValueOfVariableNotProvidedDefault", resourceCulture);
            }
        }
    }
}
