// Write your JavaScript code.
$(function () {
    $("form").submit(function () {
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            }
        });
    });
    //$('li a[href="/Inzeraty"]', 'li a[href="/Marked"]', 'li a[href="/Ignored"]').on('click', function () {
    $('a[href="/Inzeraty"], li a[href="/Inzeraty/Marked"], li a[href="/Inzeraty/Ignored"], #prev-button, #next-button').click(function () { 
        console.log("click on navbar");
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            }
        });
    });
});
