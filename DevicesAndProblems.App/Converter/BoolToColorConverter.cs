﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DevicesAndProblems.App.Converter
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new SolidColorBrush(Colors.Black);
            }

            return System.Convert.ToBoolean(value) ?
                new SolidColorBrush(Colors.Red)
              : new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
