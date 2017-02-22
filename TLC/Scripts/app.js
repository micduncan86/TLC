var app = {

    modalFunction: function (modalTitle, modalContent, modalButton) {
        var modal = $("#myModal");
        $(".modal-title").html(modalTitle);
        modal.find(".modal-body").html(modalContent);
        $(modalButton).appendTo($("#myModal").find(".modal-footer").empty());
        modal.show();
    },
    SuccessAlert: function (alertTitle, alertMessage, callback) {
        var dAlert = $("<div/>", {class: "alert alert-success", role: "alert", style: "position: absolute; top: 0; margin-top: 70px; width: 350px;display:none;"});
        $("<h4/>", {class: "alert-heading", html: alertTitle}).appendTo(dAlert);
        $("<p/>",{html: alertMessage}).appendTo(dAlert);

        $(".body-content").append(dAlert);
        $(dAlert).fadeIn(500).delay(2500).fadeOut(1500, function () {
            $(this).remove();
            if (callback) {
                callback();
            }
        });
    }
};