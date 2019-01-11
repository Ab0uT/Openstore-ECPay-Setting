using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Xml;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Localization;
using NBrightCore.common;
using NBrightDNN;
using Nevoweb.DNN.NBrightBuy.Components;
using Nevoweb.DNN.NBrightBuy.Components.Products;
using Nevoweb.DNN.NBrightBuy.Components.Interfaces;
using RazorEngine.Compilation.ImpromptuInterface.InvokeExt;
using Nevoweb.DNN.NBrightBuy.Components.Orders;
using DotNetNuke.Entities.Users;

namespace OS_AllShipping
{
    public class AjaxProvider : AjaxInterface
    {
        public override string Ajaxkey { get; set; }

        public override string ProcessCommand(string paramCmd, HttpContext context, string editlang = "")
        {
            var ajaxInfo = NBrightBuyUtils.GetAjaxFields(context);
            var lang = NBrightBuyUtils.SetContextLangauge(ajaxInfo); // Ajax breaks context with DNN, so reset the context language to match the client.
            var objCtrl = new NBrightBuyController();

            var strOut = "OS_AllShipping Error";
            switch (paramCmd)
            {
                case "os_allshipping_getsettings":
                    var info1 = objCtrl.GetPluginSinglePageData("OS_AllShipping", "SHIPPING", lang);
                    strOut = NBrightBuyUtils.RazorTemplRender("settingsfields.cshtml", 0, "", info1, "/DesktopModules/NBright/OS_AllShipping", "config", lang, StoreSettings.Current.Settings());
                    break;
                case "os_allshipping_savesettings":
                    strOut = objCtrl.SavePluginSinglePageData(context);
                    break;
                case "os_allshipping_selectlang":
                    objCtrl.SavePluginSinglePageData(context);
                    var nextlang = ajaxInfo.GetXmlProperty("genxml/hidden/nextlang");
                    var info2 = objCtrl.GetPluginSinglePageData("OS_AllShipping", "SHIPPING", nextlang);
                    strOut = NBrightBuyUtils.RazorTemplRender("settingsfields.cshtml", 0, "", info2, "/DesktopModules/NBright/OS_AllShipping", "config", nextlang, StoreSettings.Current.Settings());
                    break;
                case "os_allshipping_getcarttotals":
                    var cartd = new CartData(PortalSettings.Current.PortalId);

                    cartd.PurchaseInfo.SetXmlProperty("genxml/OS_AllShippingmessage", "");
                    cartd.PurchaseInfo.SetXmlProperty("genxml/OS_AllShippinglistidx", ajaxInfo.GetXmlProperty("genxml/radiobuttonlist/list"));
                    cartd.PurchaseInfo.SetXmlProperty("genxml/OS_AllShippinglistcode", "");
                    cartd.PurchaseInfo.SetXmlProperty("genxml/OS_AllShippingaddress", "");

                    cartd.Save();
                    strOut = NBrightBuyUtils.RazorTemplRender("CheckoutTotals.cshtml", 0, "", cartd, "/DesktopModules/NBright/NBrightBuy", "Default", Utils.GetCurrentCulture(), StoreSettings.Current.Settings());
                    break;
                case "os_allshipping_getlist":
                    strOut = OrderAdminList(context);
                    break;
            }

            return strOut;

        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        private static String OrderAdminList(HttpContext context)
        {
            try
            {
                if (UserController.Instance.GetCurrentUserInfo().UserID > 0)
                {
                    var settings = NBrightBuyUtils.GetAjaxDictionary(context);
                    return GetOrderListData(settings);
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        private static String GetOrderListData(Dictionary<String, String> settings, bool paging = true, bool csv = false)
        {
            if (UserController.Instance.GetCurrentUserInfo().UserID <= 0) return "";

            var strOut = "";

            if (!settings.ContainsKey("selecteduserid")) settings.Add("selecteduserid", "");

            if (!settings.ContainsKey("themefolder")) settings.Add("themefolder", "");
            if (!settings.ContainsKey("userid")) settings.Add("userid", "-1");
            if (!settings.ContainsKey("razortemplate")) settings.Add("razortemplate", "");
            if (!settings.ContainsKey("returnlimit")) settings.Add("returnlimit", "0");
            if (!settings.ContainsKey("pagenumber")) settings.Add("pagenumber", "0");
            if (!settings.ContainsKey("pagesize")) settings.Add("pagesize", "0");
            if (!settings.ContainsKey("searchtext")) settings.Add("searchtext", "");
            if (!settings.ContainsKey("dtesearchdatefrom")) settings.Add("dtesearchdatefrom", "");
            if (!settings.ContainsKey("dtesearchdateto")) settings.Add("dtesearchdateto", "");
            if (!settings.ContainsKey("searchorderstatus")) settings.Add("searchorderstatus", "");
            if (!settings.ContainsKey("portalid")) settings.Add("portalid", PortalSettings.Current.PortalId.ToString("")); // aways make sure we have portalid in settings

            if (!Utils.IsNumeric(settings["userid"])) settings["pagenumber"] = "1";
            if (!Utils.IsNumeric(settings["pagenumber"])) settings["pagenumber"] = "1";
            if (!Utils.IsNumeric(settings["pagesize"])) settings["pagesize"] = "20";
            if (!Utils.IsNumeric(settings["returnlimit"])) settings["returnlimit"] = "50";

            var themeFolder = settings["themefolder"];
            var razortemplate = settings["razortemplate"];
            var returnLimit = Convert.ToInt32(settings["returnlimit"]);
            var pageNumber = Convert.ToInt32(settings["pagenumber"]);
            var pageSize = Convert.ToInt32(settings["pagesize"]);
            var portalId = Convert.ToInt32(settings["portalid"]);
            var userid = settings["userid"];
            var selecteduserid = settings["selecteduserid"];

            var searchText = settings["searchtext"];
            var searchdatefrom = settings["dtesearchdatefrom"];
            var searchdateto = settings["dtesearchdateto"];
            var searchorderstatus = settings["searchorderstatus"];

            var filter = "";
            if (searchText != "")
            {
                filter += " and (    (([xmldata].value('(genxml/billaddress/genxml/textbox/firstname)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/billaddress/genxml/textbox/lastname)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/billaddress/genxml/textbox/unit)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/billaddress/genxml/textbox/street)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/billaddress/genxml/textbox/postalcode)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/billaddress/genxml/textbox/email)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/shipaddress/genxml/textbox/firstname)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/shipaddress/genxml/textbox/lastname)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/shipaddress/genxml/textbox/unit)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/shipaddress/genxml/textbox/street)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/shipaddress/genxml/textbox/postalcode)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/shipaddress/genxml/textbox/email)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/productrefs)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))";
                filter += " or (([xmldata].value('(genxml/ordernumber)[1]', 'nvarchar(max)') like '%" + searchText + "%' collate sql_latin1_general_cp1_ci_ai ))  ) ";
            }

            if (Utils.IsNumeric(selecteduserid))
            {
                filter += " and (NB1.UserId = " + selecteduserid + ")   ";
            }

            if (searchdateto != "" && searchdatefrom != "")
            {
                filter += " and  ( ([xmldata].value('(genxml/createddate)[1]', 'datetime') >= convert(datetime,'" + searchdatefrom + "') ) and ([xmldata].value('(genxml/createddate)[1]', 'datetime') <= convert(datetime,'" + searchdateto + "') ) )  ";
            }
            if (searchdateto == "" && searchdatefrom != "")
            {
                filter += " and  ([xmldata].value('(genxml/createddate)[1]', 'datetime') >= convert(datetime,'" + searchdatefrom + "') ) ";
            }
            if (searchdateto != "" && searchdatefrom == "")
            {
                filter += " and ([xmldata].value('(genxml/createddate)[1]', 'datetime') <= convert(datetime,'" + searchdateto + "') ) ";
            }

            if (searchorderstatus != "")
            {
                filter += " and ([xmldata].value('(genxml/dropdownlist/orderstatus)[1]', 'nvarchar(max)') = '" + searchorderstatus + "')   ";
            }

            // check for user or manager.
            if (!NBrightBuyUtils.CheckRights())
            {
                filter += " and ( userid = " + UserController.Instance.GetCurrentUserInfo().UserID + ")   ";
            }

            var recordCount = 0;

            if (themeFolder == "")
            {
                themeFolder = StoreSettings.Current.ThemeFolder;
                if (settings.ContainsKey("themefolder")) themeFolder = settings["themefolder"];
            }

            var objCtrl = new NBrightBuyController();

            if (paging) // get record count for paging
            {
                if (pageNumber == 0) pageNumber = 1;
                if (pageSize == 0) pageSize = 20;

                // get only entity type required
                recordCount = objCtrl.GetListCount(PortalSettings.Current.PortalId, -1, "ORDER", filter);

            }

            var orderby = "   order by [XMLData].value('(genxml/createddate)[1]','datetime') DESC, ModifiedDate DESC  ";
            var list = objCtrl.GetList(portalId, -1, "ORDER", filter, orderby, 0, pageNumber, pageSize, recordCount);

            var passSettings = settings;
            foreach (var s in StoreSettings.Current.Settings()) // copy store setting, otherwise we get a byRef assignement
            {
                if (passSettings.ContainsKey(s.Key))
                    passSettings[s.Key] = s.Value;
                else
                    passSettings.Add(s.Key, s.Value);
            }

            if (csv)
            {
                var securekey = Utils.GetUniqueKey(24);

                var strOutCSV = "";
                foreach (NBrightInfo nbi in list)
                {
                    string str1 = "";
                    str1 += nbi.GetXmlProperty("genxml/ordernumber").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/billaddress/genxml/textbox/firstname").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/billaddress/genxml/textbox/lastname").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/billaddress/genxml/textbox/email").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/billaddress/genxml/textbox/street").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/billaddress/genxml/textbox/city").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/billaddress/genxml/textbox/region").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/billaddress/genxml/textbox/postalcode").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/billaddress/genxml/dropdownlist/country/@selectedtext").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/createddate").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlPropertyDouble("genxml/appliedtotal");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/item/genxml/unitcost").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlPropertyDouble("genxml/item/genxml/dealercost");
                    str1 += ";";
                    str1 += nbi.GetXmlPropertyDouble("genxml/item/genxml/saleprice");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/item/genxml/modelref").Replace(";", ":");
                    str1 += ";";
                    str1 += nbi.GetXmlProperty("genxml/item/genxml/modeldesc").Replace(";", ":");
                    strOutCSV += str1 + Environment.NewLine;
                }

                Utils.SaveFile(StoreSettings.Current.FolderTempMapPath + "/secure" + securekey, strOutCSV);
                strOut = StoreSettings.Current.FolderTemp + "/secure" + securekey;
            }
            else
            {
                strOut = NBrightBuyUtils.RazorTemplRenderList(razortemplate, 0, "", list, "/DesktopModules/NBright/NBrightBuy", themeFolder, Utils.GetCurrentCulture(), passSettings);
            }

            // add paging if needed
            if (paging && (recordCount > pageSize))
            {
                var pg = new NBrightCore.controls.PagingCtrl();
                strOut += pg.RenderPager(recordCount, pageSize, pageNumber);
            }

            return strOut;
        }
    }
}
