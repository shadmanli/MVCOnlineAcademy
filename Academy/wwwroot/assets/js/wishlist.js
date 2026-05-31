document.addEventListener('DOMContentLoaded', () => {
    updateWishlistBadge();
    updateHeartIcons();

    const msg = sessionStorage.getItem('wishlistMsg');
    if (msg) {
        showCustomToast(msg, true);
        sessionStorage.removeItem('wishlistMsg');
    }

    if (document.getElementById('wishlist-container')) {
        renderWishlistPage();
    }
});

// ── Hər istifadəçi üçün ayrıca localStorage key ──────────────
function getWishlistKey() {
    // Layout-da meta tag vasitəsilə user ID oxunur
    const meta = document.querySelector('meta[name="user-id"]');
    const userId = meta ? meta.content : 'guest';
    return 'wishlist_' + userId;
}

function getWishlist() {
    const list = localStorage.getItem(getWishlistKey());
    return list ? JSON.parse(list) : [];
}

function saveWishlist(wishlist) {
    localStorage.setItem(getWishlistKey(), JSON.stringify(wishlist));
}

// Kursu wishlist-? ?lav? edir v? ya ordan silir
function toggleWishlist(event, btnElement) {
    event.preventDefault(); // href qar??s?n? almaq ���n
    event.stopPropagation();

    // HTML5 data atributlardan m?lumatlar? oxuyuruq
    const id = parseInt(btnElement.getAttribute('data-course-id'));
    const title = btnElement.getAttribute('data-course-title');
    const price = btnElement.getAttribute('data-course-price');
    const imageUrl = btnElement.getAttribute('data-course-image');
    const instructor = btnElement.getAttribute('data-course-instructor');

    let wishlist = getWishlist();

    // Yoxlayaq kurs art?q varm?
    const existingIndex = wishlist.findIndex(item => item.id === id);

    if (existingIndex > -1) {
        // Varsa, sil
        wishlist.splice(existingIndex, 1);

        // ?g?r wishlist s?hif?sind?yiks? kart? d?rhal DOM-dan da sil? bil?rik
        const card = document.getElementById(`wishlist-card-${id}`);
        if(card) {
            card.remove();
            checkEmptyWishlist(); // say? v? bo? ekran? yenil?
        }

        saveWishlist(wishlist);
        updateWishlistBadge();
        updateHeartIcons();

        const pageCount = document.getElementById('wishlist-page-count');
        if (pageCount) {
            pageCount.innerText = wishlist.length;
        }

        showCustomToast("Kurs uğurla wishlist-dən silindi!", false);

    } else {
        wishlist.push({
            id: id,
            title: title,
            price: price,
            imageUrl: imageUrl,
            instructor: instructor || "Unknown Instructor"
        });

        saveWishlist(wishlist);

        // Yeni t?l?b: wishlist s?hif?sin? y�nl?ndir v? mesaj �t�r
        sessionStorage.setItem('wishlistMsg', 'Kurs u?urla wishlist-? ?lav? edildi!');
        window.location.href = '/Wishlist';
        return;
    }
}

// X�susi x?b?rdarl?q (Toast) funksiyas?
function showCustomToast(message, isSuccess = true) {
    let toastContainer = document.getElementById('js-toast-container');
    if (!toastContainer) {
        toastContainer = document.createElement('div');
        toastContainer.id = 'js-toast-container';

        Object.assign(toastContainer.style, {
            position: 'fixed',
            bottom: '28px',
            left: '50%',
            transform: 'translateX(-50%)',
            zIndex: '9999',
            display: 'flex',
            flexDirection: 'column',
            gap: '10px'
        });
        document.body.appendChild(toastContainer);
    }

    const toast = document.createElement('div');
    const icon = isSuccess ? '?' : '??';

    toast.innerHTML = `<span style="margin-right:10px;">${icon}</span><span>${message}</span>`;

    Object.assign(toast.style, {
        background: '#0c2320',
        color: 'white',
        padding: '14px 24px',
        borderRadius: '12px',
        border: `1px solid ${isSuccess ? 'rgba(0,196,168,0.3)' : 'rgba(255,100,100,0.3)'}`,
        fontSize: '14px',
        boxShadow: '0 10px 40px rgba(0,0,0,0.3)',
        display: 'flex',
        alignItems: 'center',
        transform: 'translateY(100px)',
        opacity: '0',
        transition: 'transform 0.4s ease, opacity 0.4s ease'
    });

    toastContainer.appendChild(toast);

    // Ekrana g?lsin
    setTimeout(() => {
        toast.style.transform = 'translateY(0)';
        toast.style.opacity = '1';
    }, 10);

    // 3 saniy? sonra getsin
    setTimeout(() => {
        toast.style.transform = 'translateY(100px)';
        toast.style.opacity = '0';
        setTimeout(() => toast.remove(), 400);
    }, 3000);
}

