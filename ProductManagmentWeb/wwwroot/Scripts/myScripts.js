
$(document).ready(function () {
    alert('1')
    //Add button click event
    $('#add').click(function () {
        //validation and add order items
        var isAllValid = true;
        if ($('#product').val() == "0") {
            isAllValid = false;
            $('#product').siblings('span.text-danger').css('visibility', 'visible');
        }
        else {
            $('#product').siblings('span.text-danger').css('visibility', 'hidden');
        }

        if ($('#warehouse').val() == "0") {
            isAllValid = false;
            $('#warehouse').siblings('span.text-danger').css('visibility', 'visible');
        }
        else {
            $('#warehouse').siblings('span.text-danger').css('visibility', 'hidden');
        }
        if ($('#unit').val() == "0") {
            isAllValid = false;
            $('#unit').siblings('span.text-danger').css('visibility', 'visible');
        }
        else {
            $('#unit').siblings('span.text-danger').css('visibility', 'hidden');
        }
        if ($('#tax').val() == "0") {
            isAllValid = false;
            $('#tax').siblings('span.text-danger').css('visibility', 'visible');
        }
        else {
            $('#tax').siblings('span.text-danger').css('visibility', 'hidden');
        }

        if (!($('#quantity').val().trim() != '' && (parseInt($('#quantity').val()) || 0))) {
            isAllValid = false;
            $('#quantity').siblings('span.text-danger').css('visibility', 'visible');
        }
        else {
            $('#quantity').siblings('span.text-danger').css('visibility', 'hidden');
        }

        if (!($('#price').val().trim() != '' && !isNaN($('#price').val().trim()))) {
            isAllValid = false;
            $('#price').siblings('span.text-danger').css('visibility', 'visible');
        }
        else {
            $('#price').siblings('span.text-danger').css('visibility', 'hidden');
        }

        if (!($('#subtotal').val().trim() != '' && !isNaN($('#subtotal').val().trim()))) {
            isAllValid = false;
            $('#subtotal').siblings('span.text-danger').css('visibility', 'visible');
        }
        else {
            $('#subtotal').siblings('span.text-danger').css('visibility', 'hidden');
        }

        if (isAllValid) {
            var $newRow = $('#mainrow').clone().removeAttr('id');
            $('.product', $newRow).val($('#product').val());
            $('.warehouse', $newRow).val($('#warehouse').val());
            $('.unit', $newRow).val($('#unit').val());
            $('.tax', $newRow).val($('#tax').val());
           

            alert('$newRow ');
            //Replace add button with remove button
            $('#add', $newRow).addClass('remove').val('Remove').removeClass('btn-success').addClass('btn-danger');

            //remove id attribute from new clone row
            $('#product,#warehouse,#unit,#tax,#quantity,#price,#subtotal,#add', $newRow).removeAttr('id');
            $('span.error', $newRow).remove();

            //append clone row
            $('#quotationxproducts').append($newRow);

            //clear select data
            //$('#product,#warehouse,#unit,#tax').val('0');
            //$('#quantity,#price,#subtotal').val('');
            //$('#quotationxproductsError').empty();
        }

    })

    //remove button click event
    $('#quotationxproducts').on('click', '.remove', function () {
        $(this).parents('tr').remove();
    });

    //$('#submit').click(function () {
    //    var isAllValid = true;

    //    //validate order items
    //    $('#orderItemError').text('');
    //    var list = [];
    //    var errorItemCount = 0;
    //    $('#quotationxproducts tbody tr').each(function (index, ele) {
    //        if (
    //            $('select.product', this).val() == "0" ||
    //            (parseInt($('.quantity', this).val()) || 0) == 0 ||
    //            $('.rate', this).val() == "" ||
    //            isNaN($('.rate', this).val())
    //        ) {
    //            errorItemCount++;
    //            $(this).addClass('error');
    //        } else {
    //            var orderItem = {
    //                ProductID: $('select.product', this).val(),
    //                Quantity: parseInt($('.quantity', this).val()),
    //                Rate: parseFloat($('.rate', this).val())
    //            }
    //            list.push(orderItem);
    //        }
    //    })

    //    if (errorItemCount > 0) {
    //        $('#orderItemError').text(errorItemCount + " invalid entry in order item list.");
    //        isAllValid = false;
    //    }

    //    if (list.length == 0) {
    //        $('#orderItemError').text('At least 1 order item required.');
    //        isAllValid = false;
    //    }

    //    if ($('#orderNo').val().trim() == '') {
    //        $('#orderNo').siblings('span.error').css('visibility', 'visible');
    //        isAllValid = false;
    //    }
    //    else {
    //        $('#orderNo').siblings('span.error').css('visibility', 'hidden');
    //    }

    //    if ($('#orderDate').val().trim() == '') {
    //        $('#orderDate').siblings('span.error').css('visibility', 'visible');
    //        isAllValid = false;
    //    }
    //    else {
    //        $('#orderDate').siblings('span.error').css('visibility', 'hidden');
    //    }

    //    if (isAllValid) {
    //        var data = {
    //            OrderNo: $('#orderNo').val().trim(),
    //            OrderDateString: $('#orderDate').val().trim(),
    //            Description: $('#description').val().trim(),
    //            OrderDetails: list
    //        }

    //        $(this).val('Please wait...');

    //        $.ajax({
    //            type: 'POST',
    //            url: '/home/save',
    //            data: JSON.stringify(data),
    //            contentType: 'application/json',
    //            success: function (data) {
    //                if (data.status) {
    //                    alert('Successfully saved');
    //                    //here we will clear the form
    //                    list = [];
    //                    $('#orderNo,#orderDate,#description').val('');
    //                    $('#orderdetailsItems').empty();
    //                }
    //                else {
    //                    alert('Error');
    //                }
    //                $('#submit').val('Save');
    //            },
    //            error: function (error) {
    //                console.log(error);
    //                $('#submit').val('Save');
    //            }
    //        });
    //    }

    //});

});

