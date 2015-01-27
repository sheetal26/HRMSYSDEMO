function loadCountries(ddlId, ddlStateId, ddlCityId) {
    $.ajax({
        url: 'Demo.asmx/loadCountries',
        async: true,
        success: function (msg) {
            var countries = msg.d;

            $(ddlStateId).empty();
            $(ddlStateId).append($("<option></option>").val("").html("--Select--"));

            $(ddlCityId).empty();
            $(ddlCityId).append($("<option></option>").val("").html("--Select--"));

            $(ddlId).empty();
            $.each(countries, function (key, value) {
                $(ddlId).append($("<option></option>").val
                (value.strCountryId).html(value.stCountry));
            });
        },
        datatype: 'json',
        contentType: 'application/json; charset=utf-8',
        cache: false,
        type: 'POST',
        failure: function (response) {
            alert(response);
        }
    });
}

function loadDemo() {
    $.ajax({
        url: 'Demo.asmx/TestDemo',
        async: true,
        success: function (msg) {
            alert("hi");
            alert(msg.d);
        },
        datatype: 'json',
        contentType: 'application/json; charset=utf-8',
        cache: false,
        type: 'POST',
        failure: function (response) {
            alert(response.responseText);
        }
    });
}


function loadStates(ddlId, ddlCityId, IntCountryId) {
    $.ajax({
        url: 'Demo.asmx/loadStates',
        async: true,
        data: '{IntCountryId:' + IntCountryId + '}',
        success: function (retstate) {
            var states = retstate.d;

            $(ddlCityId).empty();
            $(ddlCityId).append($("<option></option>").val("").html("--Select--"));

            $(ddlId).empty();
            $.each(states, function (key, value) {
                $(ddlId).append($("<option></option>").val
                    (value.strStateId).html(value.strState));
            });
        },
        datatype: 'json',
        contentType: 'application/json; charset=utf-8',
        cache: false,
        type: 'POST',
        faliure: function (response) {
            alert(response);
        }
    });
}

function loadCities(ddlId, IntCountryId, IntStateId) {
    $.ajax({
        url: 'Demo.asmx/loadCities',
        async: true,
        data: '{IntCountryId:' + IntCountryId + ', IntStateId:' + IntStateId + '}',
        success: function (retcity) {
            var citys = retcity.d;
            $(ddlId).empty();
            $.each(citys, function (key, value) {
                $(ddlId).append($("<option></option>").val(value.strCityId).html(value.strCity));
            });
        },
        datatype: 'json',
        contentType: 'application/json; charset=utf-8',
        cache: false,
        type: 'POST',
        faliuer: function (response) {
            alert(response);
        }
    });
}