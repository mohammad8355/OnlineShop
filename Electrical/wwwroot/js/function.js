
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
function ZoomBoxImageView(Wrapper, prevBtn, nextBtn, slide) {
    var wrapper = document.querySelector(Wrapper);
    var wrapperchildrencount = wrapper.childElementCount;
    var valueTranslateX = 0;
    var direction = 0;
    var counter = 1;
    var zoom = 0.1;
    $(wrapper).css("width", (wrapperchildrencount * 100) + "%")
    $('.zoom-in').click(function () {
        Zoom('.current', zoom);
        zoom += 0.1;
    })
    $('.zoom-out').click(function () {
        zoom -= 0.1;
        Zoom('.current', zoom);
    })
    function Zoom(identifier, zoomValue) {
        var image = document.querySelector(identifier);
        $(image).css("transform", "scale(" + (1 + zoomValue) + ")");
    }
    function zoomReSet(identifier) {
        var image = document.querySelector(identifier);
        $(image).css("transform", "scale(1)");
        console.log('reset');
    }
    function updateCurrentSlide() {
        $(Wrapper + ' ' + slide).removeClass("current");
        $(Wrapper).children().eq(counter - 1).addClass("current");
    }
    $(prevBtn).click(function () {
        if (direction != -1) {
            direction = -1;
        }
        if (counter > 1) {
            zoomReSet('.current')
            valueTranslateX -= 100;
            $(wrapper).css("transform", "translateX(-" + valueTranslateX + "%)");
            counter -= 1;
            updateCurrentSlide();
        }
    })
    $(nextBtn).click(function () {
        if (direction != 1) {
            direction = 1;
        }
        if (counter < wrapperchildrencount) {
            zoomReSet('.current');
            valueTranslateX += 100;
            $(wrapper).css("transform", "translateX(-" + valueTranslateX + "%)");
            counter += 1;
            updateCurrentSlide();
        }
    })
}