using DecisionTree.Extensions;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DecisionTree.Controls
{
    public partial class ConnectionPropertiesBox
    {
        public LabeledConnectionLine BindedConnection { set; get; }

        #region ConnectionNumber Property
        public int ConnectionNumber
        {
            get { return (int)GetValue(ConnectionNumberProperty); }
            set { SetValue(ConnectionNumberProperty, value); }
        }

        public static readonly DependencyProperty ConnectionNumberProperty =
            DependencyProperty.Register("ConnectionNumber", typeof(int), typeof(ConnectionPropertiesBox), new PropertyMetadata(1, OnConnectionNumberChanged));

        private static void OnConnectionNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectionPropertiesBox control = d as ConnectionPropertiesBox;
            int newValue = (int)e.NewValue;

            if (control != null)
            {
                control.ConnectionTitle.Text = "Connection #" + newValue;
            }
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
            DependencyProperty.Register("HasProbability", typeof(bool), typeof(ConnectionPropertiesBox), new PropertyMetadata(true, OnHasProbabilityChanged));

        private static void OnHasProbabilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectionPropertiesBox control = d as ConnectionPropertiesBox;
            Visibility newVisibility = ((bool)e.NewValue) ? Visibility.Visible : Visibility.Collapsed;

            if (control != null)
            {
                control.ProbabilityRow1.Visibility = newVisibility;
                control.ProbabilityRow2.Visibility = newVisibility;
            }
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
                BindingHelper.RegisterBinding("Label", BindedConnection.Connection, LabelTextBox, TextBox.TextProperty, BindingMode.TwoWay);
                BindingHelper.RegisterBinding("Payout", BindedConnection.Connection, PayoutTextBox, TextBox.TextProperty, BindingMode.TwoWay);
                BindingHelper.RegisterBinding("Probability", BindedConnection.Connection, ProbabilityTextBox, TextBox.TextProperty, BindingMode.TwoWay);
            }
        }

        private void ProbabilityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            char[] allowedChars = "0123456789.".ToCharArray();

            TextBox box = sender as TextBox;

            if (box != null)
            {
                int selection = box.SelectionStart;
                string newText = "";
                bool hadDecimalSeparator = false;

                foreach (char item in box.Text)
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

            if (box != null)
            {
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
}
