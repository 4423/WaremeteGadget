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
            if ((bool)banker["IsApply"])
            {
                DrawImage();
            }
        }


        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }


        private void DrawImage()
        {
            DataBanker banker = DataBanker.GetInstance;
            var size = (SizeData)banker["SizeInfo"];
            var charInfo = (CharDataFile)banker["CharaInfo"];
            var dressInfo = (ImageDataFile)banker["DressImage"];
            var eyeInfo = (ImageDataFile)banker["EyeImage"];
            var mouthInfo = (ImageDataFile)banker["MouthImage"];

            string baseName = Directory.GetCurrentDirectory() + @"\data\" + charInfo.PoseFile + "_" + size.Value + "_";
            string dressName = baseName + dressInfo.LayerId + ".png";
            string eyeName = baseName + eyeInfo.LayerId + ".png";
            string mouthName = baseName + mouthInfo.LayerId + ".png";


            //身体
            BitmapImage biBody = new BitmapImage();
            using (FileStream fs = new FileStream(dressName, FileMode.Open))
            {
                biBody.BeginInit();
                biBody.CacheOption = BitmapCacheOption.OnLoad;
                biBody.StreamSource = fs;
                biBody.EndInit();
            }
            Thickness thickBody = new Thickness(Convert.ToInt32(dressInfo.Left), Convert.ToInt32(dressInfo.Top) -22, 0, 0);

            //目
            BitmapImage biEye = new BitmapImage();
            using (FileStream fs = new FileStream(eyeName, FileMode.Open))
            {
                biEye.BeginInit();
                biEye.CacheOption = BitmapCacheOption.OnLoad;
                biEye.StreamSource = fs;
                biEye.EndInit();
            }
            Thickness thickEye = new Thickness(Convert.ToInt32(eyeInfo.Left), Convert.ToInt32(eyeInfo.Top) -22, 0, 0);

            //口
            BitmapImage biMouth = new BitmapImage();
            using (FileStream fs = new FileStream(mouthName, FileMode.Open))
            {
                biMouth.BeginInit();
                biMouth.CacheOption = BitmapCacheOption.OnLoad;
                biMouth.StreamSource = fs;
                biMouth.EndInit();
            }
            Thickness thickMouth = new Thickness(Convert.ToInt32(mouthInfo.Left), Convert.ToInt32(mouthInfo.Top) -22, 0, 0);


            //描画
            img_body.Source = biBody;

            img_eyes.Margin = thickEye;
            img_eyes.Source = biEye;

            img_mouth.Margin = thickMouth;
            img_mouth.Source = biMouth;
        }
    }
}
