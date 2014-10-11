using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WaremeteGadget.Properties;

namespace WaremeteGadget
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ConfigWindow window = new ConfigWindow();
            window.ShowDialog();

            //設定の変更
            DataBanker banker = DataBanker.GetInstance;
            //×ボタンで閉じられたとき
            if (banker.Keys.Count == 0) return;

            if ((bool)banker["IsApply"])
            {
                DrawImage();
            }

            this.img_main.Source = null;
        }


        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (Settings.Default.IsFirst)
            {
                BitmapImage defaultImage = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\main.png"));
                this.img_main.Source = defaultImage;
                BitmapSource source = (BitmapSource)this.img_main.Source;
                this.img_main.Width = source.PixelWidth;
                this.img_main.Height = source.PixelHeight;

                Settings.Default.IsFirst = false;
                Settings.Default.Save();
            }
            else
            {                
                LoadedDrawImage();
            }
        }


        private void DrawImage()
        {
            DataBanker banker = DataBanker.GetInstance;
            var size = (SizeData)banker["SizeInfo"];
            var charInfo = (CharDataFile)banker["CharaInfo"];
            var dressInfo = (ImageDataFile)banker["DressImage"];
            var eyeInfo = (ImageDataFile)banker["EyeImage"];
            var mouthInfo = (ImageDataFile)banker["MouthImage"];
            
            int dress_x = Settings.Default.dress_x = Convert.ToInt32(dressInfo.Left);
            int dress_y = Settings.Default.dress_y = Convert.ToInt32(dressInfo.Top);
            int eye_x = Settings.Default.eye_x = Convert.ToInt32(eyeInfo.Left);
            int eye_y = Settings.Default.eye_y = Convert.ToInt32(eyeInfo.Top);
            int mouth_x = Settings.Default.mouth_x = Convert.ToInt32(mouthInfo.Left);
            int mouth_y = Settings.Default.mouth_y = Convert.ToInt32(mouthInfo.Top);

            string baseName = Directory.GetCurrentDirectory() + @"\data\" + charInfo.PoseFile + "_" + size.Value + "_";
            string dressName = Settings.Default.dressName = baseName + dressInfo.LayerId + ".png";
            string eyeName = Settings.Default.eyeName = baseName + eyeInfo.LayerId + ".png";
            string mouthName = Settings.Default.mouthName = baseName + mouthInfo.LayerId + ".png";           

            Settings.Default.Save();

            //身体
            BitmapImage biBody = new BitmapImage(new Uri(dressName));
            Thickness thickBody = new Thickness(dress_x, dress_y, 0, 0);

            //目
            BitmapImage biEye = new BitmapImage(new Uri(eyeName));
            Thickness thickEye = new Thickness(eye_x, eye_y, 0, 0);

            //口
            BitmapImage biMouth = new BitmapImage(new Uri(mouthName));
            Thickness thickMouth = new Thickness(mouth_x, mouth_y, 0, 0);
            
            //描画
            img_body.Margin = thickBody;
            img_body.Source = biBody;

            img_eyes.Margin = thickEye;
            img_eyes.Source = biEye;

            img_mouth.Margin = thickMouth;
            img_mouth.Source = biMouth;
        }




        private void LoadedDrawImage()
        {
            int dress_x = Settings.Default.dress_x;
            int dress_y = Settings.Default.dress_y;
            int eye_x = Settings.Default.eye_x;
            int eye_y = Settings.Default.eye_y;
            int mouth_x = Settings.Default.mouth_x;
            int mouth_y = Settings.Default.mouth_y;
            string dressName = Settings.Default.dressName;
            string eyeName = Settings.Default.eyeName;
            string mouthName = Settings.Default.mouthName;

            //身体
            BitmapImage biBody = new BitmapImage(new Uri(dressName));
            Thickness thickBody = new Thickness(dress_x, dress_y, 0, 0);

            //目
            BitmapImage biEye = new BitmapImage(new Uri(eyeName));
            Thickness thickEye = new Thickness(eye_x, eye_y, 0, 0);

            //口
            BitmapImage biMouth = new BitmapImage(new Uri(mouthName));
            Thickness thickMouth = new Thickness(mouth_x, mouth_y, 0, 0);
            
            //描画
            img_body.Margin = thickBody;
            img_body.Source = biBody;

            img_eyes.Margin = thickEye;
            img_eyes.Source = biEye;

            img_mouth.Margin = thickMouth;
            img_mouth.Source = biMouth;
        }

    }
}
