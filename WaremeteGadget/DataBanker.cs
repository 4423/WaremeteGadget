using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaremeteGadget
{
    class DataBanker
    {
        private static readonly DataBanker instance = new DataBanker();

        private DataBanker() { }

        public static DataBanker GetInstance { get { return instance; } }

        private IDictionary holder = new Hashtable();

        /// <summary>  
        /// インデクサ  
        /// キーを元にデータを取得する  
        /// </summary>  
        public object this[object key]
        {
            get
            {
                return holder[key];
            }
            set
            {
                if (holder.Contains(key))
                {
                    // 重複する場合は削除  
                    holder.Remove(key);
                }
                holder[key] = value;
            }
        }

        /// <summary>  
        /// keyの情報を削除します。  
        /// </summary>  
        /// <param name="key"></param>  
        public void Remove(string key)
        {
            holder.Remove(key);
        }

        /// <summary>  
        /// すべての情報を削除します。  
        /// </summary>  
        public void RemoveAll()
        {
            holder.Clear();
        }

        /// <summary>  
        ///　キーの情報を返します。  
        /// </summary>  
        public ICollection Keys
        {
            get
            {
                return holder.Keys;
            }
        }
    }
}
