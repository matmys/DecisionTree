using System.Linq;
using System.Windows.Input;
using DecisionTree.Extensions;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.IO;
using System.Text;
using System.Windows;
using System;

namespace DecisionTree.Controls
{
    public partial class DesignerGrid
    {
        public readonly KeyGestureManager KeyGestureManager = new KeyGestureManager();

        public DesignerGrid()
        {
            InitializeComponent();

            connectionDecorator.InitializeConnections(this);
            rubberbandSelection.InitializeConnections(this);

            KeyGestureManager.DelKeyGesture += Delete_KeyGesture;
            KeyGestureManager.CrtlAKeyGesture += SelectAll_KeyGesture;
            KeyGestureManager.CrtlNKeyGesture += Clear_KeyGesture;
        }

        #region Clear Command
        private void Clear_KeyGesture(object sender, KeyEventArgs e)
        {
            this.Clear();
        }

        public void Clear()
        {
            foreach (DesignerItem item in this.Children.OfType<DesignerItem>().ToList())
            {
                item.Delete();
            }
        }
        #endregion

        #region Delete Command
        private void Delete_KeyGesture(object sender, KeyEventArgs e)
        {
            foreach (LabeledDesignerItem item in this.SelectedItems.ToList())
            {
                item.Delete();
            }
        }
        #endregion

        #region SelectAll Command
        private void SelectAll_KeyGesture(object sender, KeyEventArgs e)
        {
            foreach (DesignerItem item in this.Designer.Children.OfType<DesignerItem>())
            {
                item.IsSelected = true;
            }
        }
        #endregion

        #region ExportAsImage Command
        private void ExportAsImage_KeyGesture(object sender, KeyEventArgs e)
        {
            ImageSaver.SaveAsPNG(this.Designer);
        }
        #endregion

        public void SaveContent()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Decision Tree Files (.dtf)|*.dft";

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
            };

            IEnumerable<ISerializable> toSerialize = this.Children.OfType<ISerializable>();

            if (saveFileDialog.ShowDialog() == true)
            {
                string json = "[\r\n  ";

                foreach (ISerializable item in toSerialize)
                {
                    settings.ContractResolver = new DynamicContractResolver(item.PropertiesToSerialize);
                    string temp = JsonConvert.SerializeObject(item, Formatting.Indented, settings);
                    temp = temp.Replace("\r\n", "\r\n  ");
                    json += temp + ",\r\n  ";
                }

                json = json.Substring(0, json.Length - 5);
                json += "\r\n]";


                Stream stream = saveFileDialog.OpenFile();
                byte[] textBytes = UTF8Encoding.UTF8.GetBytes(json);
                stream.Write(textBytes, 0, textBytes.Length);
                stream.Close();
            }
        }

        public void LoadContent()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Decision Tree Files (.dtf)|*.dft|All Files (*.*)|*.*";

            string json = "";

            if (openFileDialog.ShowDialog() == true)
            {
                this.Clear();

                Stream stream = openFileDialog.File.OpenRead();
                StreamReader reader = new StreamReader(stream);
                json = reader.ReadToEnd();
                stream.Close();

                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
                };

                IEnumerable<ISerializable> items;

                try
                {
                    items = JsonConvert.DeserializeObject<IEnumerable<ISerializable>>(json, settings);

                    foreach (LabeledDesignerItem item in items.OfType<LabeledDesignerItem>())
                    {
                        this.AddDesignerItem(item);
                    }

                    foreach (LabeledConnectionLine line in items.OfType<LabeledConnectionLine>())
                    {
                        DesignerItem parent = this.GetDeisgnerItemById(line.ParentId);
                        DesignerItem child = this.GetDeisgnerItemById(line.ChildId);

                        if (parent != null && child != null)
                        {
                            line.Connect(parent, child);
                            this.AddConnectionLine(line);
                        }
                    }
                }
                catch (System.Exception)
                {
                    MessageBox.Show("File is corrupt!");
                }  
            }  
        }

        public void SymetricAutoLayout()
        {
            LabeledDesignerItem item = this.Children.OfType<LabeledDesignerItem>().FirstOrDefault();

            if (item != null)
            {
                LabeledDesignerItem root = item.ModelItem.Root.ItemPresenter as LabeledDesignerItem;

                double[] connectionLengths = new double[0];

                root.SymetricMeasure(1, ref connectionLengths);

                root.SymetricAlign(20, 10 + (root.FamilySize.Height / 2), connectionLengths, 1);
            }
        }

        public void CondensedAutoLayout()
        {
            LabeledDesignerItem item = this.Children.OfType<LabeledDesignerItem>().FirstOrDefault();

            if (item != null)
            {
                LabeledDesignerItem root = item.ModelItem.Root.ItemPresenter as LabeledDesignerItem;

                double[] connectionLengths = new double[0];

                root.CondensedMeasure(1, ref connectionLengths);

                root.CondensedAlign(20, 10 + (root.FamilySize.Height / 2), connectionLengths, 1);
            }
        }

        public void SaveContentAsPNG()
        {
            ImageSaver.SaveAsPNG(this.Designer);
        }
    }
}
