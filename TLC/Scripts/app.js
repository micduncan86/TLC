var app = {

    modalFunction: function (modalTitle, modalContent, modalButton,callback) {
        var modal = $("#myModal");
        $(".modal-title").html(modalTitle);
        modal.find(".modal-body").html(modalContent);
        if (modalButton) {
            $(modalButton).appendTo($("#myModal").find(".modal-footer").empty());
        }        
        modal.modal("show");
        $(modal).on("hidden.bs.modal", function () {
            if (callback) {
                callback();
            }
        });
    },
    SuccessAlert: function (alertTitle, alertMessage, callback) {
        var dAlert = $("<div/>", {class: "alert alert-success mySuccessMessage", role: "alert"});
        $("<h4/>", {class: "alert-heading", html: alertTitle}).appendTo(dAlert);
        $("<p/>",{html: alertMessage}).appendTo(dAlert);

        $(".body-content").append(dAlert);
        $(dAlert).fadeIn(500);
        $(dAlert).delay(2500).fadeOut(1500, function () {
            $(this).remove();
            if (callback) {
                callback();
            }
        });
    }
};