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