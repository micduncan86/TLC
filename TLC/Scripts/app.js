﻿var app = {
    teamLeaderList: null,
    modalFunction: function (modalTitle, modalContent, modalButton, callback) {
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
        app.Alert(alertTitle, alertMessage, "alert-success", callback)
    },
    ErrorAlert: function (alertTitle, alertMessage, callback) {
        app.Alert(alertTitle, alertMessage, "alert-danger", callback);
        $("footer").find(".ajaxError").remove();
        $("footer").append("<p class='ajaxError'>" + alertMessage + "</p>");
    },
    Alert: function (title, message, cssClass, callback) {
        var dAlert = $("<div/>", { class: "alert " + cssClass + " mySuccessMessage", role: "alert" });
        $("<h4/>", { class: "alert-heading", html: title }).appendTo(dAlert);
        $("<p/>", { html: message }).appendTo(dAlert);

        $(".body-content").append(dAlert);
        $(dAlert).fadeIn(500);
        $(dAlert).delay(2500).fadeOut(1500, function () {
            $(this).remove();
            if (callback) {
                callback();
            }
        });
    },
    ChangeLeader: function (ele) {
        if (ele) {
            app.GetTeamLeader(false, function (leaders) {
                var container = $(ele).parent();
                $(ele).remove();
                var sel = $("<select>").addClass("form-control").css("display", "inline-block").width(150).appendTo(container);
                $("<option>").html("Not Assigned").attr("value", "-1").appendTo(sel);
                var currentLeader = container.find("span").html();
                console.log(currentLeader);
                $.each(leaders, function (i, e) {
                    $("<option>").attr("value", e.UserId).html(e.UserName)
                        .prop("selected", e.UserName == currentLeader)
                        .appendTo(sel);
                });
                container.find("span").remove();
                sel.on("change", function () {
                    var leader = { Id: $(this).find(":selected").val(), Name: $(this).find(":selected").text(), TeamId: $("#hdnTeamId").val() };
                    app.SaveLeaderChange(leader, function (newleader) {
                        $("<span>", { html: newleader.Name }).insertBefore(sel);
                        $("<a>", { href: "#" }).html("<span class='glyphicon glyphicon-cog'></span>")
                            .on("click", function (e) {
                                e.preventDefault();
                                app.ChangeLeader($(this));
                            })
                            .insertBefore(sel);
                        sel.remove();
                    });
                });
            });
        }
        return false;
    },
    SaveLeaderChange: function (data, callback) {
        var jdata = { leader: new Array() };
        jdata.leader.push(JSON.stringify(data));
        $.ajax({
            url: "global/fetch.aspx/SaveTeamLeader",
            type: "POST",
            data: JSON.stringify(jdata),
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            success: function (resp) {
                if (callback) {
                    callback(data);
                }
            }
        });

    },
    ImportData: function (data, callback) {
        if (!data || data.length == 0) {
            return;
        }
        var jdata = { data: new Array() };
        jdata.data.push(JSON.stringify(data));
        $.ajax({
            url: "../global/fetch.aspx/ImportData",
            type: "POST",
            data: JSON.stringify(jdata),
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            success: function (resp) {
                if (callback) {
                    callback();
                }
            },
            error: function (xhr, ajaxoptions, thrownError) {
                app.ErrorAlert("Import Data Erorr", xhr.responseText);
            }
        });
    },
    GetTeamLeader: function (forceFetch, callback) {
        if (app.teamLeaderList == null || forceFetch) {
            $(document.body).css("cursor", "wait");
            $.ajax({
                url: "global/fetch.aspx/GetTeamLeaders",
                type: "POST",
                data: {},
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (resp) {
                    app.teamLeaderList = $.map(resp.d, function (n) {
                        return { UserId: n.UserId, UserName: n.UserName };
                    });
                    $(document.body).css("cursor", "default");
                    if (callback) {
                        callback(app.teamLeaderList);
                    }
                    return app.teamLeaderList;
                }
            });
        } else {
            if (callback) {
                callback(app.teamLeaderList);
            }
            return app.teamLeaderList;
        }
    },
    gDialog: null,
    GlobalDialog: function (Header, closeCallback) {
        $(".modal.in").modal("hide");
        var div = $("<div>")
                .attr("id", "bootModal")
                .attr("role", "dialog")
                .addClass("modal fade")
                .attr("title", Header);
        var divDialog = $("<div>").addClass("modal-dialog").appendTo(div);
        var divContent = $("<div>").addClass("modal-content").appendTo(divDialog);
        var divHeader = $("<div>").addClass("modal-header").appendTo(divContent);
        var divBody = $("<div>").addClass("modal-body").appendTo(divContent);
        var divFooter = $("<div>").addClass("modal-footer").appendTo(divContent);

        var btnX = $("<button>").attr("type", "button").addClass("close").attr("data-dismiss", "modal").html("&times;").appendTo(divHeader);
        var title = $("<h4>").html(div.attr("title")).appendTo(divHeader);

        var btnClose = $("<button>").attr("type", "button").addClass("btn btn-default").attr("data-dismiss", "modal").html("Close").appendTo(divFooter);



        div.modal("show");
        app.gDialog = div;
        return div;
    }
};

var tlcEvent = {
    AddNew: function (DialogTitle) {
        var n = app.GlobalDialog(DialogTitle, null);
        tlcEvent.BuildForm(n.find(".modal-body"));
        $(".datepicker").datepicker({
            format: "mm/dd/yyyy",
            startDate: "-1d"
        });
    },
    BuildForm: function(dialogContentElement){
        var form = $("<div>").appendTo(dialogContentElement);
        var fields = [
            { Name: "Title", type: "text", style: "max-width: none; width: 425px;", class: "" },
            { Name: "Team", type: "select", style: "width: 250px;", class: "" },
            { Name: "Date", type: "text", style: "", class: "datepicker" },
            {Name:"Description", type:"textarea", style:"",class:""},
            { Name: "Notes", type: "textarea", style: "", class: "" },
            { Name: "Complete", type: "checkbox", style: "", class: "" },

        ];
        $.each(fields, function (i, ele) {
            var d = $("<div>").addClass("form-group").appendTo(form);
            switch(ele.type){
                case "text":
                    $("<input>")
                        .attr("id","txt" + ele.Name)
                        .attr("type","text")
                        .attr("style",ele.style)
                        .attr("placeholder", ele.Name)
                        .addClass(ele.class)
                        .addClass("form-control")
                        .appendTo(d);
                    break;
                case "textarea":
                    $("<textarea>")
                        .attr("id","txt" + ele.Name)
                        .attr("type","text")
                        .attr("style",ele.style)
                        .attr("placeholder",ele.Name)
                        .attr("rows","3")
                        .addClass("form-control")
                        .addClass(ele.class)
                        .appendTo(d);
                    break;
                case "checkbox":
                    $("<label>")
                        .attr("style", ele.style)                        
                    .append(
                        $("<input>")
                       .attr("id","chk" + ele.Name)
                       .attr("type","checkbox")
                    )
                        .append("<span>Is " + ele.Name + "</span>")
                        .appendTo(d);
                    break;
                case "select":
                    $("<select>")
                      .attr("id","cbo" + ele.Name)       
                      .attr("style",ele.style)           
                      .addClass("form-control")
                      .addClass(ele.class)
                      .appendTo(d);
                    break;
            }
        });  
    }
};