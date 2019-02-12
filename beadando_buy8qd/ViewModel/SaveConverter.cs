// <copyright file="SaveConverter.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.ViewModel
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Windows.Data;

    /// <summary>
    /// converter to convert a badly-formatted save full path into a visually appealing one to the user
    /// </summary>
    public class SaveConverter : IValueConverter
    {
        /// <summary>
        /// ctor for the converter
        /// </summary>
        /// <param name="value">the value coming form binding</param>
        /// <param name="targetType">the type coming from the binding</param>
        /// <param name="parameter">any paramteres the converter might have</param>
        /// <param name="culture">localization info</param>
        /// <returns>returns a well-formatted string</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string unformatted = (string)value;

            // split the text
            string[] temp = unformatted.Split('\\');

            // now we have only the name
            StringBuilder builder = new StringBuilder(temp[temp.Length - 1]);

            // we clip the ".xml" part
            builder.Remove(builder.Length - 4, 4);

            // we replace the '_' s with ':'s to make it more user-friendly
            builder.Replace('_', ':');
            return builder.ToString();
        }

        /// <summary>
        /// Does not do anything for the time being
        /// </summary>
        /// <param name="value">the val from binding</param>
        /// <param name="targetType">target type</param>
        /// <param name="parameter">any parameter the converter might have</param>
        /// <param name="culture">provides cukture info</param>
        /// <returns>literally noting</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // we will never have to convert back, so it's safe to use this
            return Binding.DoNothing;
        }
    }
}
