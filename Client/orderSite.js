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
        displayOrder(data)
        getOrders()
    });
      return false
}

function getOrders() {
    if (!loginRes) {
        document.location.href = 'index.html'
    }

    var url = ''
    if (loginRes.UserType == 0) url = 'Client/' + loginRes.UserId
    if (loginRes.UserType == 1) url = 'Shop/' + loginRes.ShopId

    $.get(server + 'Order/Get/' + url, (data) => {
        orders = data // save orders
        
        var temp = document.getElementById('msgForNoOrders')
        document.getElementById('orderList').innerHTML = ''
        document.getElementById('orderList').appendChild(temp)

        if (data.length > 0) {
            document.getElementById('msgForNoOrders').className = 'display-none'

            $.each(data, (key, val) => {
                displayOrder(val)
            })
        }
    })
}

function displayOrder(order) {
    var div = document.createElement('DIV')
    div.id = order.Id

    var childDiv = document.createElement('DIV')
    div.innerHTML = '<div>' + order.Id + '</div>'
    div.innerHTML += '<div>' + order.CreatedByName + '</div>'
    div.innerHTML += '<div>' + order.Model + '</div>'
    div.innerHTML += '<div>' + order.RegNum + '</div>'
    div.innerHTML += '<div>' + order.StateName + '</div>'
    div.innerHTML += '<div>' + order.Created + '</div>'
    div.innerHTML += '<button class="modalBtn" onClick=showDescription(' + order.Id + ')>Show more</button>'

    $('#orderList').append(div)
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
