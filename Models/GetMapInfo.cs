using DotNetNuke.Entities.Portals;
using Nevoweb.DNN.NBrightBuy.Components;

namespace OS_AllShipping.Models
{
    public  class GetMapInfo
    {
        private const string genxml = "genxml/mapinfo/genxml/textbox";

        private  CartData Info => new CartData(PortalSettings.Current.PortalId);

        public  string LogisticsSubType => Info.PurchaseInfo.GetXmlProperty($"{genxml}/logisticssubtype");

        public  string CVSStoreID => Info.PurchaseInfo.GetXmlProperty($"{genxml}/cvsstoreid");

        public  string CVSStoreName => Info.PurchaseInfo.GetXmlProperty($"{genxml}/cvsstoreName");
    }
}
