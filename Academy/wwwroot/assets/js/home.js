"use strict"

// console.log("Salam JavaScript");
// console.log("hello  dostlar");
// console.log("Menim adim Intizardir");


// let soyad = "Shadmanli";
// const dogumTarixi = 2004 ;
// // console.log(ad,soyad,dogumTarixi);


// let name = "Intizar";
// let age = 12;
// let telebe = true;
// console.log(typeof name);
// console.log(typeof age);
// console.log(typeof telebe);
// let ad = "Intizar";
// console.log(ad.toUpperCase());

// let change = "Javascript";
// change = change.length;
// console.log(change);

// let word = "Frontend Developer";
// word = word.slice(0,8);
// console.log(word);


// let a = 12;
// let b= 20;
// console.log(a+b);


//  let  user = {
//     ads:"Intizar",
//     yass:12,
//     soyad:"Shadmanli",
//     pese:"Developer"

//  };
//  console.log(user.ads);
//  console.log(user.yass);
//  console.log(user.pese);


//  let meyveler = ["alma","heyva","nar","armud","gilas"];
//  meyveler.push("kivi");
//  console.log(meyveler[1]);
//  console.log(meyveler.length);


//  //adini cap eden funksiya yaz

//  function names(){
//     console.log("Intizar");
//  }
//  names();

//  let z =5;
//  let g=10;
//  function sum(){
//     return z+g;
//  }
// console.log(sum());


// let d= 12;
// function changes(){
//     return a*a;
// }
// console.log(changes());


// function namim(){
// console.log("Intizar");
// }
// namim();

// function max(a,b,c){
//     return Math.max(a,b,c);
// }
// console.log(max(12,13,18));

//  let i= 0;
//  while(i<5){
//     console.log("Say",i);   
//     i++;
//  }




//  //1-dən 10-a qədər ədədləri for ilə yazdır.



//   for(let i=1;i<=10;i++){
//       console.log(i);
//   }



//   //10-dan 1-ə qədər while ilə yazdır.
//  let n = 10;
//  while(n>=1){
//     console.log(n);
//     n--;
//  }





// // let reng = [];

// // for (let i = 0; i < 5; i++) {
// //     let color = prompt("Bir rəng daxil et");
// //     reng.push(color); 
// // }

// // console.log("Sənin daxil etdiyin rənglər:");
// // for (let color of reng) {
// //     console.log(color);
// // }




// let users = {
// ad:"Intizar",
// soyad:"shadmanli",
// yas:22
// };

//  for(let item  in users){
//     console.log(item + "  " +users[item]);
//  }




//  let day = 5;
//  switch(day){
//    case 1:
//       console.log("birinci gundur");
//       break;
//       case 2:
//          console.log("ikinci gundur");
//          break;
//          case 5:
//             console.log("besinci gundur");
//             break;
//  }


//  let y = 5;
//  console.log(y);


//  let w = 10;
//  let h= 30;
//  console.log(w+h);


//  ///verilen eded cutdur ya tekdir


//  let  c =10;
//  if(n%2==0){
//    console.log("eded cut")
//  }
//  else{
//    console.log("eded tekdir");
//  }



//  ///  1den n e qeder ededleri  cap ele


//  let l  =20;
//  for(let i=1;i<l;i++ ){
//    console.log(i);
//  }

//   ///  1den n e qeder cut  ededleri  cap ele


//   let r = 29;
//   for(let i=1;i<29;i++)
//    if(i%2==0){
//       console.log(i);
//    }
   

//      ///  1den n e qeder  cut ededlerin  cemin   cap ele


//    //   let j =32;
//    //   let sum = 0;
//    //   for( let i=1;i<j;i++)
//    //    if(i%2==0){
//    //       sum = sum+i;
//      ///  1den n e qeder  cut ededlerin  cemin   cap ele

// let j = 32;
// let sum = 0;

// for (let i = 1; i <= j; i++) {  // 32 daxil olsun deyə <=
//     if (i % 2 === 0) {           // === tövsiyə olunur
//         sum += i;                // sum = sum + i
//     }
// }

// console.log(sum);  // loop bitdikdən sonra



//1-dən N-ə qədər cəm

