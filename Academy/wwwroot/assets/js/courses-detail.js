"use strict";

// ─── UTILITY ───
function showToast(msg, icon = '✅') {
    const t = document.getElementById('toast');
    t.querySelector('.t-icon').textContent = icon;
    t.querySelector('.t-msg').textContent = msg;
    t.classList.add('show');
    setTimeout(() => t.classList.remove('show'), 3200);
}

// ─── TABS ───
document.querySelectorAll('.tab-btn').forEach(btn => {
    btn.addEventListener('click', () => {
        document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
        document.querySelectorAll('.tab-content').forEach(c => c.classList.remove('active'));
        btn.classList.add('active');
        const target = document.getElementById(btn.dataset.tab);
        if (target) target.classList.add('active');
        if (btn.dataset.tab === 'curriculum') buildAccordion();
    });
});

// ─── HAMBURGER ───
const hamburger = document.querySelector('.hamburger');
const menuCommon = document.querySelector('.menu-list');
const navActions = document.querySelector('.nav-actions');
if (hamburger) {
    hamburger.addEventListener('click', () => {
        menuCommon?.classList.toggle('open');
        navActions?.classList.toggle('open');
    });
}

// ─── MODAL ───
function openModal(id) {
    document.querySelectorAll('.modal').forEach(m => m.classList.remove('active'));
    document.getElementById(id)?.classList.add('active');
}
function closeAllModals() {
    document.querySelectorAll('.modal').forEach(m => m.classList.remove('active'));
}
document.querySelectorAll('.closeBtn').forEach(btn => btn.addEventListener('click', closeAllModals));
window.addEventListener('click', e => {
    if (e.target.classList.contains('modal')) closeAllModals();
});
document.getElementById('loginBtn')?.addEventListener('click', () => openModal('loginModal'));
document.getElementById('registerBtn')?.addEventListener('click', () => openModal('registerModal'));
document.querySelectorAll('[data-open-modal]').forEach(el => {
    el.addEventListener('click', () => openModal(el.dataset.openModal));
});

// Modal tab switching
document.querySelectorAll('.modal-tab').forEach(tab => {
    tab.addEventListener('click', () => {
        const parent = tab.closest('.modal-box');
        parent.querySelectorAll('.modal-tab').forEach(t => t.classList.remove('active'));
        tab.classList.add('active');
        parent.querySelectorAll('.modal-form-section').forEach(s => s.classList.remove('active'));
        parent.querySelector('#' + tab.dataset.target)?.classList.add('active');
    });
});

// ─── COUNTDOWN TIMER (price urgency) ───
(function () {
    const el = document.getElementById('countdownTimer');
    if (!el) return;
    let total = 2 * 3600 + 47 * 60 + 33;
    const tick = () => {
        if (total <= 0) { el.textContent = 'BİTDİ'; return; }
        total--;
        const h = Math.floor(total / 3600);
        const m = Math.floor((total % 3600) / 60);
        const s = total % 60;
        el.textContent = `${String(h).padStart(2, '0')}:${String(m).padStart(2, '0')}:${String(s).padStart(2, '0')}`;
    };
    tick();
    setInterval(tick, 1000);
})();

// ─── ENROLL BTN ───
document.querySelector('.btn-enroll')?.addEventListener('click', () => {
    openModal('loginModal');
    showToast('Kursa yazılmaq üçün daxil olun', '🎓');
});

// ─── VIDEO PLAYER (Overview Demo) ───
let playing = false, secs = 0, total = 504, timer;
const progressFill = document.getElementById('progressFill');
const timeTxt = document.getElementById('timeTxt');
const playBtnBig = document.getElementById('playBtnBig');
const playCtrl = document.getElementById('playCtrl');

function formatTime(s) {
    return `${Math.floor(s / 60)}:${String(s % 60).padStart(2, '0')}`;
}
function togglePlay() {
    playing = !playing;
    [playBtnBig, playCtrl].forEach(b => {
        if (!b) return;
        b.classList.toggle('playing', playing);
    });
    if (playing) {
        timer = setInterval(() => {
            secs = Math.min(secs + 1, total);
            if (progressFill) progressFill.style.width = (secs / total * 100) + '%';
            if (timeTxt) timeTxt.textContent = `${formatTime(secs)} / 8:24`;
            if (secs >= total) { playing = false; clearInterval(timer); }
        }, 1000);
    } else {
        clearInterval(timer);
    }
}
playBtnBig?.addEventListener('click', togglePlay);
playCtrl?.addEventListener('click', togglePlay);
document.getElementById('demoPlayBtn')?.addEventListener('click', () => {
    document.querySelector('[data-tab="overview"]')?.click();
    setTimeout(togglePlay, 100);
});

