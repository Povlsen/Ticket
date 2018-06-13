function login() {
    let info = { 
        email: $('#login-email')[0].value, 
        password: $('#login-password')[0].value 
    }

    $.post('http://localhost:64881/API/User/Login', info, (data) => {
        // set the header to contain the token to all future requests
        $.ajaxSetup({
            headers: {
                'Authorization': 'Basic ' + data.Token
            }
        });

        document.location.href = 'orderSite.html';
    });
      return false
}

function signup() {
    let info = {
        name: $('#signUp-firstName')[0].value + ' ' + $('#signUp-lastName')[0].value,
        phone: $('#signUp-phoneNumber')[0].value,
        email: $('#signUp-email')[0].value, 
        password: $('#signUp-password1')[0].value,
        password2: $('#signUp-password2')[0].value
    }

    $.post('http://localhost:64881/API/User/Post', info, (data) => {
        if (data.length == 0) {
            // login
            showSignin();
        } else {
            //display errors
            console.log(data)
        }
    });
      return false
}