$(document).ready(function(){
    $("#notUserBtn").click(function(){
        
        $("#login").slideUp("slow");
        $("#signUp").slideDown("slow");

    });
    $("#backBtn").click(function(){
        
        $("#signUp").slideUp("slow");
        $("#login").slideDown("slow");
    });
});