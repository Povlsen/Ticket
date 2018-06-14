var server = 'https://ticket-api.nas-tech.dk/API/'

function login() {
    let info = { 
        email: $('#login-email')[0].value, 
        password: $('#login-password')[0].value 
    }
    $.post(server + 'User/Login', info, (data) => {
        document.location.href = 'orderSite.html?data=' + encodeURIComponent(JSON.stringify(data));
    });
    return false
}

function signup() {
    var myList = document.getElementById('errorList');
    myList.innerHTML ='';
    let info = {
        name: $('#signUp-firstName')[0].value + ' ' + $('#signUp-lastName')[0].value,
        phone: $('#signUp-phoneNumber')[0].value,
        email: $('#signUp-email')[0].value, 
        password: $('#signUp-password1')[0].value,
        password2: $('#signUp-password2')[0].value
    }

    $.post(server + 'User/Post', info, (data) => {
        if (data.length == 0) {
            // login
            showSignin();
            $('#signupSuccess').empty();
            $('#signupSuccess').append('User Created! :D');
        } else {

            //display errors
            console.log(data);
            var errorList = $('#errorList');
            $.each(data, function (index, val){
                errorList.append('<li>' + val + '</li>');
            });
        }
    });
    return false
}