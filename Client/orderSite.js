
let loginRes = JSON.parse(new URLSearchParams(window.location.search).get('data'))
$.ajaxSetup({ headers: { 'Authorization': 'Basic ' + loginRes.Token }});
let server = 'https://ticket-api.nas-tech.dk/API/'

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

   