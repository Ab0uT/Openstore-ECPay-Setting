using DotNetNuke.Entities.Portals;
using Nevoweb.DNN.NBrightBuy.Components;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_AllShipping.Models
{
    public class MapInfo
    {
        public string MerchantID { get; set; }

        public string MerchantTradeNo { get; set; }

        public string LogisticsSubType { get; set; }

        public string CVSStoreID { get; set; }

        public string CVSStoreName { get; set; }

        public string CVSAddress { get; set; }

        public string CVSTelephone { get; set; }

        public string CVSOutSide { get; set; }

        public string ExtraData { get; set; }


    }
}
