var getMap = $('#emapSubmit');
var shipping = $('input[name=extrainfo_shippingproviderradio]:checked');
var index = shipping.val();
console.log(index);
var typeName = ["", "UNIMART", "FAMI", "HILIFE"];
if (index !== "os_allshipping") {
    $("#pickupForm").show();
    let params = { 'LogisticsSubType': typeName[index] };

    getMap.click(function (e) {
        e.preventDefault();
        $.post(apiRoot + "GetMap/PostMap", params, function (result) {
            myWindow = window.open("", "NewFile", "width=730,height=345,left=100,top=100,resizable=yes,scrollbars=yes");
            myWindow.document.write(result);
            refreshTime();
        });
    });
}
else {
    $("#pickupForm").hide();
    $("#pickup_logisticstype").val("Home");
}
    

function refreshTime() {
    let closed = false;
    let checkWin = setInterval(function () {
        if (myWindow) {
            if (myWindow.closed) {
                closed = true;
                $.get(apiRoot + "GetMap/GetAddress", function (result) {
                    let model = result.model;
                    $("#pickup_logisticstype").val("CVS");
                    $("#pickup_logisticssubtype").val(model.LogisticsSubType);
                    $("#pickup_cvsstoreid").val(model.CVSStoreID);
                    $("#pickup_cvsstorename").val(model.CVSStoreName);
                });
                clearInterval(checkWin);
            }
        }
        console.log(closed);
    }, 2000);
}