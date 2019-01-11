using DotNetNuke.Entities.Portals;
using NBrightCore.common;
using NBrightDNN;
using Nevoweb.DNN.NBrightBuy.Components;
using Nevoweb.DNN.NBrightBuy.Components.Interfaces;
using OS_AllShipping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OS_AllShipping.Components
{
    public class UniMartPickup : ShippingInterface
    {
        public override string Shippingkey { get; set; }

        public override NBrightInfo CalculateShipping(NBrightInfo cartInfo)
        {
            cartInfo.SetXmlPropertyDouble("genxml/shippingcost", "0");
            cartInfo.SetXmlPropertyDouble("genxml/shippingcostTVA", "0");
            cartInfo.SetXmlPropertyDouble("genxml/shippingdealercost", "0");

            NBrightBuyController nbCtrl = new NBrightBuyController();
            NBrightInfo nbInfo = nbCtrl.GetByGuidKey(PortalSettings.Current.PortalId, -1, "SHIPPING", Shippingkey);
            if (nbInfo == null)
                return cartInfo;


            double shippingcost = 0;
            if (cartInfo.GetXmlPropertyInt("genxml/OS_AllShippinglistidx") == 1)
            {
                shippingcost = 99.99;
            }

            
            if (Merchant.IsEnable)
            {
                shippingcost = Merchant.UniMartCost;

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

        public override string GetDeliveryLabelUrl(NBrightInfo cartInfo) => string.Empty;

        public override string GetTemplate(NBrightInfo cartInfo) => GetTemplateData("carttemplate.cshtml", cartInfo);

        public override bool IsValid(NBrightInfo cartInfo)
        {
            if (cartInfo.GetXmlPropertyDouble("genxml/totalweight") <= 99)
            {
                return true;
            }
            return false;
        }

        public override string Name() => $"7-11取貨( {NBrightBuyUtils.FormatToStoreCurrency(Merchant.UniMartCost)} )";


        private String GetTemplateData(String templatename, NBrightInfo cartInfo)
        {

            var nbCtrl = new NBrightBuyController();

            var info = nbCtrl.GetPluginSinglePageData(Shippingkey, "SHIPPING", Utils.GetCurrentCulture());
            if (info == null)
                return string.Empty;

            string ctrlMapPath = HttpContext.Current.Server.MapPath("/DesktopModules/NBright/OS_AllShipping");
            var templateCtrl = new NBrightCore.TemplateEngine.TemplateGetter(PortalSettings.Current.HomeDirectoryMapPath, ctrlMapPath, "Themes\\config", "");
            string template = templateCtrl.GetTemplateData(templatename, Utils.GetCurrentCulture());

            return template;
        }
    }
}
