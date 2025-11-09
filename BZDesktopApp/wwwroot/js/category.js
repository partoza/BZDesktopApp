// Initialize Flowbite modal
window.initCategoryModal = (modalId) => {
    const modalEl = document.getElementById(modalId);
    if (modalEl && !modalEl._modal) {
        modalEl._modal = new Modal(modalEl, { backdrop: 'static' });
    }
};

// Close Flowbite modal
window.closeCategoryModal = (modalId) => {
    const modalEl = document.getElementById(modalId);
    if (modalEl && modalEl._modal) {
        modalEl._modal.hide();
    } else {
        console.warn(`Modal with id '${modalId}' not found or not initialized.`);
    }
};