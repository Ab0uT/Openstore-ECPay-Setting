using DotNetNuke.Entities.Portals;
using NBrightDNN;
using Nevoweb.DNN.NBrightBuy.Components;

namespace OS_AllShipping.Models
{
    public static class Links
    {
        private static NBrightInfo Info => _nbrightInfo();

        public static string Create => Info.GetXmlProperty("genxml/textbox/b2ccreate");
        public static string ReturnHome => Info.GetXmlProperty("genxml/textbox/b2creturnhome");
        public static string ReturnFamiCVS => Info.GetXmlProperty("genxml/textbox/b2creturnfami");
        public static string ReturnUniMartCVS => Info.GetXmlProperty("genxml/textbox/b2creturnunimart");
        public static string TradeInfo => Info.GetXmlProperty("genxml/textbox/tradeinfo");
        public static string TradeDocument => Info.GetXmlProperty("genxml/textbox/b2cprinttradedocument");
        public static string PrintUniMartC2C => Info.GetXmlProperty("genxml/textbox/c2cprintunimart");
        public static string PrintFAMIC2C => Info.GetXmlProperty("genxml/textbox/c2cprintfami");
        public static string Emap => Info.GetXmlProperty("genxml/textbox/emap");

        private static NBrightInfo _nbrightInfo()
        {
            NBrightBuyController _nBright = new NBrightBuyController();

            return _nBright.GetByGuidKey(PortalSettings.Current.PortalId, -1, "SHIPPING", "OS_AllShipping");
        }



    }
}
