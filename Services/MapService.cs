using Nevoweb.DNN.NBrightBuy.Components;
using OS_AllShipping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_AllShipping.Services
{
    public class MapService : CartData
    {
        public MapService(int portalId, string nameAppendix = "", string cartid = "") : base(portalId, nameAppendix, cartid)
        {
        }

        public void AddMapInfo(MapInfo model)
        {
            string strXml = "<mapinfo>";
            strXml += "<genxml>";
            strXml += "<textbox>";
            strXml += $"<merchantid>{model.MerchantID}</merchantid>";
            strXml += $"<merchanttradeno>{model.MerchantTradeNo}</merchanttradeno>";
            strXml += $"<logisticssubtype>{model.LogisticsSubType}</logisticssubtype>";
            strXml += $"<cvsstoreid>{model.CVSStoreID}</cvsstoreid>";
            strXml += $"<cvsstoreName>{model.CVSStoreName}</cvsstoreName>";
            strXml += $"<cvsaddress>{model.CVSAddress}</cvsaddress>";
            strXml += $"<cvstelephone>{model.CVSTelephone}</cvstelephone>";
            strXml += $"<extradata>{model.ExtraData}</extradata>";
            strXml += "</textbox>";
            strXml += "</genxml>";
            strXml += "</mapinfo>";
            PurchaseInfo.RemoveXmlNode("genxml/mapinfo");
            PurchaseInfo.AddXmlNode(strXml, "mapinfo", "genxml");
        }

        public string PostFormData(string url, MapModel value)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<form name='postdata' target='NewFile' id='postdata' action='" + url + "' method='POST'>").AppendLine();
            foreach (var i in value)
            {
                builder.Append("<input type='hidden' name='" + i.Key + "' value='" + i.Value + "'>").AppendLine();
            }
            builder.Append("<script> var theForm = document.forms['postdata'];  if (!theForm) { theForm = document.postdata; } theForm.submit(); </script>");
            builder.Append("</form>").AppendLine();
            return builder.ToString();

        }
    }
}