// Progress bar click seek
document.querySelector('.progress-bar')?.addEventListener('click', function (e) {
    const rect = this.getBoundingClientRect();
    const pct = (e.clientX - rect.left) / rect.width;
    secs = Math.floor(pct * total);
    if (progressFill) progressFill.style.width = pct * 100 + '%';
    if (timeTxt) timeTxt.textContent = `${formatTime(secs)} / 8:24`;
});

// ─── DEMO LESSONS (Overview tab) ───
document.querySelectorAll('.demo-lessons .lesson-item:not(.locked)').forEach(item => {
    item.addEventListener('click', () => {
        document.querySelectorAll('.demo-lessons .lesson-item').forEach(i => i.classList.remove('playing'));
        item.classList.add('playing');
        const name = item.dataset.name;
        const dur = item.dataset.dur;
        const meta = item.dataset.meta;
        document.getElementById('npTitle').textContent = name;
        document.getElementById('npMeta').textContent = `${meta} · ${dur}`;
        document.getElementById('videoLabel').textContent = name;
        if (progressFill) progressFill.style.width = '0%';
        secs = 0;
        if (!playing) togglePlay();
        else { clearInterval(timer); playing = false; togglePlay(); }
    });
});

// ─── CURRICULUM ACCORDION ───
const curriculum = [
    {
        title: "Bölmə 1: Başlanğıc", icon: "🚀", lessons: [
            { name: "JavaScript nədir?", dur: "8:24", demo: true },
            { name: "Mühit qurulması (VS Code)", dur: "12:05", free: true },
            { name: "Dəyişənlər: var, let, const", dur: "18:32" },
            { name: "Məlumat növləri", dur: "14:15" },
            { name: "Şərt operatorları", dur: "11:40" },
        ]
    },
    {
        title: "Bölmə 2: Funksiyalar & Scope", icon: "⚙️", lessons: [
            { name: "Funksiya nədir?", dur: "10:40" },
            { name: "Arrow funksiyalar", dur: "16:22" },
            { name: "Closure və Scope", dur: "22:10" },
            { name: "Higher-Order Functions", dur: "19:05" },
        ]
    },
    {
        title: "Bölmə 3: DOM & Events", icon: "🖱️", lessons: [
            { name: "DOM nədir?", dur: "9:55" },
            { name: "Element seçmək", dur: "13:30" },
            { name: "Event Listeners", dur: "19:45" },
            { name: "Form validasiyası", dur: "16:20" },
        ]
    },
    {
        title: "Bölmə 4: Asinxron JavaScript", icon: "⚡", lessons: [
            { name: "Callbacks", dur: "12:10" },
            { name: "Promises", dur: "17:55" },
            { name: "Async/Await", dur: "20:30" },
            { name: "Fetch API & REST", dur: "15:45" },
        ]
    },
    {
        title: "Bölmə 5: React.js", icon: "⚛️", lessons: [
            { name: "React nədir? Virtual DOM", dur: "14:20" },
            { name: "Components & Props", dur: "18:35" },
            { name: "State & Hooks", dur: "25:10" },
            { name: "useEffect & API çağırışı", dur: "22:45" },
        ]
    },
    {
        title: "Bölmə 6: Real Layihə", icon: "🏗️", lessons: [
            { name: "Layihə Planlaması", dur: "8:30" },
            { name: "Full-Stack App qurulması", dur: "38:20" },
            { name: "Deploy etmək (Vercel)", dur: "12:15" },
        ]
    },
];

let totalCompleted = 0;
let activeLesson = null;

function buildAccordion() {
    const el = document.getElementById('accordionList');
    if (!el || el.children.length > 0) return;

    curriculum.forEach((ch, ci) => {
        const item = document.createElement('div');
        item.className = 'accordion-item';
        if (ci === 0) item.classList.add('open');

        const lessonCount = ch.lessons.length;
        const dur = ch.lessons.reduce((acc, l) => {
            const [m, s] = l.dur.split(':').map(Number);
            return acc + m * 60 + s;
        }, 0);
        const durMin = Math.floor(dur / 60);

        const lessonsHTML = ch.lessons.map((l, li) => {
            const badge = l.demo
                ? '<span class="badge badge-demo">DEMO</span>'
                : l.free
                    ? '<span class="badge badge-free">FREE</span>'
                    : '<span class="badge badge-locked">🔒</span>';
            const icon = l.demo || l.free ? '▶' : '🔒';
            const clickable = l.demo || l.free ? `onclick="loadCurrLesson(${ci},${li},this)"` : '';
            return `<div class="acc-lesson ${l.demo ? 'playing' : ''}" ${clickable}>
        <span class="acc-l-icon">${icon}</span>
        <span class="acc-l-name">${l.name}</span>
        ${badge}
        <span class="acc-l-dur">${l.dur}</span>
      </div>`;
        }).join('');

        item.innerHTML = `
      <div class="accordion-head" onclick="toggleAccordion(this.parentElement)">
        <span class="accordion-left">
          <span class="accordion-icon">${ch.icon}</span>
          <span>${ch.title}</span>
        </span>
        <span style="display:flex;align-items:center;gap:12px">
          <span class="accordion-meta">${lessonCount} dərs · ${durMin} dəq</span>
          <span class="accordion-arrow">▼</span>
        </span>
      </div>
      <div class="accordion-body">${lessonsHTML}</div>`;
        el.appendChild(item);
    });
}

