using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace SpotlightBrowser
{
    /// <summary>
    /// Provides an implementation of INotifyPropertyChanged
    /// </summary>
    public class ViewModelBase 
        : INotifyPropertyChanged
    {
        private string m_name;

        public string Name
        {
            get { return m_name; }
            set { SetValue_(ref m_name, value, () => Name); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged_(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void OnPropertyChanged_<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
            {
                throw new ArgumentNullException("selectorExpression");
            }

            var body = selectorExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("The body must be a member expression");
            }

            OnPropertyChanged_(body.Member.Name);
        }

        protected bool SetValue_<T>(ref T field, T value, Expression<Func<T>> selectorExpression)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged_(selectorExpression);
            return true;
        }
    }
}
