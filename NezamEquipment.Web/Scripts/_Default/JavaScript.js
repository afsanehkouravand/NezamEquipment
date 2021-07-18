function isValidMobile(dateString) {

    var regexDate = /(\+98|0)?9\d{9}/;

    if (!regexDate.test(dateString)) {
        return false;
    }

    return true;
}
function isValidDate(dateString) {

    var regexDate = /^\d{4}\/\d{1,2}\/\d{1,2}$/;

    if (!regexDate.test(dateString)) {
        return false;
    }

    var parts = dateString.split("/");
    var day = parseInt(parts[2], 10);
    var month = parseInt(parts[1], 10);
    var year = parseInt(parts[0], 10);

    if (year < 1000 || year > 3000 || month == 0 || month > 12) {
        return false;
    }

    return day > 0 && day <= 31;
}
function checkCodeMeli(code) {
    var L = code.length;
    if (L < 8 || parseInt(code, 10) == 0) return false;
    code = ('0000' + code).substr(L + 4 - 10);
    if (parseInt(code.substr(3, 6), 10) == 0) return false;
    var c = parseInt(code.substr(9, 1), 10);
    var s = 0;
    for (var i = 0; i < 9; i++)
        s += parseInt(code.substr(i, 1), 10) * (10 - i);
    s = s % 11;
    return (s < 2 && c == s) || (s >= 2 && c == (11 - s));
}
function resetCoverImage(target) {
    target = target || "#CoverPreview";
    $(target).attr("src", "../../../Content/images/NoImageAvailable.png");
}
function getIdAndUpdateTarget(e, target) {
    $(target).val($(e).parents("[data-id]").data("id"));
}
function hideElementByDataId(id) {
    if (id instanceof Array) {
        for (var i = 0; i < id.length; i++) {
            $("tr[data-id=" + id[i] + "]").hide(500).remove();
        }
    } else 
        $("tr[data-id=" + id + "]").hide(500).remove();
}
function resetForm(target) {
    target = target || "#form form";
    if ($(target).length == 1) {
        $(target)[0].reset();
    }
    if ($("#form-send form").length == 1) {
        $(target)[0].reset();
    }
}
function removeClassFromTarget(className, target) {
    if (className.indexOf(" ") > 0) {
        var list = className.split(" ");
        for (var i = 0; i < list.length; i++) {
            $(target).removeClass(list[i]);
        }
    } else
        $(target).removeClass(className);
}
function addClassToTarget(className, target) {
    if (className.indexOf(" ") > 0) {
        var list = className.split(" ");
        for (var i = 0; i < list.length; i++) {
            $(target).addClass(list[i]);
        }
    } else
        $(target).addClass(className);
}
function hideModal(target) {
    $(target).modal('hide');
}
function showModal(target) {
    $(target).modal('show');
}


function disableWithClass(target, toggle) {
    if (target.indexOf(" ") > 0) {
        var list = target.split(" ");
        for (var i = 0; i < list.length; i++) {
            disableTarget(list[i], toggle);
        }
    } else
        disableTarget(target, toggle);
}
function disableTarget(target, toggle) {
    var e = $("." + target);
    if (toggle == "yes") {
        e.attr("disabled", "disabled");
    } else if (toggle == "no") {
        e.removeAttr("disabled");
    }
}
function readImageWithOutPutObject(input, output, attribute) {
    if (input.files && input.files[0]) {
        var fr = new FileReader();
        fr.onload = function (e) {
            output.attr(attribute, e.target.result);
        };
        fr.readAsDataURL(input.files[0]);
    }
}
function readImage(input, output, attribute) {
    if (input.files && input.files[0]) {
        var fr = new FileReader();
        fr.onload = function (e) {
            $('#' + output).attr(attribute, e.target.result);
        };
        fr.readAsDataURL(input.files[0]);
    }
}
function setPathWithOutPutObject(input, output) {
    output.val(input.files[0].name);
}
function setPath(input, id) {
    $("#" + id).val(input.files[0].name);
}
function showNoty(type, text) {
    var n = noty({
        text: text,
        template: '<div class="noty_message"><h4><span class="noty_text"></span></h2><div class="noty_close"></div></div>',
        layout: 'center',
        timeout: 3000,
        killer: true,
        maxVisible: 1,
        type: type
    });
}
function showNotyFailure(text) {
    text = text || 'خطایی در هنگام ارسال اطلاعات به سرور رخ داد.';
    showNoty('error', text);
}
function showNotySuccess(text) {
    text = text || 'اطلاعات ارسال شده با موفقیت ذخیره شدند.';
    showNoty('success', text);
}
function showNotyDanger(text) {
    text = text || 'امکان ذخیره اطلاعات ارسال شده وجود ندارد.';
    showNoty('error', text);
}
function GoToTop() {
    $("html, body").animate({ scrollTop: 0 }, 600);
}
function GoToElement(e) {
    $('html, body').animate({
        scrollTop: $(e).offset().top
    }, 600);
}
function GoToPage(path, time) {
    time = time || 2000;
    setTimeout(function () {
        window.location.href = path;
    }, time);
}
(function () {

    $(document).delegate('*[data-toggle="lightbox"]', 'click', function (event) {
        event.preventDefault();
        $(this).ekkoLightbox({
            loadingMessage: "در حال بارگذاری"
        });
    });

    $(".makeshamsidate").datepicker({
        format: "yyyy/mm/dd",
        minViewMode: 0,
        autoclose: true
    });

    $(".makemiladidate").datetimepicker({
        format: "yyyy/mm/dd",
        weekStart: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 2,
        forceParse: 0,
    });

    $(".checkonlynumber").mask("9?9?9?9?9?9?9?9?9?9?9?", { placeholder: "", autoclear: false });

})();


