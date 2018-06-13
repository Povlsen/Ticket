function login() {
    console.log('called...')
    $.ajax({
        url: 'https://ticket-api.nas-tech.dk/API/User/Login',
        type: 'POST',
        data: "{'UserName': 'Mikkel', 'Password': '123456'}",
        dataType: 'json',
        contentType: 'application/json',
        success: (res) => {
            console.log(res)
        }
      });

      return false
}