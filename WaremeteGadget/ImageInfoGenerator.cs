using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WaremeteGadget
{
    class ImageInfoGenerator
    {
        private SelectedItems items;

        public ImageInfoGenerator(SelectedItems items)
        {
            this.items = items;
        }


        /// <summary>
        /// 服装のファイルパスを生成します。
        /// </summary>
        /// <returns>服装のファイルパス</returns>
        public Uri DressFileName()
        {
            return new Uri(
                String.Format("{0}\\data\\{1}_{2}_{3}.png",
                    Directory.GetCurrentDirectory(),
                    items.CharacterInfo.PoseFile,
                    items.Size.Value,
                    items.Dress.LayerId
                ));
        }

        /// <summary>
        /// 目のファイルパスを生成します。
        /// </summary>
        /// <returns>複数の目のファイルパス</returns>
        public IEnumerable<Uri> EyesFileName()
        {            
            foreach (var eye in items.Eyes)
            {
                yield return new Uri(
                    String.Format("{0}\\data\\{1}_{2}_{3}.png",
                        Directory.GetCurrentDirectory(),
                        items.CharacterInfo.PoseFile,
                        items.Size.Value,
                        eye.LayerId
                    ));
            }
        }

        /// <summary>
        /// 口のファイルパスを生成します。
        /// </summary>
        /// <returns>複数の口のファイルパス</returns>
        public IEnumerable<Uri> MouthsFileName()
        {
            foreach (var mouth in items.Mouths)
            {
                yield return new Uri(
                    String.Format("{0}\\data\\{1}_{2}_{3}.png",
                        Directory.GetCurrentDirectory(),
                        items.CharacterInfo.PoseFile,
                        items.Size.Value,
                        mouth.LayerId
                    ));
            }
        }


        public Thickness DressMargin()
        {
            return new Thickness(
                int.Parse(items.Dress.Left),
                int.Parse(items.Dress.Top),
                0, 0);
        }


        public IEnumerable<Thickness> EyesMargin()
        {
            foreach (var eye in items.Eyes)
	        {
                yield return new Thickness(
                    int.Parse(eye.Left), 
                    int.Parse(eye.Top), 
                    0, 0);
	        }            
        }
        

        public IEnumerable<Thickness> MouthsMargin()
        {
            foreach (var mouth in items.Mouths)
            {
                yield return new Thickness(
                    int.Parse(mouth.Left),
                    int.Parse(mouth.Top),
                    0, 0);
            }
        }

    }
}
