let server = 'https://ticket-api.nas-tech.dk/API/'
let loginRes = undefined
var orders = undefined

const onload = () => {
    var uri = window.location.toString();
    if (uri.indexOf("?") > 0) {
        loginRes = JSON.parse(new URLSearchParams(window.location.search).get('data'))
        $.ajaxSetup({ headers: { 'Authorization': 'Basic ' + loginRes.Token }});

        var clean_uri = uri.substring(0, uri.indexOf("?"));
        window.history.replaceState({}, document.title, clean_uri);
    }
    
    getOrders()
}
function logOut(){
    loginRes = undefined;
    document.location.href = 'index.html';
}

$(document).ajaxError((event, jqxhr, settings, exception) => {
    if (jqxhr.status === 401) {
        logOut();
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

function getOrders() {
    //$.get(server + 'Order/Get/Client/' + loginRes.UserId, (data) => {
    $.get(server + 'Order/Get/Shop/1', (data) => {
        orders = data // save orders
        if (data.length > 0) {
            $.each(data, (key, val) => {
                var div = document.createElement('DIV')
                div.id = val.Id

                var childDiv = document.createElement('DIV')
                div.innerHTML = '<div>' + val.Id + '</div>'
                div.innerHTML += '<div>' + val.CreatedByName + '</div>'
                div.innerHTML += '<div>' + val.Model + '</div>'
                div.innerHTML += '<div>' + val.RegNum + '</div>'
                div.innerHTML += '<div>' + val.StateName + '</div>'
                div.innerHTML += '<div>' + val.Created + '</div>'
                div.innerHTML += '<button class="modalBtn" onClick=showDescription(' + val.Id + ')>Show more</button>'

                $('#orderList').append(div)
            })
        }
    })
}

const showDescription = (orderId) => {
    document.getElementById('modal-description').innerText = orders.filter(x => x.Id == orderId)[0].Description
    $(".modals").slideDown()
} 

$(document).ready(() => {
    $(".modalBtn").click(() => {
        $(".modals").slideDown()
    })
    $(".close").click(() => {
        $(".modals").slideUp()
    })
})
