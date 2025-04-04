// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    $(document).ready(function () {
        const searchBar = $('#InputVal');
    const table = $('table');

        searchBar.on('keyup', function (event) {
            console.log("new new hello");
            var searchValue = searchBar.val();

    $.ajax({
        url: '/Employee/Search',
    type: 'Get',
        data: { SearchInput: searchValue },
    success: function (result) {
        table.html(result);
                },
    error: function (xhr, status, error) {
        console.log(error);
                }
            });
        });
    });