var GoToPageOnSuccessUrl = null;
var HideRowOnSuccessById = null;

////////////////////////////////////////////////////////////////////////
var HideDeleteModal = null;
function showModalForDelete(id) {
    $("#DeleteModal #Id").val(id);
    $("#DeleteModal").modal('show');
    HideDeleteModal = true;
}
$("body").on("click", "[data-delete=true]", function (e) {
    showModalForDelete($(this).data("id"));
    HideRowOnSuccessById = $(this).data("id");
    $("#DeleteModal form").attr("data-ajax-url", $(this).data("url"));
    var gotopage = $(this).attr("data-gotopage");
    if (typeof gotopage !== typeof undefined && gotopage !== false) {
        GoToPageOnSuccessUrl = $(this).data("gotopage");
    }
});


///////////////////////////////////////////////////////////////////////

//$("[data-delete]").click(function () {
//});

function OnBegin() {
    disableWithClass("fieldset", "yes");
}
function OnComplete() {
    //disableWithClass("fieldset", "no");
}
function OnCompleteFieldsetNo() {
    disableWithClass("fieldset", "no");
}
function OnFailure() {
    disableWithClass("fieldset", "no");
    showNotyFailure();
    HideDeleteModal = null;
}
function OnSuccess(data) {
    disableWithClass("fieldset", "no");
    if (typeof data == "string") {
        showNotyDanger(data);
    } else {
        if (data.IsSucceed == true) {
            disableWithClass("fieldset", "yes");
            showNotySuccess("اطلاعات مورد نظر با موفقیت ذخیره شدند.");
            if (HideDeleteModal != null) {
                hideModal("#DeleteModal");
                HideDeleteModal = null;
            }
            if (HideRowOnSuccessById != null) {
                hideElementByDataId(HideRowOnSuccessById);
                HideRowOnSuccessById = null;
            }
            if (GoToPageOnSuccessUrl != null) {
                GoToPage(GoToPageOnSuccessUrl);
                GoToPageOnSuccessUrl = null;
            }
        } else if (data.IsSucceed == false) {
            showNotyDanger(data.Message);
        } else {
            showNotyDanger("خطایی در هنگام ذخیره اطلاعات رخ داد.");
        }
    }
}
$("body").on("click", "[data-form='true']", function (e) {
    var form = $(this).data("form-id");
    if ($("#" + form).length > 0) {
        $("#" + form).submit();
    }
});
$("body").on("click", "[data-highlight=true]", function (e) {
    if ($(this).is(":checked")) {
        $(this).parents("tr").addClass("warning");
    } else {
        $(this).parents("tr").removeClass("warning");
    }
});

$("body").on("click", "[data-checkinputs]", function (e) {
    var name = $(this).data("checkinputs");
    $("input[name='" + name + "']").each(function () {
        $(this).prop('checked', true);
        if ($("[data-checkinputs]").is(":checked")) {
            $(this).prop('checked', true);
            $(this).parents("tr").addClass("warning");
        } else {
            $(this).prop('checked', false);
            $(this).parents("tr").removeClass("warning");
        }
    });
});

$("body").on("click", "[data-tabajax='true']", function (e) {
    DataAjaxTrue(this);
});

$("body").on("click", "[data-tabajax='true'].activeloading", function (e) {
    DataAjaxTrue($("[data-tabajax='true'].activeloading"));
});

function DataAjaxTrue(e) {
    var id = $(e).attr("aria-controls");
    if ($("#" + id + " .loading").length > 0) {
        $("#tabajax-loading form").attr("action", $(e).data("tabajax-url"));
        $("#tabajax-loading form").attr("data-ajax-url", $(e).data("tabajax-url"));
        $("#tabajax-loading form").attr("data-ajax-loading", "AjaxTabLoading");
        $("#tabajax-loading form").attr("data-ajax-update", "#" + $(e).attr("aria-controls"));
        $("#tabajax-loading form").attr("data-ajax-complete", "OnCompleteFieldsetNo");
        $("#tabajax-loading form").submit();
    }
}


