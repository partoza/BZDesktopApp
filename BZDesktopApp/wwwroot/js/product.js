// Initialize Flowbite modal for Products
window.initBrandModal = (modalId) => {
    const modalEl = document.getElementById(modalId);
    if (modalEl && !modalEl._modal) {
        modalEl._modal = new Modal(modalEl, { backdrop: 'static' });
    }
};

// Show the modal
window.showBrandModal = (modalId) => {
    const modalEl = document.getElementById(modalId);
    if (modalEl && modalEl._modal) {
        modalEl._modal.show();
    }
};

// Hide the modal
window.closeBrandModal = (modalId) => {
    const modalEl = document.getElementById(modalId);
    if (modalEl && modalEl._modal) {
        modalEl._modal.hide();
    }
};
