// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $('#InputVal').on('keyup', function () {
        const searchBar = $(this);
        const searchValue = searchBar.val();
        const searchUrl = searchBar.data('search-url');
        console.log("Search triggered for:", searchUrl);

        $.ajax({
            url: searchUrl,
            type: 'GET',
            data: { SearchInput: searchValue },
            success: function (result) {
                //$(tableSelector).html($(result).find(tableSelector).html());
                //$('table tbody').html($(result).find('tbody').html());
                $('table tbody').html(result);
            },
            error: function (xhr, status, error) {
                console.log("Error:", error);
            }
        });
    });
});
