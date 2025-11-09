window.SetupSupplierJS = () => {
    // Confirmation Modal Logic
    const confirmCreateBtn = document.getElementById('confirmCreateBtn');
    const confirmationModal = document.getElementById('confirmationModal');
    const cancelConfirmBtn = document.getElementById('cancelConfirmBtn');
    const finalCreateBtn = document.getElementById('finalCreateBtn');
    const supplierForm = document.getElementById('supplierForm');

    // Show confirmation modal when Create Supplier is clicked
    if (confirmCreateBtn) {
        confirmCreateBtn.addEventListener('click', function () {
            // First validate the form
            if (!supplierForm.checkValidity()) {
                supplierForm.reportValidity();
                return;
            }

            // Check if required product categories are selected (using checkboxes)
            const selectedProducts = document.querySelectorAll('input[name="product_categories"]:checked');
            if (selectedProducts.length === 0) {
                alert('Please select at least one product category!');
                return;
            }

            // Show confirmation modal
            if (confirmationModal) {
                confirmationModal.classList.remove('hidden');
                confirmationModal.classList.add('flex');
            }
        });
    }

    // Close confirmation modal when cancel is clicked
    if (cancelConfirmBtn) {
        cancelConfirmBtn.addEventListener('click', function () {
            if (confirmationModal) {
                confirmationModal.classList.add('hidden');
                confirmationModal.classList.remove('flex');
            }
        });
    }

    // Final form submission
    if (finalCreateBtn) {
        finalCreateBtn.addEventListener('click', function () {
            // Here you would typically submit the form to your backend
            // For now, we'll just show an alert and close both modals
            alert('Supplier created successfully!');

            // Close both modals
            if (confirmationModal) {
                confirmationModal.classList.add('hidden');
                confirmationModal.classList.remove('flex');
            }

            const addSupplierModal = document.getElementById('addSupplierModal');
            if (addSupplierModal) {
                addSupplierModal.classList.add('hidden');
            }

            // Reset the form
            if (supplierForm) {
                supplierForm.reset();
            }

            // Reset checkboxes
            const productCheckboxes = document.querySelectorAll('input[name="product_categories"]');
            productCheckboxes.forEach(checkbox => {
                checkbox.checked = false;
            });
            updateSelectedCount();
        });
    }

    // Close confirmation modal when clicking the X button
    const closeConfirmationBtn = confirmationModal?.querySelector('[data-modal-hide="confirmationModal"]');
    if (closeConfirmationBtn) {
        closeConfirmationBtn.addEventListener('click', function () {
            if (confirmationModal) {
                confirmationModal.classList.add('hidden');
                confirmationModal.classList.remove('flex');
            }
        });
    }

    // Close confirmation modal when clicking outside
    if (confirmationModal) {
        confirmationModal.addEventListener('click', function (e) {
            if (e.target === confirmationModal) {
                confirmationModal.classList.add('hidden');
                confirmationModal.classList.remove('flex');
            }
        });
    }

    // Phone number formatting
    const phoneInput = document.querySelector('input[name="phone_number"]');
    if (phoneInput) {
        phoneInput.addEventListener('input', function (e) {
            // Remove any non-digit characters
            let value = e.target.value.replace(/\D/g, '');

            // Format as (XXX) XXX-XXXX for US numbers, or keep as is for international
            if (value.length <= 10) {
                if (value.length > 3 && value.length <= 6) {
                    value = `(${value.slice(0, 3)}) ${value.slice(3)}`;
                } else if (value.length > 6) {
                    value = `(${value.slice(0, 3)}) ${value.slice(3, 6)}-${value.slice(6, 10)}`;
                }
            }

            e.target.value = value;
        });
    }

    // Email validation enhancement
    const emailInput = document.querySelector('input[name="email"]');
    if (emailInput) {
        emailInput.addEventListener('blur', function (e) {
            const email = e.target.value;
            if (email && !isValidEmail(email)) {
                e.target.setCustomValidity('Please enter a valid email address');
                e.target.reportValidity();
            } else {
                e.target.setCustomValidity('');
            }
        });
    }

    // Company name auto-format (capitalize each word)
    const companyNameInput = document.querySelector('input[name="company_name"]');
    if (companyNameInput) {
        companyNameInput.addEventListener('blur', function (e) {
            if (e.target.value) {
                e.target.value = e.target.value.replace(/\b\w/g, l => l.toUpperCase());
            }
        });
    }

    // Product categories checkbox functionality
    const selectAllBtn = document.getElementById('selectAllProducts');
    const productCheckboxes = document.querySelectorAll('input[name="product_categories"]');

    if (selectAllBtn && productCheckboxes.length > 0) {
        selectAllBtn.addEventListener('click', function () {
            const allChecked = Array.from(productCheckboxes).every(checkbox => checkbox.checked);

            productCheckboxes.forEach(checkbox => {
                checkbox.checked = !allChecked;
            });

            selectAllBtn.textContent = allChecked ? 'Select All' : 'Deselect All';
            updateSelectedCount();
        });
    }

    // Update selected count function
    function updateSelectedCount() {
        const selectedCount = document.querySelectorAll('input[name="product_categories"]:checked').length;
        let counterElement = document.getElementById('selectedProductsCount');
        const productContainer = document.querySelector('.mb-4');

        if (!counterElement && productContainer) {
            counterElement = document.createElement('div');
            counterElement.id = 'selectedProductsCount';
            counterElement.className = 'text-xs text-gray-500 mt-1';
            productContainer.appendChild(counterElement);
        }

        if (counterElement) {
            if (selectedCount > 0) {
                counterElement.textContent = `${selectedCount} categor${selectedCount === 1 ? 'y' : 'ies'} selected`;
            } else {
                counterElement.textContent = 'Select all applicable product categories';
            }
        }

        // Update Select All button text
        if (selectAllBtn) {
            if (selectedCount === productCheckboxes.length) {
                selectAllBtn.textContent = 'Deselect All';
            } else {
                selectAllBtn.textContent = 'Select All';
            }
        }
    }

    // Add event listeners to all checkboxes
    productCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', updateSelectedCount);
    });

    // Initialize count on load
    updateSelectedCount();

    // Helper function for email validation
    function isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    console.log('Supplier JS initialized successfully');
};