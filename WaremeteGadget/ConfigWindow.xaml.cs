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
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WaremeteGadget
{
    /// <summary>
    /// ConfigWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ConfigWindow : Window
    {
        DataBanker banker;


        public ConfigWindow()
        {
            InitializeComponent();

            banker = DataBanker.GetInstance;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string filepath = Directory.GetCurrentDirectory() + @"\data\charinit.csv";

            CharInitReader reader = new CharInitReader(filepath);
            //キャラクター名でグルーピング
            var groupingCharData = reader.ReadAll().ToLookup(p => p.CharName);
            this.comboBox_char.ItemsSource = groupingCharData;

            //初期選択可能項目
            this.comboBox_blush.ItemsSource = BlushItems();
            this.comboBox_size.ItemsSource = SizeData();

            //サイズが選択されるまでは他のComboBoxを無効にする
            EnableComboBox(false);
        }


        private void comboBox_pose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboBox_pose.SelectedItem == null) return;

            //選択された立ち絵
            var charData = (CharDataFile)this.comboBox_pose.SelectedItem;

            //服装・表情データ読み込み
            string filepath = GetLayerInfoFilePath();
            if (!File.Exists(filepath))
            {
                MessageBox.Show("このポーズは使用することが出来ません。\n他のポーズを選択して下さい。", "",
                    MessageBoxButton.OK, MessageBoxImage.Asterisk);
                InitializeComboBox();
                return;
            }

            FgFileReader reader = new FgFileReader(filepath);
            var imageData = reader.ReadAll(); 
           
            this.comboBox_dress.ItemsSource = imageData.Where(s => s.Name.IndexOf("腕") == 0);
            this.comboBox_eye.ItemsSource = imageData.Where(s => s.Name.IndexOf("目") == 0 && s.LayerType == "2");
            this.comboBox_mouth.ItemsSource = imageData.Where(s => s.Name.IndexOf("口") == 0 && s.LayerType == "2");
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


        /// <summary>
        /// 服装と表情に関するComboBoxのアイテムを初期化する。
        /// </summary>
        private void InitializeComboBox()
        {
            this.comboBox_dress.ItemsSource = null;
            this.comboBox_dress.Items.Clear();
            this.comboBox_eye.ItemsSource = null;
            this.comboBox_eye.Items.Clear();
            this.comboBox_mouth.ItemsSource = null;
            this.comboBox_mouth.Items.Clear();
        }


        /// <summary>
        /// ComboBoxの操作可否を設定する。
        /// </summary>
        /// <param name="isEnable">ComboBoxの操作可否。</param>
        private void EnableComboBox(bool isEnable)
        {
            this.comboBox_char.IsEnabled = isEnable;
            this.comboBox_pose.IsEnabled = isEnable;
            this.comboBox_dress.IsEnabled = isEnable;
            this.comboBox_eye.IsEnabled = isEnable;
            this.comboBox_mouth.IsEnabled = isEnable;
            this.comboBox_blush.IsEnabled = isEnable;
        }


        /// <summary>
        /// レイヤー情報が含まれてるtxtファイルのファイルパスを、
        /// 選択されているComboBoxの値から生成します。
        /// </summary>
        /// <returns></returns>
        private string GetLayerInfoFilePath()
        {
            return String.Format("{0}\\data\\{1}_{2}.txt",
                                       Directory.GetCurrentDirectory(),
                                       SelectedPose().PoseFile,
                                       SelectedSize().Value);
        }


        /// <summary>
        /// SizeDataのセットを提供します。
        /// </summary>
        /// <returns></returns>
        private List<SizeData> SizeData()
        {
            var sizeData = new List<SizeData>();
            sizeData.Add(new SizeData() { Label = "Small", Value = "1" });
            sizeData.Add(new SizeData() { Label = "Medium", Value = "2" });
            sizeData.Add(new SizeData() { Label = "Large", Value = "3" });
            sizeData.Add(new SizeData() { Label = "Face", Value = "4" });

            return sizeData;
        }

        private List<BlushItems> BlushItems()
        {
            var blushLevel = new List<BlushItems>();
            blushLevel.Add(new BlushItems() { Level = BlushLevel.None });
            blushLevel.Add(new BlushItems() { Level = BlushLevel.Low });
            blushLevel.Add(new BlushItems() { Level = BlushLevel.High });
            return blushLevel;
        }


    /****ボタン処理****/
        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAllChecked())
            {
                MessageBox.Show("全ての項目を設定して下さい。", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string filepath = GetLayerInfoFilePath();

            //選択された表情のIDに一致する差分表情を取得
            FgFileReader reader = new FgFileReader(filepath);
            var eyeLayers = reader.GetGroupLayers(SelectedEye().LayerId);
            var mouthLayers = reader.GetGroupLayers(SelectedMouth().LayerId);
                        
            SelectedItems items = new SelectedItems()
            {
                LayerInfoFilePath = filepath,
                Size = SelectedSize(),
                CharacterInfo = SelectedPose(),
                Dress = SelectedDress(),
                IsWink = (bool)this.checkbox_wink.IsChecked,
                Eyes = eyeLayers.Where(s=> SlectedBlushLevel().IsMatch(s.Name)),
                IsLipSync = false,
                Mouths = mouthLayers,
                BlushLevel = SlectedBlushLevel()
            };

            banker["SelectedItems"] = items;
            banker["IsApply"] = true;

            this.Close();
        }


        private bool IsAllChecked()
        {
            return this.comboBox_blush.SelectedIndex != -1
                && this.comboBox_char.SelectedIndex != -1
                && this.comboBox_dress.SelectedIndex != -1
                && this.comboBox_eye.SelectedIndex != -1
                && this.comboBox_mouth.SelectedIndex != -1
                && this.comboBox_pose.SelectedIndex != -1
                && this.comboBox_size.SelectedIndex != -1;
        }


        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {            
            banker["IsApply"] = false;
            this.Close();
        }


        private void button_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);            
        }



    /****ComboBoxのSelectedItemのラッパー****/
        private SizeData SelectedSize()
        {
            return (SizeData)this.comboBox_size.SelectedItem;
        }

        private CharDataFile SelectedPose()
        {
            return (CharDataFile)this.comboBox_pose.SelectedItem;
        }

        private ImageDataFile SelectedDress()
        {
            return (ImageDataFile)this.comboBox_dress.SelectedItem;
        }

        private ImageDataFile SelectedEye()
        {
            return (ImageDataFile)this.comboBox_eye.SelectedItem;
        }

        private ImageDataFile SelectedMouth()
        {
            return (ImageDataFile)this.comboBox_mouth.SelectedItem;
        }

        private BlushItems SlectedBlushLevel()
        {
            return (BlushItems)this.comboBox_blush.SelectedItem;
        }
    }

}
