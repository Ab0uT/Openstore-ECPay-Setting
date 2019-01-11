using OS_AllShipping.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_AllShipping.Models
{
    public class MapModel : InsertDictionary
    {
        [Required]
        public string MerchantID
        {
            get { return GetString("MerchantID"); }
            set { SetString("MerchantID", value); }
        }

        public string MerchantTradeNo
        {
            get { return GetString("MerchantTradeNo"); }
            set { SetString("MerchantTradeNo", value); }
        }

        [Required]
        public string LogisticsType
        {
            get { return GetString("LogisticsType"); }
            set { SetString("LogisticsType", value); }
        }

        [Required]
        public string LogisticsSubType
        {
            get { return GetString("LogisticsSubType"); }
            set { SetString("LogisticsSubType", value); }
        }

        [Required]
        public string IsCollection
        {
            get { return GetString("IsCollection"); }
            set { SetString("IsCollection", value); }
        }

        [Required]
        public string ServerReplyURL
        {
            get { return GetString("ServerReplyURL"); }
            set { SetString("ServerReplyURL", value); }
        }

        public string ExtraData
        {
            get { return GetString("ExtraData"); }
            set { SetString("ExtraData", value); }
        }

        public int Device
        {
            get { return GetInt("Device"); }
            set { SetInt("Device", value); }
        }
    }
}
