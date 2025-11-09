let activeMenuItem = null;
let openMenus = new Set();
let lastButtonClickTime = 0;

// Initialize the menu state for current URL
function initializeMenuState(currentUrl) {
    try {
        if (!currentUrl) currentUrl = window.location.pathname;
        const normalized = (currentUrl || '').replace(/\/$/, '');

        const items = document.querySelectorAll('.nav-child[href], .nav-parent[href]');
        let matched = null;
        items.forEach(el => {
            const href = (el.getAttribute('href') || '').replace(/\/$/, '');
            if (href && (normalized === href || normalized.endsWith(href))) matched = el;
        });
        if (matched) setActiveMenuItem(matched);
    } catch { /* no-op */ }
}

// Toggle dropdown menu (button with data-menu-title)
function toggleDropdown(buttonElement) {
    if (!buttonElement) return;
    const now = Date.now();
    if (now - lastButtonClickTime < 250) return; // prevent double-click flicker
    lastButtonClickTime = now;

    const menuTitle = buttonElement.getAttribute('data-menu-title');
    const content = buttonElement.nextElementSibling;
    const arrow = buttonElement.querySelector('svg.dropdown-arrow'); // only the arrow
    const wasOpen = openMenus.has(menuTitle);

    // Close others first
    document.querySelectorAll('button[data-menu-title]').forEach(btn => {
        if (btn === buttonElement) return;
        const title = btn.getAttribute('data-menu-title');
        const list = btn.nextElementSibling;
        if (list) list.classList.add('hidden');
        btn.querySelector('svg.dropdown-arrow')?.classList.remove('rotate-90');
        openMenus.delete(title);
    });

    if (wasOpen) {
        content?.classList.add('hidden');
        arrow?.classList.remove('rotate-90');
        openMenus.delete(menuTitle);
    } else {
        content?.classList.remove('hidden');
        arrow?.classList.add('rotate-90'); // rotate on click even without selecting a child
        openMenus.add(menuTitle);
    }
}

// Apply active styles depending on item type
function setActiveMenuItem(element) {
    clearChildActiveStates();

    if (element.classList.contains('nav-child')) {
        element.classList.add('text-primary', 'font-medium');

        // Make the dot primary for active child
        const dot = element.querySelector('.nav-dot');
        if (dot) {
            dot.classList.remove('bg-gray-400', 'dark:bg-gray-500');
            dot.classList.add('bg-primary');
        }

        const parentBtn = element.closest('ul')?.previousElementSibling;
        if (parentBtn) {
            const title = parentBtn.getAttribute('data-menu-title');
            parentBtn.classList.add('bg-primary/10', 'text-primary', 'font-medium');
            parentBtn.querySelector('svg.dropdown-arrow')?.classList.add('rotate-90');
            element.closest('ul')?.classList.remove('hidden');
            if (title) openMenus.add(title);
        }
    } else if (element.classList.contains('nav-parent') && element.getAttribute('href')) {
        // Non-dropdown parent link active
        element.classList.add('bg-primary/10', 'text-primary', 'font-medium');
    }

    activeMenuItem = element;
}

// Remove all active styles from children
function clearChildActiveStates() {
    document.querySelectorAll('.nav-child').forEach(el => {
        el.classList.remove('text-primary', 'font-medium');
        const dot = el.querySelector('.nav-dot');
        if (dot) {
            dot.classList.remove('bg-primary');
            // restore original gray if not already
            if (!dot.classList.contains('bg-gray-400')) {
                dot.classList.add('bg-gray-400', 'dark:bg-gray-500');
            }
        }
    });
}

// Click delegation for the whole menu list
function handleMenuClick(event) {
    const target = event.target.closest('button[data-menu-title], a.nav-parent, a.nav-child');
    if (!target) return;

    if (target.hasAttribute('data-menu-title')) {
        event.preventDefault();
        toggleDropdown(target);
    } else if (target.matches('a.nav-parent, a.nav-child')) {
        setActiveMenuItem(target);
    }
}

// Setup called from Blazor after initial render
function setupNavMenu() {
    const menu = document.getElementById('nav-menu');
    if (menu && !menu.__navSetupDone) {
        menu.addEventListener('click', handleMenuClick);
        menu.__navSetupDone = true;
    }

    initializeMenuState(window.location.pathname);

    // Update active state on browser navigation
    window.addEventListener('popstate', () => {
        initializeMenuState(window.location.pathname);
    });
}

// Expose globally for JS interop
window.setupNavMenu = setupNavMenu;
window.initializeMenuState = initializeMenuState;
window.setActiveMenuItem = setActiveMenuItem;
window.toggleDropdown = toggleDropdown;


