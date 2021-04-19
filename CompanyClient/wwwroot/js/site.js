//Load a company when the all companies table is clicked
$("#allCompaniesTable").on('click', 'tr', function () {
    var isin = $(this).children('td').eq(1).text();

    $.ajax({
        url: './Index?handler=Company',
        method: 'GET',
        data: { isin: isin },
        success: function (result) {
            var jsondata = $.parseJSON(result);
            $("#loadedName").val(jsondata.Name);
            $("#loadedISIN").val(jsondata.ISIN)
            $("#loadedWebsite").val(jsondata.Website)
            $("#loadedTickers").val(jsondata.Ticker)
            $("#loadedExchanges").val(jsondata.CompanyExchange)
            $("#loadedId").val(jsondata.Id)
        }
    });
});