var teams = {
    getTeams: function (callback) {
        var jqXHR = $.ajax({
            url: "api/team",
            type: "GET",
            dataType: "json",
            success: function (response) {
                callback(response);
            }
        });
    },
    deleteTeam: function (TeamId, callback) {
        var jqXHR = $.ajax({
            url: "api/team/" + TeamId,
            type: "DELETE",
            success: function (results) {
                callback();
            }
        });
    },
    addTeam: function (postdata,callback) {
        var uri = "api/team"
        $.ajax({
            url: uri,
            data: { "": postdata },
            type: "POST",
            success: function (results) {
                callback(results);
            },
            error: function (xhr, ajaxOption, thrownError) {
                alert(xhr.status + ":" + thrownError);
            }
        });            
    },
    editTeam: function (url, teamid, callback) {
        //"teams/add"
        var body = $("#myModal").find(".modal-body").html("Loading....");
        $(".modal-title").html("Edit Team");
        if (teamid) {
            url += (!isNaN(teamid) ? "?id=" + teamid : "");
        }
        $.get(url, function (data, status) {
            var d = $(data).find(".form-group").html();
            var form = $("<form/>", { html: d });
            body.html("").append(form);
            $("<button/>", { class: "btn btn-success btn-sm", html: "Update Team" }).appendTo($("#myModal").find(".modal-footer").empty())
                .click(function (e) {
                    e.preventDefault();
                    var uri = "api/team/" + teamid;
                    $.ajax({
                        url: uri,
                        data: { "": body.find("form").serialize() },
                        type: "PUT",
                        success: function (results) {
                            callback(results);
                        },
                        error: function (xhr, ajaxOption, thrownError) {
                            alert(xhr.status + ":" + thrownError);
                        }
                    });
                });
        });
    },
    modalFunction: function (modalTitle, modalContent, modalButton) {
        var modal = $("#myModal");
        $(".modal-title").html(modalTitle);
        modal.find(".modal-body").html(modalContent);
        $(modalButton).appendTo($("#myModal").find(".modal-footer").empty());
    },
    newTeamListItem: function (data, callback) {
        var li = $("<li/>", { class: "list-group-item list-group-item-info" });
        $("<h4/>", { class: "list-group-item-heading pull-left", html: data.Name }).appendTo(li);
        var idiv = $("<div/>", { class: "pull-right" }).appendTo(li);
        $("<span/>", { class: "label label-success", html: "<label>Members:</label><span class='badge'>" + data.Members.length + "</span>" }).appendTo(idiv);
        $("<button/>", { class: "btn btn-xs btn-warning btn-teamedit", "data-team-id": data.TeamId, "data-toggle": "modal", "data-target": "#myModal" }).append("<span class='glyphicon glyphicon-pencil'></span>")
            .click(function (e) {
                var teamId = $(this).attr("data-team-id");
                var originLi = $(this).closest("li");
                e.preventDefault();
                teams.editTeam("teams/add", teamId, function (data) {
                    originLi.replaceWith(teams.newTeamListItem(data, function (li) {
                        $("#myModal").modal("hide");
                    }));
                });
            })
         .appendTo(idiv);
        $("<button/>", { class: "btn btn-xs btn-warning", "data-team-id": data.TeamId, "data-team-name": data.Name, "data-toggle": "modal", "data-target": "#myModal" }).append("<span class='glyphicon glyphicon-trash'></span>")
            .click(function (e) {
                var btn = $(this);
                var teamId = btn.attr("data-team-id");
                var teamName = btn.attr("data-team-name");
                teams.modalFunction("Delete Team", function () {
                    var s = "<strong>Are you sure you want to remove " + teamName + "</strong>";
                    s += "<p>Enter team name to verify removal</p>";
                    s += "<input type='text' id='verify' class='form-control' placeholder='" + teamName + "' />";
                    return s;
                }, $("<button>", { class: "btn btn-success btn-sm", html: "Delete Team", "data-id":teamId }).click(function (e) {
                    e.preventDefault();
                    if ($("#myModal").find("#verify").val() == teamName) {
                        teams.deleteTeam($(this).attr("data-id"), function () { btn.closest("li").remove();$("#myModal").modal("hide"); });
                    }
                }));
            })
            .appendTo(idiv);

        var displayFields = { "Group Number:": data.GroupNumber, "Leader:": data.TeamLeader.FullName, "Events:": data.Events.length };
        var ulfields = $("<ul/>", { class: "list-group" }).appendTo($("<div/>", { style: "clear:both" }).appendTo(li));
        $.each(displayFields, function (k, v) {
            $("<li/>", { class: "label label-primary" })
                .append($("<label/>", { html: k }))
                .append($("<span/>", { class: "badge", html: v }))
            .appendTo(ulfields);
        });
        //li.appendTo(ul);
        if (callback) {
            callback(li);
        }
        return li;
    }
}


