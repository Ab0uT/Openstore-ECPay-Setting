using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;
using NBrightCore.common;
using NBrightDNN;
using Nevoweb.DNN.NBrightBuy.Components;
using DotNetNuke.Common.Utilities;
using OS_AllShipping.Models;


namespace OS_AllShipping
{
    public class ProviderUtils
    {

        public static List<NBrightInfo> GetPickupPoints(string key, string address, string zipcode, string city)
        {

            var rtnlist = new List<NBrightInfo>();

            var url = "";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)httpRequest.GetResponse();
            var nbi = new NBrightInfo();
            nbi.XMLData = response.ToString();

            // code to build list


            return rtnlist;
        }

    }
}
