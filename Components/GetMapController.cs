using DotNetNuke.Entities.Users;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Web.Api;
using NBrightCore.common;
using NBrightDNN;
using Nevoweb.DNN.NBrightBuy.Components;
using OS_AllShipping.Models;
using OS_AllShipping.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace OS_AllShipping.Components
{
    public class GetMapController : DnnApiController
    {
        private readonly MapService mapInfo;

        private string Host { get { return HttpContext.Current.Request.Url.Host; } }
        private string Http { get { return HttpContext.Current.Request.IsSecureConnection ? "https" : "http"; } }


        public GetMapController()
        {
            mapInfo = new MapService(PortalSettings.PortalId);
        }

        [HttpPost]
        [DnnAuthorize]
        public HttpResponseMessage PostMap(MapModel value)
        {
            if (Merchant.IsEnable == true)
            {
                value.MerchantID = Merchant.MerchantID;
                value.IsCollection = Merchant.IsCollection ? "Y" : "N";
                value.LogisticsType = "CVS";
                value.ServerReplyURL = $"{Http}://{Host}/API/OS_AllShipping/GetMap/ResponseAddress";
                value.ExtraData = "";
                value.Device = 0;
            }

            if (ModelState.IsValid)
            {
                string printHtml = mapInfo.PostFormData(Links.Emap, value);

                var response = new HttpResponseMessage
                {
                    Content = new StringContent(printHtml)
                };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");

                return response;
            }
            return Request.CreateResponse("hell no");
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ResponseAddress(MapInfo model)
        {

            mapInfo.AddMapInfo(model);
            mapInfo.Save(StoreSettings.Current.DebugMode, true);


            return Request.CreateResponse("<script>window.close();</script>");

        }

        [HttpGet]
        [DnnAuthorize(StaticRoles = "Registered Users")]
        public HttpResponseMessage GetAddress()
        {
            var model = new GetMapInfo();

            if (model == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var response = new
            {
                success = true,
                model
            };

            return Request.CreateResponse(response);
        }
    }
}