// ?konu bo? (b?y?nilm?yib) v? ya dolu (b?y?nilib) g�st?rir
function updateHeartIcons() {
    const wishlist = getWishlist();
    const courseIds = wishlist.map(c => c.id);

    // S?hif?d?ki b�t�n heart ikonlar?n? yoxla
    document.querySelectorAll('[data-wishlist-btn]').forEach(btn => {
        const id = parseInt(btn.getAttribute('data-wishlist-id'));
        const icon = btn.querySelector('i');

        if (courseIds.includes(id)) {
            icon.classList.remove('far'); // empty heart
            icon.classList.add('fas');    // filled heart
            icon.style.color = '#e74c3c'; // q?rm?z?
        } else {
            icon.classList.remove('fas');
            icon.classList.add('far');
            icon.style.color = '#718096'; // boz
        }
    });
}

// Yuxar?dak? menyuda say?ac? yenil?
function updateWishlistBadge() {
    const wishlist = getWishlist();
    const badge = document.getElementById('wishlist-badge');

    if (badge) {
        badge.innerText = wishlist.length;
        if (wishlist.length > 0) {
            badge.style.display = 'inline-block';
        } else {
            badge.style.display = 'none';
        }
    }
}

// Wishlist s?hif?sind? kurslar? m?hdud HTML il? �ap edir (CourseCard dizayn?na uy?un)
function renderWishlistPage() {
    const wishlist = getWishlist();
    const container = document.getElementById('wishlist-container');
    const emptyMsg = document.getElementById('empty-wishlist');
    const pageCount = document.getElementById('wishlist-page-count');

    pageCount.innerText = wishlist.length;
    container.innerHTML = '';

    if (wishlist.length === 0) {
        container.style.display = 'none';
        emptyMsg.style.display = 'block';
        return;
    }

    container.style.display = 'flex';
    emptyMsg.style.display = 'none';

    wishlist.forEach(course => {
        // Dinamik kart HTML strukturu
        const cardHtml = `
            <div class="card-courses" id="wishlist-card-${course.id}" style="width: 300px; background: #fff; border-radius: 12px; overflow: hidden; box-shadow: 0 10px 25px rgba(0,0,0,0.05); position: relative;">

                <button 
                    onclick="toggleWishlist(event, this)" 
                    data-course-id="${course.id}"
                    data-course-title="${course.title}"
                    data-course-price="${course.price}"
                    data-course-image="${course.imageUrl}"
                    data-course-instructor="${course.instructor}"
                    data-wishlist-btn data-wishlist-id="${course.id}" 
                    style="position: absolute; top: 15px; right: 15px; z-index: 10; background: #fff; border: none; width: 35px; height: 35px; border-radius: 50%; box-shadow: 0 3px 10px rgba(0,0,0,0.1); cursor: pointer; display: flex; align-items: center; justify-content: center;">
                    <i class="fas fa-heart" style="color: #e74c3c; font-size: 18px;"></i>
                </button>

                <a href="/course/detail/${course.id}" style="text-decoration: none; color: inherit;">
                    <div class="courses-img" style="height: 200px; overflow: hidden;">
                        <img src="${course.imageUrl.startsWith('/images') || course.imageUrl.startsWith('http') ? course.imageUrl : '/uploads/course/' + course.imageUrl}" 
                             alt="${course.title}" 
                             style="width: 100%; height: 100%; object-fit: cover;" />
                    </div>

                    <div class="card-content" style="padding: 20px;">
                        <span class="instructor" style="color: #718096; font-size: 14px;">${course.instructor}</span>
                        <h3 class="title" style="margin: 10px 0; font-size: 18px; font-weight: 600;">${course.title}</h3>

                        <div class="rating" style="display: flex; justify-content: space-between; align-items: center; margin-top: 15px;">
                            <span style="color: #f1c40f; font-size: 12px;">
                                <i class="fa-solid fa-star"></i>
                                <i class="fa-solid fa-star"></i>
                                <i class="fa-solid fa-star"></i>
                                <i class="fa-solid fa-star"></i>
                                <i class="fa-regular fa-star"></i>
                            </span>
                            <span class="price" style="font-weight: 700; color: #00bfa6; font-size: 18px;">$${course.price}</span>
                        </div>
                    </div>
                </a>
            </div>
        `;

        // Kart? container-? ?lav? et
        container.insertAdjacentHTML('beforeend', cardHtml);
    });
}

function checkEmptyWishlist() {
    const container = document.getElementById('wishlist-container');
    if (container && container.children.length === 0) {
        container.style.display = 'none';
        document.getElementById('empty-wishlist').style.display = 'block';
    }
}