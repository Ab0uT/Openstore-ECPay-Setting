using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml;
using DotNetNuke.Entities.Portals;
using NBrightCore.common;
using NBrightDNN;
using Nevoweb.DNN.NBrightBuy.Components;
using Nevoweb.DNN.NBrightBuy.Components.Interfaces;
using System.Globalization;
using OS_AllShipping.Models;
using Nevoweb.DNN.NBrightBuy.Components.Orders;

namespace OS_AllShipping.Components
{
    public class HomeDelivery : ShippingInterface
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <returns></returns>
        public override NBrightInfo CalculateShipping(NBrightInfo cartInfo)
        {
            // return zero if we have invalid data
            cartInfo.SetXmlPropertyDouble("genxml/shippingcost", "0");
            cartInfo.SetXmlPropertyDouble("genxml/shippingcostTVA", "0");
            cartInfo.SetXmlPropertyDouble("genxml/shippingdealercost", "0");
            var modCtrl = new NBrightBuyController();
            var info = modCtrl.GetByGuidKey(PortalSettings.Current.PortalId, -1, "SHIPPING", Shippingkey);
            if (info == null) return cartInfo;


            double shippingcost = 0;
            if (cartInfo.GetXmlPropertyInt("genxml/OS_AllShippinglistidx") == 1)
            {
                shippingcost = 99.99;
            }

            if (Merchant.IsEnable && Merchant.Type == "B")
            {
                shippingcost = Merchant.HomeCost;

                if (cartInfo.GetXmlPropertyDouble("genxml/appliedsubtotal") >= Merchant.FreeCost)
                {
                    shippingcost = 0;
                }
            }


            var shippingdealercost = shippingcost;
            cartInfo.SetXmlPropertyDouble("genxml/shippingcostTVA", "0");
            cartInfo.SetXmlPropertyDouble("genxml/shippingcost", shippingcost);
            cartInfo.SetXmlPropertyDouble("genxml/shippingdealercost", shippingdealercost);

            return cartInfo;

        }

        public override string Shippingkey { get; set; }

        public override string Name()
        {
            var objCtrl = new NBrightBuyController();
            var info = objCtrl.GetPluginSinglePageData("OS_AllShipping", "SHIPPING", Utils.GetCurrentCulture());
            //var rtn = info.GetXmlProperty("genxml/lang/genxml/textbox/name");
            //if (rtn == "") rtn = info.GetXmlProperty("genxml/textbox/name");
            //if (rtn == "") rtn = "OS_AllShipping";


            var shippingName = string.Empty; ;

            if (Merchant.IsEnable && Merchant.Type == "B")
                shippingName = $"宅配( {NBrightBuyUtils.FormatToStoreCurrency(Merchant.HomeCost)} )";

            return shippingName;
        }

        public override string GetTemplate(NBrightInfo cartInfo)
        {
            return GetTemplateData("carttemplate.cshtml", cartInfo);
        }

        public override string GetDeliveryLabelUrl(NBrightInfo cartInfo)
        {
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatename"></param>
        /// <param name="cartInfo"></param>
        /// <returns></returns>
        private string GetTemplateData(String templatename, NBrightInfo cartInfo)
        {

            var modCtrl = new NBrightBuyController();

            var info = modCtrl.GetPluginSinglePageData(Shippingkey, "SHIPPING", Utils.GetCurrentCulture());
            if (info == null) return "";

            var controlMapPath = HttpContext.Current.Server.MapPath("/DesktopModules/NBright/OS_AllShipping");
            var templCtrl = new NBrightCore.TemplateEngine.TemplateGetter(PortalSettings.Current.HomeDirectoryMapPath, controlMapPath, "Themes\\config", "");
            var templ = templCtrl.GetTemplateData(templatename, Utils.GetCurrentCulture());

            return templ;
        }


        public override bool IsValid(NBrightInfo cartInfo)
        {
            if (cartInfo.GetXmlPropertyDouble("genxml/totalweight") <= 99 && Merchant.Type == "B")
            {
                return true;
            }
            return false;
        }

    }

}
