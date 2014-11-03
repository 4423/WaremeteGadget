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
using System.Windows.Threading;

namespace WaremeteGadget
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        
        DispatcherTimer timer;
        DataBanker banker;
        ImageInfoGenerator generator;


        public MainWindow()
        {
            InitializeComponent();

            banker = DataBanker.GetInstance;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(dispatcherTimer_Tick);
            timer.Start();
        }


        int openEyeSec = 5;
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (openEyeSec-- == 0)
            {
                //乱数生成
                openEyeSec = new Random().Next(2, 9);
                Wink();
            }

        }

        
        private async void Wink()
        {
            if (generator == null)
            {
                return;
            }

            var eyeLayers = generator.EyesFileName();
            
            //半目
            img_eyes.Source = new BitmapImage(eyeLayers.ElementAt(1));
            img_eyes.Margin = generator.EyesMargin().ElementAt(1);
            Utility.DoEvents();
            await Task.Delay(100);

            //閉じる
            img_eyes.Source = new BitmapImage(eyeLayers.ElementAt(2));
            img_eyes.Margin = generator.EyesMargin().ElementAt(2);
            Utility.DoEvents();
            await Task.Delay(70);

            //半目
            img_eyes.Source = new BitmapImage(eyeLayers.ElementAt(1));
            img_eyes.Margin = generator.EyesMargin().ElementAt(1);
            Utility.DoEvents();
            await Task.Delay(70);

            //開く
            img_eyes.Source = new BitmapImage(eyeLayers.ElementAt(0));
            img_eyes.Margin = generator.EyesMargin().ElementAt(0);
            Utility.DoEvents();

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

            //if (Settings.Default.IsFirst)
            if(true)
            {
                BitmapImage defaultImage = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\main.png"));
                this.img_main.Source = defaultImage;
                BitmapSource source = (BitmapSource)this.img_main.Source;
                this.img_main.Width = source.PixelWidth;
                this.img_main.Height = source.PixelHeight;

                Settings.Default.IsFirst = false;
                Settings.Default.Save();
            }
        }


        private void DrawImage()
        {
            var items = (SelectedItems)banker["SelectedItems"];
            generator = new ImageInfoGenerator(items);

            //TODO 暫定的First()

            //身体
            img_body.Source = new BitmapImage(generator.DressFileName());
            img_body.Margin = generator.DressMargin();

            //目
            var eyeLayers = generator.EyesFileName();   //目パチ用レイヤー
            img_eyes.Source = new BitmapImage(eyeLayers.First());
            img_eyes.Margin = generator.EyesMargin().First();

            //口
            var mouthLayers = generator.MouthsFileName();
            img_mouth.Source = new BitmapImage(mouthLayers.Last());
            img_mouth.Margin = generator.MouthsMargin().Last();
        }

    }
}
