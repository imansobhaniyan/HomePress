
$(document).ready(function () {

    $("input.numeric").numeric();
    $("input.money").money();

    $("form.need-validation").each(function () {

        $(this).find("input,select,textarea").each(function () {
            if (isRequired(this) || $(this).prop("type") == "email")
                $(this).on("keyup input change", function () {
                    if (hasValue(this)) {
                        $(this).removeClass("is-invalid");
                        var eq = 0;
                        if ($(this).data("required-error-skip-row"))
                            eq = $(this).data("required-error-skip-row");
                        $(this).parents(".row:eq(" + eq + ")").find(".error").remove();
                    }
                });

            if (isRequired(this)) {
                $(this).prop("required", false);

                var inputElement = $(this);
                inputElement.data("required", true);

                $(this).parents(".row:eq(0)").find("label").addClass("required");
            }

            if ($(this).prop("type") == "email")
                $(this)
                    .prop("type", "text")
                    .data("email", true);
        });

        $(this).submit(function () {
            var isValid = true;

            $("button[type=submit]").prop("disabled", true);

            $(this).find("input,select,textarea").each(function () {
                if (isRequired(this) && !hasValue(this)) {
                    $(this).addClass("is-invalid");
                    var eq = 0;
                    if ($(this).data("required-error-skip-row"))
                        eq = $(this).data("required-error-skip-row");
                    $(this).parents(".row:eq(" + eq + ")").append("<small class='col-xl-4 col-lg-3 col-md-12 text-danger text-sm mt-3 error'>This field cannot be left blank</small>");
                    isValid = false;
                } else if (hasValue(this) && $(this).data("email") && !isValidEmail($(this).val())) {
                    $(this).addClass("is-invalid");
                    $(this).parents(".row:eq(0)").append("<small class='col-xl-4 col-lg-3 col-md-12 text-danger text-sm mt-3 error'>Invalid email address</small>");
                    isValid = false;
                }
                else {
                    $(this).removeClass("is-invalid");
                    var eq = 0;
                    if ($(this).data("required-error-skip-row"))
                        eq = $(this).data("required-error-skip-row");
                    $(this).parents(".row:eq(" + eq + ")").find(".error").remove();
                }
            });

            if (!isValid)
                $("button[type=submit]").prop("disabled", false);

            return isValid;
        });
    });

    $(".select-item,.select-all,.select-all-action").change(function () {
        setTimeout(function () {
            var count = 0;

            $(".select-all-action")
                .prop("indeterminate", false)
                .prop("checked", true);

            $(".select-item").each(function () {
                if ($(this).is(":checked"))
                    count++;
                else
                    $(".select-all-action")
                        .prop("indeterminate", true)
                        .prop("checked", false);
            });

            if (count == 0) {
                $(".selected-items-container").addClass("hidden");
                $(".select-all").show();
            }
            else {
                $(".select-all").hide();
                $(".selected-items-container").removeClass("hidden");
                $(".selected-items-count").text(count + " item(s) selected!");
            }

        }, 50);
    }).change();

    function isValidEmail(mailText) {
        return /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i.test(mailText);
    }

    function isRequired(inputElement) {
        return $(inputElement).prop("required") || $(inputElement).data("required");
    }

    function hasValue(inputElement) {
        return $(inputElement).val().trim().length != 0;
    }
});

function ShowDeleteModal(id) {

    if (!id) {
        id = "";
        $(".select-item").each(function () {
            if ($(this).is(":checked")) {
                if (id.length != 0)
                    id += ",";
                id += $(this).val();
            }
        });
    }

    $(".delete-modal").modal("show");
    $(".delete-modal").find("input[name=ids]").val(id);
}

function SetPage(pageNumber) {

    var href = location.href.split("?")[0];
    var hash = location.href.split("?")[1];

    if (hash) {
        hash = hash.split("&");
        for (var i = 0; i < hash.length; i++) {
            if (hash[i].indexOf("pageNumber=") == -1) {
                if (href.indexOf("?") == -1)
                    href += "?"
                else
                    href += "&";
                href += hash[i];
            }
        }
    }

    if (pageNumber != 1) {
        if (href.indexOf("?") == -1)
            href += "?pageNumber=" + pageNumber;
        else
            href += "&pageNumber=" + pageNumber;
    }

    location.href = href;
}









