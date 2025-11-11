(function () {
    // Expose a global for Blazor to call modals explicitly
    window.BZ = window.BZ || {};

    // Show modal
    window.BZ.showModal = (id) => {
        const el = document.getElementById(id);
        if (!el) return console.warn(`Modal with id '${id}' not found.`);

        // Use stored reference or create a new modal
        let modal = el._modal;
        if (!modal) {
            modal = new Modal(el, { backdrop: 'static' });
            el._modal = modal;
        }
        modal.show();
    };

    // Hide modal
    window.BZ.hideModal = (id) => {
        const el = document.getElementById(id);
        if (!el) return console.warn(`Modal with id '${id}' not found.`);

        const modal = el._modal;
        if (modal) modal.hide();
    };

    // Auto-init buttons with data-modal-toggle (non-Blazor rendered)
    document.querySelectorAll('[data-modal-toggle]').forEach(btn => {
        if (!btn._flowbiteAttached) {
            btn.addEventListener('click', () => {
                const targetId = btn.getAttribute('data-modal-target');
                if (targetId) window.BZ.showModal(targetId);
            });
            btn._flowbiteAttached = true;
        }
    });

    // Dropdown toggle for user menu
    window.BZ.toggleDropdown = (dropdownId, toggleId) => {
        const btn = document.getElementById(toggleId) || document.querySelector(`[data-dropdown-toggle="${dropdownId}"]`);
        const dropdown = document.getElementById(dropdownId);
        if (!btn || !dropdown) return;

        btn.addEventListener('click', () => {
            dropdown.classList.toggle('hidden');
        });

        // Optional: close dropdown if clicking outside
        document.addEventListener('click', (e) => {
            if (!btn.contains(e.target) && !dropdown.contains(e.target)) {
                dropdown.classList.add('hidden');
            }
        });
    };

    // Optional: re-init modals dynamically if Blazor adds new nodes
    const observer = new MutationObserver(() => {
        document.querySelectorAll('[data-modal-toggle]').forEach(btn => {
            if (!btn._flowbiteAttached) {
                btn.addEventListener('click', () => {
                    const targetId = btn.getAttribute('data-modal-target');
                    if (targetId) window.BZ.showModal(targetId);
                });
                btn._flowbiteAttached = true;
            }
        });
    });
    observer.observe(document.body, { childList: true, subtree: true });


})();
