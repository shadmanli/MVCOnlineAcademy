"use strict"

window.addEventListener("load", function() {
  const bars = document.querySelectorAll(".progress-bar");

  bars.forEach(bar => {
    let percent = bar.getAttribute("data-percent");
    setTimeout(() => {
      bar.style.width = percent + "%";
    }, 300);
  });
});



document.addEventListener('DOMContentLoaded', function () {
  const swiper = new Swiper('.swiper-container', {
    slidesPerView: 2,           
    spaceBetween: 1,           
    loop: false,              
    pagination: {
      el: '.swiper-pagination',   
      clickable: true,            
    },
    slideToClickedSlide: true,    
  });
});

const hamburger = document.querySelector(".hamburger");
const menuss = document.querySelector(".menu-common");

hamburger.addEventListener("click", () => {
  menuss.classList.toggle("active");
});




const loginModal = document.getElementById("loginModal");
const registerModal = document.getElementById("registerModal");
const loginBtn = document.getElementById("loginBtn");
const registerBtn = document.getElementById("registerBtn");

const closeBtns = document.querySelectorAll(".closeBtn");


loginBtn.addEventListener("click", () => {
  registerModal.classList.remove("active");
  loginModal.classList.add("active");
});


loginBtn.addEventListener("click", () => {
  loginModal.classList.remove("active");
  registerModal.classList.add("active");
});


closeBtns.forEach(btn => {
  btn.addEventListener("click", () => {
    loginModal.classList.remove("active");
    registerModal.classList.remove("active");
  });
});


window.addEventListener("click", (e)=>{
  if(e.target === loginModal){
    loginModal.classList.remove("active");
  }
  if(e.target === registerModal){
    registerModal.classList.remove("active");
  }
});


function openLogin(){
  registerModal.classList.remove("active");
  loginModal.classList.add("active");
}

function openRegister(){
  loginModal.classList.remove("active");
  registerModal.classList.add("active");
}