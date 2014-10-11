using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using LINQtoCSV;

namespace WaremeteGadget
{
    /// <summary>
    /// ConfigWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string filepath = Directory.GetCurrentDirectory() + @"\data\charinit.csv";

            //CSVファイル読み込み
            var context = new CsvContext();
            var description = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                TextEncoding = Encoding.GetEncoding(932)
            };

            //キャラクター名でグルーピング
            var charData = (
                from data in context.Read<CharDataFile>(filepath, description).Skip(1)
                select data).ToLookup(p => p.CharName);

            this.comboBox_char.ItemsSource = charData;

            var sizeData = new List<SizeData>();
            sizeData.Add(new SizeData() { Label = "Small", Value = "1" });
            sizeData.Add(new SizeData() { Label = "Medium", Value = "2" });
            sizeData.Add(new SizeData() { Label = "Large", Value = "3" });
            sizeData.Add(new SizeData() { Label = "Face", Value = "4" });
            this.comboBox_size.ItemsSource = sizeData;

            //サイズが選択されるまでは他のComboBoxを無効にする
            EnableComboBox(false);
        }


        private void comboBox_pose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboBox_pose.SelectedItem == null) return;

            //選択された立ち絵
            var charData = (CharDataFile)this.comboBox_pose.SelectedItem;

            //服装・表情データ読み込み
            string size = ((SizeData)this.comboBox_size.SelectedItem).Value;
            string filename = charData.PoseFile + "_" + size + ".txt";
            string filepath = Directory.GetCurrentDirectory() + @"\data\" + filename;

            if (!File.Exists(filepath))
            {
                MessageBox.Show("このポーズは使用することが出来ません。\n他のポーズを選択して下さい。", "",
                    MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InitializeComboBox();
                return;
            }

            var context = new CsvContext();
            var description = new CsvFileDescription
            {
                SeparatorChar = '\t',
                FirstLineHasColumnNames = false,    //TODO ヘッダの末尾に\tがあるファイルがあるのでtrueにすると落ちる
                EnforceCsvColumnAttribute = true,
                TextEncoding = Encoding.GetEncoding(932)
            };

            var imageData =
                from data in context.Read<ImageDataFile>(filepath, description).Skip(2)
                select data;
            
            this.comboBox_dress.ItemsSource = imageData.Where(s => s.Name.IndexOf("腕") == 0);
            this.comboBox_eye.ItemsSource = imageData.Where(s => s.Name.IndexOf("目_") != -1);
            this.comboBox_mouth.ItemsSource = imageData.Where(s => s.Name.IndexOf("口_") != -1);
        }


        private void comboBox_char_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeComboBox();            
        }


        private void comboBox_size_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboBox_size.SelectedItem != null)
            {
                EnableComboBox(true);
            }
        }


        private void InitializeComboBox()
        {
            this.comboBox_dress.ItemsSource = null;
            this.comboBox_dress.Items.Clear();
            this.comboBox_eye.ItemsSource = null;
            this.comboBox_eye.Items.Clear();
            this.comboBox_mouth.ItemsSource = null;
            this.comboBox_mouth.Items.Clear();
        }

        private void EnableComboBox(bool isEnable)
        {
            this.comboBox_char.IsEnabled = isEnable;
            this.comboBox_pose.IsEnabled = isEnable;
            this.comboBox_dress.IsEnabled = isEnable;
            this.comboBox_eye.IsEnabled = isEnable;
            this.comboBox_mouth.IsEnabled = isEnable;
        }


        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            DataBanker banker = DataBanker.GetInstance;

            banker["SizeInfo"] = this.comboBox_size.SelectedItem;
            banker["CharaInfo"] = this.comboBox_pose.SelectedItem;
            banker["DressImage"] = this.comboBox_dress.SelectedItem;
            banker["EyeImage"] = this.comboBox_eye.SelectedItem;
            banker["MouthImage"] = this.comboBox_mouth.SelectedItem;

            banker["IsApply"] = true;
            this.Close();
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            DataBanker banker = DataBanker.GetInstance;
            banker["IsApply"] = false;
            this.Close();
        }

        private void button_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);            
        }

    }


    class ImageDataFile
    {
        [CsvColumn(Name = "#layer_type", FieldIndex = 1)]
        public string LayerType { get; set; }
        [CsvColumn(Name = "name", FieldIndex = 2)]
        public string Name { get; set; }
        [CsvColumn(Name = "left", FieldIndex = 3)]
        public string Left { get; set; }
        [CsvColumn(Name = "top", FieldIndex = 4)]
        public string Top { get; set; }
        [CsvColumn(Name = "width", FieldIndex = 5)]
        public string Width { get; set; }
        [CsvColumn(Name = "height", FieldIndex = 6)]
        public string Height { get; set; }
        [CsvColumn(Name = "type", FieldIndex = 7, CanBeNull = true)]
        public string Type { get; set; }
        [CsvColumn(Name = "opacity", FieldIndex = 8, CanBeNull = true)]
        public string Opacity { get; set; }
        [CsvColumn(Name = "visible", FieldIndex = 9, CanBeNull = true)]
        public string Visible { get; set; }
        [CsvColumn(Name = "layer_id", FieldIndex = 10, CanBeNull = true)]
        public string LayerId { get; set; }
        [CsvColumn(Name = "group_layer_id", FieldIndex = 11, CanBeNull = true)]
        public string GroupLayerId { get; set; }
        [CsvColumn(Name = "base", FieldIndex = 12, CanBeNull = true)]
        public string Base { get; set; }
        [CsvColumn(Name = "images", FieldIndex = 13, CanBeNull = true)]
        public string Images { get; set; }
        [CsvColumn(Name = "", FieldIndex = 14, CanBeNull = true)]
        public string Unknown { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    class SizeData
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Label;
        }
    }

}
