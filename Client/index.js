$(document).ready(() => {
    $("#notUserBtn").click(function(){
        $("#login").slideUp("slow");
        $("#signUp").slideDown("slow");
    });
});

const showSignin = () => {
    $(document).ready(() => {
        $("#signUp").slideUp("slow");
        $("#login").slideDown("slow");
    });
}
	function Load(url) {
    // display loading image here...
    document.getElementById('loading').visible = true;
    // request your data...
    var req = new XMLHttpRequest();
    req.open("POST", url, true);

    req.onreadystatechange = function () {
        if (req.readyState == 4 && req.status == 200) {
            // content is loaded...hide the gif and display the content...
            if (req.responseText) {
                document.getElementById('content').innerHTML = req.responseText;
                document.getElementById('loading').visible = false;
            }
        }
    }
    request.send(vars);
}