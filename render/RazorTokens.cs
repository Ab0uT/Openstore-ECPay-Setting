using NBrightDNN;
using RazorEngine.Text;
using NBrightBuy.render;
using System;
using DotNetNuke.Entities.Users;
using Nevoweb.DNN.NBrightBuy.Components;

namespace OS_AllShipping.render
{
    public class RazorTokens<T> : NBrightBuyRazorTokens<T>
    {

        public IEncodedString ShippingDropDownList(NBrightInfo info, String xPath, String formSelector, String dataFields, String attibutes = "", Boolean allowEmpty = true)
        {


            return new RawString("");
        }

        public IEncodedString ShippingDropDown(NBrightInfo info, string xpath, string attributes = "", bool allowblank = true)
        {
            try
            {
                var resxPath = StoreSettings.NBrightBuyPath() + "/App_LocalResources/Admin.ascx.resx";

                var strOut = string.Empty;

                return new RawString(strOut);
            }
            catch (Exception e)
            {
                return new RawString(e.ToString());
            }
        }
    }
}
