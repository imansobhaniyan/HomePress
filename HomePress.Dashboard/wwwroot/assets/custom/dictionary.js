$(document).ready(function () {
    $("a[href='#show-add-modal']").click(function () {
        event.preventDefault();
        var modal = $("#modal-add-key");
        modal.find("input[name=key]").val("").removeClass("is-invalid");
        modal.find("input[name=value]").val("").removeClass("is-invalid");
        modal.find(".error").remove();
        var form = modal.find("form");
        var token = form.find("input[name=__RequestVerificationToken]");
        token.remove();
        form.prepend(token);
        modal.modal("show");
    });

    var index = 0;

    $(".translate-input").each(function () {
        $(this).data("value", $(this).val());
        $(this).data("index", index++);
    });

    $(".translate-input").on("keyup", function (e) {

        if (e.key == "Enter") {
            if ($(this).data("value") != $(this).val())
                $(this).parent().find(".submit").click();
            var index = parseInt($(this).data("index"));
            $(".translate-input").each(function () {
                if ($(this).data("index") == index + 1) {
                    $(this).focus();
                    var that = this;
                    setTimeout(function () { that.selectionStart = that.selectionEnd = 10000; }, 10);
                }
            });
        }

        if ($(this).data("value") != $(this).val()) {
            $(this)
                .addClass("pr-50")
                .parent().find(".actions-container").removeClass("hidden");
        } else {
            $(this)
                .parent().find(".actions-container").addClass("hidden");
        }
    });

    $(".reset").click(function () {
        var input = $(this).parents(".translate-cell:eq(0)").find("input");
        input.val(input.data("value"));
        input.keyup();
    });

    $(".submit").click(function () {
        var input = $(this).parents(".translate-cell:eq(0)").find("input");

        $.ajax({
            method: "POST",
            url: "/settings/dictionary/setvalue",
            data: {
                key: input.data("key"),
                iso: input.data("iso"),
                value: input.val()
            },
            success: function (response) {
                if (!response.success)
                    console.log(response.exceptionMessage);
            }
        });

        input.data("value", input.val());
        input.keyup();
    });
});