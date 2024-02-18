
  function autoCloseAlert(parentIdentifier,timeout,fadeTime,message,header,type){

      $(parentIdentifier).prepend("<div class='alert auto-close alert-" + type + " position-absolute top-50 left-50  alert-dismissible fade show' role='alert'><strong>" + header + "</strong><hr><p class='row'>" + message + "</p<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>");
   window.setTimeout(function(){
    $(".auto-close").fadeOut(fadeTime,function()
    {
      $(".auto-close").remove();
    });
 },timeout);
}