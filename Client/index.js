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
$(window).load(function() {      //Do the code in the {}s when the window has loaded 
  $("#loader").fadeOut("fast");  //Fade out the #loader div
});
}