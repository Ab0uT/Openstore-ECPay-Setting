

$(document).ready(function () {

    $('#OS_AllShipping_cmdSave').unbind("click");
    $('#OS_AllShipping_cmdSave').click(function () {
        $('.processing').show();
        $('.actionbuttonwrapper').hide();
        nbxget('os_allshipping_savesettings', '.OS_AllShippingdata', '#datadisplay');
    });

    $('.selectlang').unbind("click");
    $(".selectlang").click(function () {
        $('.editlanguage').hide();
        $('.actionbuttonwrapper').hide();
        $('.processing').show();
        $("#nextlang").val($(this).attr("editlang"));
        nbxget('os_allshipping_selectlang', '.OS_AllShippingdata', '.OS_AllShippingdata');
    });


    $(document).on("nbxgetcompleted", NBS_os_allshipping_nbxgetCompleted); // assign a completed event for the ajax calls

    // function to do actions after an ajax call has been made.
    function NBS_os_allshipping_nbxgetCompleted(e) {

        $('.processing').hide();
        $('.actionbuttonwrapper').show();
        $('.editlanguage').show();
    }
});

