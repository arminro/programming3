// <copyright file="VisibilityConverter.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts bool to visibility
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converting from bool to visibility
        /// </summary>
        /// <param name="value">the given value</param>
        /// <param name="targetType">the type of the target</param>
        /// <param name="parameter">the parameter, if there is one</param>
        /// <param name="culture">info on culture settings</param>
        /// <returns>visibility</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            return val == true ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="targetType">the target type</param>
        /// <param name="parameter">the parameter, if there is one</param>
        /// <param name="culture">info on the culture</param>
        /// <returns>nothing</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
