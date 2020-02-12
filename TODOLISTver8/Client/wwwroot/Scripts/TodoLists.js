var table;
$(document).ready(function () {
    var Status = $('#selectedChange').val();
    $('#TodolistData').on('keyup', function () {
        table
            .columns(3)
            .search(this.value)
            .draw();
    });
    //method change
    //$('#selectedChange').change(function () {
    //    //window.alert($('#selectedChange').val());
    table = $("#TodolistData").DataTable({
        
        serverSide: true,
        ajax:
        //"scripts/server_processing.php",
            "/User/PageData/" + Status
        ,
        "columns": [
            {
                "render": function (data, type, row) {
                    if (row.status == false) {
                        return '<button type="button" class="btn btn-secondary" id="Edit" onclick="return UpdateStatus(' + row.id + ')"><i class="fa fa-square-o" title="Checklist"></i></button>';
                    } else {
                        return '<button type="button" class="btn btn-secondary" id="Edit" onClick="return UncheckStatus(' + row.id + ')"><i class="fa fa-check-square-o" title="Unchecklist"></i></button>';
                    }
                    //return '<button type="button" class="btn btn-secondary" id="Edit" onclick="return UpdateStatus(' + row.Id + ')"><i class="fa fa-square-o" title="Checklist"></i></button>';
                }
            },

            { "data": "name" },
            {
                "render": function (data, type, row) {
                    if (row.status == 0) {
                        return 'Active'
                    }
                    else {
                        return 'Complete';
                    }
                }
            },
            //{ "data": "status" },
            {
                "render": function (data, type, row) {
                    if (row.status == false) {
                        return '<button type="button" class="btn btn-warning" id="Edit" onclick="return GetById(' + row.id + ')"><i class="material-icons" data-toggle="tooltip" title="Edit">&#xE254;</i></button> ' +
                            '<button type = "button" class="btn btn-danger" id="Delete" onclick="return Delete(' + row.id + ')" ><i class="material-icons" data-toggle="tooltip" title="Delete">&#xE872;</i></button >';
                    }
                    else {
                        return 'No Action';
                    }
                }
            }
        ]
    });
    //});

    //$('#TodolistData').dataTable({
    //    "aoColumns": [{ "bSortable": false }, null, null, null, { "bSortable": false }],
    //    "ajax": loadData(),
    //    "responsive": true,
    //    "serverSide": true
    //});
});

//function loadData() {
//    var Status = $('#selectedChange').val();
//    debugger;
//    $.ajax({
//        url: "/User/List",
//        type: "GET",
//        data: {status: Status},
//        contentType: "application/json;charset=utf-8",
//        dataType: "JSON",
//        async: false,
//        success: function (result) {
//            var html = '';
//            var no = 1;
//            $.each(result, function (key, item) {
//                html += '<tr>';
//                html += '<td><button type="button" class="btn btn-secondary" id="Edit" onclick="return UpdateStatus(' + item.id + ')"><i class="fa fa-square-o" title="Checklist"></i></button></td > ';
//                html += '<td>' + no++ + '</td>';
//                html += '<td>' + item.name + '</td>';
//                if (item.status == 0) {
//                    html += '<td> Active </td>';
//                } if (item.status == 1) {
//                    html += '<td> Complete</td>';
//                }
//                html += '<td><button type="button" class="btn btn-warning" id="Edit" onclick="return GetById(' + item.id + ')">Edit</button> ' +
//                    '<button type = "button" class="btn btn-danger" id="Delete" onclick="return Delete(' + item.id + ')" > Delete</button ></td > ';
//                html += '</tr>';
//            });
//            $('.TodolistDataTable').html(html);
//        },
//        error: function (errormessage) {
//            console.error(errormessage);
//        }
//    });
//}
$('#selectedChange').change(function () {
    //alert(selectedValue);
    table.ajax.url("/User/PageData/" + $('#selectedChange').val()).load();
});
function getJSessionId() {
    var jsId = document.cookie.match(/JSESSIONID=[^;]+/);
    if (jsId != null) {
        if (jsId instanceof Array)
            jsId = jsId[0].substring(11);
        else
            jsId = jsId.substring(11);
    }
    return jsId;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var todolist = new Object();
    todolist.Id = $('#Id').val();
    todolist.Name = $('#Name').val();
    $.ajax({
        url: "/User/Insert/",
        data: todolist,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            clearTextBox();
            $('#myModal').modal('hide');
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Insert Successfully',
                showConfirmButton: false,
                timer: 1500
            });
            table.ajax.reload();
        }
        else {
            Swal.fire('Error', 'Insert Failed', 'error');
        }
    });
}

function GetById(Id) {
    $.ajax({
        url: "/User/GetById/",
        type: "GET",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        data: { id: Id },
        success: function (result) {
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#Name').val(obj.Name);

            $('#myModal').modal('show');
            $('#Update').show();
            $('#Add').hide();

        }
    })
}

//function for updating supplier's record
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var todolist = new Object();
    todolist.Id = $('#Id').val();
    todolist.Name = $('#Name').val();
    $.ajax({
        url: "/User/Update/",
        data: todolist,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            debugger;
            $('#myModal').modal('hide');
            clearTextBox();
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 6000
            });
            table.ajax.reload();
        }
        else {
            Swal.fire('Error!', 'Update Failed.', 'error');
        }
    })
}

function UpdateStatus(Id) {
    $.ajax({
        url: "/User/UpdateCheckedTodoList/",
        data: { id: Id },
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 6000
            });
            table.ajax.reload();
        }
        else {
            Swal.fire('Error!', 'Update Failed.', 'error');
        }
    });
}

function UncheckStatus(Id) {
    debugger;
    $.ajax({
        url: "/User/updateUnCheckedTodoList/",
        data: { id: Id },
        type: "POST"
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 6000
            });
            table.ajax.reload();
        }
        else {
            Swal.fire('Error!', 'Update Failed.', 'error');
        }
    });
}

function Delete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/User/Delete/",
                type: "DELETE",
                data: { id: id },
                success: function (result) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Delete Successfully'
                    });
                    table.ajax.reload();
                },
                error: function (result) {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            });
        }
    })
}
//Function for clearing the textboxes
function clearTextBox() {
    $('#Id').val("");
    $('#Name').val("");
    $('#Email').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#Name').css('border-color', 'lightgrey');
}
//Valdidation using jquery
function validate() {
    var isValid = true;
    if ($('#Name').val().trim() == "") {
        $('#Name').css('border-color', 'Red');
        $('#Name').focus();
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }

    return isValid;
}
$(function () {
    $("[id*=btnSweetAlert]").on("click", function () {
        var id = $(this).closest('tr').find('[id*=id]').val();
        swal({
            title: 'Are you sure?' + ids,
            text: "You won't be able to revert this!" + id,
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
            confirmButtonClass: 'btn btn-success',
            cancelButtonClass: 'btn btn-danger',
            buttonsStyling: false
        }).then(function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/DeleteClick",
                    data: "{id:" + id + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        if (r.d == "Deleted") {
                            location.reload();
                        }
                        else {
                            swal("Data Not Deleted", r.d, "success");
                        }
                    }
                });
            }
        },
            function (dismiss) {
                if (dismiss == 'cancel') {
                    swal('Cancelled', 'No record Deleted', 'error');
                }
            });
        return false;
    });
});