
//documentready
$(function () {

    //InitAccordion();
    //InitAccordionMain();

    //if the status responce 403 then not authorized and catch the message came by json
    $(document).ajaxError(function (e, xhr) {
        if (xhr.status === 403) {
            var response = $.parseJSON(xhr.responseText);
            window.location = response.LogOnUrl;
        }
    });

    //when ajax call success
    $(document).ajaxSuccess(function (event, xhr, settings) {
        //if (settings.url == "ajax/test.html") {
        //    $(".log").text("Triggered ajaxSuccess handler. The Ajax response was: " +
        //      xhr.responseText);
        //}

        //reinitialaize datepicker
        initdatepicker();

        //reinitialaize click event for all Panels because the new added panels didnt inialized  when we called main.init()
        jQuery(document).ready(function () {
            $('.panel-tools .panel-collapse').unbind('click');
            $('.panel-tools .panel-collapse').bind('click', function (e) {
                e.preventDefault();
                var el = jQuery(this).parent().closest(".panel").children(".panel-body");
                if ($(this).hasClass("collapses")) {
                    $(this).addClass("expand").removeClass("collapses");
                    el.slideUp(200);
                } else {
                    $(this).addClass("collapses").removeClass("expand");
                    el.slideDown(200);
                }
            });

        });
    });


});
function initdatepicker() {
    //$(".picker").datepicker();
    $(document).ready(function () {
        $('.picker').datepicker({ showOn: 'focus' }).focus();
    });

}
//$(document).ready(function () {
//    //Finding AntiForgeryToken input
//    var antiForgeryToken = $('input[name=__RequestVerificationToken]').first();
//    if (antiForgeryToken.length > 0) {
//        //Serializing AntiForgeryToken
//        var antiForgeryTokenSerialized = antiForgeryToken.serialize();
//        //For each anchor in page
//        $("a[data-ajax=true]").each(function (index, element) {
//            //Replace placeholder with serialized AntiForgeryToken
//            $(element).attr('href', $(element).attr('href').replace('__RequestVerificationToken=_', antiForgeryTokenSerialized));
//        });
//    }
//});

function OpenModal() {

    //$("#SystemModal").modal('show');
    $("#SystemModal").modal({
        backdrop: 'static',
        keyboard: false,
        show:true
    });

   // $("#SystemModal").modal("toggle");
  
    //$("#dialog").dialog("open");
}
function CloseModal() {
    $("#SystemModal").modal("hide");
    //$("#dialog").dialog("close");
}

//interval for close bootstrap alert
function createAutoClosingAlert(selector, delay) {
    var alert = $(selector).alert();
    window.setTimeout(function () { alert.alert('close') }, delay);
}


////Accordion Section
//function InitAccordion() {

//    $(".accordion").accordion({
//        active: false,
//        autoHeight: false,
//        collapsible: true,
//        heightStyle: "content"


//    });
//}
////Accordion Section
//function InitAccordionMain(input) {
//    if (input === null)
//        input = 0;
//    else
//        input = false;

//    $(".accordionMain").accordion({
//        active: input,
//        autoHeight: false,
//        collapsible: true,
//        heightStyle: "content"

//    });
//}





////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////// Ajax Section //////////////////////////////////////////////////////

//
function CasscadingReturnJason(target, data, URL) {

    $("#" + target).empty();

    $.ajax({

        type: 'POST',

        url: URL, // we are calling json method

        dataType: 'json',

        data: data,

        // here we are get value of selected country and passing same value as inputto json method GetStates.

        success: function (colData) {

            // states contains the JSON formatted list

            // of states passed from the controller

            $.each(colData, function (i, data) {
                if (data.Selected === true)
                    $("#" + target).append('<option value="' + data.Value + '" selected="' + data.Selected + '">' + data.Text + '</option>');
                else
                    $("#" + target).append('<option value="' + data.Value + '">' + data.Text + '</option>');


                // here we are adding option for States



            });

        },

        error: function (ex) {

            alert('Failed to retrieve data.' + ex);

        }

    });

}

function AjaxReturnHTML(target, data, URL) {

    $("#" + target).empty();

    $.ajax({

        type: 'POST',

        url: URL, // we are calling json method

        dataType: 'html',

        data: data,

        // here we are get value of selected country and passing same value as inputto json method GetStates.

        success: function (data) {
            $("#" + target).html(data);

        },

        error: function (ex) {

            alert('Failed to retrieve data.' + ex);

        }

    });

}

//jsson  class
CallJsonMethod = {
    data: null,
    URL: null,
    returnType: 'HTML',
    PostGet: 'POST',
    onsuccess: null,
    onFail: null,
    callAjax: function () {
        $.ajax({

            type: this.PostGet,

            url: this.URL, // we are calling json method

            dataType: this.returnType,

            data: this.data,

            // here we are get value of selected country and passing same value as inputto json method GetStates.

            success: this.onsuccess,

            error: this.onFail
            //function (ex) {


            // }

        });
    }
}





