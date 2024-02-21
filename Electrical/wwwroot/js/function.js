
  function autoCloseAlert(parentIdentifier,timeout,fadeTime,message,header,type){

      $(parentIdentifier).prepend("<div style='width:15em;position:absolute;z-index:1000;' class='alert auto-close alert-" + type + " alert-dismissible fade show' role='alert'><strong>" + header + "</strong><hr><p class='row'>" + message + "</p<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>");
   window.setTimeout(function(){
    $(".auto-close").fadeOut(fadeTime,function()
    {
      $(".auto-close").remove();
    });
 },timeout);
}
function intCounter(Identifier, Count, isMinus = false) {
    $(document).ready(function () {
        var element = document.querySelector(Identifier);
        var stringvalue = element.innerHTML;
        var intValue = parseInt(stringvalue);
        if (isMinus) {
            intValue -= Count;
        }
        else {
            intValue += Count;
        }
        element.innerHTML = intValue.toString();

    });
}
function reverseContent(Identifier, NewContent) {
    var element = document.querySelector(Identifier);
    element.innerHTML = NewContent.toString();
}