window.toggleAccordion = function (item) {
    item.classList.toggle('open');
};

window.loadCurrLesson = function (ci, li, el) {
    const l = curriculum[ci].lessons[li];
    document.querySelectorAll('.acc-lesson').forEach(e => e.classList.remove('playing'));
    el.classList.add('playing');

    document.getElementById('videoLabel2').textContent = l.name;
    document.getElementById('npTitle2').textContent = l.name;
    document.getElementById('npMeta2').textContent = `Bölmə ${ci + 1} · Dərs ${li + 1} · ${l.dur}`;
    document.getElementById('timeTxt2').textContent = `0:00 / ${l.dur}`;
    document.getElementById('progressFill2').style.width = '0%';

    // progress
    totalCompleted++;
    const totalLessons = curriculum.reduce((a, c) => a + c.lessons.length, 0);
    const pct = Math.min((totalCompleted / totalLessons) * 100, 100).toFixed(0);
    document.getElementById('currProgFill').style.width = pct + '%';
    document.getElementById('currProgLabel').textContent = `${pct}% tamamlandı`;

    showToast(`"${l.name}" dərsi başladı`, '▶');
};

// ─── CURRICULUM PLAYER 2 ───
let playing2 = false, secs2 = 0, total2 = 504, timer2;
const pf2 = document.getElementById('progressFill2');
const tt2 = document.getElementById('timeTxt2');
const pb2 = document.getElementById('playBtnBig2');
const pc2 = document.getElementById('playCtrl2');

function togglePlay2() {
    playing2 = !playing2;
    [pb2, pc2].forEach(b => b?.classList.toggle('playing', playing2));
    if (playing2) {
        timer2 = setInterval(() => {
            secs2 = Math.min(secs2 + 1, total2);
            if (pf2) pf2.style.width = (secs2 / total2 * 100) + '%';
            if (tt2) {
                const cur = `${formatTime(secs2)}`;
                const tot = tt2.textContent.split('/')[1]?.trim() || '8:24';
                tt2.textContent = `${cur} / ${tot}`;
            }
            if (secs2 >= total2) { playing2 = false; clearInterval(timer2); }
        }, 1000);
    } else clearInterval(timer2);
}
pb2?.addEventListener('click', togglePlay2);
pc2?.addEventListener('click', togglePlay2);

// ─── ZOOM SESSION COUNTDOWN ───
(function () {
    const el = document.getElementById('nextSessionTimer');
    if (!el) return;
    // next session in 1 day, 3 hours
    let t = 27 * 3600 + 15 * 60;
    const tick = () => {
        if (t <= 0) { el.textContent = 'İndi Başlayır!'; return; }
        t--;
        const h = Math.floor(t / 3600);
        const m = Math.floor((t % 3600) / 60);
        const s = t % 60;
        el.textContent = `${h}s ${String(m).padStart(2, '0')}d ${String(s).padStart(2, '0')}s`;
    };
    tick();
    setInterval(tick, 1000);
})();

// ─── ZOOM JOIN BTN ───
document.getElementById('zoomJoinBtn')?.addEventListener('click', () => {
    showToast('Zoom sessiyasına qoşulursunuz...', '📹');
    setTimeout(() => {
        // In production: window.open('https://zoom.us/j/YOUR_MEETING_ID', '_blank');
        alert('Zoom linki: https://zoom.us/j/123456789\n\nŞifrə: JS2025\n\nGörüşdə uğurlar! 🎉');
    }, 800);
});

document.querySelectorAll('.zoom-session').forEach(s => {
    s.addEventListener('click', function () {
        const title = this.querySelector('.session-info span')?.textContent;
        showToast(`"${title}" sessiyasına qeydiyyat`, '📅');
    });
});