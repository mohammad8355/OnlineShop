// imageCount
timeLeft = 5000;
var images = document.querySelectorAll(".carousel-slide");
var wrapperElement = document.querySelector(".carousel-wrapper");
var carouselcontainer = document.querySelector(".carousel-container");
var leftNav = document.querySelector(".left-nav");
var direction = 1;
var rightNav = document.querySelector(".right-nav");
var translate = 100;
var imageCounter = 0;
var slider = function(timeLeft){
    wrapperElement.style.transform = 'translateX(' + translate + '%)';
    translate = 100;
}
wrapperElement.addEventListener("transitionend",function (){
    if(direction == 1){
        wrapperElement.appendChild(wrapperElement.firstElementChild);
    }
    else{
        wrapperElement.prepend(wrapperElement.lastElementChild);
        direction = 1;
        console.log("prepend");
    }
    wrapperElement.style.transition = 'none';
    wrapperElement.style.transform = 'translateX(0)';
    setTimeout(function(){
        wrapperElement.style.transition = 'all 2s';
    })
    translate = 100;
})
setInterval(slider,timeLeft);
leftNav.addEventListener("click",function(){
     carouselcontainer.style.justifyContent = 'flex-end';
      translate = 100;
     direction = 1;
     slider();
});
rightNav.addEventListener("click",function(){
     carouselcontainer.style.justifyContent = 'flex-start';
     translate = -100;
     direction = -1;
    //  wrapperElement.prepend(wrapperElement.lastElementChild);
     slider();
});



//scroll navigation 
var rightNavScroll = document.querySelectorAll(".right-nav-scroll");
var leftNavScroll = document.querySelectorAll(".left-nav-scroll");
var ScrollAmount = 200;

$(rightNavScroll).on("click", function () {
    var container = this.parentNode.previousElementSibling;
    console.log(container.scrollLeft)
    var currentScrollLeft = $(container).scrollLeft();
    $(container).animate({scrollLeft: currentScrollLeft + ScrollAmount},500);
      
});

$(leftNavScroll).on("click", function () {
    var container = this.parentNode.previousElementSibling;
    var currentScrollLeft = $(container).scrollLeft();
    $(container).animate({scrollLeft: currentScrollLeft - ScrollAmount},500);
     
});



//end scroll navigation 
function createCircularCardSlider(containerSelector, cardSelector, prevBtnSelector, nextBtnSelector, autoplayInterval) {
    const cardSliderContainer = document.querySelector(containerSelector);
    const cardSlider = cardSliderContainer.querySelector('.card-slider');
    const prevBtn = cardSliderContainer.querySelector(prevBtnSelector);
    const nextBtn = cardSliderContainer.querySelector(nextBtnSelector);
    
    let currentIndex = 0;
    
    function moveSlider(direction) {
      const cardWidth = cardSliderContainer.querySelector(cardSelector).offsetWidth;
      const maxIndex = cardSlider.children.length - 1;
      
      if (direction === 'next') {
        currentIndex = currentIndex === maxIndex ? 0 : currentIndex + 1;
      } else {
        currentIndex = currentIndex === 0 ? maxIndex : currentIndex - 1;
      }
      
      const newPosition = -currentIndex * cardWidth;
      cardSlider.style.transform = `translateX(${newPosition}px)`;
    }
    
    prevBtn.addEventListener('click', () => moveSlider('prev'));
    nextBtn.addEventListener('click', () => moveSlider('next'));
    
    if (autoplayInterval) {
      setInterval(() => moveSlider('next'), autoplayInterval);
    }
  }
