function login() {
    console.log('here...')
    $.ajax({
        url: 'https://ticket-api.nas-tech.dk/API/User/Login',
        type: 'POST',
        data: JSON.stringify({UserName: 'Mikkel', Password: '123456'}),
        dataType: 'json',
        contentType: 'application/json',
        success: (res) => {
            console.log(res)
        }
      });

      return false
}