/*---------------------Drop Down List----------------------*/
$.fn.clearSelect = function () {
    return this.each(function () {
        if (this.tagName == 'SELECT')
            this.options.length = 0;
    });
};
$.fn.fillSelect = function (data) {
    console.log(data);
    return this.clearSelect().each(function () {
        if (this.tagName != 'SELECT')
            return;
        var dropdownList = this;
        $.each(data, function (index, optionData) {
            var option = $('<option ' + ((optionData.Selected) ? 'Selected' : '') + '>' + optionData.Id + '</option>').val(optionData.Id).html(optionData.Name);
            var option = new Option(optionData.Text, optionData.Value, optionData.Selected);
            //if ($.browser.msie)
            //    dropdownList.add(option);
            //else
            dropdownList.add(option, null);
        });
    });
};
function fillDropDownList(selector, data) {
    $(selector).each(function (index, select) {
        $(select).fillSelect(data);
        $(select).trigger("chosen:updated");
    });
}
function cascadeDDL(ddl, source, data) {
    $.ajax({
        type: "POST",
        url: source,
        data: data,
        dataType: "Json",
        success: function (result) {
            $(ddl).fillSelect(result);
            $(ddl).trigger("chosen:updated");
        },
        async: false
    });
}
function cascadeMultiSelect(ddl, source, data) {
    $.ajax({
        type: "POST",
        url: source,
        data: data,
        dataType: "Json",
        success: function (result) {
            debugger;
            $(ddl).fillMultiSelect(result);
        },
        async: false
    });
}
$.fn.fillMultiSelect = function (data) {
    return this.clearSelect().each(function () {
        if (this.tagName != 'SELECT')
            return;
        var dropdownList = $(this);
        $.each(data, function (index, optionData) {
            debugger;
            var option = $('<option ' + ((optionData.Selected) ? 'Selected' : '') + '>'+optionData.Key+'</option>').val(optionData.Value).html(optionData.Text);
            dropdownList.append(option);
        });
    });
};

/*---------------------Ajax Request----------------------*/
/*"http://api.jquery.com/jquery.ajax/"*/
function ajaxRequest(type, url, data, dataType, contentType, processData, cache, async, showLoader) {
    if (dataType == null) {
        dataType = "json";
    }
    if (contentType == null) {
        contentType = 'application/x-www-form-urlencoded; charset=UTF-8';
    }
    if (processData == null) {
        processData = true;
    }
    if (cache == null) {
        cache = true;
    }
    if (async == null) {
        async = false;
    }
    if (showLoader == null) {
        showLoader = true;
    }
    var options = {
        dataType: dataType,
        contentType: contentType,
        cache: cache,
        type: type,
        data: data || null,
        processData: processData,
        async: async,
        beforeSend: function () {
            if (showLoader) {
                $('.loading').show('fade', null, 1000);
            }
        }
    };
    var antiForgeryToken = $("#antiForgeryToken").val();
    if (antiForgeryToken) {
        options.headers = {
            'RequestVerificationToken': antiForgeryToken
        };
    }
    return $.ajax(url, options).success(function (data, textStatus, request) {
        if (request.responseText.indexOf("REQUIRES_AUTH") > 0) {
            window.location.reload();
        };
    }).complete(function () {
        $('.loading').hide('fade', null, 1000);
    }).error(function () {
        $('.loading').hide('fade', null, 1000);
        //todo:write appropriate message for failed requests
        showMessage('error', 'Error', 'error');
    });
}

/*---------------------Form Submit----------------------*/
var getValidationSummaryErrors = function ($form) {
    var errorSummary = $form.find('.validation-summary-errors, .validation-summary-valid');
    return errorSummary;
};
var displayErrors = function (form, errors) {
    var errorSummary = getValidationSummaryErrors(form)
        .removeClass('validation-summary-valid')
        .addClass('validation-summary-errors');

    var items = $.map(errors, function (error) {
        return '<li>' + error + '</li>';
    }).join('');

    errorSummary.find('ul').empty().append(items);
};
$.fn.formSubmitHandler = function (e) {
    var $form = $(this);

    // We check if jQuery.validator exists on the form
    if (!$form.valid || $form.valid()) {
        $.post($form.attr('action'), $form.serializeArray())
            .done(function (json) {
                json = json || {};

                // In case of success, we redirect to the provided URL or the same page.
                if (json.success) {
                    window.location = json.redirect || location.href;
                } else if (json.errors) {
                    displayErrors($form, json.errors);
                }
            })
            .error(function () {
                displayErrors($form, ['An unknown error happened.']);
            });
    }

    // Prevent the normal behavior since we opened the dialog
    e.preventDefault();
};

