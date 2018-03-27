using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class ClassA
    {
        public ClassA(string v)
        {
            value = v;
        }
        public string value;

        private static Dictionary<string, ClassA> pri = new Dictionary<string, ClassA>() { { "a", new ClassA("a") } };

        public static ReadOnlyDictionary<string, ClassA> pub
        {
            get
            {
                return new ReadOnlyDictionary<string, ClassA>(pri);
            }
        }


        public static void set()
        {
            pri.Add("b", new ClassA("b"));
        }

        private static Dictionary<string, ClassA> _Instance = new Dictionary<string, ClassA>() { };

        public static ClassA Instance
        {
            get
            {
                if (_Instance.ContainsKey("Default")) return _Instance["Default"];
                else
                {
                    _Instance.Add("Default", new ClassA("Default"));
                    return _Instance["Default"];
                }
            }
        }

        /// <summary>
        /// 获取指定Key 的ClassA对象
        /// </summary>
        public class ClassAKey
        {
            public ClassAKey() { }
            public ClassA this[string key]
            {
                get
                {
                    if (_Instance.ContainsKey(key)) return _Instance[key];
                    else return null;
                }
            }
        }

        /// <summary>
        /// 获取指定Key的数据库链接对象，再使用前需要进行Load，如果找不到则返回Null
        /// </summary>
        public static ClassAKey InstanceKey
        {
            get
            {
                return new ClassAKey();
            }
        }
    }
}