// let n = 10;
// let sum = 0;
// for(let i = 1;i<n;i++){
//    sum=sum+i;
// }
// console.log(sum);




//Faktoriyal


// let n =5;
// let factorial =1;
// for(let i =1;i<=n;i++){
//    factorial= factorial*i;
// }
//    console.log(factorial);



//edədin reqemlerinin cemini tap

// let n =312;
// let sum =0;
// while(n>0){
//    sum = sum+n%10;
//    n=Math.floor(n/10);
// }
// console.log(sum);



//23) Rəqəmlərin sayı

// let n= 123;
// let count=0;
// while(n>0){
//    count++
//    n=Math.floor(n/10);
// }
// console.log(count);


//n ve m ededleri var. n ve m ededleri arasindaki tek ededleri ekranda gosterin.

// let n= 13;
// let m = 22;
// for(let i=n;i<m;i++){
//    if(i%2==1){
//       console.log(i);
//    }
// }



//arrayin icindeki elementlerin cemin tap;

// let numbers = [2,3,4,5,6];
// let sum=0;
// for(let i =0;i<numbers.length;i++){
//    sum= sum+numbers[i];
    
//    }

// console.log(sum);



//arrayin icindeki cut  ededlerin sayin tapmaq


// let count=0;
// let numbers=[2,3,45,23,12];
// for(let i=0;i<numbers.length;i++){
//    count++
// }
// console.log(count);



 //ededin faktorialini hesablamaq


//  let n=22;
//  let fac=1;
//  for(let i=1;i<=n;i++){
//    fac= fac*i
//  }
//  console.log(fac);


//ededin sade ve ya murekkeb oldugunu tapin

// let n =22;
// let count=0;
// for(let i =1;i<n;i++){
//    if(n%i==0){
//       count++
//    }
// }
// if(count>2){
//    console.log("eded murekkebh")
// }


//// 6) arrayin icinden 6-e modu 4-den boyuk olanlarin sayini tapin.

//  let count=0;
//  let numbers=[12,3,24,25,6];
//  for(let i=0;i<numbers.length;i++){
// if(numbers[i]%6>4){
//    count++
// }
//  }
//  console.log(count);
 
// function dogumIliTap(yas) {
//     let indikiIl = new Date().getFullYear();
//     return indikiIl - yas;
// }

// console.log(dogumIliTap(30));
///parametr olaraq gelen stringi tersine ceviren function yazin.
// Parametr olaraq gelen yasha gore hansi ilden olduqugunu gosteren function yazin. Meselen 30 gelirse function geriye 1993 qaytarmalidir.

// function reverseString(text){
//     let result = "";
//    for(let i =text.length-1;i>=0;i--){
//       result = result+text[i];
//    }
//    return result;
// }
// console.log(reverseString("salam"));




// function birthYear(yas){
//    let atMoment = new Date().getFullYear();
//    return atMoment-yas;
// }
// console.log(birthYear("23"));
// function searchByFullName(searchText){
//    let result = [];

//    for(let i = 0; i < persons.length; i++){
//       if(persons[i].fullName.toLowerCase().includes(searchText.toLowerCase())){
//          result.push(persons[i]);
//       }
//    }

//    return result;
// }

// console.log(searchByFullName("intizar"));


const  persons=[
   {
      Id:1,
      Name:"Intizar",
      fullName:"Intizar Shadmanli",
      age:21,
      salary:2000

   },
      {
      Id:1,
      Name:"Aidan",
      fullName:"Bayramova Aidan",
      age:22,
      salary:2000

   },
      {
      Id:1,
      Name:"Zahra",
      fullName:"Zahra Cabbarli",
      age:21,
      salary:2000

   },
]
///    1) Personlarin umumi sayini geri qaytaran.
let count=0;
function sumPeople(){
   for(let i =0;i<persons.length;i++){
      count++
   }
   return count;
   
}
console.log(sumPeople());


///Personlarin maashlarinin umumi ortalamasini qaytaran.

let sum = 0;
function sumSalary(){
   for(let i =0;i<persons.length;i++){
      sum = sum+persons[i].salary;
   }
   return sum/persons.length;
}
console.log(sumSalary());




//Personlarin fullName-ne gore search function-u yazin. (elave parametr olaraq searchText de qebul edecek)


