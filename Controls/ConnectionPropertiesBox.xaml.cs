using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace DecisionTree.Controls
{
    public partial class ConnectionPropertiesBox : UserControl
    {
        public LabeledConnectionLine BindedConnection { set; get; }

        #region ConnectionNumber Property
        public int ConnectionNumber
        {
            get { return (int)GetValue(ConnectionNumberProperty); }
            set { SetValue(ConnectionNumberProperty, value); }
        }

        public static readonly DependencyProperty ConnectionNumberProperty =
            DependencyProperty.Register("ConnectionNumber", typeof(int), typeof(ConnectionPropertiesBox), new PropertyMetadata(1, new PropertyChangedCallback(OnConnectionNumberChanged)));

        private static void OnConnectionNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectionPropertiesBox control = d as ConnectionPropertiesBox;
            int newValue = (int)e.NewValue;

            control.ConnectionTitle.Text = "Connection #" + newValue;
        }
        #endregion

        #region HasProbability Property
        public bool HasProbability
        {
            get { return (bool)GetValue(HasProbabilityProperty); }
            set { SetValue(HasProbabilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasProbability.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasProbabilityProperty =
            DependencyProperty.Register("HasProbability", typeof(bool), typeof(ConnectionPropertiesBox), new PropertyMetadata(true, new PropertyChangedCallback(OnHasProbabilityChanged)));

        private static void OnHasProbabilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectionPropertiesBox control = d as ConnectionPropertiesBox;
            Visibility newVisibility = ((bool)e.NewValue) ? Visibility.Visible : Visibility.Collapsed;

            control.ProbabilityRow1.Visibility = newVisibility;
            control.ProbabilityRow2.Visibility = newVisibility;
        }
        #endregion

        public ConnectionPropertiesBox()
        {
            InitializeComponent();
        }

        public ConnectionPropertiesBox(int number, bool hasProbability)
        {
            InitializeComponent();
            ConnectionNumber = number;
            HasProbability = hasProbability;
        }

        public void InitializeBindings()
        {
            if (BindedConnection != null)
            {
                this.registerBinding("Label", LabelTextBox, TextBox.TextProperty);
                this.registerBinding("Payout", PayoutTextBox, TextBox.TextProperty);
                this.registerBinding("Probability", ProbabilityTextBox, TextBox.TextProperty);
            }
        }

        private void registerBinding(string propertyName, FrameworkElement targetElement, DependencyProperty targetProperty, string stringFormat="")
        {
            Binding binding = new Binding(propertyName);

            binding.Source = BindedConnection.Connection;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            binding.Mode = BindingMode.TwoWay;
            binding.StringFormat = stringFormat;

            targetElement.SetBinding(targetProperty, binding);
        }

        private void ProbabilityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            char[] allowedChars = "0123456789.".ToCharArray();

            TextBox box = sender as TextBox;

            int selection = box.SelectionStart;
            string newText = "";
            bool hadDecimalSeparator = false;

            foreach (char item in box.Text.ToCharArray())
            {
                if (allowedChars.Contains(item))
                {
                    if (item == '.')
                    {
                        if (!hadDecimalSeparator)
                        {
                            newText += item;
                            hadDecimalSeparator = true;
                        }
                    }
                    else
                    {
                        newText += item;
                    }
                }
            }

            if (newText != "" && newText[0] == '.')
            {
                newText = "";
            }

            string testString = newText.Replace('.', CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]);

            if (testString != "" && double.Parse(testString) > 100)
            {
                newText = "100";
            }

            box.Text = newText;
            box.SelectionStart = selection;
        }

        private void Label_GotFocus(object sender, RoutedEventArgs e)
        {
            this.BindedConnection.IsFocused = true;
        }

        private void Label_LostFocus(object sender, RoutedEventArgs e)
        {
            this.BindedConnection.IsFocused = false;
        }

        private void PayoutTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            char[] allowedChars = "0123456789.-".ToCharArray();

            TextBox box = sender as TextBox;

            int selection = box.SelectionStart;
            int textLength = box.Text.Length;
            string newText = "";
            bool hadDecimalSeparator = false;

            for (int i = 0; i < box.Text.Length; i++)
            {
                char c = box.Text[i];

                if (allowedChars.Contains(c))
                {
                    if (c == '.')
                    {
                        if (!hadDecimalSeparator)
                        {
                            newText += c;
                            hadDecimalSeparator = true;
                        }
                    }
                    else if (c != '-' || (c == '-' && i == 0))
                    {
                        newText += c;
                    }
                }
            }

            if (newText != "" && newText[0] == '.')
            {
                newText = "";
            }

            box.Text = newText;

            box.SelectionStart = selection - (textLength - newText.Length);
        }
    }
}
