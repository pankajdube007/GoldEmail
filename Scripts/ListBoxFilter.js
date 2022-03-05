$(document).ready(function () {

    JSFilter();
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(JSFilter);
});



function JSFilter() {

    if ($('table').find('JS-Filter') !== undefined) {

        var c = $("table[JS-Filter]");
        $(c).each(function () {
            if ($(this).attr('JS-Filter') == 'True') {
                var id = $(this).attr('id');
                var parCtrl = $(this).parent();
                var dialog;
                var wd = $(this).css('width');
                dialog = document.createElement('input');
                dialog.type = 'text';
                dialog.className = 'form-control JS-Search-txt';
                dialog.style = 'width:' + wd + '; margin: 0; padding: 0;';

                if ($(parCtrl).prev().hasClass('JS-Search-txt') == false) {
                    $(dialog).insertBefore($(parCtrl));
                }
            }

        });
        //sikandar
        
    }

    $('.JS-Search-txt').keyup(function (event) {

        if ($(this).val().trim() != '') {
            var sRt = $(this).val().trim();

            $(this).next().find('.js-filter-class').find('table:visible').find('tr').each(function (i) {

                if ($(this).find('td:nth-child(2)').text().toLowerCase().indexOf(sRt.toLowerCase()) > -1) {

                    $(this).css('display', 'block');
                }
                else {
                    $(this).css('display', 'none');
                }
            });
        }
        else {
            $(this).next().find('.js-filter-class').find('table:visible').find('tr').css('display', 'block');
        }
        $('.js-filter-class').addClass('cls-Search');
    });
}