// function searhcName(searchText){
//     let result=[];
// for(let i =0;i<persons.length;i++){
//   if(persons[i].fullName.toLowerCase().includes(searchText.toLowerCase())){
//     result.push(persons[i])
//   }

// }
//   return result;
// }
// console.log(searhcName("Intizar"));

const container = document.getElementById("logosContainer");

// İçindəkiləri klonlayırıq
container.innerHTML += container.innerHTML;



const cardsContainer = document.querySelector(".cards");
const prevBtn = document.querySelector(".prev-btn");
const nextBtn = document.querySelector(".next-btn");

let currentTranslate = 0;

function getMaxTranslate() {
  const carouselWidth = document.querySelector(".carousel").offsetWidth;
  const cardsWidth = cardsContainer.scrollWidth;
  return cardsWidth - carouselWidth;
}

function updateButtons() {
  const maxTranslate = getMaxTranslate();

  prevBtn.disabled = currentTranslate === 0;
  nextBtn.disabled = currentTranslate >= maxTranslate;
}

nextBtn.addEventListener("click", () => {
  const cardWidth = document.querySelector(".card").offsetWidth + 30;
  const maxTranslate = getMaxTranslate();

  currentTranslate += cardWidth;

  if (currentTranslate > maxTranslate) {
    currentTranslate = maxTranslate;
  }

  cardsContainer.style.transform = `translateX(-${currentTranslate}px)`;
  updateButtons();
});

prevBtn.addEventListener("click", () => {
  const cardWidth = document.querySelector(".card").offsetWidth + 30;

  currentTranslate -= cardWidth;

  if (currentTranslate < 0) {
    currentTranslate = 0;
  }

  cardsContainer.style.transform = `translateX(-${currentTranslate}px)`;
  updateButtons();
});

window.addEventListener("resize", () => {
  currentTranslate = 0;
  cardsContainer.style.transform = `translateX(0px)`;
  updateButtons();
});

updateButtons();



document.addEventListener('DOMContentLoaded', function () {
  // Swiper başlatma
  const swiper = new Swiper('.swiper-container', {
    slidesPerView: 4,           
    spaceBetween: 20,            
    loop: false,                  
    pagination: {
      el: '.swiper-pagination', 
      clickable: true,            
    },
    slideToClickedSlide: true,    
  });
});



///1den 20e qeder ededler
for(let i =1; i<=20;i++){
  console.log(i);
}



///1den 20e qeder cut ededler

for(let i= 1;i<=20;i++){
  if(i%2==0){
    console.log(i);
  }
}
///1den 50e qeder ededlerin cemin


let cem =0;
for(let i= 1;i<=30;i++){
  sum =sum +i;
}
console.log(sum);



//5 factorialin hesablanmasi


let fac=1;
for(let i =1;i<=12;i++){
  fac = fac*i;
}
console.log(fac);




let n =12;
let counts =0;
for(let i =0;i<n;i++){
  if(n%i==0){
    count++
  }
}
if(count>2){
  console.log("eded murekkebdir");
}



function sayHello(){
  console.log("salam");
}
sayHello();


function  greet(name){
  console.log("salam" + " "+ name);
}
 greet("Zahra");


 function sums(a,b){
  return a+b;
  
 }
console.log(sums(2,4));


function isEven(n){
  if(n%2==0){
    console.log("cutdur");
  }
}
isEven(10);


function max(a,b){
  if(a>b){
    console.log(a);
  }
  else{
    console.log(b);
  }

}
max(2,3);



function repeatString(str,n){
  let res = "";
  for(let i =0;i<n;i++){
    res = res + str;
  }
  console.log(res);
}
repeatString("salam",5);



  function reverseName(text){
    let res = "";
    for(let i = text.length-1;i>=0;i--){
      res = res+ text[i]
    }
  console.log(res);
  }
  reverseName("salam");



let user = {
  name:"John",
  surname:"Smith"
};
user.name = "Lala";
delete user.name;
console.log(user);



///eger cedvelde element varsa false qaytarsin eger yoxdusa true;

