<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="import.aspx.cs" Inherits="TLC.global.import" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Import</h3>
    <div class="row">
        <div class="col-md-10" style="margin-bottom: 10px;">
            <label>Find File:</label>
            <asp:FileUpload ID="fileUpload" runat="server" AllowMultiple="false" style="display:inline-block; border: solid 1px silver; min-width: 300px;" />
            <asp:Button ID="btnSubmit" runat="server" Text="Upload" OnClick="btnSubmit_Click" /> 
                
        </div>
    </div>
    <div class="row">
        <div class="col-md-12" style="min-height: 200px; max-height: 500px; overflow:auto;">
            <div id="divImport"></div>            
        </div>
        <button type="button" id="btnImport" class="btn btn-sm btn-info">Import</button>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnImport").on("click", function (e) {
                e.preventDefault();
                var isMapped = true;
                var data = {};
                $(".mapper").each(function (i, ele) {
                    var field = $(ele).val();
                    if (field == "") {
                        isMapped = false;                
                    }
                    data[field] = null;                    
                });
                if (isMapped) {
                    var rows = [];
                    $("#tblImport > tbody > tr").each(function (r, row) {
                        var rowdata = {};
                        $(this).find("td").each(function (c, col) {
                            rowdata[Object.keys(data)[c]] = $(col).html();
                        });
                        rows.push(rowdata);
                    });
                    app.ImportData(rows, function () {
                        app.SuccessAlert("Import Data","Import Success");
                    });
                } else {
                    app.ErrorAlert("Import Data", "Import failed. Not all field are mapped.");
                }
            });
        });

        
        function ShowData(data) {
            if (!data) {
                return "no data";
            }
            var datarows = JSON.parse(data.data);
            var tbl = $("<table>").attr("id","tblImport").addClass("table table-striped table-hover");
            var thead = $("<thead>").appendTo(tbl);
            var tr = $("<tr>").appendTo(thead);
            $.each(data.dataCols, function (i, col) {
                var th = $("<th>").html(col).appendTo(tr);
                FieldMapperControl(col).appendTo(th);
            });           

            var tbody = $("<tbody>").appendTo(tbl);           
            $.each(datarows, function (r, row) {
                tr = $("<tr>").appendTo(tbody);
                $.each(data.dataCols, function (c, col) {
                    $("<td>").html(row[col]).appendTo(tr);
                });
            });
            tbl.appendTo($("#divImport"));
            $(".mapper").each(function (i, ele) {
                $(ele).trigger("change");
            });
        }
        function FieldMapperControl(DefaultValue) {
            if (!importdata){
                return "<span>No Data</span>";
            }
            var sel = $("<select>").addClass("mapper form-control").width(125);
            $("<option>", { html: '', value: '' }).appendTo(sel);
            $.each(importdata.dbCols, function (i, ele) {
                $("<option>", { html: ele, value:ele, selected: ele == DefaultValue? true : false }).appendTo(sel);
            });

            sel.on("focus", function (e) {
                var val = $(this).val();
                $(".mapper").find("option[value='" + val + "']").removeAttr("disabled");
            }).on("change", function (e) {
                var val = $(this).val();    
                $(".mapper").find("option[value='" + val + "']").attr("disabled", "disabled");
                $(this).find("option[value='" + val + "']").removeAttr("disabled");
            });

            return sel;
        }
    </script>
</asp:Content>
