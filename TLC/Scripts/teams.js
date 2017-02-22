var app = {};
var teams = {
    myteam: null,
    list: [],
    getTeams: function (callback) {
        teams.myteam = isNaN(parseInt(teams.myteam)) ? -1 : parseInt(teams.myteam);
        var url = "api/team" + (teams.myteam < 0 ? "" : "/" + teams.myteam);
        var jqXHR = $.ajax({
            url: url,
            type: "GET",
            dataType: "json",
            success: function (response) {
                teams.list = response;
                if (callback) {
                    callback(response);
                }                
            }
        });
    },
    init: function (callback) {
        if (teams.list.length == 0) {
            teams.getTeams(callback);
        } else {
            if (callback) {
                callback();
            }
        }
        
    }
};

app.LoadTeamData = function (teamdata,callback) {
    var pnl = $("#pnlTeamInfo");
    if (!pnl){
        return;
    }
    pnl.find(".panel-heading").find(".teamName").html(teamdata.TeamName);
    pnl.find(".panel-heading").find(".badge").find("span:first-child").text(teamdata.Members.length);
    pnl.find(".panel-body").find(".teamNumber").html(teamdata.TeamNumber);
    pnl.find(".panel-body").find(".teamLeader").html(function () {
        var rtn = teamdata.TeamLeader.FullName + teamdata.TeamLeader.Email;
        return $("<p>").append($("<a>", { href: "members/index.aspx?Id=" + teamdata.TeamLeader.MemberId, html: rtn })).html();
    });
    if (callback) {
        callback();
    }
    //app.LoadMembers(teamdata.Members);
};
app.LoadMembers = function (membersdata,callback) {
    var pnl = $("#pnlTeamMembers");
    if (!pnl) {
        return;
    }
    var tbody = pnl.find("table > tbody");
    if (!membersdata || membersdata.length == 0) {
        tbody.empty();
        $("<tr>", { html: "<td colspan='5'>No Data</td>" }).appendTo(tbody);
    }
    var rows = [];
    $.each(membersdata, function (indx, ele) {
        var row = $("<tr>");
        $("<td>", { html: ele.FullName }).appendTo(row);
        $("<td>", { html: ele.Email }).appendTo(row);
        $("<td>", { html: ele.Phone }).appendTo(row);
        $("<td>", { html: ele.Address }).appendTo(row);
        var actionCol = $("<td>", { html: "" }).appendTo(row);
        $("<button>", { class: "btn btn-xs btn-info", title: "Record Note" }).append($("<span>", { class: "glyphicon glyphicon-comment" })).appendTo(actionCol);
        $("<button>", { class: "btn btn-xs btn-danger", title: "Remove Member" }).append($("<span>", { class: "glyphicon glyphicon-remove" })).appendTo(actionCol);
        rows.push(row);
    });
    tbody.empty();
    tbody.append(rows);
    var runTime = new Date().toJSON();
    pnl.find(".panel-heading").find("label").remove();
    pnl.find(".panel-heading").append($("<label>", { html: "Loaded: " + runTime + " ", id: "lbltime", style: "float:right;" }));
    if (callback) {
        callback();
    }
};
app.LoadEvent = function (eventsdata,callback) {
    var pnl = $("#pnlTeamEvents");
    if (!pnl) {
        return;
    }
    var tbody = pnl.find("table > tbody");
    if (!eventsdata || eventsdata.length == 0) {
        tbody.empty();
        $("<tr>", { html: "<td colspan='5'>No Data</td>" }).appendTo(tbody);
    }
    var rows = [];
    $.each(eventsdata, function (indx, ele) {
        var row = $("<tr>");
        $("<td>", { html: ele.Title }).appendTo(row);
        $("<td>", { html: ele.EventDate }).appendTo(row);
        $("<td>", { html: ele.Description }).appendTo(row);
        $("<td>", { html: ele.Status }).appendTo(row);
        var actionCol = $("<td>", { html: "" }).appendTo(row);
        $("<button>", { class: "btn btn-xs btn-danger", title: "Remove Event" }).append($("<span>", { class: "glyphicon glyphicon-remove" })).appendTo(actionCol);
        rows.push(row);
    });
    tbody.empty();
    tbody.append(rows);
    var runTime = new Date().toJSON();
    pnl.find(".panel-heading").find("label").remove();
    pnl.find(".panel-heading").append($("<label>", { html: "Loaded: " + runTime + " ", id: "lbltime", style: "float:right;" }));
    if (callback) {
        callback();
    }
}