let schedule ={
subject:"math",
sport:"sports",
music:"differences"
};
function isEmpty(obj){
  for(let element in obj){
    return false;
  }
   return true;
}
console.log(isEmpty(schedule));


  let salaries  = {
    John:100,
    Ann:160,
    Pete:103
  }

  let cems = 0;
  for(let key in salaries){
    cems = cems + salaries[key]
  }
  console.log(cems);

let menu = {
  width:300, 
  height:500,
  title:"My menu"
}

 function numberMultiple(obj){
  for(let key in obj){
    if( typeof(obj[key]) == 'number'){
      obj[key ]= obj[key]*2;
    }
  }
  
 }
numberMultiple(menu);
  console.log(menu);




  let arr = ["I " ,"am","learning","Javascript"];
  delete arr[1];


  let arrs = [1,2,3,4,5]
  let newArr = arrs.slice(1,3)

let array = [15, 2, 1];

// // döngü ilə ters çeviririk
// for (let i = 0; i < array.length / 2; i++) {
//   let temp = array[i];             // mövcud elementi saxlayırıq
//   array[i] = array[array.length - 1 - i]; // qarşı tərəfdən gələn elementi qoyuruq
//   array[array.length - 1 - i] = temp;    // saxlanmış elementi qarşı tərəfə qoyuruq
// }

console.log(array); // [1, 2, 15]

  // let numbers = [1,2,3,4 ];
  // for(let i= 0;i<numbers.length;i++){
  //   numbers[i] = numbers[i]*numbers[i];
  // }
  // console.log(numbers);

let numbers = [1,2,3,4];
let squared = numbers .map(x=>x*x);
console.log(squared);

 let arrss =[1,2,3,44,5];
 arrss.push(8);
 console.log(arrss);

for(let i =0 ; i<arrss.length;i++){
  console.log(arrss[i]);
}


// document.addEventListener('DOMContentLoaded',function(){
//   let text = document.querySelectorAll('#main-start p,#main-start h2');
//   text.forEach(function(el){
//     el.style.color='red';
//   });
// });

// document.addEventListener('DOMContentLoaded',function(){
//   let back = document.getElementById('feedback-part');
//   back.style.background='red';

  
// });


// document.addEventListener('DOMContentLoaded',function(){
//   let text = document.querySelectorAll('#many-card .card');
//   text.forEach(function(el){
//     el.style.background='red';
//   });
// });



const hamburger = document.querySelector(".hamburger");
const menuss = document.querySelector(".menu-common");

hamburger.addEventListener("click", () => {
  menuss.classList.toggle("active");
});




///  Ədəd cüt yoxsa tək?

let number = 12;
if(number%2==0){
  console.log("eded cutdur");
}
else{
  console.log("eded tekdir");
}


///1-dən N-ə qədər cəmi tap.

 function sumON(n){
  let cemi =0;
  for(let i = 1;i<=n;i++){
    cemi = cemi + i;
  }
  return cemi;
 }
 console.log(sumON(20));


///arrayde olan elementlerin icinde en boyuyun;
let arrayde = [2,3,4,6,7,89,10];
let maxing = arrayde[0];
for(let i =1;i<arrayde.length;i++){
    if(arrayde[i]>maxing){
      maxing=arrayde[i];
    }
}
console.log(maxing);



//arrayde en kicik elementi tap

let arraying = [2,3,45,67,8];
let min = arraying[0];
for(let i = 1; i<arraying.length;i++){
  if(arraying[i]<min){
    min = arraying[i];
  }
}
console.log(min);


//String-i tərsinə çevir.

 function reverseText(text){
  let reserve = "";
  for(let i = text.length-1;i>=0;i--){
    reserve=reserve+text[i];
  }
  return reserve;
 }
 console.log(reverseText("salam"));

 ///stringin uzunlugun length istifade etmeden tap:


function strLength(text){
  let count =0;
  for(let char of text){
    count++
  }
  return count;
}
console.log(strLength("salam"));



////Array-də neçə cüt ədəd var?

let arrays = [2,3,4,6,710,13,14,16];
let say =0;
for(let i =0;i<arrays.length;i++){
  if(arrays[i]%2==0){
    say++
  }
}
console.log(say);


