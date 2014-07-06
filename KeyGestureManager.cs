using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DecisionTree
{
    public class KeyGestureManager
    {
        public delegate void KeyGestureHandler(object sender, KeyEventArgs e);

        //Control + KeyGestures
        public event KeyGestureHandler Crtl_A_KeyGesture;

        //Other KeyGestures
        public event KeyGestureHandler Del_KeyGesture;

        public void RiseEvent(object sender, KeyEventArgs e)
        {
            switch (Keyboard.Modifiers)
            {
                case ModifierKeys.Control:
                {
                    switch (e.Key)
                    {
                        case Key.A:
                            {
                                Crtl_A_KeyGesture(sender, e);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    break;
                }

                case ModifierKeys.None:
                {
                    switch (e.Key)
                    {
                        case Key.Delete:
                        {
                            Del_KeyGesture(sender, e);
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                    break;
                }

                default:
                {
                    break;
                }
            }

            if (Keyboard.Modifiers == ModifierKeys.None)
            {
                switch (e.Key)
                {
                    case Key.Delete:
                    {
                        Del_KeyGesture(sender, e);
                        break;
                    }

                    default:
                    {
                        break;
                    }
                }
            }
        }
    }
}
