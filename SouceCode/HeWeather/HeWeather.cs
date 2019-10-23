using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace HttpsUtility.HeWeather
{
    public class Basic
    {
        /// <summary>
        /// 
        /// </summary>
        public string cid;
        /// <summary>
        /// 北京
        /// </summary>
        public string location;
        /// <summary>
        /// 北京
        /// </summary>
        public string parent_city;
        /// <summary>
        /// 北京
        /// </summary>
        public string admin_area;
        /// <summary>
        /// 中国
        /// </summary>
        public string cnty;
        /// <summary>
        /// 
        /// </summary>
        public string lat;
        /// <summary>
        /// 
        /// </summary>
        public string lon;
        /// <summary>
        /// 
        /// </summary>
        public string tz;
    }

    public class Update
    {
        /// <summa>
        /// 
        /// </summary>
        public string loc;
        /// <summary>
        /// 
        /// </summary>
        public string utc;
    }

    public class Now
    {
        /// <summary>
        /// 
        /// </summary>
        public string cloud;
        /// <summary>
        /// 
        /// </summary>
        public string cond_code;
        /// <summary>
        /// 晴
        /// </summary>
        public string cond_txt;
        /// <summary>
        /// 
        /// </summary>
        public string fl;
        /// <summary>
        /// 
        /// </summary>
        public string hum;
        /// <summary>
        /// 
        /// </summary>
        public string pcpn;
        /// <summary>
        /// 
        /// </summary>
        public string pres;
        /// <summary>
        /// 
        /// </summary>
        public string tmp;
        /// <summary>
        /// 
        /// </summary>
        public string vis;
        /// <summary>
        /// 
        /// </summary>
        public string wind_deg;
        /// <summary>
        /// 北风
        /// </summary>
        public string wind_dir;
        /// <summary>
        /// 
        /// </summary>
        public string wind_sc;
        /// <summary>
        /// 
        /// </summary>
        public string wind_spd;
    }

    public class HeWeather6Item
    {
        /// <summary>
        /// 
        /// </summary>
        public Basic basic;
        /// <summary>
        /// 
        /// </summary>
        public Update update;
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Now now;
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public List<HeWeather6Item> HeWeather6;
    }
}