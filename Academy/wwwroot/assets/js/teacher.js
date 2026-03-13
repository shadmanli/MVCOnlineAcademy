"use strict"

$(document).ready(function(){
  $('.slider').slick({
    slidesToShow: 2,
    slidesToScroll: 1,
    dots: true,
    arrows: false,
    infinite: true,
    responsive: [
      {
        breakpoint: 768,
        settings: {
          slidesToShow: 1
        }
      }
    ]
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