////arrayde menfi ededleri tap
function negativeNumber(arr){
  let yaz = [];
  for(let i =0;i<arr.length;i++){
    if(arr[i]<0){
      yaz = yaz + arr[i];;
    }
  }
  return yaz;
}
console.log(negativeNumber([2,3,-2,-5,6]));



////Verilmiş ədədin factorialı (loop ilə).
 let numss= 12;
 let facc =1;
 for(let i =1;i<=numss;i++){
    facc = facc*i;
 }
 console.log(facc);




 ///verilmis ededin quvvetin tap

 function powNumber(base,exp){
  let reseult =1;
  for(let i =1;i<=exp;i++ ){
 reseult = reseult *base; 
  }
   return reseult; 
  
 }
 console.log(powNumber(3,5));

///arrayin ortalamasini tap


function calculateArr(arraydi){
  let cemi=0;

  for(let i=0;i<arraydi.length;i++){
    cemi = cemi + arraydi[i];
    
  }
  let avg = cemi/arraydi.length;
return avg;

}
console.log(calculateArr([2,3,5,6,7,8]));


// ///String-də neçə sait hərf var?

// function saitFunction(texts){
//   let stringg = "";
//   for(let i =0;i<texts.length;i++){
//     if()
//   }
// }

////1-dən 100-ə qədər 3 və 5-ə bölünən ədədləri çap et.


for(let i =1;i<=100;i++){
  if(i%3==0 && i%5==0){
    console.log(i);
  }
}


/// n ve m ededleri verilib n ve m ededleri arasindaki tek ededleri gosterin

let y =12;
let g = 25;
for(let i =12; i<=25;i++){
  if(i%2==1){
    console.log(i);
  }
}


  const slides = document.querySelectorAll(".slide, .slide-active");
  if (slides.length > 0) {
    let currentIndex = 0;
    slides.forEach(function (s, i) {
      if (s.classList.contains("slide-active")) currentIndex = i;
    });

    function changeSlide() {
      slides[currentIndex].classList.remove("slide-active");
      slides[currentIndex].classList.add("slide");
      currentIndex = (currentIndex + 1) % slides.length;
      slides[currentIndex].classList.remove("slide");
      slides[currentIndex].classList.add("slide-active");
    }

    setInterval(changeSlide, 5000);
  }


let arayy = [20,3,4,5,6];
let cemim =0;
for(let i =0;i<arayy.length;i++){
  cemim = cemim +arayy[i];
}
console.log(cemim);


// arrayin icerisindeki cut ededlerin sayini tapmaq

let arrayim = [2,3,4,5,6];
let sayidi=0;
for(let i =0;i<arrayim.length;i++){
  if(arrayim[i]%2==0){
    sayidi++
  }
}
console.log(sayidi);


////ededin faktorialini hesablamaq

let reqemdi =12;
let factoriali =1;
for(let i =1;i<reqemdi;i++){
  factoriali =factoriali*i;
}
console.log(factoriali);


/// ededin sade ve ya murekkeb oldugunu tapin

let eded =45;
let sayimiz =0;
for(let i =1;i<eded;i++){
  if(eded%i==0){
    sayimiz++
  }
}
if(sayimiz==2){
  console.log("eded sadedir")
}

else{
  console.log("eded murekkebdir")
}


////arrayin icinden 6-e modu 4-den boyuk olanlarin sayini tapin.

let marray= [12,24,34,2,15,37,41];
let sayidir =0;
for(let i =0;i<marray.length;i++){
  if(marray[i]%6>4){
    sayidir++
  }
}
console.log(sayidir);



//student objectleriniz olacaq. Studentlerin name, surname,email,age keyleri olacaq.

// Ashagidaki tasklari ona uygun yazin:
// 7) Studentlerin yashlarinin cemini tapin.
// 8) Studentlerin yashlarinin ortalamasini tapin.
// 9) Studentlerin sayi ile yashlarinin ceminin hasilini tapin.


let telebeler =[
  {
    Name:"Intizar",
    Surname:"shadmanliii",
    Email:"shadmanli@gmail.com",
    Age:32
  },
  {
        Name:"Zahra",
    Surname:"cabbarli",
    Email:"cabbarli@gmail.com",
    Age:42
  }
]

let cemii=0;
for(let i=0;i<telebeler.length;i++){
  cemii= cemii + telebeler[i].Age
}
console.log(cemii);

