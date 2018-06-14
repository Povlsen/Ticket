$(document).ready(() => {
    $("#notUserBtn").click(function(){
        $("#login").slideUp("slow");
        $("#signUp").slideDown("slow");
    });

    $("#backBtn").click(showSignin());
});

const showSignin = () => {
    $(document).ready(() => {
        $("#signUp").slideUp("slow");
        $("#login").slideDown("slow");
    });
}