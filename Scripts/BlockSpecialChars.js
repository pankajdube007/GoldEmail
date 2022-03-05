var specialKeys = new Array();
//specialKeys.push(8); //Backspace
//specialKeys.push(9); //Tab
//specialKeys.push(46); //Delete
//specialKeys.push(36); //Home
//specialKeys.push(35); //End
//specialKeys.push(37); //Left
//specialKeys.push(39); //Right

function handlePaste(e) {
    var clipboardData, pastedData;
    var oldVal = '';
    clipboardData = e.clipboardData || window.clipboardData;
    pastedData = clipboardData.getData('Text');
    //debugger;
    if ($(e.target).is("textarea")) {
        //debugger;
        var ctrl = $(e.target);
        oldVal = $(ctrl).val() == undefined ? '' : $(ctrl).val().toString();
        $(ctrl).val(oldVal + '' + pastedData.replace(/'/g, '').replace(/&/g, ''));
    }
    if ($(e.target).attr('type') == 'text') {
        //debugger;
        var ctrl = $(e.target);
        oldVal = $(ctrl).val() == undefined ? '' : $(ctrl).val().toString();
        $(ctrl).val(oldVal + '' + pastedData.replace(/([~!#$%^&*()_+=`{}\[\]\|\\:;'<>,?])+/g, '').replace(/^(-)+|(-)+$/g, ''));
        //$(e.target).attr('value', oldVal + '' + pastedData.replace(/([~!#$%^&*()_+=`{}\[\]\|\\:;'<>,\/?])+/g, '').replace(/^(-)+|(-)+$/g, ''));
    }

    e.stopPropagation();
    e.preventDefault();
}

$(document).ready(function () {
    document.getElementById('aspnetForm').addEventListener('paste', handlePaste);
});



//$(document).on('paste', function (event) {
//    if ($(event.target).attr('type') == 'text') {
//        debugger;
//        //event.preventDefault();
//        //showDialog('Warning', 'Paste is not allowed here.', 'warning', 2);
//    }
//});


$(document).keypress(function (e) {

    if ($(e.target).is("textarea")) {
        var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;

        if ((keyCode == 38) || (keyCode == 39)) {
            e.preventDefault();
            showDialog('Warning', 'Special characters except single quote and & are blocked due to security reason.', 'warning', 2);
        }
        else {

        }
    }

    if ($(e.target).attr('type') == 'text') {
        var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
        //debugger;
        if ((keyCode == 46) || (keyCode == 32) || (keyCode == 47) || (keyCode == 92) || (keyCode == 45) || (keyCode == 13) || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 64 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode)) {

            //if ($(e.target).val().match(/./igm).length > 1) {
            //    e.preventDefault();
            //}
        }
        else {
            e.preventDefault();
            showDialog('Warning', 'Few special characters blocked due to security reason.', 'warning', 2);
        }
    }

});