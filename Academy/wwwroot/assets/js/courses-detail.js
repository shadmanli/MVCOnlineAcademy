"use strict"

const tabs = document.querySelectorAll(".tab-btn");
const contents = document.querySelectorAll(".tab-content");

tabs.forEach(tab => {

tab.addEventListener("click", () => {

tabs.forEach(btn => btn.classList.remove("active"));
contents.forEach(content => content.classList.remove("active"));

tab.classList.add("active");

document
.getElementById(tab.dataset.tab)
.classList.add("active");

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


const curriculum = [
    {
        title: "Bölmə 1: Başlanğıc", icon: "🚀", lessons: [
            { name: "JavaScript nədir?", dur: "8:24", demo: true },
            { name: "Mühit qurulması (VS Code)", dur: "12:05", free: true },
            { name: "Dəyişənlər: var, let, const", dur: "18:32" },
            { name: "Məlumat növləri", dur: "14:15" },
        ]
    },
    {
        title: "Bölmə 2: Funksiyalar", icon: "⚙️", lessons: [
            { name: "Funksiya nədir?", dur: "10:40" },
            { name: "Arrow funksiyalar", dur: "16:22" },
            { name: "Closure və Scope", dur: "22:10" },
        ]
    },
    {
        title: "Bölmə 3: DOM & Events", icon: "🖱️", lessons: [
            { name: "DOM nədir?", dur: "9:55" },
            { name: "Element seçmək", dur: "13:30" },
            { name: "Event Listeners", dur: "19:45" },
        ]
    },
    {
        title: "Bölmə 4: Asinxron JS", icon: "⚡", lessons: [
            { name: "Promises", dur: "17:55" },
            { name: "Async/Await", dur: "20:30" },
            { name: "Fetch API", dur: "15:45" },
        ]
    },
];

// Tab switching
document.querySelectorAll('.tab-btn').forEach(btn => {
    btn.addEventListener('click', () => {
        document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
        document.querySelectorAll('.tab-content').forEach(c => c.classList.remove('active'));
        btn.classList.add('active');
        document.getElementById(btn.dataset.tab).classList.add('active');
        if (btn.dataset.tab === 'curriculum') buildAccordion();
    });
});

// Accordion builder
function buildAccordion() {
    const el = document.getElementById('accordionList');
    if (!el || el.children.length > 0) return;
    curriculum.forEach((ch, ci) => {
        const div = document.createElement('div');
        div.className = 'chapter';
        const lessons = ch.lessons.map((l, li) => {
            const badge = l.demo ? '<span class="badge badge-demo">DEMO</span>'
                : l.free ? '<span class="badge badge-free">PULSUz</span>'
                    : '<span class="badge badge-locked">🔒</span>';
            return `<div class="lesson-item" onclick="loadLesson(${ci},${li})">
        <span class="l-icon">${l.demo || l.free ? '▶' : '🔒'}</span>
        <div class="l-info"><span>${l.name}</span><small>${l.dur}</small></div>
        ${badge}
      </div>`;
        }).join('');
        div.innerHTML = `
      <div class="chapter-header" onclick="this.classList.toggle('open');this.nextElementSibling.classList.toggle('open')">
        <span>${ch.icon} ${ch.title}</span><span class="arrow">▼</span>
      </div>
      <div class="chapter-lessons">${lessons}</div>`;
        el.appendChild(div);
    });
}

// Load lesson into curriculum video player
function loadLesson(ci, li) {
    const l = curriculum[ci].lessons[li];
    document.getElementById('videoLabel2').textContent = l.name;
    document.getElementById('npTitle2').textContent = l.name;
    document.getElementById('npMeta2').textContent = `Bölmə ${ci + 1} · Dərs ${li + 1} · ${l.dur}`;
    document.getElementById('timeTxt2').textContent = `0:00 / ${l.dur}`;
    document.getElementById('progressFill2').style.width = '0%';
}

// Demo lesson switcher (overview tab)
document.querySelectorAll('.demo-lessons .lesson-item:not(.locked)').forEach(item => {
    item.addEventListener('click', () => {
        document.querySelectorAll('.demo-lessons .lesson-item').forEach(i => i.classList.remove('playing'));
        item.classList.add('playing');
        document.getElementById('npTitle').textContent = item.dataset.name;
        document.getElementById('npMeta').textContent = item.dataset.meta + ' · ' + item.dataset.dur;
        document.getElementById('videoLabel').textContent = item.dataset.name;
        document.getElementById('progressFill').style.width = '0%';
    });
});

// Simple progress simulation
let playing = false, secs = 0, total = 504, timer;
document.getElementById('playBtn')?.addEventListener('click', togglePlay);
document.getElementById('playCtrl')?.addEventListener('click', togglePlay);
document.getElementById('demoPlayBtn')?.addEventListener('click', togglePlay);

function togglePlay() {
    playing = !playing;
    if (playing) {
        timer = setInterval(() => {
            secs = Math.min(secs + 1, total);
            document.getElementById('progressFill').style.width = (secs / total * 100) + '%';
            const m = Math.floor(secs / 60), s = secs % 60;
            document.getElementById('timeTxt').textContent = `${m}:${s.toString().padStart(2, '0')} / 8:24`;
        }, 1000);
    } else clearInterval(timer);
}