jQuery.fn.numeric = function (decimal, callback) {
    decimal = decimal || ".";
    callback = typeof callback == "function" ? callback : function () { };
    this.keypress(
        function (e) {
            var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
            // allow enter/return key (only when in an input box)
            if (key == 13 && this.nodeName.toLowerCase() == "input") {
                return true;
            }
            else if (key == 13) {
                return false;
            }
            var allow = false;
            // allow Ctrl+A
            if ((e.ctrlKey && key == 97 /* firefox */) || (e.ctrlKey && key == 65) /* opera */) return true;
            // allow Ctrl+X (cut)
            if ((e.ctrlKey && key == 120 /* firefox */) || (e.ctrlKey && key == 88) /* opera */) return true;
            // allow Ctrl+C (copy)
            if ((e.ctrlKey && key == 99 /* firefox */) || (e.ctrlKey && key == 67) /* opera */) return true;
            // allow Ctrl+Z (undo)
            if ((e.ctrlKey && key == 122 /* firefox */) || (e.ctrlKey && key == 90) /* opera */) return true;
            // allow or deny Ctrl+V (paste), Shift+Ins
            if ((e.ctrlKey && key == 118 /* firefox */) || (e.ctrlKey && key == 86) /* opera */
                || (e.shiftKey && key == 45)) return true;
            // if a number was not pressed
            if (key < 48 || key > 57) {
                /* '-' only allowed at start */
                if (key == 45 && this.value.length == 0) return true;
                /* only one decimal separator allowed */
                if (key == decimal.charCodeAt(0) && this.value.indexOf(decimal) != -1) {
                    allow = false;
                }
                // check for other keys that have special purposes
                if (
                    key != 8 /* backspace */ &&
                    key != 9 /* tab */ &&
                    key != 13 /* enter */ &&
                    key != 35 /* end */ &&
                    key != 36 /* home */ &&
                    key != 37 /* left */ &&
                    key != 39 /* right */ &&
                    key != 46 /* del */
                ) {
                    allow = false;
                }
                else {
                    // for detecting special keys (listed above)
                    // IE does not support 'charCode' and ignores them in keypress anyway
                    if (typeof e.charCode != "undefined") {
                        // special keys have 'keyCode' and 'which' the same (e.g. backspace)
                        if (e.keyCode == e.which && e.which != 0) {
                            allow = true;
                        }
                        // or keyCode != 0 and 'charCode'/'which' = 0
                        else if (e.keyCode != 0 && e.charCode == 0 && e.which == 0) {
                            allow = true;
                        }
                    }
                }
                // if key pressed is the decimal and it is not already in the field
                if (key == decimal.charCodeAt(0) && this.value.indexOf(decimal) == -1) {
                    allow = true;
                }
            }
            else {
                allow = true;
            }
            return allow;
        }
    )
        .blur(
            function () {
                var val = jQuery(this).val();
                if (val != "") {
                    var re = new RegExp("^\\d+$|\\d*" + decimal + "\\d+");
                    if (!re.exec(val)) {
                        callback.apply(this);
                    }
                }
            }
        )
    return this;
}

jQuery.fn.money = function (decimal, callback) {
    decimal = decimal || ".";
    callback = typeof callback == "function" ? callback : function () { };
    this.keypress(
        function (e) {

            var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
            // allow enter/return key (only when in an input box)
            if (key == 13 && this.nodeName.toLowerCase() == "input") {
                return true;
            }
            else if (key == 13) {
                return false;
            }
            var allow = false;
            // allow Ctrl+A
            if ((e.ctrlKey && key == 97 /* firefox */) || (e.ctrlKey && key == 65) /* opera */) return true;
            // allow Ctrl+X (cut)
            if ((e.ctrlKey && key == 120 /* firefox */) || (e.ctrlKey && key == 88) /* opera */) return true;
            // allow Ctrl+C (copy)
            if ((e.ctrlKey && key == 99 /* firefox */) || (e.ctrlKey && key == 67) /* opera */) return true;
            // allow Ctrl+Z (undo)
            if ((e.ctrlKey && key == 122 /* firefox */) || (e.ctrlKey && key == 90) /* opera */) return true;
            // allow or deny Ctrl+V (paste), Shift+Ins
            if ((e.ctrlKey && key == 118 /* firefox */) || (e.ctrlKey && key == 86) /* opera */
                || (e.shiftKey && key == 45)) return true;
            // if a number was not pressed
            if (key < 48 || key > 57) {
                /* '-' only allowed at start */
                if (key == 45 && this.value.length == 0) return true;
                /* only one decimal separator allowed */
                if (key == decimal.charCodeAt(0) && this.value.indexOf(decimal) != -1) {
                    allow = false;
                }
                // check for other keys that have special purposes
                if (
                    key != 8 /* backspace */ &&
                    key != 9 /* tab */ &&
                    key != 13 /* enter */ &&
                    key != 35 /* end */ &&
                    key != 36 /* home */ &&
                    key != 37 /* left */ &&
                    key != 39 /* right */ &&
                    key != 46 /* del */
                ) {
                    allow = false;
                }
                else {
                    // for detecting special keys (listed above)
                    // IE does not support 'charCode' and ignores them in keypress anyway
                    if (typeof e.charCode != "undefined") {
                        // special keys have 'keyCode' and 'which' the same (e.g. backspace)
                        if (e.keyCode == e.which && e.which != 0) {
                            allow = true;
                        }
                        // or keyCode != 0 and 'charCode'/'which' = 0
                        else if (e.keyCode != 0 && e.charCode == 0 && e.which == 0) {
                            allow = true;
                        }
                    }
                }
                // if key pressed is the decimal and it is not already in the field
                if (key == decimal.charCodeAt(0) && this.value.indexOf(decimal) == -1) {
                    allow = true;
                }
            }
            else {
                allow = true;
            }
            return allow;
        }
    )
        .keyup(
            function () {
                var ctrl = this;
                var separator = ",";
                var int = ctrl.value.replace(new RegExp(separator, "g"), "");
                var regexp = new RegExp("\\B(\\d{3})(" + separator + "|$)");
                do {
                    int = int.replace(regexp, separator + "$1");
                }
                while (int.search(regexp) >= 0)
                ctrl.value = int;
            }
        )
    return this;
}

jQuery.fn.checked = function (decimal, callback) {
    return $(this).is(":checked");
}