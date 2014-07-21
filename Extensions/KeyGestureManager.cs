using System.Windows.Input;

namespace DecisionTree.Extensions
{
    public class KeyGestureManager
    {
        public delegate void KeyGestureHandler(object sender, KeyEventArgs e);

        //Control + KeyGestures
        public event KeyGestureHandler CrtlAKeyGesture;
        public event KeyGestureHandler CrtlNKeyGesture;

        //Other KeyGestures
        public event KeyGestureHandler DelKeyGesture;

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
                                this.CrtlAKeyGesture(sender, e);
                                break;
                            }
                        case Key.N:
                            {
                                this.CrtlNKeyGesture(sender, e);
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
                            this.DelKeyGesture(sender, e);
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
                        this.DelKeyGesture(sender, e);
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
