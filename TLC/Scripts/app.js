var app = {
    teamLeaderList: null,
    selectedTeam: null,
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
    BrowserDetect: function(){
        return window.matchMedia("all and (max-width: 479px)").matches;
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
    ShowLoading: function(onOpen){
        var n = app.GlobalDialog.init("Loading...", function(){
            if (onOpen) {
                setTimeout(function () { onOpen();}, 350);
            }
        });
        $("<img>").attr("src", "../images/load_indicator.gif").appendTo(n.find(".modal-body"));
        $("<label>").css("padding","0 5px").html("Loading....").appendTo(n.find(".modal-body"));
        n.find(".modal-dialog").width(150);
        n.find(".modal-header").hide();
        n.find(".modal-footer").hide();
        var nTop = (($(window).height() + $(window).scrollTop()) * .5);
        n.css({
            "position": "absolute",
            "top": nTop + "px",
            "margin-top": "-125px"           
        });
        if (app.BrowserDetect()) {
            n.css({
                "margin-left": "-75px",
                "left": "50%"
            });
        }
    },
    ChangeLeader: function (ele) {
        if (ele) {
            app.GetTeamLeader(false, function (leaders) {
                var container = $(ele).parent();
                $(ele).remove();
                var sel = $("<select>").addClass("form-control").css("display", "inline-block").width(150).appendTo(container);
                $("<option>").html("Not Assigned").attr("value", "-1").appendTo(sel);
                var currentLeader = container.find("span").html();
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
    teamList: null,
    GetTeams: function (forceFetch, callback) {
        if (app.teamList == null || forceFetch) {
            $(document.body).css("cursor", "wait");
            $.ajax({
                url: "global/fetch.aspx/GetTeams",
                type: "POST",
                data: {},
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (resp) {
                    app.teamList = resp.d;
                    $(document.body).css("cursor", "default");
                    if (callback) {
                        callback(app.teamList);
                    }
                    return app.teamList;
                }
            });
        } else {
            if (callback) {
                callback(app.teamList);
            }
            return app.teamList;
        }
    },
    GetEvents: function (teamId, callback) {
        var id = teamId ? teamId : "";
        var data = JSON.stringify({ teamid: id });
        $.ajax({
            url: "global/fetch.aspx/GetEvents",
            type: "POST",
            data: data,
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            success: function (resp) {
                var tEvents = JSON.parse(resp.d);
                if (callback) {
                    callback(tEvents);
                }
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
    GlobalDialog: {
        init: function (Header, afterOpen) {
            $(".modal.in").modal("hide");
            var div = $("#bootModal").length == 0 ? $("<div>") : $("#bootModal");

            div.attr("id", "bootModal").attr("role", "dialog").addClass("modal fade").attr("title", Header).empty();

            var divDialog = $("<div>").addClass("modal-dialog").appendTo(div);
            var divContent = $("<div>").addClass("modal-content").appendTo(divDialog);
            var divHeader = $("<div>").addClass("modal-header").appendTo(divContent);
            var divBody = $("<div>").addClass("modal-body").appendTo(divContent);
            var divFooter = $("<div>").addClass("modal-footer").appendTo(divContent);

            var btnX = $("<button>").attr("type", "button").addClass("close").attr("data-dismiss", "modal").html("&times;").appendTo(divHeader);
            var title = $("<h4>").html(div.attr("title")).appendTo(divHeader);

            //var btnClose = $("<button>").attr("type", "button").addClass("btn btn-default").attr("data-dismiss", "modal").html("Close").appendTo(divFooter);



            div.modal("show");
            app.gDialog = div;

            if (afterOpen) {
                div.off("shown.bs.modal").on('shown.bs.modal', function (e) {
                    afterOpen();
                });
            }
            return div;
        },
        AddButton: function (ButtonText, OnClickEvent) {
            var txt = "Button";
            var footer = $("#bootModal").find(".modal-footer");
            if (ButtonText) {
                txt = ButtonText;
            }
            var btn = $("<button>").attr("type", "button").addClass("btn btn-default").html(txt).appendTo(footer);
            if (OnClickEvent) {
                btn.on("click", function (e) {
                    e.preventDefault();
                    OnClickEvent();
                });
            }
        }
    }
};

var tlcEvent = {
    AddNew: function (DialogTitle) {
        var n = app.GlobalDialog.init(DialogTitle, function () {
            tlcEvent.BuildForm(n.find(".modal-body"), function () {
                $(".datepicker").datepicker({
                    format: "mm/dd/yyyy",
                    startDate: "-1d"
                });
                app.GetTeams(true, function (teams) {
                    var cbo = $("#bootModal").find("#cboTeam");
                    $.each(teams, function (indx, ele) {
                        $("<option>").attr("value", ele.TeamId).html(ele.TeamName).appendTo(cbo);
                    });
                    app.GlobalDialog.AddButton("Add New Event", function () {
                        tlcEvent.AddEvent(null, function () {
                            app.SuccessAlert("Success", "New Event Has been added", function () {
                                $(".modal.in").modal("hide");
                            });
                        });
                    });
                });
            });
        });

    },
    AddEvent: function (data, callback) {
        if (!data) {
            data = {};
            data.TeamId = $("#bootModal").find("#cboTeam").val();
            data.Title = $("#bootModal").find("#txtTitle").val();
            data.EventDate = $("#bootModal").find("#txtDate").val();
            data.Description = $("#bootModal").find("#txtDescription").val();
            data.Notes = $("#bootModal").find("#txtNotes").val();
            data.Completed = $("#bootModal").find("#chkComplete").is(":checked");
            data.Cancelled = $("#bootModal").find("#chkCancelled").is(":checked");
        }
        var jdata = { data: new Array() };
        jdata.data.push(JSON.stringify(data));
        $.ajax({
            url: "global/fetch.aspx/AddEvent",
            type: "POST",
            data: JSON.stringify(jdata),
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            success: function (resp) {
                var addedEvent = JSON.parse(resp.d);
                if (callback) {
                    callback();
                }
            }
        });
    },
    BuildForm: function (dialogContentElement, callback) {
        var form = $("<div>").appendTo(dialogContentElement);
        var fields = [
            {
                Name: "Title", type: "text", style: "max-width: none; width: 425px;", class: ""
            },
            {
                Name: "Team", type: "select", style: "width: 250px;", class: ""
            },
            {
                Name: "Date", type: "text", style: "", class: "datepicker"
            },
            {
                Name: "Description", type: "textarea", style: "", class: ""
            },
            {
                Name: "Notes", type: "textarea", style: "", class: ""
            },
            {
                Name: "Complete", type: "checkbox", style: "", class: ""
            },
            {
                Name: "Cancelled", type: "checkbox", style: "", class: ""
            }

        ];
        $.each(fields, function (i, ele) {
            var d = $("<div>").addClass("form-group").appendTo(form);
            switch (ele.type) {
                case "text":
                    $("<input>")
                        .attr("id", "txt" + ele.Name)
                        .attr("type", "text")
                        .attr("style", ele.style)
                        .attr("placeholder", ele.Name)
                        .addClass(ele.class)
                        .addClass("form-control")
                        .appendTo(d);
                    break;
                case "textarea":
                    $("<textarea>")
                        .attr("id", "txt" + ele.Name)
                        .attr("type", "text")
                        .attr("style", ele.style)
                        .attr("placeholder", ele.Name)
                        .attr("rows", "3")
                        .addClass("form-control")
                        .addClass(ele.class)
                        .appendTo(d);
                    break;
                case "checkbox":
                    $("<label>")
                        .attr("style", ele.style)
                    .append(
                        $("<input>")
                       .attr("id", "chk" + ele.Name)
                       .attr("type", "checkbox")
                    )
                        .append("<span>Is " + ele.Name + "?</span>")
                        .appendTo(d);
                    break;
                case "select":
                    $("<label>").html(ele.Name).appendTo(d);
                    $("<select>")
                      .attr("id", "cbo" + ele.Name)
                      .attr("style", ele.style)
                      .addClass("form-control")
                      .addClass(ele.class)
                      .appendTo(d);
                    break;
            }
        });
        if (callback) {
            callback();
        }
    },
    LoadEvents: function (teamId, container, callback) {
        app.GetEvents(teamId, function (tEvents) {
            var columns = [
                {
                    Name: "", class: "col-xs-1", dataField: ""
                },
                {
                    Name: "Title", class: "col-xs-2", dataField: "Title"
                },
                {
                    Name: "Date", class: "col-xs-2", dataField: "EventDate"
                },
                {
                    Name: "Description", class: "col-xs-7", dataField: "Description"
                }
            ];

            container.empty();

            var div = $("<div>").css({
                "overflow-x": "hidden",
                "overflow-y": "auto",
                "height": "105px"
            }).appendTo(container);

            var tbl = $("<table>").addClass("table table-striped table-hover").appendTo(div);
            var thead = $("<thead>").appendTo(tbl);
            var tr = $("<tr>").appendTo(thead);

            $.each(columns, function (i, ele) {
                $("<th>").addClass(ele.class).html(ele.Name).appendTo(tr);
            });

            var tbody = $("<tbody>").appendTo(tbl);
            $.each(tEvents, function (idx, evnt) {
                tr = $("<tr>").appendTo(tbody);
                $.each(columns, function (indx, col) {
                    $("<td>").addClass(col.class).html(evnt[col.dataField]).appendTo(tr);
                });                
            });
            


        });
    }
    //<div style="overflow-x: hidden; overflow-y: auto; height: 105px;">
    //                            <table class="table table-striped table-hover">
    //                                <thead>
    //                                    <tr>
    //                                        <th class="col-xs-1"></th>
    //                                        <th class="col-xs-2">Title</th>
    //                                        <th class="col-xs-2 col-mobile">Date</th>
    //                                        <th class="col-xs-7 col-mobile">Description</th>
    //                                    </tr>
    //                                </thead>
    //                                <tbody>
    //                                    <asp:PlaceHolder ID="phGroup" runat="server"></asp:PlaceHolder>
    //                                </tbody>
    //                            </table>
    //                        </div>
};
$(document).ready(function(){
    $(".body-content a").on("click",function(e){
        app.ShowLoading(e);
    });
});

