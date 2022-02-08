
$(document).ready(function () {
    $("form.need-validation").each(function () {

        $(this).find("input,select").each(function () {
            if (isRequired(this) || $(this).prop("type") == "email")
                $(this).on("keyup, input, change", function () {
                    if (hasValue(this)) {
                        $(this).removeClass("is-invalid");
                        $(this).parents(".row:eq(0)").find(".error").remove();
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

            $(this).find("input,select").each(function () {
                if (isRequired(this) && !hasValue(this)) {
                    $(this).addClass("is-invalid");
                    $(this).parents(".row:eq(0)").append("<small class='col-xl-4 col-lg-3 col-md-12 text-danger text-sm mt-3 error'>This field cannot be left blank</small>");
                    isValid = false;
                } else if (hasValue(this) && $(this).data("email") && !isValidEmail($(this).val())) {
                    $(this).addClass("is-invalid");
                    $(this).parents(".row:eq(0)").append("<small class='col-xl-4 col-lg-3 col-md-12 text-danger text-sm mt-3 error'>Invalid email address</small>");
                    isValid = false;
                }
                else {
                    $(this).removeClass("is-invalid");
                    $(this).parents(".row:eq(0)").find(".error").remove();
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