﻿// <copyright file="SaveConverter.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.ViewModel
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Windows.Data;

    public class SaveConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // we will never have to convert back, so it's safe to use this
            return Binding.DoNothing;
        }
    }
}
