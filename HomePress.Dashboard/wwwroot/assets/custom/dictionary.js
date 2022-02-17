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

    $(".translate-input").each(function () {
        $(this).data("value", $(this).val());
    });

    $(".translate-input").on("keyup", function () {
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