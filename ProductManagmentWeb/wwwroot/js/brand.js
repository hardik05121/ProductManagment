var dataTable;

$(document).ready(function () {
    loadDataTable();
});

$('#tblData').DataTable({
    initComplete: function () {
        this.api()
            .columns()
            .every(function () {
                var column = this;
                var title = column.footer().textContent;

                // Create input element and add event listener
                $('<input type="text" placeholder="Search ' + title + '" />')
                    .appendTo($(column.footer()).empty())
                    .on('keyup change clear', function () {
                        if (column.search() !== this.value) {
                            column.search(this.value).draw();
                        }
                    });
            });
    },
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/brand/getall' },
        "columns": [
            { data: 'brandName', "width": "25%" },
            {
                "data": "brandImage",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <img src="${data}"  height="40px" width="40px">
                    </div>`
                },
                "width": "25%"
            },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/brand/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=Delete('/admin/brand/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "50%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}






/*Try Method :-*/

//var dataTable;

//$(document).ready(function () {
//    loadDataTable();
//});

//function loadDataTable() {
//    dataTable = $('#tblData').DataTable({
//        paging: false,
//        searching: false
//        initComplete: function () {
//            this.api()
//                .columns()
//                .every(function () {
//                    var column = this;
//                    var title = column.footer().textContent;

//                    // Create input element and add event listener
//                    $('<input type="text" placeholder="Search ' + title + '" />')
//                        .appendTo($(column.footer()).empty())
//                        .on('keyup change clear', function () {
//                            if (column.search() !== this.value) {
//                                column.search(this.value).draw();
//                            }
//                        });
//                });
//        },
//        "ajax": { url: '/admin/brand/getall' },
//        "columns": [
//            { data: 'brandName', "width": "25%" },
//            {
//                "data": "brandImage",
//                "render": function (data) {
//                    return `<div class="w-75 btn-group" role="group">
//                    <img src="${data}"  height="40px" width="40px">
//                    </div>`;
//                },
//                "width": "25%"
//            },
//            {
//                data: 'id',
//                "render": function (data) {
//                    return `<div class="w-75 btn-group" role="group">
//                     <a href="/admin/brand/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
//                     <a onClick=Delete('/admin/brand/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
//                    </div>`;
//                },
//                "width": "50%"
//            }
//        ]
//    });
//}

//function Delete(url) {
//    Swal.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        confirmButtonText: 'Yes, delete it!'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: url,
//                type: 'DELETE',
//                success: function (data) {
//                    dataTable.ajax.reload();
//                    toastr.success(data.message);
//                }
//            });
//        }
//    });
//}





