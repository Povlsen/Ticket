let server = 'https://ticket-api.nas-tech.dk/API/'
let loginRes = undefined
const onload = () => {
    var uri = window.location.toString();
    if (uri.indexOf("?") > 0) {
        loginRes = JSON.parse(new URLSearchParams(window.location.search).get('data'))
        $.ajaxSetup({ headers: { 'Authorization': 'Basic ' + loginRes.Token }});

        var clean_uri = uri.substring(0, uri.indexOf("?"));
        window.history.replaceState({}, document.title, clean_uri);
    }
}

$(document).ajaxError((event, jqxhr, settings, exception) => {
    if (jqxhr.status === 401) {
        document.location.href = 'index.html'
    }
});

function orderCreate() {
    // set the header to contain the token to all future requests
    let info = {
        Model: $('#orderForm-model')[0].value,
        RegNum: $('#orderForm-regNum')[0].value,
        Description: $('#orderForm-description')[0].value, 
    }

    $.post(server + 'Order/Post', info, (data) => {
        console.log(data)
    });
      return false
}

   