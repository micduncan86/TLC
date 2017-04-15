using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TLC.Data
{
    public class CacheManager
    {
        private Dictionary<string,object> _mylist = null;
        public Dictionary<string, object> CacheList
        {
            get
            {
                if (_mylist == null)
                {
                    _mylist = (HttpContext.Current.Cache["CacheList"] as Dictionary<string, object>);
                    if (_mylist == null)
                    {
                        _mylist = new Dictionary<string, object>();
                          HttpContext.Current.Cache.Insert("CacheList", _mylist);
                    }
                }
                return _mylist;
            }
            set
            {
                HttpContext.Current.Cache.Insert("CacheList", value);
            }
        }

        public void Clear()
        {

            HttpContext.Current.Cache.Remove("CacheList");
        }
    }
}