$("body").on("click", "[data-popup='true']", function (e) {
    $("#PopUpModalBody").empty();
    $("#PopUpModalLoading").show();
    $("#PopUpModal .modal-title").html($(this).data("title"));
    $("#PopUpModal").modal("show");
    $("#FormPopUpModalRequest form").attr("action", $(this).data("url"));
    $("#FormPopUpModalRequest form").attr("data-ajax-url", $(this).data("url"));
    $("#FormPopUpModalRequest form").submit();
});

function OnSuccessPopUpModal() {
    $("#PopUpModalLoading").hide();
    disableWithClass("fieldset", "no");
}

$('#PopUpModal').on('hidden.bs.modal', function (e) {
        $("#PopUpModalBody").empty();
        $("#PopUpModal .modal-title").html("-");
    });


function formonsubmit(e) {
    var gotopage = $(e).attr("data-gotopage");
    if (typeof gotopage !== typeof undefined && gotopage !== false) {
        GoToPageOnSuccessUrl = $(e).data("gotopage");
    }
}




/*
    currencyPrice = نرخ ارز
    totalValueOfCurrencyDeclaration = ارزش کل کالا
    priceReference = ماخذ
    facilitationRate = تسهیل
    insuranceCost = بیمه
    TermsOfDeliveryCode = نوع حمل - fob cfr
    currencyCode = کد ارز
*/
function GlobalCheckPrice(
    currencyPrice,
    totalValueOfCurrencyDeclaration,
    priceReference,
    facilitationRate,
    insuranceCost,
    termsOfDeliveryCode,
    currencyCode) {

    if (totalValueOfCurrencyDeclaration == "" || totalValueOfCurrencyDeclaration == "0") {
        return {
            SaifRialValue: 0,
            ComplicationsOfRial: 0,
            Facilitate: 0,
            ComplicationsForPay: 0,
        };
    }

    if (priceReference == "") {
        priceReference = "0";
    }

    if (facilitationRate == "") {
        facilitationRate = "0";
    }

    var saifrial = currencyPrice * totalValueOfCurrencyDeclaration;
    if (currencyCode == "jpy" || currencyCode == "JPY") { // ین ژاپن
        saifrial = saifrial / 100;
    }
    if (currencyCode == "krw" || currencyCode == "KRW") { // وون کره
        saifrial = saifrial / 1000;
    }

    var bime;

    if (insuranceCost == "" || insuranceCost == "0") {
        bime = (0.5 / 100) * saifrial;
    } else {
        bime = parseInt(insuranceCost);
    }

    var saifrialbime = Math.ceil(saifrial + bime);

    if (termsOfDeliveryCode == "fob" || termsOfDeliveryCode == "fca" || termsOfDeliveryCode == "fas" ||
        termsOfDeliveryCode == "FOB" || termsOfDeliveryCode == "FCA" || termsOfDeliveryCode == "FAS") {
        saifrialbime = Math.ceil(saifrialbime + (0.10 * saifrialbime));
    } else if (termsOfDeliveryCode == "cfr" || termsOfDeliveryCode == "cif" || termsOfDeliveryCode == "cip" ||
        termsOfDeliveryCode == "CFR" || termsOfDeliveryCode == "CIF" || termsOfDeliveryCode == "CIP") {
        // معمولی
    } else {
        showNotyDanger("محاسبات برای این نوع رویه گمرکی در نظر گرفته نشده.");
        return {
            SaifRialValue: 0,
            ComplicationsOfRial: 0,
            Facilitate: 0,
            ComplicationsForPay: 0,
        };
    }

    var avarez = Math.ceil((priceReference / 100) * saifrialbime);

    var facilitate = Math.ceil((avarez / 100) * facilitationRate);

    var complicationsForPay = Math.ceil(avarez - facilitate);

    var values =
    {
        SaifRialValue: saifrialbime,
        ComplicationsOfRial: avarez,
        Facilitate: facilitate,
        ComplicationsForPay: complicationsForPay,
    };

    return values;
}

$(document).on('show.bs.modal', '.modal', function () {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
});
$(document).on('hidden.bs.modal', '.modal', function () {
    $('.modal:visible').length && $(document.body).addClass('modal-open');
});


function ImagetoPrint(source) {
    return "<html><head><script>function step1(){\n" +
        "setTimeout('step2()', 10);}\n" +
        "function step2(){window.print();window.close()}\n" +
        "</scri" + "pt></head><body onload='step1()'>\n" +
        "<img src='" + source + "' /></body></html>";
}
function PrintImage(source) {
    var pagelink = "about:blank";
    var pwa = window.open(pagelink, "_new");
    pwa.document.open();
    pwa.document.write(ImagetoPrint(source));
    pwa.document.close();
}
$("body").on("click", "[data-print='true']", function (e) {
    var parents = $(this).parents("td");
    var link = $("a[data-toggle]", parents).attr("href");
    if (link != undefined) {
        PrintImage(link);
        return;
    } else {
        showNotyDanger("امکان پرینت این فایل وجود ندارد.");
    }
});
