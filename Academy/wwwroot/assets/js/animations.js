/**
 * ============================================================
 *  Inzara Academy — animations.js
 *  6 Premium Frontend Effekti
 *  Mövcud funksionallığa toxunmur, yalnız üstünə əlavə edir.
 * ============================================================
 */

(function () {
  'use strict';

  /* ── prefers-reduced-motion yoxlaması ── */
  var reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
  var isMobile = window.matchMedia('(max-width: 768px)').matches;

  /* ============================================================
     1. 🌊 CURSOR SPOTLIGHT
     Siçanı izləyən yumşaq violet-cyan parıltı
  ============================================================ */
  function initCursorSpotlight() {
    if (reducedMotion || isMobile) return;

    var spotlight = document.createElement('div');
    spotlight.id = 'cursor-spotlight';
    spotlight.style.cssText = [
      'position:fixed',
      'top:0', 'left:0',
      'width:100vw', 'height:100vh',
      'pointer-events:none',
      'z-index:0',
      'opacity:0',
      'transition:opacity 0.6s ease',
      'background:radial-gradient(600px circle at var(--mouse-x,50%) var(--mouse-y,50%), rgba(99,102,241,0.07) 0%, rgba(14,165,233,0.04) 40%, transparent 70%)'
    ].join(';');
    document.body.appendChild(spotlight);

    var raf;
    var tx = 0, ty = 0;

    document.addEventListener('mousemove', function (e) {
      tx = e.clientX;
      ty = e.clientY;
      spotlight.style.opacity = '1';
      cancelAnimationFrame(raf);
      raf = requestAnimationFrame(function () {
        spotlight.style.setProperty('--mouse-x', tx + 'px');
        spotlight.style.setProperty('--mouse-y', ty + 'px');
      });
    });

    document.addEventListener('mouseleave', function () {
      spotlight.style.opacity = '0';
    });
  }

  /* ============================================================
     2. ✨ AURORA ARXA FON
     Yavaş hərəkət edən şimal işığı blob-ları
  ============================================================ */
  function initAurora() {
    if (reducedMotion) return;

    var style = document.createElement('style');
    style.textContent = [
      '@keyframes auroraA{0%{transform:translate(0,0) scale(1)}33%{transform:translate(80px,-60px) scale(1.1)}66%{transform:translate(-40px,80px) scale(0.95)}100%{transform:translate(0,0) scale(1)}}',
      '@keyframes auroraB{0%{transform:translate(0,0) scale(1)}25%{transform:translate(-100px,60px) scale(1.15)}75%{transform:translate(60px,-80px) scale(0.9)}100%{transform:translate(0,0) scale(1)}}',
      '@keyframes auroraC{0%{transform:translate(0,0) scale(1)}40%{transform:translate(70px,90px) scale(1.05)}80%{transform:translate(-80px,-40px) scale(1.1)}100%{transform:translate(0,0) scale(1)}}',
      '@keyframes auroraD{0%{transform:translate(0,0) scale(1)}50%{transform:translate(-60px,-70px) scale(0.9)}100%{transform:translate(0,0) scale(1)}}'
    ].join('');
    document.head.appendChild(style);

    var blobs = [
      { color: '#6366f1', size: 600, top: '10%',  left: '15%',  anim: 'auroraA', dur: '28s' },
      { color: '#0ea5e9', size: 500, top: '60%',  left: '70%',  anim: 'auroraB', dur: '35s' },
      { color: '#8b5cf6', size: 450, top: '40%',  left: '40%',  anim: 'auroraC', dur: '22s' },
      { color: '#06b6d4', size: 400, top: '80%',  left: '10%',  anim: 'auroraD', dur: '40s' }
    ];

    var container = document.createElement('div');
    container.id = 'aurora-bg';
    container.style.cssText = 'position:fixed;inset:0;z-index:-1;overflow:hidden;pointer-events:none';

    blobs.forEach(function (b) {
      var el = document.createElement('div');
      el.style.cssText = [
        'position:absolute',
        'width:' + b.size + 'px',
        'height:' + b.size + 'px',
        'top:' + b.top,
        'left:' + b.left,
        'background:' + b.color,
        'border-radius:50%',
        'filter:blur(80px)',
        'opacity:' + (isMobile ? '0.06' : '0.10'),
        'animation:' + b.anim + ' ' + b.dur + ' ease-in-out infinite',
        'will-change:transform'
      ].join(';');
      container.appendChild(el);
    });

    document.body.insertBefore(container, document.body.firstChild);
  }

  /* ============================================================
     3. 🃏 KART 3D TILT + SHEEN
     Kurs kartlarına siçan mövqeyinə görə 3D əyilmə
  ============================================================ */
  function initCardTilt() {
    if (isMobile) return;

    var SELECTORS = [
      '.premium-course-card',
      '.course-card',
      '.card-general',
      '.student-result-card',
      '.p-stat-card',
      '[data-tilt]'
    ].join(',');

    function applyTilt(card) {
      if (card.dataset.tiltInit) return;
      card.dataset.tiltInit = '1';

      card.style.willChange = 'transform';
      card.style.transition = 'transform 0.1s ease';

      /* Sheen overlay */
      var sheen = document.createElement('div');
      sheen.style.cssText = [
        'position:absolute', 'inset:0',
        'border-radius:inherit',
        'pointer-events:none',
        'opacity:0',
        'transition:opacity 0.3s ease',
        'z-index:10'
      ].join(';');
      if (getComputedStyle(card).position === 'static') {
        card.style.position = 'relative';
      }
      card.appendChild(sheen);

      card.addEventListener('mousemove', function (e) {
        var rect = card.getBoundingClientRect();
        var x = (e.clientX - rect.left) / rect.width;
        var y = (e.clientY - rect.top)  / rect.height;
        var rx = (y - 0.5) * -8;
        var ry = (x - 0.5) *  8;

        card.style.transform = 'perspective(1000px) rotateX(' + rx + 'deg) rotateY(' + ry + 'deg) scale(1.02)';

        sheen.style.opacity = '1';
        sheen.style.background = 'radial-gradient(circle at ' + (x * 100) + '% ' + (y * 100) + '%, rgba(255,255,255,0.08) 0%, transparent 60%)';
      });

      card.addEventListener('mouseleave', function () {
        card.style.transition = 'transform 300ms ease';
        card.style.transform  = 'perspective(1000px) rotateX(0deg) rotateY(0deg) scale(1)';
        sheen.style.opacity   = '0';
      });
    }

    function scanCards() {
      try {
        document.querySelectorAll(SELECTORS).forEach(applyTilt);
      } catch (e) {}
    }

    scanCards();

    /* Dinamik yüklənən kartlar üçün MutationObserver */
    var mo = new MutationObserver(function () { scanCards(); });
    mo.observe(document.body, { childList: true, subtree: true });
  }

  /* ============================================================
     4. 🔢 COUNTUP SAYACI
     Intersection Observer ilə görünəndə 0-dan hədəfə say
  ============================================================ */
  function initCountUp() {
    function easeOutCubic(t) { return 1 - Math.pow(1 - t, 3); }

    function animateCount(el, target, suffix, duration) {
      if (el.dataset.counted) return;
      el.dataset.counted = '1';
      var start = null;

      function step(ts) {
        if (!start) start = ts;
        var progress = Math.min((ts - start) / duration, 1);
        var val = Math.floor(easeOutCubic(progress) * target);
        el.textContent = val.toLocaleString() + suffix;
        if (progress < 1) requestAnimationFrame(step);
        else el.textContent = target.toLocaleString() + suffix;
      }

      if (reducedMotion) {
        el.textContent = target.toLocaleString() + suffix;
        return;
      }
      requestAnimationFrame(step);
    }

    function parseTarget(el) {
      var raw = el.dataset.countTarget || el.textContent.trim();
      var suffix = '';
      var match = raw.match(/^([\d,]+)(\D*)$/);
      if (!match) return null;
      var num = parseInt(match[1].replace(/,/g, ''), 10);
      suffix = match[2] || '';
      return { num: num, suffix: suffix };
    }

    /* Numbers bölməsindəki statistika elementləri */
    var STAT_SELECTORS = [
      '.quality-container span',
      '[data-countup]',
      '.stat-card-custom h3',
      '.p-stat-num[data-target]'
    ].join(',');

    var observer = new IntersectionObserver(function (entries) {
      entries.forEach(function (entry) {
        if (!entry.isIntersecting) return;
        var el = entry.target;
        var parsed = parseTarget(el);
        if (!parsed || isNaN(parsed.num)) return;
        animateCount(el, parsed.num, parsed.suffix, 2000);
        observer.unobserve(el);
      });
    }, { threshold: 0.3 });

    function scanCounters() {
      try {
        document.querySelectorAll(STAT_SELECTORS).forEach(function (el) {
          if (!el.dataset.counted) {
            /* Orijinal dəyəri data-count-target-ə saxla */
            if (!el.dataset.countTarget) {
              el.dataset.countTarget = el.textContent.trim();
            }
            observer.observe(el);
          }
        });
      } catch (e) {}
    }

    scanCounters();
    var mo = new MutationObserver(scanCounters);
    mo.observe(document.body, { childList: true, subtree: true });
  }

  /* ============================================================
     5. 💫 KONFETTİ EFFEKTİ
     Uğur anlarında konfetti partlayışı
  ============================================================ */
  function initConfetti() {
    /* canvas-confetti CDN-dən yüklənir */
    var script = document.createElement('script');
    script.src = 'https://cdn.jsdelivr.net/npm/canvas-confetti@1.9.2/dist/confetti.browser.min.js';
    document.head.appendChild(script);

    var BRAND_COLORS = ['#6366f1', '#0ea5e9', '#a78bfa', '#38bdf8', '#ffffff', '#c4b5fd'];

    window.launchConfetti = function () {
      if (reducedMotion || typeof confetti === 'undefined') return;

      var count = 180;
      var defaults = {
        origin: { y: 0.7 },
        colors: BRAND_COLORS,
        zIndex: 9999
      };

      function fire(particleRatio, opts) {
        confetti(Object.assign({}, defaults, opts, {
          particleCount: Math.floor(count * particleRatio)
        }));
      }

      fire(0.25, { spread: 26, startVelocity: 55 });
      fire(0.20, { spread: 60 });
      fire(0.35, { spread: 100, decay: 0.91, scalar: 0.8 });
      fire(0.10, { spread: 120, startVelocity: 25, decay: 0.92, scalar: 1.2 });
      fire(0.10, { spread: 120, startVelocity: 45 });
    };

    /* Uğur toast-larını izlə */
    function watchSuccessToasts() {
      var mo = new MutationObserver(function (mutations) {
        mutations.forEach(function (m) {
          m.addedNodes.forEach(function (node) {
            if (node.nodeType !== 1) return;
            var text = node.textContent || '';
            var isSuccess =
              node.classList.contains('auth-alert-success') ||
              node.classList.contains('p-toast-success') ||
              (node.id === 'custom-toast' && text.includes('uğur')) ||
              text.includes('Uğurla') ||
              text.includes('uğurla') ||
              text.includes('Konfetti') ||
              text.includes('tamamlandı') ||
              text.includes('Sertifikat');

            if (isSuccess) {
              setTimeout(window.launchConfetti, 300);
            }
          });
        });
      });
      mo.observe(document.body, { childList: true, subtree: true });
    }

    script.onload = function () {
      watchSuccessToasts();

      /* Basket uğurlu əlavə — səhifə yüklənəndə yoxla */
      var successMsg = document.querySelector('.auth-alert-success, .alert-success');
      if (successMsg) {
        setTimeout(window.launchConfetti, 500);
      }
    };
  }

  /* ============================================================
     6. 📊 STAGGER AÇILIŞ ANİMASİYASI
     Viewport-a girən elementlər ardıcıl fade+translateY ilə açılır
  ============================================================ */
  function initStaggerReveal() {
    var SELECTORS = [
      '.premium-course-card',
      '.course-card',
      '.card',
      '.quality-container',
      '.stat-card-custom',
      '.student-result-card',
      '.blog-card',
      '.team-card',
      '.testimonial-card',
      '.card-general',
      'section > .container > *',
      '[data-reveal]'
    ].join(',');

    var style = document.createElement('style');
    style.textContent = [
      '.reveal-hidden{opacity:0;transform:translateY(20px);transition:opacity 500ms cubic-bezier(0.4,0,0.2,1),transform 500ms cubic-bezier(0.4,0,0.2,1)}',
      '.reveal-visible{opacity:1!important;transform:translateY(0)!important}'
    ].join('');
    document.head.appendChild(style);

    if (reducedMotion) return;

    var observer = new IntersectionObserver(function (entries) {
      entries.forEach(function (entry) {
        if (!entry.isIntersecting) return;
        var el = entry.target;
        el.classList.add('reveal-visible');
        observer.unobserve(el);
      });
    }, { threshold: 0.1, rootMargin: '0px 0px -40px 0px' });

    var staggerIndex = 0;
    var lastParent = null;

    function scanElements() {
      try {
        document.querySelectorAll(SELECTORS).forEach(function (el) {
          if (el.dataset.revealInit) return;
          el.dataset.revealInit = '1';

          /* Stagger delay — aynı parent-dəki elementlər ardıcıl gəlir */
          if (el.parentElement !== lastParent) {
            staggerIndex = 0;
            lastParent = el.parentElement;
          }
          var delay = Math.min(staggerIndex * 80, 480);
          staggerIndex++;

          el.classList.add('reveal-hidden');
          el.style.transitionDelay = delay + 'ms';
          observer.observe(el);
        });
      } catch (e) {}
    }

    /* DOM hazır olduqdan sonra scan et */
    if (document.readyState === 'loading') {
      document.addEventListener('DOMContentLoaded', scanElements);
    } else {
      scanElements();
    }

    /* Dinamik yüklənən elementlər üçün */
    var mo = new MutationObserver(function () {
      setTimeout(scanElements, 100);
    });
    mo.observe(document.body, { childList: true, subtree: true });
  }

  /* ============================================================
     INIT — Hamısını başlat
  ============================================================ */
  function init() {
    initAurora();
    initCursorSpotlight();
    initCardTilt();
    initCountUp();
    initConfetti();
    initStaggerReveal();
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }

})();