//// Parametr olaraq gelen stringi tersine ceviren function yazin. Meselen: Cavid gelirse divaC qaytarsin.


function turnName(ad){
  let world ="";
  for(let i=ad.length-1;i>=0;i--){
    world = world + ad[i];
  }
  return world;
}
console.log(turnName("Intizar"));




//2) Parametr olaraq gelen yasha gore hansi ilden olduqugunu gosteren function yazin. Meselen 30 gelirse function geriye 1993 qaytarmalidir.

function yearBirthday(yas){
let existYear = new Date().getFullYear();
return existYear-yas;
}
console.log(yearBirthday(40));

// 3) Personlardan ibaret objectleriniz olacaq. (id,name,fullName,age,salary) . Ashagidaki tasklari array qebul eden functionlar seklinde yazin.
//      1) Personlarin umumi sayini geri qaytaran.
//      2) Personlarin maashlarinin umumi ortalamasini qaytaran.
//      3) Personlarin fullName-ne gore search function-u yazin. (elave parametr olaraq searchText de qebul edecek)




let usaxlar= [
  {
    Id:12,
    Name:"Inti",
    fullName:"Shadmanli",
    age:32,
    salary:1500

  },
    {
    Id:13,
    Name:"Intizzz",
    fullName:"Shadmanliyyy",
    age:35,
    salary:1700

  },

      {
    Id:14,
    Name:"Salam",
    fullName:"Shadmanzade",
    age:39,
    salary:1900

  },
]

function commonPeople(arr){
  return arr.length;
}

console.log(commonPeople(usaxlar));



function avgSummary(arr){
  let cemimm= 0;
  for(let i =0;i<arr.length;i++){
    cemimm = cemimm+ arr[i].salary;
  }
  return cemimm/arr.length;
}
console.log(avgSummary(usaxlar));



// function searchFull(arr,searchText){
//   let result =[];
//   for(let i =0;i<arr.length;i++){
//     if(arr[i].fullName.toLowerCase().includes(searchText.toLowerCase())){
//       result.push(arr[i]);
//     }
//   }
//   return result;
// }
// console.log(searchFull(usaxlar,"shadm"));



Number.prototype.squared = function(a){
  return this*a

}
  let num =12;
console.log(num.squared(15));


String.prototype.getLength =function(){
  return this.length
}
let namess= "Intis";
console.log(namess.getLength());


// let elem = document.getElementsByTagName("h1")


// let button = document.querySelector(".btn button");
// button.addEventListener("click",function(){
//   alert("salam intis")
// });
// console.log(button);
// let input = document.querySelector(".box input");
// input.addEventListener("keyup", function () {
//   console.log("keyup");
// });



// let input = document.querySelector(".box input")
// let validationMsj =document.querySelector(".box span");
// input.addEventListener("keyup",function(){
//    let inputValue=input.value;
 
//    if(inputValue==""){
//     validationMsj.classList.remove("d-none");
//    }
//    else{
//     validationMsj.classList.add("d-none");
//    }
// });



// let closeIcon = document.querySelector(".sidebar .icon .close");
// let openIcon = document.querySelector(".sidebar .icon .open");
// let sidebar = document.querySelector(".sidebar");

// openIcon.addEventListener("click",function(){
// sidebar.classList.remove("move-sidebar");
// this.classList.romove("d-none");
// this.previousElementSibling.classList.add("d-none")
// })


// let button = document.querySelector(".buttons.btn-primary");
// let a =document.querySelector(".for-js a")
// button.addEventListener("click",function(){
//   let hasAtr = a.hasAttribute("href");
//  if(hasAtr){
//   a.setAttribute("href","https://www.linkedin.com/feed/")
//   a.innerHTML="linkedin";
//  }
// })


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
const searchToggle = document.getElementById('search-toggle');
const searchPanel = document.getElementById('search-panel');
const closeSearch = document.getElementById('close-search');

searchToggle.addEventListener('click', function(e) {
    e.preventDefault();
    searchPanel.style.display =
        searchPanel.style.display === 'flex' ? 'none' : 'flex';
});

closeSearch.addEventListener('click', function() {
    searchPanel.style.display = 'none';
});