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

        /// <summary>
        /// 目パチレイヤーのファイルパス
        /// </summary>
        IEnumerable<Uri> eyeLayers;

        /// <summary>
        /// 口パクレイヤーのファイルパス
        /// </summary>
        IEnumerable<Uri> mouthLayers;


        public MainWindow()
        {
            InitializeComponent();

            banker = DataBanker.GetInstance;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(dispatcherTimer_Tick);
        }


        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ConfigWindow window = new ConfigWindow();
            try
            {
                window.ShowDialog();
            }
            catch (NullReferenceException ex) { return; }

            //設定の変更
            DataBanker banker = DataBanker.GetInstance;
            //×ボタンで閉じられたとき
            if (banker.Keys.Count == 0) return;

            if ((bool)banker["IsApply"])
            {
                DrawImage();
                timer.Start();
            }

            this.img_main.Source = null;
        }


        bool isTimerStop;
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            //ドラッグ中に目パチをさせない
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                timer.Stop();
                isTimerStop = true;
                DragMove();
            }
            else
            {
                if (isTimerStop && banker["SelectedItems"] != null)
                {
                    timer.Start();
                    isTimerStop = false;
                }                
            }
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

            //身体
            img_body.Source = new BitmapImage(generator.DressFileName());
            img_body.Margin = generator.DressMargin();

            //目
            eyeLayers = generator.EyesFileName();
            img_eyes.Source = new BitmapImage(eyeLayers.First());
            img_eyes.Margin = generator.EyesMargin().First();

            //口
            mouthLayers = generator.MouthsFileName();
            img_mouth.Source = new BitmapImage(mouthLayers.Last());
            img_mouth.Margin = generator.MouthsMargin().Last();
        }


/*****************目パチ******************/

        int openEyeSec = 1;
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            if (openEyeSec-- == 0)
            {
                //乱数生成
                openEyeSec = new Random().Next(2, 9);
                if (((SelectedItems)banker["SelectedItems"]).IsWink)
                {
                    Wink();
                }
            }

        }

        
        private async void Wink()
        {
            if (generator == null || eyeLayers == null)
            {
                return;
            }

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


    }
}
