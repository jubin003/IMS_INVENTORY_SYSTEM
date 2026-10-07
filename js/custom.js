$(document).ready(function () {
    $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl03_ddlActivity").attr('disabled', 'disabled');
    $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl04_ddlActivity").attr('disabled', 'disabled');
    var value1 = $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl02_ddlActivity option:selected").val();
    var value2 = $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl03_ddlActivity option:selected").val();
    if (value1 != '----Select----' && value2 == '----Select----') {
        $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl03_ddlActivity").prop("disabled", false);
            $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl03_ddlActivity option").each(function () {
                var $thisOption = $(this);
                var valueToCompare = $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl02_ddlActivity option:selected").val();
           
                if (parseInt($thisOption.val()) == parseInt(valueToCompare)) {
                    $thisOption.attr("disabled", "disabled");
                }
        });
    } else{
        if (value2 != '----Select----') {
            $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl02_ddlActivity").attr('disabled', 'disabled');
            $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl03_ddlActivity").attr('disabled', 'disabled');
            $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl04_ddlActivity").prop("disabled", false);
            $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl04_ddlActivity option").each(function () {
                var $thisOption = $(this);
                var valueOfFrist = $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl02_ddlActivity option:selected").val();
                var valueOfSecond = $("#ctl00_ContentPlaceHolder1_gridActivityList_ctl03_ddlActivity option:selected").val();
                console.log(valueOfFrist);
                console.log(valueOfSecond);
                //alert(valueToCompare + "-" + $thisOption.val())
                if (parseInt($thisOption.val()) == parseInt(valueOfFrist) || parseInt($thisOption.val()) == parseInt(valueOfSecond)) {
                    $thisOption.attr("disabled", "disabled");
                }
            });
        }
    }


            $('.datepicker').nepaliDatePicker({
                dateFormat: 'dd/mm/yyyy',
            });

            $('.datepicker1').nepaliDatePicker({
                dateFormat: 'dd/mm/yyyy',
            });

            $('.bdatepicker').datepicker({
                minViewMode: 2,
                format: 'yyyy'
            });

            $('.expiyDate').datepicker({
                dateFormat: "dd/mm/yy"
            });
            $('[data-toggle="tooltip"]').tooltip();
            $('.dropdown').click(function () {


                if ($(this).find('.dropdown-menu').find('.col-md-4').length == 1) {
                    $(this).find('.dropdown-menu').addClass('width-200');
                    if ($(this).find('.dropdown-menu').find('.col-md-4')) {
                        $(this).find('.dropdown-menu').find('.col-md-4').removeClass('col-md-4').addClass('col-md-12');
                    }
                }

                else if ($(this).find('.dropdown-menu').find('.col-md-4').length == 2) {
                    $(this).find('.dropdown-menu').addClass('width-400');
                    if ($(this).find('.dropdown-menu').find('.col-md-4')) {
                        $(this).find('.dropdown-menu').find('.col-md-4').removeClass('col-md-4').addClass('col-md-6');
                    }
                }

                else if ($(this).find('.dropdown-menu').find('.col-md-4').length == 3) {
                    $(this).find('.dropdown-menu').addClass('width-500');

                }

                else if ($(this).find('.dropdown-menu').find('.col-md-4').length > 3) {
                    $(this).find('.dropdown-menu').addClass('width-700');
                    if ($(this).find('.dropdown-menu').find('.col-md-4')) {
                        $(this).find('.dropdown-menu').find('.col-md-4').removeClass('col-md-4').addClass('col-md-3');
                    }

                }






            });



            $(function () {

                $(document).on('scroll', function () {

                    if ($(window).scrollTop() > 100) {
                        $('.scroll-top-wrapper').addClass('show');
                    } else {
                        $('.scroll-top-wrapper').removeClass('show');
                    }
                });

                $('.scroll-top-wrapper').on('click', scrollToTop);
            });

            function scrollToTop() {
                verticalOffset = typeof (verticalOffset) != 'undefined' ? verticalOffset : 0;
                element = $('body');
                offset = element.offset();
                offsetTop = offset.top;
                $('html, body').animate({ scrollTop: offsetTop }, 500, 'linear');
            }


            //$(".dropdown").click(function () {
            //    $(this).find(".breadcrumb").addClass("remove-zindex");
            //});
        });