/*---------------------Jquery Cookies----------------------*/
var cookieList = function (cookieName) {
    //When the cookie is saved the items will be a comma seperated string
    //So we will split the cookie by comma to get the original array
    var cookie = $.cookie(cookieName);
    //Load the items or a new array if null.
    var items = cookie ? cookie.split(/,/) : new Array();

    //Return a object that we can use to access the array.
    //while hiding direct access to the declared items array
    //this is called closures see http://www.jibbering.com/faq/faq_notes/closures.html
    return {
        "add": function (val) {
            //Add to the items.
            items.push(val);
            //Save the items to a cookie.
            //EDIT: Modified from linked answer by Nick see 
            //      http://stackoverflow.com/questions/3387251/how-to-store-array-in-jquery-cookie
            $.cookie(cookieName, items.join(','));
        },
        "clear": function () {
            items = null;
            //clear the cookie.
            $.cookie(cookieName, null);
        },
        "items": function () {
            //Get all the items.
            return items;
        }
    };
};

/*---------------------DisableButton----------------------*/
function disableButton(button) {
    button.attr("disabled", "disabled");
    button.css("color", "silver");
}

/*--------------------EnableButton-------------------------*/
function enableButton(button) {
    button.removeAttr("disabled");
    button.css("color", "#333333");
}

function SwitchDiv(fromId, toId, effect, focusElementId) {
    $(fromId).hide(effect || "slide", function () {
        $(toId).show(effect || "slide", function () {
            if ($(focusElementId).length) {
                $(focusElementId).focus();
            }
        });
    });
}

/*--------------------JQueryEffects-------------------------*/
function runEffect(divId, effect, options) {
    // most effect types need no options passed by default { to: "#button", className: "ui-effects-transfer" }
    // run the effect
    $(divId).effect(effect, options, 500);
};

// callback function to bring a hidden box back
function callback(divId) {
    setTimeout(function () {
        $(divId).removeAttr("style").hide().fadeIn();
    }, 1000);
};

/*----------------------Show Alert Message------------------*/
function showMessage(type, title, message) {
    $('.tooltip').css('display', 'none');
    var $class = "alert-success";
    if (type == "error") {
        $class = "alert-danger";
    } else if (type == "warning") {
        $class = "alert-warning";
    } else if (type == "info") {
        $class = "alert-info";
    }
    $('.messages').append('<div class="control-group"><div class="alert ' + $class + '"><a class="close">×</a><strong>' + (title || '') + '</strong>' + message + '</div></div>');
}
/*-------------js alert popup function ---------------*/

//function ShowMessage(type, title, message) {
//    var footer1 =  ('<div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button><button type="button" class="btn btn-primary">Ok</button></div>');
//    var footer2=('<div class="modal-footer"><button type="button" class="btn btn-danger" data-dismiss="modal">No</button><button type="button" class="btn btn-success">Yes</button></div>')
//    if (type == "error") {
//        $class = footer1;
//    } else if (type == "warning") {
//        $class = footer2;
//    } else if (type == "info") {
//        $class = footer2;
//    }
//}
//$('.modal-content').append('<div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title" id="myModalLabel">' + (title || '') + '</h4></div><div class="modal-body">' + message + '</div>' + $class + '');


/*--------------------DatePicker----------------------------*/
function enableDatePicker(object) {
    $(object).datepicker({
        showStatus: true,
        showWeeks: true,
        highlightWeek: true,
        numberOfMonths: 1,
        showOtherMonths: true,
        selectOtherMonths: true,
        showAnim: "slide",
        showOptions: {
            origin: ["top", "left"]
        },
        autoSize: true,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        onClose: function () {
            if ($(this).parents("form").length) {
                $(this).valid();
            }
        }
    });
};

/*-----------------------TimePicker-------------------------*/
function enableTimePicker(object) {
    $(object).timepicker({
        onClose: function () {
            if ($(this).parents("form").length) {
                $(this).valid();
            }
        }
    });
};
/*-------------------chosen---------------------------------*/
function enableChosen() {
    $(".chzn-select").chosen({
        width: "100%"
    });
    $(".chzn-select-deselect").chosen({ allow_single_deselect: true, width: "100%" });
}

/*------------------------ChangeCursor----------------------*/
function progressCursor() {
    $("a").css("cursor", "progress");
}
function defaultCursor() {
    $('a').css("cursor", "pointer");
}

/*------------------------SerializeObject----------------------*/
var miscFeatures = {
    serializeJson: function (jsonObject) {
        var serializedString = "";
        for (item in jsonObject) {
            var obj = jsonObject[item];
            for (var prop in obj) {
                var propName = encodeURI(prop);
                var propValue = encodeURI(obj[prop]);
                serializedString += "&" + propName + "=" + propValue;
            }
        }
        return serializedString;
    }
};
var miscUriFeatures = {
    serializeJson: function (jsonObject) {
        var serializedString = "";
        for (item in jsonObject) {
             
            var propName = encodeURIComponent(item);
            var propValue = encodeURIComponent(jsonObject[item]);
            if(serializedString=="")
                serializedString = propName + "=" + propValue;
            else 
                serializedString += "&" + propName + "=" + propValue;
            
        }
        return serializedString;
    }
};
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        var name = this.name.replace('.', '_');
        if (o[name] !== undefined) {
            if (!o[name].push) {
                o[name] = [o[name]];
            }
            o[name].push(this.value || '');
        } else {
            o[name] = this.value || '';
        }
    });
    return o;
};