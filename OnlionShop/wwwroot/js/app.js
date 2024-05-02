document.addEventListener("DOMContentLoaded", function() {
  const carouselWrapper = document.querySelector('.carousel-wrapper');
  const slides = document.querySelectorAll('.carousel-slide');
  const totalSlides = slides.length;
  const prevBtn = document.querySelector('.left-nav');
  const nextBtn = document.querySelector('.right-nav');
  const slideWidth = slides[0].clientWidth;
  let currentIndex = 1;
  let autoplayInterval;

  // Clone first and last slides for infinite loop
  const firstClone = slides[0].cloneNode(true);
  const lastClone = slides[totalSlides - 1].cloneNode(true);

  carouselWrapper.appendChild(firstClone);
  carouselWrapper.insertBefore(lastClone, slides[0]);

  carouselWrapper.style.transform = `translateX(-${slideWidth}px)`;

  function slideNext() {
      currentIndex++;

      carouselWrapper.style.transition = 'transform 0.5s ease-in-out';
      carouselWrapper.style.transform = `translateX(-${slideWidth * currentIndex}px)`;

      if (currentIndex >= totalSlides + 1) {
          setTimeout(() => {
              carouselWrapper.style.transition = 'none';
              carouselWrapper.style.transform = `translateX(-${slideWidth}px)`;
              currentIndex = 1;
          }, 500);
      }
  }

  function slidePrev() {
      currentIndex--;

      carouselWrapper.style.transition = 'transform 0.5s ease-in-out';
      carouselWrapper.style.transform = `translateX(-${slideWidth * currentIndex}px)`;

      if (currentIndex <= 0) {
          setTimeout(() => {
              carouselWrapper.style.transition = 'none';
              carouselWrapper.style.transform = `translateX(-${slideWidth * (totalSlides + 1)}px)`; // Move to the position after the last clone
              currentIndex = totalSlides;
          }, 500);
      }
  }

  // Previous button click event
  prevBtn.addEventListener('click', function() {
      clearInterval(autoplayInterval);
      slidePrev();
      setTimeout(() => {
          autoplayInterval = setInterval(slideNext, 3000);
      }, 500);
  });

  // Next button click event
  nextBtn.addEventListener('click', function() {
      clearInterval(autoplayInterval);
      slideNext();
      setTimeout(() => {
          autoplayInterval = setInterval(slideNext, 3000);
      }, 500);
  });

  // Autoplay
  autoplayInterval = setInterval(slideNext, 3000);

  // Pause autoplay on hover
  carouselWrapper.addEventListener('mouseenter', () => {
      clearInterval(autoplayInterval);
  });

  // Resume autoplay on mouse leave
  carouselWrapper.addEventListener('mouseleave', () => {
      autoplayInterval = setInterval(slideNext, 3000);
  });
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
