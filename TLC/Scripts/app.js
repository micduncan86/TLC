var app = {

    modalFunction: function (modalTitle, modalContent, modalButton) {
        var modal = $("#myModal");
        $(".modal-title").html(modalTitle);
        modal.find(".modal-body").html(modalContent);
        $(modalButton).appendTo($("#myModal").find(".modal-footer").empty());
        modal.show();
    },
};